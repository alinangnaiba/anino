using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutPlansController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<WorkoutPlan>> GetWorkoutPlans([FromQuery] string? goal = null, [FromQuery] string? difficulty = null)
    {
        var plans = new List<WorkoutPlan>
        {
            new WorkoutPlan
            {
                Id = 1,
                Name = "Beginner Full Body Strength",
                Description = "A comprehensive 8-week program designed for beginners to build foundational strength",
                Difficulty = "Beginner",
                DurationWeeks = 8,
                WorkoutsPerWeek = 3,
                Goal = "Strength Building",
                Equipment = new List<string> { "Dumbbells", "Resistance Bands", "Bench" },
                Workouts = new List<WorkoutTemplate>
                {
                    new WorkoutTemplate
                    {
                        Id = 1,
                        Name = "Full Body A",
                        DayOfWeek = 1, // Monday
                        Type = "Strength Training",
                        EstimatedDuration = 45,
                        Exercises = new List<TemplateExercise>
                        {
                            new TemplateExercise
                            {
                                Exercise = new Exercise { Id = 1, Name = "Goblet Squats", Category = "Legs" },
                                Sets = 3,
                                RepsRange = "8-12",
                                RestTime = 60,
                                Notes = "Focus on proper form and depth"
                            },
                            new TemplateExercise
                            {
                                Exercise = new Exercise { Id = 2, Name = "Dumbbell Bench Press", Category = "Push" },
                                Sets = 3,
                                RepsRange = "8-12",
                                RestTime = 90,
                                Notes = "Control the weight on both up and down phases"
                            }
                        }
                    }
                }
            },
            new WorkoutPlan
            {
                Id = 2,
                Name = "Advanced Powerlifting Program",
                Description = "12-week advanced program focused on maximizing the big three lifts",
                Difficulty = "Advanced",
                DurationWeeks = 12,
                WorkoutsPerWeek = 4,
                Goal = "Strength & Power",
                Equipment = new List<string> { "Barbell", "Power Rack", "Olympic Plates", "Bench" },
                Workouts = new List<WorkoutTemplate>
                {
                    new WorkoutTemplate
                    {
                        Id = 2,
                        Name = "Heavy Squat Day",
                        DayOfWeek = 1,
                        Type = "Powerlifting",
                        EstimatedDuration = 90,
                        Exercises = new List<TemplateExercise>
                        {
                            new TemplateExercise
                            {
                                Exercise = new Exercise { Id = 10, Name = "Back Squat", Category = "Legs" },
                                Sets = 5,
                                RepsRange = "1-3",
                                RestTime = 300,
                                Notes = "Work up to 90-95% 1RM"
                            }
                        }
                    }
                }
            }
        };

        // Filter by goal if provided
        if (!string.IsNullOrEmpty(goal))
        {
            plans = plans.Where(p => p.Goal.Contains(goal, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Filter by difficulty if provided
        if (!string.IsNullOrEmpty(difficulty))
        {
            plans = plans.Where(p => p.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return Ok(plans);
    }

    [HttpGet("{id}")]
    public ActionResult<WorkoutPlan> GetWorkoutPlan(int id)
    {
        var plan = new WorkoutPlan
        {
            Id = id,
            Name = "Fat Loss HIIT Circuit",
            Description = "High-intensity interval training program designed for maximum fat burn and cardiovascular improvement",
            Difficulty = "Intermediate",
            DurationWeeks = 6,
            WorkoutsPerWeek = 4,
            Goal = "Fat Loss",
            Equipment = new List<string> { "Bodyweight", "Kettlebell", "Jump Rope", "Medicine Ball" },
            Workouts = new List<WorkoutTemplate>
            {
                new WorkoutTemplate
                {
                    Id = 10,
                    Name = "HIIT Circuit A",
                    DayOfWeek = 1,
                    Type = "HIIT",
                    EstimatedDuration = 30,
                    Exercises = new List<TemplateExercise>
                    {
                        new TemplateExercise
                        {
                            Exercise = new Exercise { Id = 20, Name = "Burpees", Category = "Full Body" },
                            Sets = 4,
                            RepsRange = "30 seconds",
                            RestTime = 30,
                            Notes = "Maximum intensity for 30 seconds"
                        },
                        new TemplateExercise
                        {
                            Exercise = new Exercise { Id = 21, Name = "Mountain Climbers", Category = "Core" },
                            Sets = 4,
                            RepsRange = "30 seconds",
                            RestTime = 30,
                            Notes = "Keep hips level and core tight"
                        },
                        new TemplateExercise
                        {
                            Exercise = new Exercise { Id = 22, Name = "Jump Rope", Category = "Cardio" },
                            Sets = 4,
                            RepsRange = "45 seconds",
                            RestTime = 15,
                            Notes = "Maintain steady rhythm"
                        }
                    }
                },
                new WorkoutTemplate
                {
                    Id = 11,
                    Name = "Active Recovery",
                    DayOfWeek = 3,
                    Type = "Recovery",
                    EstimatedDuration = 20,
                    Exercises = new List<TemplateExercise>
                    {
                        new TemplateExercise
                        {
                            Exercise = new Exercise { Id = 30, Name = "Light Walking", Category = "Cardio" },
                            Sets = 1,
                            RepsRange = "20 minutes",
                            RestTime = 0,
                            Notes = "Easy pace, focus on movement and recovery"
                        }
                    }
                }
            }
        };

        return Ok(plan);
    }

    [HttpPost]
    public ActionResult<WorkoutPlan> CreateWorkoutPlan(WorkoutPlan plan)
    {
        plan.Id = 100;
        
        return CreatedAtAction(nameof(GetWorkoutPlan), new { id = plan.Id }, plan);
    }

    [HttpPut("{id}")]
    public ActionResult<WorkoutPlan> UpdateWorkoutPlan(int id, WorkoutPlan plan)
    {
        plan.Id = id;
        
        return Ok(plan);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWorkoutPlan(int id)
    {
        return NoContent();
    }

    [HttpPost("{id}/start")]
    public ActionResult<object> StartWorkoutPlan(int id, int userId)
    {
        var startedPlan = new
        {
            PlanId = id,
            UserId = userId,
            StartDate = DateTime.UtcNow,
            CurrentWeek = 1,
            CompletedWorkouts = 0,
            NextWorkout = new
            {
                Name = "Full Body A",
                ScheduledDate = DateTime.UtcNow.AddDays(1),
                EstimatedDuration = 45
            },
            Progress = new
            {
                WeeksCompleted = 0,
                TotalWeeks = 8,
                CompletionPercentage = 0.0
            }
        };

        return Ok(startedPlan);
    }

    [HttpGet("categories")]
    public ActionResult<IEnumerable<string>> GetPlanCategories()
    {
        var categories = new List<string>
        {
            "Strength Building",
            "Fat Loss",
            "Muscle Gain",
            "Endurance",
            "Powerlifting",
            "Bodybuilding",
            "Functional Fitness",
            "HIIT",
            "Calisthenics"
        };

        return Ok(categories);
    }
}