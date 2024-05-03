using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class UserLoginModel
    {
        [Required]
        public required string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}