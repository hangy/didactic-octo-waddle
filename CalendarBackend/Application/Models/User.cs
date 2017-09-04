namespace CalendarBackend.Application.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Required, Key]
        public string Id { get; set; }

        [Required, StringLength(264)]
        public string UserName { get; set; }

        [Required, StringLength(20)]
        public string DisplayName { get; set; }

        [Required, StringLength(25)]
        public string Color { get; set; }

        [Required, EmailAddress]
        public string MailAddress { get; set; }
    }
}
