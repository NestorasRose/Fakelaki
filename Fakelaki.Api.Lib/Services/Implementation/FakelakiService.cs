using System;
using System.Collections.Generic;
using System.Linq;
using Fakelaki.Api.Lib.DAL;
using Fakelaki.Api.Lib.Models;
using Fakelaki.Api.Lib.Services.Interfaces;

namespace Fakelaki.Api.Lib.Services.Implementation
{
    public class FakelakiService : IFakelakiService
    {
        private FakelakiContext _context;
        private IMailService _mailService;

        public FakelakiService(FakelakiContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }


        public ICollection<Models.Fakelaki> GetByEvent(int fakelakiId)
        {
            return _context.Events.Find(fakelakiId).Fakelakia;
        }

        public void SetSuccessfullPayment(string paymentIntentId)
        {
            var fakelaki = _context.Fakelakia.Where(x => x.PaymentIntentId == paymentIntentId).FirstOrDefault();
            
            if(fakelaki == null)
            {
                throw new Exception($"Unable to find fakelaki for payment intend id '{paymentIntentId}'.");
            }

            fakelaki.SuccessfullPayment = true;
            _context.SaveChanges();
        }

        public Models.Fakelaki Create(Models.Fakelaki fakelaki, int emailTemplateId, int eventId)
        {

            // validation
            if (string.IsNullOrWhiteSpace(fakelaki.SenderName))
            {
                throw new Exception("SenderName is required.");
            }

            // validation
            if (string.IsNullOrWhiteSpace(fakelaki.SenderSurname))
            {
                throw new Exception("SenderSurname is required.");
            }

            // validation
            if (string.IsNullOrWhiteSpace(fakelaki.Message))
            {
                throw new Exception("Message is required.");
            }

            // validation
            if (fakelaki.Amount <= 0)
            {
                throw new Exception("Amount is required.");
            }

            fakelaki.CreatedDate = DateTime.Now;

            fakelaki.EmailTemplate = _context.EmailTemplates.Find(emailTemplateId);

            // TODO: Generate QR Code 
            _context.Events.Find(eventId).Fakelakia.Add(fakelaki);
            _context.SaveChanges();

            //_mailService.SendEmailAsync(new MailRequest()
            //{
            //    Subject = $"Fakelaki - {fakelaki.SenderName} {fakelaki.SenderSurname}",
            //    ToEmail = _context.Users.Where(u => u.Events.Find(eventId) != null)
            //}) ;

            return fakelaki;
        }

    }
}
