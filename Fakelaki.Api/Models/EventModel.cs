using System;
using System.ComponentModel.DataAnnotations;

namespace Fakelaki.Api.Models
{
    public class EventModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string QRCodeBase64 { get; set; }
    }
}
