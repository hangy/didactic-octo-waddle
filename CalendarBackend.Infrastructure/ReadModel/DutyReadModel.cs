namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using CalendarBackend.Infrastructure.EventStore;
    using MediatR;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DutyReadModel : INotificationHandler<IDomainEvent>
    {
        private readonly IList<Duty> entries = new List<Duty>();

        private readonly IEventStream eventStream;

        private bool initialized;

        public DutyReadModel(IEventStream eventStream)
        {
            this.eventStream = eventStream ?? throw new ArgumentNullException(nameof(eventStream));
        }

        public async Task<IEnumerable<Duty>> GetEntriesAsync(CancellationToken cancellationToken = default)
        {
            if (!this.initialized)
            {
                await this.InitializeAsync(cancellationToken).ConfigureAwait(false);
            }

            return this.entries;
        }

        public async Task Handle(IDomainEvent notification, CancellationToken cancellationToken)
        {
            if (!this.initialized)
            {
                await this.InitializeAsync(cancellationToken).ConfigureAwait(false);
            }

            await HandleAsync(notification, cancellationToken).ConfigureAwait(false);
        }

        private static void AssignEmptyTimeRange(Duty entry, AssignedOnDuty? dummyAssignmentForNewEntry = null)
        {
            var dummyEnumerable = dummyAssignmentForNewEntry != null ? new[] { dummyAssignmentForNewEntry } : Enumerable.Empty<AssignedOnDuty>();
            var users = entry.Assignments.Union(dummyEnumerable).OrderBy(a => a.Interval.Start).Select(a => a.UserId).Distinct().ToList();
            if (users.Count == 0)
            {
                return;
            }

            var lastKnownDate = entry.Assignments.Count > 0 ? entry.Assignments.Max(a => a.Interval.End) : entry.Interval.Start;
            var nextIntervalStart = entry.Assignments.Count > 0 ? lastKnownDate.DayOfWeek == IsoDayOfWeek.Monday ? lastKnownDate : lastKnownDate.Next(IsoDayOfWeek.Monday) : entry.Interval.Start;

            var userIndex = 0;
            var maxUserIndex = users.Count - 1;
            while (nextIntervalStart <= entry.Interval.End)
            {
                if (entry.Assignments.Any(a => a.Interval.Start == nextIntervalStart))
                {
                    nextIntervalStart = nextIntervalStart.Next(IsoDayOfWeek.Monday);
                    continue;
                }

                var userId = users[userIndex];
                var end = nextIntervalStart.Next(IsoDayOfWeek.Friday);

                entry.Assignments.Add(new AssignedOnDuty
                {
                    UserId = userId,
                    Interval = new DateInterval(nextIntervalStart, end)
                });

                nextIntervalStart = nextIntervalStart.Next(IsoDayOfWeek.Monday);
                userIndex = userIndex == maxUserIndex ? 0 : userIndex + 1;
            }
        }

        private void Handle(DutyCreatedEvent e)
        {
            this.entries.Add(new Duty { Id = e.DutyId, Name = e.Name, Interval = e.Interval, Assignments = new List<AssignedOnDuty>(), DomainEvents = new List<IDomainEvent> { e } });
        }

        private void Handle(OnDutySubstitutedDefinedEvent e)
        {
            var entry = this.entries.SingleOrDefault(ent => ent.Id == e.DutyId);
            if (entry != null)
            {
                foreach (var assignment in entry.Assignments.Where(a => a.Interval.Start >= e.Interval.Start && a.Interval.End <= e.Interval.End))
                {
                    assignment.UserId = e.UserId;
                    assignment.Substitution = true;
                }
            }
        }

        private void Handle(UserAddedToDutyEvent e)
        {
            var entry = this.entries.SingleOrDefault(ent => ent.Id == e.DutyId);
            if (entry != null)
            {
                // Remove everything beginning from the start date except if a another user had substituted for this period
                entry.Assignments.RemoveAll(a => a.Interval.Start >= e.Start && (!a.Substitution && e.UserId != a.UserId));
                AssignEmptyTimeRange(entry, new AssignedOnDuty { UserId = e.UserId, Interval = new DateInterval(e.Start, e.Start) });
            }
        }

        private void Handle(UserRemovedFromDutyEvent e)
        {
            var entry = this.entries.SingleOrDefault(ent => ent.Id == e.DutyId);
            if (entry != null)
            {
                entry.Assignments.RemoveAll(a => a.UserId == e.UserId && a.Interval.Start >= e.End);

                // Move later entries forward to fill gaps except if a another user had substituted for this period
                var laterAssignments = entry.Assignments.Where(a => a.Interval.Start >= e.End);
                var lastIntervalEnd = e.End.DayOfWeek == IsoDayOfWeek.Sunday ? e.End : e.End.Next(IsoDayOfWeek.Sunday);
                foreach (var assignment in laterAssignments)
                {
                    if (assignment.Substitution)
                    {
                        lastIntervalEnd = assignment.Interval.End.DayOfWeek == IsoDayOfWeek.Sunday ? assignment.Interval.End : assignment.Interval.End.Next(IsoDayOfWeek.Sunday);
                    }
                    else
                    {
                        var start = lastIntervalEnd.Next(IsoDayOfWeek.Monday);
                        lastIntervalEnd = start.Next(IsoDayOfWeek.Friday);
                        assignment.Interval = new DateInterval(start, lastIntervalEnd);
                    }
                }

                AssignEmptyTimeRange(entry);
            }
        }

        private async Task HandleAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
        {
            switch (@event)
            {
                case DutyCreatedEvent e:
                    this.Handle(e);
                    break;
                case UserAddedToDutyEvent e:
                    this.Handle(e);
                    break;
                case OnDutySubstitutedDefinedEvent e:
                    this.Handle(e);
                    break;
                case UserRemovedFromDutyEvent e:
                    this.Handle(e);
                    break;
            }
        }

        private async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var events = await this.eventStream.ReadAllEventsAsync(cancellationToken).ConfigureAwait(false);
            foreach (var @event in events)
            {
                await this.HandleAsync(@event, cancellationToken).ConfigureAwait(false);
            }

            this.initialized = true;
        }
    }
}
