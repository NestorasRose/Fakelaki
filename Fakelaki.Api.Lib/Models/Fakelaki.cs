using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fakelaki.Api.Lib.Models
{
    public class Fakelaki
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SenderName { get; set; }

        public string SenderSurname { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Message { get; set; }

        public int Amount { get; set; }

        public string PaymentIntentId { get; set; }

        public bool SuccessfullPayment { get; set; }

        public int EventId { get; set; }

        public virtual Event Event { get; set; }

        public virtual EmailTemplate EmailTemplate { get; set; }
    }
}
