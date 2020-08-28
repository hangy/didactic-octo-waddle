namespace CalendarBackend.Domain.AggregatesModel.UserAggregate
{
    public record UserColor
    {
        public UserColor(string color)
        {
            this.Color = color;
        }

        public string Color { get; }
    }
}