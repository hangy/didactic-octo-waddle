﻿namespace CalendarBackend.Infrastructure.ReadModel
{
    using CalendarBackend.Domain.Events;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OutOfOffice
    {
        [Required, Key]
        public Guid Id { get; set; }

        [Required]
        public Interval Interval { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public string Reason { get; set; }

        [Required]
        public string UserId { get; set; }

        public List<IDomainEvent> DomainEvents { get; set; }
    }
}
