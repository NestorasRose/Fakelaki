using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fakelaki.Api.Lib.DAL
{
    public class FakelakiGenerator
    {
        private readonly FakelakiContext ctx;
        public FakelakiGenerator(FakelakiContext dbContext)
        {
            ctx = dbContext;
        }

        public void FakelakiDataSeed()
        {

            if (ctx.Events.Any())
            {
                return;
            }


            string password = "test";
            byte[] passwordHash, passwordSalt;
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            ctx.Users.Add(new Models.User()
            {
                Username = "nrose@techlink.com.cy",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                AccountId = "test",
                Id = 1
            });

            ctx.Users.FirstOrDefault().Events.Add(new Models.Event()
            {
                Id = 1,
                Name = "Event Test",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                Type = Enums.EventType.Wedding
            });

            ctx.Users.FirstOrDefault().Events.FirstOrDefault().Fakelakia.Add(new Models.Fakelaki()
            {
                SuccessfullPayment = true,
                Amount = 10,
                EmailTemplate = null,
                CreatedDate = DateTime.Today.AddDays(2),
                Message = "I wish you all the best!",
                SenderName = "Jon",
                SenderSurname = "Dow"
            });

            ctx.SaveChanges();
        }
    }
}
