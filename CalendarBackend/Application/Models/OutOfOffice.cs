namespace CalendarBackend.Application.Models
{
    using NodaTime;
    using System.ComponentModel.DataAnnotations;

    public class OutOfOffice
    {
        [Required, Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public Interval Interval { get; set; }

        [StringLength(10, MinimumLength = 0)]
        public string Reason { get; set; }
    }
}
