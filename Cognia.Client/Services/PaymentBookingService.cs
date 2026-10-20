using System.Net.Http.Json;
using Cognia.Shared.Models;

namespace Cognia.Client.Services;

public class PaymentBookingService
{
    private readonly HttpClient _http;

    public PaymentBookingService(HttpClient http) => _http = http;

    public async Task<List<Therapist>> GetTherapistsAsync(string? specialization = null)
    {
        var url = string.IsNullOrWhiteSpace(specialization)
            ? "api/therapists"
            : $"api/therapists?specialization={Uri.EscapeDataString(specialization)}";
        return await _http.GetFromJsonAsync<List<Therapist>>(url) ?? [];
    }

    public async Task<Therapist?> GetTherapistByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Therapist>($"api/therapists/{id}");
    }

    public async Task<TherapistSession?> BookSessionAsync(BookSessionRequest request)
    {
        using var response = await _http.PostAsJsonAsync("api/sessions", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TherapistSession>()
            ?? throw new InvalidDataException("The API returned no session record.");
    }

    public async Task<PaystackInitializeResponse?> InitializePaymentAsync(InitializePaymentRequest request)
    {
        using var response = await _http.PostAsJsonAsync("api/payments/initialize", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PaystackInitializeResponse>();
    }

    public async Task<bool> VerifyPaymentAsync(string reference)
    {
        var response = await _http.GetFromJsonAsync<PaystackVerifyResponse>(
            $"api/payments/verify/{Uri.EscapeDataString(reference)}");
        return response?.Status == true && response.Data?.Status == "success";
    }

    public async Task<List<TherapistSession>> GetUserSessionsAsync()
    {
        return await _http.GetFromJsonAsync<List<TherapistSession>>("api/sessions") ?? [];
    }

    public async Task<List<PaymentRecord>> GetPaymentHistoryAsync()
    {
        return await _http.GetFromJsonAsync<List<PaymentRecord>>("api/payments") ?? [];
    }
}
