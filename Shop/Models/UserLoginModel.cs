using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class UserLoginModel
    {
        [Required]
        public required string Login { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}