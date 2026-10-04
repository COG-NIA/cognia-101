using Microsoft.AspNetCore.Mvc;
using Cognia.Shared.Models;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TherapistsController : ControllerBase
{
    private static readonly List<Therapist> Therapists = new()
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
            Bio = "Focused on burnout prevention, student mental health, and exam anxiety coping mechanisms tailored for university students and young professionals.",
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

    [HttpGet]
    public ActionResult<List<Therapist>> GetTherapists([FromQuery] string? specialization)
    {
        if (string.IsNullOrWhiteSpace(specialization))
        {
            return Ok(Therapists);
        }

        var filtered = Therapists.Where(t => 
            t.Specialization.Contains(specialization, StringComparison.OrdinalIgnoreCase)).ToList();

        return Ok(filtered);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Therapist> GetTherapist(int id)
    {
        var therapist = Therapists.FirstOrDefault(t => t.Id == id);
        if (therapist == null)
        {
            return NotFound(new { message = $"Therapist with ID {id} not found." });
        }
        return Ok(therapist);
    }
}
