using System.Net.Http.Json;
using Cognia.Shared.Models;

namespace Cognia.Client.Services;

public class PaymentBookingService
{
    private readonly HttpClient _http;

    private static readonly List<Therapist> MockTherapists = new()
    {
        new Therapist
        {
            Id = 1,
            Name = "Dr. Hannah Wilson",
            Title = "Clinical Psychologist, Ph.D.",
            Specialization = "Anxiety & Depression",
            Bio = "Specializing in Cognitive Behavioral Therapy (CBT) with over 8 years of experience helping young adults navigate stress, mood changes, and personal growth.",
            HourlyRate = 180.00m,
            Rating = 4.9,
            ReviewCount = 42,
            ImageUrl = "",
            AvailableDays = new List<string> { "Monday", "Tuesday", "Thursday" },
            TimeSlots = new List<string> { "09:00 AM", "11:00 AM", "02:00 PM", "04:00 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 2,
            Name = "Emmanuel Baidoo, M.Sc.",
            Title = "Licensed Counselor & Wellness Coach",
            Specialization = "Academic & Career Pressure",
            Bio = "Focused on burnout prevention, student mental health, and exam anxiety coping mechanisms tailored for university students.",
            HourlyRate = 140.00m,
            Rating = 4.8,
            ReviewCount = 31,
            ImageUrl = "",
            AvailableDays = new List<string> { "Wednesday", "Friday", "Saturday" },
            TimeSlots = new List<string> { "10:00 AM", "01:00 PM", "03:00 PM", "05:00 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 3,
            Name = "Dr. Sarah Mensah",
            Title = "Senior Consultant Psychiatrist",
            Specialization = "Trauma & Emotional Resilience",
            Bio = "Dedicated practitioner providing compassionate care for trauma recovery, grief support, and mindfulness-based stress reduction.",
            HourlyRate = 220.00m,
            Rating = 5.0,
            ReviewCount = 58,
            ImageUrl = "",
            AvailableDays = new List<string> { "Monday", "Wednesday", "Thursday" },
            TimeSlots = new List<string> { "09:30 AM", "11:30 AM", "02:30 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 4,
            Name = "Gibson Osei, MA",
            Title = "Relationship & Family Therapist",
            Specialization = "Relationships & Self-Care",
            Bio = "Helping individuals and couples build healthier communication, emotional awareness, and strong interpersonal boundaries.",
            HourlyRate = 160.00m,
            Rating = 4.7,
            ReviewCount = 19,
            ImageUrl = "",
            AvailableDays = new List<string> { "Tuesday", "Thursday", "Saturday" },
            TimeSlots = new List<string> { "10:00 AM", "12:00 PM", "03:00 PM", "06:00 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 5,
            Name = "Andrew Nii Attram, M.Sc.",
            Title = "Stress & Burnout Specialist",
            Specialization = "Stress & Burnout",
            Bio = "Guiding students and workers through workplace stress management, healthy work-life balance, and relaxation techniques.",
            HourlyRate = 150.00m,
            Rating = 4.9,
            ReviewCount = 27,
            ImageUrl = "",
            AvailableDays = new List<string> { "Monday", "Wednesday", "Friday" },
            TimeSlots = new List<string> { "08:30 AM", "11:00 AM", "03:30 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 6,
            Name = "Dr. Patricia Aryee",
            Title = "Grief & Bereavement Counselor",
            Specialization = "Grief & Coping",
            Bio = "Providing safe, supportive therapy for loss, life transitions, emotional healing, and building personal coping strategies.",
            HourlyRate = 195.00m,
            Rating = 4.9,
            ReviewCount = 36,
            ImageUrl = "",
            AvailableDays = new List<string> { "Tuesday", "Wednesday", "Friday" },
            TimeSlots = new List<string> { "10:00 AM", "01:30 PM", "04:30 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 7,
            Name = "Dr. Yaw Dankwa",
            Title = "Cognitive & Mood Disorder Specialist",
            Specialization = "Anxiety & Depression",
            Bio = "Specializing in mood regulation, panic disorder interventions, and holistic mental wellness strategies.",
            HourlyRate = 175.00m,
            Rating = 4.8,
            ReviewCount = 23,
            ImageUrl = "",
            AvailableDays = new List<string> { "Monday", "Thursday", "Saturday" },
            TimeSlots = new List<string> { "09:00 AM", "01:00 PM", "04:00 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 8,
            Name = "Afua Mensah, M.Sc.",
            Title = "University Student Counselor",
            Specialization = "Academic & Career Pressure",
            Bio = "Empowering university students to overcome study fatigue, thesis anxiety, and career decision challenges.",
            HourlyRate = 135.00m,
            Rating = 4.9,
            ReviewCount = 40,
            ImageUrl = "",
            AvailableDays = new List<string> { "Tuesday", "Wednesday", "Friday" },
            TimeSlots = new List<string> { "10:30 AM", "02:00 PM", "05:00 PM" },
            IsVerified = true
        },
        new Therapist
        {
            Id = 9,
            Name = "Grace Lamptey, M.Sc.",
            Title = "Mindfulness & Wellness Counselor",
            Specialization = "Relationships & Self-Care",
            Bio = "Supporting personal self-discovery, emotional growth, healthy boundaries, and self-compassion practices.",
            HourlyRate = 165.00m,
            Rating = 4.9,
            ReviewCount = 34,
            ImageUrl = "",
            AvailableDays = new List<string> { "Monday", "Thursday", "Friday" },
            TimeSlots = new List<string> { "09:30 AM", "01:30 PM", "04:00 PM" },
            IsVerified = true
        }
    };

    private static readonly List<TherapistSession> MockSessions = new();
    private static readonly List<PaymentRecord> MockPayments = new();
    private static int _sessionCounter = 1;
    private static int _paymentCounter = 1;

    public PaymentBookingService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Therapist>> GetTherapistsAsync(string? specialization = null)
    {
        try
        {
            var url = string.IsNullOrWhiteSpace(specialization) 
                ? "api/therapists" 
                : $"api/therapists?specialization={Uri.EscapeDataString(specialization)}";
            var result = await _http.GetFromJsonAsync<List<Therapist>>(url);
            if (result != null && result.Any()) return result;
        }
        catch { }

        if (string.IsNullOrWhiteSpace(specialization)) return MockTherapists;
        return MockTherapists.Where(t => t.Specialization.Contains(specialization, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<Therapist?> GetTherapistByIdAsync(int id)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<Therapist>($"api/therapists/{id}");
            if (result != null) return result;
        }
        catch { }

        return MockTherapists.FirstOrDefault(t => t.Id == id);
    }

    public async Task<TherapistSession?> BookSessionAsync(BookSessionRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/sessions", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TherapistSession>();
            }
        }
        catch { }

        var therapist = MockTherapists.FirstOrDefault(t => t.Id == request.TherapistId);
        var session = new TherapistSession
        {
            Id = _sessionCounter++,
            UserId = "Guest",
            ClientName = string.IsNullOrWhiteSpace(request.ClientName) ? "Anonymous Client" : request.ClientName,
            ClientEmail = string.IsNullOrWhiteSpace(request.ClientEmail) ? "client@cognia.com" : request.ClientEmail,
            TherapistId = request.TherapistId,
            TherapistName = therapist?.Name ?? "Verified Therapist",
            SessionDate = request.SessionDate,
            TimeSlot = request.TimeSlot,
            DurationMinutes = 60,
            TotalAmount = therapist?.HourlyRate ?? 150.00m,
            Notes = request.Notes,
            Status = "Pending",
            MeetingLink = string.Empty,
            PaymentReference = string.Empty,
            CreatedAt = DateTime.UtcNow
        };
        MockSessions.Add(session);
        return session;
    }

    public async Task<PaystackInitializeResponse?> InitializePaymentAsync(InitializePaymentRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/payments/initialize", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PaystackInitializeResponse>();
            }
        }
        catch { }

        var reference = $"COGNIA-PAY-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        var mockPayment = new PaymentRecord
        {
            Id = _paymentCounter++,
            SessionId = request.SessionId,
            UserId = "Guest",
            ClientEmail = request.Email,
            Amount = request.Amount,
            Currency = "GHS",
            PaymentReference = reference,
            PaymentChannel = "Mobile Money",
            PaymentStatus = "Pending",
            PaystackAccessCode = $"acc_{reference}",
            CreatedAt = DateTime.UtcNow
        };
        MockPayments.Add(mockPayment);

        var session = MockSessions.FirstOrDefault(s => s.Id == request.SessionId);
        if (session != null)
        {
            session.PaymentReference = reference;
        }

        return new PaystackInitializeResponse
        {
            Status = true,
            Message = "Payment initialized successfully",
            Data = new PaystackInitializeData
            {
                AuthorizationUrl = $"https://checkout.paystack.com/sandbox-{reference}",
                AccessCode = $"acc_{reference}",
                Reference = reference
            }
        };
    }

    public async Task<bool> VerifyPaymentAsync(string reference)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<PaystackVerifyResponse>($"api/payments/verify/{reference}");
            if (response != null && response.Status) return true;
        }
        catch { }

        var payment = MockPayments.FirstOrDefault(p => p.PaymentReference.Equals(reference, StringComparison.OrdinalIgnoreCase));
        if (payment != null)
        {
            payment.PaymentStatus = "Success";
            payment.PaidAt = DateTime.UtcNow;
        }

        var session = MockSessions.FirstOrDefault(s => s.PaymentReference.Equals(reference, StringComparison.OrdinalIgnoreCase));
        if (session != null)
        {
            session.Status = "Confirmed";
            session.MeetingLink = $"https://meet.jit.si/cognia-therapy-{reference[..Math.Min(12, reference.Length)]}";
        }

        return true;
    }

    public async Task<List<TherapistSession>> GetUserSessionsAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<TherapistSession>>("api/sessions/user/Guest");
            if (result != null && result.Any()) return result;
        }
        catch { }

        return MockSessions.OrderByDescending(s => s.CreatedAt).ToList();
    }

    public async Task<List<PaymentRecord>> GetPaymentHistoryAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<PaymentRecord>>("api/payments");
            if (result != null && result.Any()) return result;
        }
        catch { }

        return MockPayments.OrderByDescending(p => p.CreatedAt).ToList();
    }
}
