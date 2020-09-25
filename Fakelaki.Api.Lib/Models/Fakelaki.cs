﻿using System;

namespace Fakelaki.Api.Lib.Models
{
    public class Fakelaki
    {
        public int Id { get; set; }

        public string SenderName { get; set; }

        public string SenderSurname { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Message { get; set; }

        public decimal Amount { get; set; }

        public virtual EmailTemplate EmailTemplate { get; set; }
    }
}
