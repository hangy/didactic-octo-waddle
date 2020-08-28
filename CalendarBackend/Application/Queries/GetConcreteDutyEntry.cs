namespace CalendarBackend.Application.Queries
{
    using CalendarBackend.Infrastructure.ReadModel;
    using MediatR;
    using NodaTime;
    using System;

    public record GetConcreteDutyEntry : IRequest<Duty?>
    {
        public GetConcreteDutyEntry(Guid id, LocalDate? startAppointmentRange, LocalDate? endAppointmentRange)
        {
            this.Id = id;
            this.StartAppointmentRange = startAppointmentRange;
            this.EndAppointmentRange = endAppointmentRange;
        }

        public Guid Id { get; }

        public LocalDate? StartAppointmentRange { get; }

        public LocalDate? EndAppointmentRange { get; }
    }
}
