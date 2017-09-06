namespace CalendarBackend.Application.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Required, StringLength(25)]
        public string Color { get; set; }

        [Required, StringLength(20)]
        public string DisplayName { get; set; }
        [Required, Key]
        public string Id { get; set; }

        [Required, EmailAddress]
        public string MailAddress { get; set; }

        [Required, StringLength(264)]
        public string UserName { get; set; }
    }
}
