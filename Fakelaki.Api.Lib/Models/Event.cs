using Fakelaki.Api.Lib.Enums;
using System;
using System.Collections.Generic;

namespace Fakelaki.Api.Lib.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EventType Type { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual ICollection<Fakelaki> Fakelakia { get; set; }
    }
}
