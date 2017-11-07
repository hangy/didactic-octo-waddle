namespace CalendarBackend.Infrastructure.ReadModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class User
    {
        [Required, StringLength(25), DataMember]
        public string Color { get; set; }

        [Required, StringLength(20), DataMember]
        public string DisplayName { get; set; }

        [Required, Key, DataMember]
        public Guid Id { get; set; }

        [Required, EmailAddress, DataMember]
        public string MailAddress { get; set; }

        [Required, StringLength(264), DataMember]
        public string UserName { get; set; }
    }
}
