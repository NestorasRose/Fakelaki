using Fakelaki.Api.Lib.Models;
using System.Collections.Generic;

namespace Fakelaki.Api.Lib.Services.Interfaces
{
    public interface IEventService
    {
        Event Get(int eventId);

        ICollection<Event> GetByUser(int usertId);

        Event Create(Event newEvent, int userId);

        void Update(Event updateEvent);
    }
}
