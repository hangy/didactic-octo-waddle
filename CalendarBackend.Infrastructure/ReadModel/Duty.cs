namespace CalendarBackend.Infrastructure.ReadModel
{
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CalendarBackend.Domain.Events;

    public class Duty
    {
        [Required, Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateInterval Interval { get; set; }

        public List<AssignedOnDuty> Assignments { get; set; }

        public List<IDomainEvent> DomainEvents { get; set; }
    }
}
