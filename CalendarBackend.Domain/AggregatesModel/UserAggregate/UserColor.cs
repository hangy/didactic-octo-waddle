namespace CalendarBackend.Domain.AggregatesModel.UserAggregate
{
    using CalendarBackend.Domain.SeedWork;
    using System.Collections.Generic;

    public class UserColor : ValueObject
    {
        public UserColor(string color)
        {
            this.Color = color;
        }

        public string Color { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Color;
        }
    }
}