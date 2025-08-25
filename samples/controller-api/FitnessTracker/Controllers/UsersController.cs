using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "Sarah",
                LastName = "Johnson",
                Email = "sarah.johnson@email.com",
                DateOfBirth = new DateOnly(1990, 3, 15),
                Height = 165.0,
                CurrentWeight = 65.5,
                FitnessLevel = "Intermediate",
                JoinDate = DateTime.UtcNow.AddMonths(-6),
                Goals = new UserGoals
                {
                    TargetWeight = 62.0,
                    WeeklyWorkoutTarget = 4,
                    DailyStepTarget = 10000,
                    PrimaryGoal = "Weight Loss"
                },
                Preferences = new UserPreferences
                {
                    PreferredWorkoutTypes = new List<string> { "Cardio", "Strength Training" },
                    PreferredWorkoutDuration = 45,
                    PreferredWorkoutTime = new TimeOnly(7, 0),
                    NotificationsEnabled = true
                }
            }
        };

        return Ok(users);
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
        var user = new User
        {
            Id = id,
            FirstName = "Mike",
            LastName = "Chen",
            Email = "mike.chen@email.com",
            DateOfBirth = new DateOnly(1985, 11, 22),
            Height = 178.0,
            CurrentWeight = 82.3,
            FitnessLevel = "Advanced",
            JoinDate = DateTime.UtcNow.AddYears(-2),
            Goals = new UserGoals
            {
                WeeklyWorkoutTarget = 6,
                DailyStepTarget = 12000,
                PrimaryGoal = "Muscle Gain"
            },
            Preferences = new UserPreferences
            {
                PreferredWorkoutTypes = new List<string> { "Strength Training", "Powerlifting" },
                PreferredWorkoutDuration = 90,
                PreferredWorkoutTime = new TimeOnly(18, 30),
                NotificationsEnabled = false
            }
        };

        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> CreateUser(User user)
    {
        user.Id = 100;
        user.JoinDate = DateTime.UtcNow;
        
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public ActionResult<User> UpdateUser(int id, User user)
    {
        user.Id = id;
        
        return Ok(user);
    }

    [HttpGet("{id}/summary")]
    public ActionResult<WorkoutSummary> GetUserSummary(int id)
    {
        var summary = new WorkoutSummary
        {
            TotalWorkouts = 48,
            TotalDuration = 2160, // 36 hours
            TotalCalories = 18400,
            CurrentStreak = 5,
            BestStreak = 12,
            WorkoutsByType = new Dictionary<string, int>
            {
                ["Strength Training"] = 28,
                ["Cardio"] = 15,
                ["Flexibility"] = 5
            },
            Improvements = new PerformanceImprovements
            {
                WeightChange = -3.2,
                StrengthIncrease = 15.5,
                EnduranceIncrease = 22.3,
                WorkoutFrequencyIncrease = 2
            }
        };

        return Ok(summary);
    }
}