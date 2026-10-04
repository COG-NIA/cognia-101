using Cognia.Shared.Models;

namespace Cognia.API.Services;

public interface IPaystackService
{
    Task<PaystackInitializeResponse> InitializeTransactionAsync(InitializePaymentRequest request);
    Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference);
}
