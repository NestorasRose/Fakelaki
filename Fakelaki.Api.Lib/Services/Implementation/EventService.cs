using System;
using System.Collections.Generic;
using System.Linq;
using Fakelaki.Api.Lib.DAL;
using Fakelaki.Api.Lib.Models;
using Fakelaki.Api.Lib.Services.Interfaces;

namespace Fakelaki.Api.Lib.Services.Implementation
{
    public class EventService : IEventService
    {
        private FakelakiContext _context;

        public EventService(FakelakiContext context)
        {
            _context = context;
        }


        public Event Get(int eventId)
        {
            return _context.Events.Find(eventId);
        }

        public ICollection<Event> GetByUser(int usertId)
        {
            return _context.Users.Find(usertId).Events;
        }


        public Event Create(Event newEvent, int userId)
        {

            // validation
            if (string.IsNullOrWhiteSpace(newEvent.Name))
            {
                throw new Exception("Name is required.");
            }

            // validation
            if (newEvent.StartDate < DateTime.Now || newEvent.EndDate < newEvent.EndDate)
            {
                throw new Exception("Event time period selected is invalid.");
            }

            // TODO: Generate QR Code 
            _context.Users.Find(userId).Events.Add(newEvent);
            _context.SaveChanges();

            return newEvent;
        }

        public void Update(Event updateEvent)
        {
            var updatingEvent = _context.Events.Find(updateEvent.Id);

            if (updatingEvent == null)
            {
                throw new Exception("Event not found");
            }

            _context.Events.Update(updatingEvent);
            _context.SaveChanges();
        }
    }
}
