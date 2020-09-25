using System;
using System.Collections.Generic;

namespace Fakelaki.Api.Lib.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string QRCode { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual ICollection<Fakelaki> Enrollments { get; set; }
    }
}
