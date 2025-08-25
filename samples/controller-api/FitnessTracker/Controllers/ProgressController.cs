using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController : ControllerBase
{
    [HttpGet("users/{userId}")]
    public ActionResult<IEnumerable<UserProgress>> GetUserProgress(int userId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var progress = new List<UserProgress>
        {
            new UserProgress
            {
                Id = 1,
                UserId = userId,
                RecordDate = DateTime.UtcNow.AddDays(-30),
                Weight = 68.5,
                BodyFatPercentage = 22.3,
                MuscleMass = 25.8,
                Measurements = new Dictionary<string, double>
                {
                    ["chest"] = 96.5,
                    ["waist"] = 78.2,
                    ["bicep_left"] = 32.1,
                    ["bicep_right"] = 32.3,
                    ["thigh_left"] = 58.7,
                    ["thigh_right"] = 58.9
                },
                Performance = new PerformanceMetrics
                {
                    BenchPressMax = 65,
                    SquatMax = 85,
                    DeadliftMax = 95,
                    RunTime5k = TimeSpan.FromMinutes(28).Add(TimeSpan.FromSeconds(45)),
                    PushUpMax = 28,
                    PullUpMax = 8,
                    PlankMax = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(15))
                },
                Notes = "Starting point - feeling motivated!"
            },
            new UserProgress
            {
                Id = 2,
                UserId = userId,
                RecordDate = DateTime.UtcNow.AddDays(-15),
                Weight = 67.8,
                BodyFatPercentage = 21.1,
                MuscleMass = 26.2,
                Measurements = new Dictionary<string, double>
                {
                    ["chest"] = 97.2,
                    ["waist"] = 77.1,
                    ["bicep_left"] = 32.8,
                    ["bicep_right"] = 33.0,
                    ["thigh_left"] = 59.1,
                    ["thigh_right"] = 59.3
                },
                Performance = new PerformanceMetrics
                {
                    BenchPressMax = 70,
                    SquatMax = 90,
                    DeadliftMax = 100,
                    RunTime5k = TimeSpan.FromMinutes(27).Add(TimeSpan.FromSeconds(30)),
                    PushUpMax = 32,
                    PullUpMax = 10,
                    PlankMax = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(45))
                },
                Notes = "Good progress this month!"
            }
        };

        // Filter by date range if provided
        if (startDate.HasValue)
        {
            progress = progress.Where(p => p.RecordDate >= startDate.Value).ToList();
        }
        
        if (endDate.HasValue)
        {
            progress = progress.Where(p => p.RecordDate <= endDate.Value).ToList();
        }

        return Ok(progress);
    }

    [HttpGet("users/{userId}/latest")]
    public ActionResult<UserProgress> GetLatestProgress(int userId)
    {
        var latestProgress = new UserProgress
        {
            Id = 10,
            UserId = userId,
            RecordDate = DateTime.UtcNow,
            Weight = 66.9,
            BodyFatPercentage = 19.8,
            MuscleMass = 27.1,
            Measurements = new Dictionary<string, double>
            {
                ["chest"] = 98.5,
                ["waist"] = 75.8,
                ["bicep_left"] = 33.5,
                ["bicep_right"] = 33.7,
                ["thigh_left"] = 60.2,
                ["thigh_right"] = 60.4
            },
            Performance = new PerformanceMetrics
            {
                BenchPressMax = 77,
                SquatMax = 98,
                DeadliftMax = 110,
                RunTime5k = TimeSpan.FromMinutes(26).Add(TimeSpan.FromSeconds(15)),
                PushUpMax = 38,
                PullUpMax = 12,
                PlankMax = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(20))
            },
            Notes = "Reached new personal records this week!",
            Photos = new List<string> { "/images/progress_001.jpg", "/images/progress_002.jpg" }
        };

        return Ok(latestProgress);
    }

    [HttpPost("users/{userId}")]
    public ActionResult<UserProgress> RecordProgress(int userId, UserProgress progress)
    {
        progress.Id = 100;
        progress.UserId = userId;
        progress.RecordDate = DateTime.UtcNow;

        return CreatedAtAction(nameof(GetLatestProgress), new { userId = userId }, progress);
    }

    [HttpPut("{id}")]
    public ActionResult<UserProgress> UpdateProgress(int id, UserProgress progress)
    {
        progress.Id = id;
        
        return Ok(progress);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProgress(int id)
    {
        return NoContent();
    }

    [HttpGet("users/{userId}/comparison")]
    public ActionResult<object> GetProgressComparison(int userId, [FromQuery] DateTime compareDate)
    {
        var comparison = new
        {
            CompareDate = compareDate,
            CurrentDate = DateTime.UtcNow,
            Changes = new
            {
                WeightChange = -1.6, // kg
                BodyFatChange = -2.5, // percentage points
                MuscleMassChange = 1.3, // kg
                PerformanceChanges = new
                {
                    BenchPressIncrease = 12, // kg
                    SquatIncrease = 13, // kg
                    DeadliftIncrease = 15, // kg
                    RunTimeImprovement = TimeSpan.FromMinutes(-2).Add(TimeSpan.FromSeconds(-30)), // faster
                    PushUpIncrease = 10,
                    PullUpIncrease = 4,
                    PlankImprovement = TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(5))
                }
            },
            Summary = new
            {
                OverallImprovement = 85.7, // percentage
                StrengthGain = 18.4, // percentage
                EnduranceImprovement = 12.8, // percentage
                BodyCompositionImprovement = 22.1 // percentage
            }
        };

        return Ok(comparison);
    }

    [HttpGet("users/{userId}/goals-progress")]
    public ActionResult<object> GetGoalsProgress(int userId)
    {
        var goalsProgress = new
        {
            UserId = userId,
            Goals = new
            {
                TargetWeight = 65.0,
                CurrentWeight = 66.9,
                WeightProgress = 76.7, // percentage towards goal
                
                WeeklyWorkoutTarget = 4,
                CurrentWeekWorkouts = 3,
                WorkoutProgress = 75.0, // percentage this week
                
                DailyStepTarget = 10000,
                AverageStepsThisWeek = 8750,
                StepsProgress = 87.5, // percentage
                
                StrengthGoals = new
                {
                    BenchPressGoal = 80,
                    CurrentBenchPress = 77,
                    BenchPressProgress = 96.25, // percentage
                    
                    SquatGoal = 100,
                    CurrentSquat = 98,
                    SquatProgress = 98.0, // percentage
                }
            },
            ProjectedCompletionDate = DateTime.UtcNow.AddMonths(2),
            RecommendedAdjustments = new List<string>
            {
                "Consider increasing cardio frequency to reach weight goal faster",
                "Add one more workout session per week to meet consistency goal",
                "Focus on progressive overload in bench press to reach strength goal"
            }
        };

        return Ok(goalsProgress);
    }
}