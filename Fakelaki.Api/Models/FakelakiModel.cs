using System;
using System.ComponentModel.DataAnnotations;

namespace Fakelaki.Api.Models
{
    public class FakelakiModel
    {
        public int Id { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string SenderSurname { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
