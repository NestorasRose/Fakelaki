using System.Collections.Generic;

namespace Fakelaki.Api.Lib.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<Event> Events { get; set; }


        // Stripe Account Id
        public string AccountId { get; set; }
    }
}
