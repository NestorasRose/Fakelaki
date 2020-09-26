using System.ComponentModel.DataAnnotations;

namespace Fakelaki.Api.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
