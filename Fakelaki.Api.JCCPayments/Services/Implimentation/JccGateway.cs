using Fakelaki.Api.JCCPayments.Enums;
using Fakelaki.Api.JCCPayments.Helpers;
using Fakelaki.Api.JCCPayments.Models;
using Fakelaki.Api.JCCPayments.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Fakelaki.Api.JCCPayments.Services.Implimentation
{
    public class JccGateway : IJccGateway
    {
        #region Constant
        private const string CaptureFlag = "A";
        private const string Version = "1.0.0";
        private const int CurrencyExp = 2;
        private const string SignatureMethod = "SHA1";
        #endregion

        #region Fields
        private readonly JccSettings _jccSettings;
        #endregion

        #region Ctor
        public JccGateway(IOptions<JccSettings> jccSettings)
        {
            _jccSettings = jccSettings.Value;
        }

        #endregion

        #region Methods
        public bool ProcessPayment(decimal purchaseAmount, CurrencyFields currencyCode, string orderId)
        {

            JccHelper.CheckOrderID(orderId);
            var roundedOrderTotal = Math.Round(purchaseAmount, CurrencyExp);
            JccHelper.CheckPurchaseAmt(roundedOrderTotal, CurrencyExp);

            return PostData(purchaseAmount, currencyCode, orderId);
        }

        public TransactionResponse ProcessRespond(JccResponse jccResponse)
        {
            TransactionResponse transactionResponse = new TransactionResponse();

            try
            {
                string toEncrypt = $"{_jccSettings.Password}{_jccSettings.MerchantId}{_jccSettings.AcquirerId}{jccResponse.OrderId}{jccResponse.ResponseCode}{jccResponse.ReasonCode}";
                string hash = JccHelper.CalculateSHA1Hash(toEncrypt);
                if (hash.Equals(jccResponse.Signature))
                {
                    if (jccResponse.ResponseCode.Trim().Equals("1"))
                    {
                        transactionResponse.Status = StatusCodedFiled.Success;
                    }
                    else
                    {
                        transactionResponse.Status = StatusCodedFiled.Declined;
                        transactionResponse.Message = jccResponse.ReasonCodeDesc;
                    }
                }
                else
                {
                    transactionResponse.Status = StatusCodedFiled.NotVerified;
                    transactionResponse.Message = "Transaction was not verified";
                }
            }
            catch (Exception ex)
            {
                transactionResponse.Status = StatusCodedFiled.Error;
                transactionResponse.Message = ex.Message;
            }

            return transactionResponse;
        }


        #endregion

        #region Utilities

        private IDictionary<string, string> CreateQueryParameters(decimal purchaseAmount, CurrencyFields currencyCode, string orderId)
        {

            //create query parameters
            return new Dictionary<string, string>
            {
                ["Version"] = Version,
                ["MerID"] = _jccSettings.MerchantId,
                ["AcqID"] = _jccSettings.AcquirerId,
                ["MerRespURL"] = _jccSettings.ResponseURL,
                ["PurchaseAmt"] = ProcessPruchaseAmount(purchaseAmount),
                ["PurchaseCurrency"] = ((int)currencyCode).ToString(),
                ["PurchaseCurrencyExponent"] = CurrencyExp.ToString(),
                ["OrderID"] = orderId,
                ["CaptureFlag"] = CaptureFlag,
                ["Signature"] = CalculateRequestSignature(purchaseAmount, currencyCode, orderId),
                ["SignatureMethod"] = SignatureMethod
            };
        }

        protected string CalculateRequestSignature(decimal purchaseAmount, CurrencyFields currencyCode, string orderId)
        {
            var roundedOrderTotal = Math.Round(purchaseAmount, CurrencyExp);

            string value =
                _jccSettings.Password +
                _jccSettings.MerchantId +
                _jccSettings.AcquirerId +
                orderId +
                JccHelper.GetPaddedAmount(roundedOrderTotal) +
                (int)currencyCode;

            return JccHelper.CalculateSHA1Hash(value);
        }

        protected string ProcessPruchaseAmount(decimal purchaseAmount)
        {
            string amount = Math.Round(purchaseAmount, CurrencyExp).ToString();
            amount = amount.PadLeft(13, ("0".ToCharArray())[0]);
            amount = amount.Replace(".", "");
            return amount;
        }

        private bool PostData(decimal purchaseAmount, CurrencyFields currencyCode, string orderId)
        {
            // encode form data
            StringBuilder postString = new StringBuilder();
            bool first = true;
            foreach (KeyValuePair<string, string> pair in CreateQueryParameters(purchaseAmount, currencyCode, orderId))
            {
                if (first)
                    first = false;
                else
                    postString.Append("&");
                postString.AppendFormat("{0}={1}", pair.Key, System.Web.HttpUtility.UrlEncode(pair.Value));
            }
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(postString.ToString());

            // set up request object
            HttpWebRequest request;
            try
            {
                request = WebRequest.Create(JccHelper.GetJccURL(_jccSettings.IsTestMode)) as HttpWebRequest;
            }
            catch (UriFormatException)
            {
                request = null;
            }
            if (request == null)
                throw new ApplicationException("Invalid URL: " + JccHelper.GetJccURL(_jccSettings.IsTestMode));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;

            // add post data to request
            System.IO.Stream postStream = request.GetRequestStream();
            postStream.Write(postBytes, 0, postBytes.Length);
            postStream.Close();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
