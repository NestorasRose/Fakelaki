using Fakelaki.Api.JCCPayments.Models;

namespace Fakelaki.Api.JCCPayments.Services.Interfaces
{
    public interface IJccGateway
    {
        bool ProcessPayment(decimal purchaseAmount, CurrencyFields currencyCode, string orderId);
        TransactionResponse ProcessRespond(JccResponse jccResponse);
    }
}
