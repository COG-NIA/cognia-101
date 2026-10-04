namespace Cognia.Shared.Models;

public class Therapist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = "Licensed Mental Health Counselor";
    public string Specialization { get; set; } = "General Counseling";
    public string Bio { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; } = 150.00m; // GHS
    public double Rating { get; set; } = 4.9;
    public int ReviewCount { get; set; } = 24;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> AvailableDays { get; set; } = new() { "Monday", "Wednesday", "Friday" };
    public List<string> TimeSlots { get; set; } = new() { "09:00 AM", "11:00 AM", "02:00 PM", "04:00 PM" };
    public bool IsVerified { get; set; } = true;
    public int YearsExperience { get; set; } = 8;
    public List<string> Languages { get; set; } = new() { "English", "Twi" };
    public string Education { get; set; } = "Ph.D. in Clinical Psychology, University of Ghana";
    public string Approach { get; set; } = "Cognitive Behavioral Therapy (CBT), Mindfulness-Based Stress Reduction";
}

public class TherapistSession
{
    public int Id { get; set; }
    public string UserId { get; set; } = "Guest";
    public string ClientName { get; set; } = "Anonymous User";
    public string ClientEmail { get; set; } = "user@cognia.com";
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
    public string UserId { get; set; } = "Guest";
    public string ClientEmail { get; set; } = "user@cognia.com";
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
