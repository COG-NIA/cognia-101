using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cognia.Shared.Models;

namespace Cognia.API.Services;

public class PaystackService : IPaystackService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PaystackService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<PaystackInitializeResponse> InitializeTransactionAsync(InitializePaymentRequest request)
    {
        var secretKey = _configuration["Paystack:SecretKey"];
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            return new PaystackInitializeResponse { Status = false, Message = "Paystack is not configured." };
        }

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.paystack.co/transaction/initialize");
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
        httpRequest.Content = JsonContent.Create(new
        {
            email = request.Email,
            amount = (int)(request.Amount * 100),
            currency = "GHS",
            callback_url = request.CallbackUrl
        });
        using var response = await _httpClient.SendAsync(httpRequest);
        var result = await response.Content.ReadFromJsonAsync<PaystackInitializeResponse>();
        return response.IsSuccessStatusCode && result is not null
            ? result
            : new PaystackInitializeResponse { Status = false, Message = "Paystack payment initialization failed." };
    }

    public async Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference)
    {
        var secretKey = _configuration["Paystack:SecretKey"];
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            return new PaystackVerifyResponse { Status = false, Message = "Paystack is not configured." };
        }

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.paystack.co/transaction/verify/{Uri.EscapeDataString(reference)}");
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
        using var response = await _httpClient.SendAsync(httpRequest);
        var result = await response.Content.ReadFromJsonAsync<PaystackVerifyResponse>();
        return response.IsSuccessStatusCode && result is not null
            ? result
            : new PaystackVerifyResponse { Status = false, Message = "Paystack payment verification failed." };
    }
}
