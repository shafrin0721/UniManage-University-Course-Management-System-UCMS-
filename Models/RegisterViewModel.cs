using System.ComponentModel.DataAnnotations;

namespace UniManage.Models
{
    public class RegisterViewModel
    {
        [Required, Display(Name = "Full Name"), StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password"), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
