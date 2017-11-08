namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Duty
    {
        [Required, Key, DataMember]
        public Guid Id { get; set; }

        [Required, DataMember]
        public string Name { get; set; }

        [Required, DataMember]
        public DateInterval Interval { get; set; }

        [DataMember]
        public List<AssignedOnDuty> Assignments { get; set; }

        public List<IDomainEvent> DomainEvents { get; set; }
    }
}
