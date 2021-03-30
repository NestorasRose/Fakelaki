using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakelaki.Api.Lib.Models
{
    public class User
    {
        public User()
        {
            this.Events = new List<Event>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<Event> Events { get; set; }


        // Stripe Account Id
        public string AccountId { get; set; }
    }
}
