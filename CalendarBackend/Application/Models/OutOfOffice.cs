namespace CalendarBackend.Application.Models
{
    using NodaTime;

    public class OutOfOffice
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public Interval Interval { get; set; }

        public string Reason { get; set; }
    }
}
