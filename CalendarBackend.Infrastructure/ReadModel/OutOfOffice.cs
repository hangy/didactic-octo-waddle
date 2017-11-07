namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class OutOfOffice
    {
        [Required, Key, DataMember]
        public Guid Id { get; set; }

        [Required, DataMember]
        public Interval Interval { get; set; }

        [StringLength(10, MinimumLength = 0), DataMember]
        public string Reason { get; set; }

        [Required, DataMember]
        public string UserId { get; set; }

        public List<IDomainEvent> DomainEvents { get; set; }
    }
}
