using Fakelaki.Api.Lib.Models;
using System.Collections.Generic;

namespace Fakelaki.Api.Lib.Services.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);

        IEnumerable<User> GetAll();

        User GetById(int id);

        User GetByAccountId(string accountId);

        string GetAccountIdByEventid(int eventId);

        User Create(User user, string password);

        void Update(User user, string password = null);

        void Delete(int id);
    }

}
