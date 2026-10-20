namespace Cognia.Shared.Models;

public class Therapist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> AvailableDays { get; set; } = [];
    public List<string> TimeSlots { get; set; } = [];
    public bool IsVerified { get; set; }
    public int YearsExperience { get; set; }
    public List<string> Languages { get; set; } = [];
    public string Education { get; set; } = string.Empty;
    public string Approach { get; set; } = string.Empty;
}

public class TherapistSession
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public int TherapistId { get; set; }
    public string TherapistName { get; set; } = string.Empty;
    public DateTime SessionDate { get; set; } = DateTime.Today.AddDays(1);
    public string TimeSlot { get; set; } = "10:00 AM";
    public int DurationMinutes { get; set; } = 60;
    public string SessionMode { get; set; } = "Encrypted Video Call"; // Video, Voice, Text Chat
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled
    public string MeetingLink { get; set; } = string.Empty;
    public string PaymentReference { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class PaymentRecord
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GHS";
    public string PaymentReference { get; set; } = string.Empty;
    public string PaymentChannel { get; set; } = "MTN Mobile Money"; // Mobile Money or Card
    public string PaymentStatus { get; set; } = "Pending"; // Pending, Success, Failed
    public string PaystackAccessCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
}

public class BookSessionRequest
{
    public int TherapistId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public DateTime SessionDate { get; set; } = DateTime.Today.AddDays(1);
    public string TimeSlot { get; set; } = "10:00 AM";
    public int DurationMinutes { get; set; } = 60;
    public string SessionMode { get; set; } = "Encrypted Video Call";
    public string Notes { get; set; } = string.Empty;
    public string PreferredPaymentMethod { get; set; } = "Mobile Money";
}

public class InitializePaymentRequest
{
    public int SessionId { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CallbackUrl { get; set; } = string.Empty;
}

public class PaystackInitializeResponse
{
    public bool Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public PaystackInitializeData? Data { get; set; }
}

public class PaystackInitializeData
{
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string AccessCode { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
}

public class PaystackVerifyResponse
{
    public bool Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public PaystackVerifyData? Data { get; set; }
}

public class PaystackVerifyData
{
    public string Status { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string GatewayResponse { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string PaidAt { get; set; } = string.Empty;
}
