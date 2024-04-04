using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        public int Id {  get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Entrepreneur { get; set; }
    }
}
