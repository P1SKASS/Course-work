using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class User
    {
        public int Id {  get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Entrepreneur { get; set; }

        public bool Administrator { get;private set; }
    }
}
