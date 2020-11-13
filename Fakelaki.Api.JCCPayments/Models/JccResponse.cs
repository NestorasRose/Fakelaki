namespace Fakelaki.Api.JCCPayments.Models
{
    public class JccResponse
    {
        public string Signature { get; set; }
        public string ResponseCode { get; set; }
        public string ReasonCodeDesc { get; set; }
        public string ReasonCode { get; set; }
        public string OrderId { get; set; }

    }
}
