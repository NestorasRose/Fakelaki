namespace Fakelaki.Api.JCCPayments.Helpers
{
    public class JccSettings
    {
        private string _merchantId;

        private string _acquirerId;

        private string _password;

        public string MerchantId
        { 
            get 
            { 
                return _merchantId; 
            } 
            set 
            { 
                JccHelper.CheckMerchantId(value);
                _merchantId = value;
            }
        }

        public string AcquirerId
        {
            get 
            { 
                return _acquirerId; 
            }
            set
            {
                JccHelper.CheckAcquirerId(value);
                _acquirerId = value;
            }
        }

        public string Password
        {
            get 
            { 
                return _password; 
            }
            set
            {
                JccHelper.CheckPassword(value);
                _password = value;
            }
        }

        public bool IsTestMode { get; set; }

        public string ResponseURL { get; set; }
    }
}
