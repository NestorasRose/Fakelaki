using System;
using System.Collections.Generic;
using System.Text;

namespace Fakelaki.Api.JCCPayments.Helpers
{
    internal class JccHelper
    {
        #region Methods

        public static string CalculateMD5Hash(string value)
        {
            int THE_SIZE = value.Length;
            byte[] theMessageAsBytes = new byte[THE_SIZE + 1];
            theMessageAsBytes = System.Text.Encoding.ASCII.GetBytes(value);
            byte[] theResult;
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            theResult = md5.ComputeHash(theMessageAsBytes);
            return System.Convert.ToBase64String(theResult);
        }

        public static string CalculateSHA1Hash(string value)
        {
            int THE_SIZE = value.Length;
            byte[] theMessageAsBytes = new byte[THE_SIZE + 1];
            theMessageAsBytes = System.Text.Encoding.ASCII.GetBytes(value);
            byte[] theResult;
            System.Security.Cryptography.SHA1CryptoServiceProvider sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            theResult = sha.ComputeHash(theMessageAsBytes);
            return System.Convert.ToBase64String(theResult);
        }

        public static void CheckPassword(string value)
        {
            if (VerifyString(value, 100) == false)
                throw new Exception("JCC password is invalid!");
        }

        // MerID N(15)
        public static void CheckMerID(string value)
        {
            if (VerifyNumeric(value, 15) == false)
                throw new Exception("JCC Merchant ID is invalid!");
        }

        // AcqID N(11)
        public static void CheckAcqID(string value)
        {
            if (VerifyNumeric(value, 15) == false)
                throw new Exception("JCC Acquirer ID is invalid!");
        }

        // PurchaseCurrencyExponent N(1)
        public static void CheckPurchaseCurrencyExp(int value)
        {
            if (VerifyNumeric(value, 1) == false || value < 0 || value > 3)
                throw new Exception("JCC Acquirer ID is invalid!");
        }

        // OrderID AN(150)
        public static void CheckOrderID(string value)
        {
            if (VerifyString(value, 150) == false)
                throw new Exception("JCC Order ID is invalid!");
        }

        public static bool VerifyString(string value, int length)
        {
            if (value == null)
                return false;
            if (value == string.Empty)
                return false;
            if (value.Length > length)
                return false;
            return true;
        }

        public static bool VerifyNumeric(object value, int length, bool matchExactlength = false)
        {
            if (IsNumeric(value.ToString()) == false)
                return false;
            string strValue = System.Convert.ToString(value);
            if (strValue.Length > length)
                return false;
            if (matchExactlength == true)
            {
                if (strValue.Length != length)
                    return false;
            }
            return true;
        }
        private static bool IsNumeric(string value)
        {
            return value.All(System.Char.IsDigit);
        }

        public static string GetPaddedAmount(decimal amount)
        {
            return amount.ToString().PadLeft(13, '0').Replace(".", "");
        }

        /// <summary>
        /// Gets Jcc URL
        /// </summary>
        /// <returns></returns>
        public static string GetJccURL(bool isTestMode)
        {
            return isTestMode ?
        "https://tjccpg.jccsecure.com/EcomPayment/RedirectAuthLink" :
        "https://jccpg.jccsecure.com/EcomPayment/RedirectAuthLink";
        }

        // PurchaseAmt N(12)
        public static void CheckPurchaseAmt(decimal value, int exponent)
        {
            CheckPurchaseCurrencyExp(exponent);
            decimal checkDecimalPlacesValue = new decimal();
            checkDecimalPlacesValue = decimal.Round(value, exponent);
            if (value != checkDecimalPlacesValue)
                throw new Exception("JCC Purchase amount invalid decimal places!");

            // 999999999999:      N(12)
            // 9999999999.99 	N(10) + (N2)
            // 999999999.999 	N(9) + (N3)
            switch (exponent)
            {
                case 0:
                    {
                        if (value > 999999999999)
                            throw new Exception("JCC Purchase amount out of range!");
                        break;
                    }

                case 2:
                    {
                        if (value > 9999999999.99M)
                            throw new Exception("JCC Purchase amount out of range!");
                        break;
                    }

                case 3:
                    {
                        if (value > 999999999.999M)
                            throw new Exception("JCC Purchase amount out of range!");
                        break;
                    }
            }
        }

        #endregion
    }
}
