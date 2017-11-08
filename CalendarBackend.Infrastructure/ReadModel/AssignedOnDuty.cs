namespace CalendarBackend.Infrastructure.ReadModel
{
    using NodaTime;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class AssignedOnDuty
    {

        [Required, DataMember]
        public DateInterval Interval { get; set; }

        [Required, DataMember]
        public string UserId { get; set; }

        [DataMember]
        public bool Substitution { get; set; }
    }
}
