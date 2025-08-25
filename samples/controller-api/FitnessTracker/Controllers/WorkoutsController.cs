using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Workout>> GetWorkouts([FromQuery] int? userId = null)
    {
        var workouts = new List<Workout>
        {
            new Workout
            {
                Id = 1,
                UserId = userId ?? 1,
                Name = "Push Day",
                Type = "Strength Training",
                StartTime = DateTime.UtcNow.AddHours(-2),
                EndTime = DateTime.UtcNow.AddMinutes(-15),
                Duration = 105,
                CaloriesBurned = 380,
                Intensity = "High",
                Exercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        ExerciseId = 1,
                        Exercise = new Exercise { Id = 1, Name = "Bench Press", Category = "Push", MuscleGroup = "Chest" },
                        Sets = new List<ExerciseSet>
                        {
                            new ExerciseSet { SetNumber = 1, Reps = 10, Weight = 80.0, Completed = true },
                            new ExerciseSet { SetNumber = 2, Reps = 8, Weight = 85.0, Completed = true },
                            new ExerciseSet { SetNumber = 3, Reps = 6, Weight = 90.0, Completed = true }
                        },
                        RestTime = 180
                    }
                },
                Metrics = new WorkoutMetrics
                {
                    AverageHeartRate = 135.0,
                    MaxHeartRate = 168.0,
                    TotalSets = 12,
                    TotalReps = 156,
                    TotalWeight = 2450.0,
                    RecoveryTime = 48
                },
                Notes = "Felt strong today, increased weight on bench press"
            }
        };

        return Ok(workouts);
    }

    [HttpGet("{id}")]
    public ActionResult<Workout> GetWorkout(int id)
    {
        var workout = new Workout
        {
            Id = id,
            UserId = 1,
            Name = "Full Body Circuit",
            Type = "Circuit Training",
            StartTime = DateTime.UtcNow.AddDays(-1).AddHours(-1),
            EndTime = DateTime.UtcNow.AddDays(-1).AddMinutes(-15),
            Duration = 45,
            CaloriesBurned = 320,
            Intensity = "Medium",
            Exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    ExerciseId = 10,
                    Exercise = new Exercise 
                    { 
                        Id = 10, 
                        Name = "Burpees", 
                        Category = "Full Body", 
                        MuscleGroup = "Full Body",
                        Equipment = "Bodyweight",
                        Difficulty = "Intermediate"
                    },
                    Sets = new List<ExerciseSet>
                    {
                        new ExerciseSet { SetNumber = 1, Reps = 15, Duration = 45, Completed = true },
                        new ExerciseSet { SetNumber = 2, Reps = 12, Duration = 40, Completed = true },
                        new ExerciseSet { SetNumber = 3, Reps = 10, Duration = 35, Completed = true }
                    },
                    RestTime = 60
                }
            },
            Metrics = new WorkoutMetrics
            {
                AverageHeartRate = 142.0,
                MaxHeartRate = 175.0,
                TotalSets = 15,
                TotalReps = 185,
                RecoveryTime = 24
            }
        };

        return Ok(workout);
    }

    [HttpPost]
    public ActionResult<Workout> CreateWorkout(CreateWorkoutRequest request)
    {
        var workout = new Workout
        {
            Id = 100,
            UserId = 1,
            Name = request.Name,
            Type = request.Type,
            StartTime = DateTime.UtcNow,
            Duration = 0,
            CaloriesBurned = 0,
            Intensity = "Medium",
            Exercises = request.Exercises.Select(ex => new WorkoutExercise
            {
                ExerciseId = ex.ExerciseId,
                Exercise = new Exercise { Id = ex.ExerciseId, Name = "New Exercise" },
                Sets = ex.Sets.Select((set, index) => new ExerciseSet
                {
                    SetNumber = index + 1,
                    Reps = set.Reps,
                    Weight = set.Weight,
                    Duration = set.Duration,
                    Completed = false
                }).ToList()
            }).ToList(),
            Metrics = new WorkoutMetrics()
        };

        return CreatedAtAction(nameof(GetWorkout), new { id = workout.Id }, workout);
    }

    [HttpPut("{id}")]
    public ActionResult<Workout> UpdateWorkout(int id, Workout workout)
    {
        workout.Id = id;
        workout.EndTime = DateTime.UtcNow;
        
        return Ok(workout);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWorkout(int id)
    {
        return NoContent();
    }

    [HttpPost("{id}/complete")]
    public ActionResult<Workout> CompleteWorkout(int id)
    {
        var workout = new Workout
        {
            Id = id,
            EndTime = DateTime.UtcNow,
            Duration = 65,
            CaloriesBurned = 410,
            Metrics = new WorkoutMetrics
            {
                AverageHeartRate = 138.0,
                MaxHeartRate = 165.0,
                RecoveryTime = 36
            }
        };

        return Ok(workout);
    }
}