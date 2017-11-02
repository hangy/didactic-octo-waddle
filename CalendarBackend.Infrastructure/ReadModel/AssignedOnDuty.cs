namespace CalendarBackend.Infrastructure.ReadModel
{
    using NodaTime;
    using System.ComponentModel.DataAnnotations;

    public class AssignedOnDuty
    {
        [Required]
        public Duty Duty { get; set; }

        [Required]
        public DateInterval Interval { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool Substitution { get; set; }
    }
}
