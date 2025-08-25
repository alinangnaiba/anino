using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Exercise>> GetExercises([FromQuery] string? category = null, [FromQuery] string? muscleGroup = null)
    {
        var exercises = new List<Exercise>
        {
            new Exercise
            {
                Id = 1,
                Name = "Deadlift",
                Category = "Pull",
                MuscleGroup = "Posterior Chain",
                Equipment = "Barbell",
                Difficulty = "Intermediate",
                Description = "A compound exercise that works multiple muscle groups",
                Instructions = new List<string>
                {
                    "Stand with feet hip-width apart, toes under the bar",
                    "Bend at hips and knees to grip the bar",
                    "Keep chest up and back straight",
                    "Drive through heels to lift the bar",
                    "Stand tall with shoulders back",
                    "Lower the bar with control"
                },
                Metadata = new ExerciseMetadata
                {
                    CaloriesPerMinute = 8.5,
                    Tags = new List<string> { "compound", "strength", "powerlifting" },
                    PopularityRank = 3,
                    LastUpdated = DateTime.UtcNow.AddDays(-30)
                }
            },
            new Exercise
            {
                Id = 2,
                Name = "Push-ups",
                Category = "Push",
                MuscleGroup = "Chest, Triceps, Shoulders",
                Equipment = "Bodyweight",
                Difficulty = "Beginner",
                Description = "Classic bodyweight exercise for upper body strength",
                Instructions = new List<string>
                {
                    "Start in plank position with hands shoulder-width apart",
                    "Lower body until chest nearly touches the floor",
                    "Push back up to starting position",
                    "Keep core engaged throughout movement"
                },
                Metadata = new ExerciseMetadata
                {
                    CaloriesPerMinute = 5.2,
                    Tags = new List<string> { "bodyweight", "push", "beginner-friendly" },
                    PopularityRank = 1,
                    LastUpdated = DateTime.UtcNow.AddDays(-15)
                }
            }
        };

        // Filter by category if provided
        if (!string.IsNullOrEmpty(category))
        {
            exercises = exercises.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Filter by muscle group if provided
        if (!string.IsNullOrEmpty(muscleGroup))
        {
            exercises = exercises.Where(e => e.MuscleGroup.Contains(muscleGroup, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return Ok(exercises);
    }

    [HttpGet("{id}")]
    public ActionResult<Exercise> GetExercise(int id)
    {
        var exercise = new Exercise
        {
            Id = id,
            Name = "Squats",
            Category = "Legs",
            MuscleGroup = "Quadriceps, Glutes, Hamstrings",
            Equipment = "Barbell",
            Difficulty = "Intermediate",
            Description = "Fundamental compound leg exercise for building lower body strength",
            Instructions = new List<string>
            {
                "Position barbell on upper trapezius muscles",
                "Stand with feet shoulder-width apart",
                "Initiate movement by sitting back with hips",
                "Descend until thighs are parallel to floor",
                "Drive through heels to return to standing",
                "Keep knees tracking over toes throughout movement"
            },
            Metadata = new ExerciseMetadata
            {
                CaloriesPerMinute = 7.8,
                Tags = new List<string> { "compound", "legs", "strength", "fundamental" },
                PopularityRank = 2,
                LastUpdated = DateTime.UtcNow.AddDays(-45)
            }
        };

        return Ok(exercise);
    }

    [HttpPost]
    public ActionResult<Exercise> CreateExercise(Exercise exercise)
    {
        exercise.Id = 100;
        exercise.Metadata.LastUpdated = DateTime.UtcNow;
        
        return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, exercise);
    }

    [HttpPut("{id}")]
    public ActionResult<Exercise> UpdateExercise(int id, Exercise exercise)
    {
        exercise.Id = id;
        exercise.Metadata.LastUpdated = DateTime.UtcNow;
        
        return Ok(exercise);
    }

    [HttpGet("categories")]
    public ActionResult<IEnumerable<string>> GetExerciseCategories()
    {
        var categories = new List<string>
        {
            "Push",
            "Pull", 
            "Legs",
            "Core",
            "Cardio",
            "Full Body",
            "Flexibility"
        };

        return Ok(categories);
    }

    [HttpGet("muscle-groups")]
    public ActionResult<IEnumerable<string>> GetMuscleGroups()
    {
        var muscleGroups = new List<string>
        {
            "Chest",
            "Back",
            "Shoulders",
            "Arms",
            "Biceps",
            "Triceps",
            "Quadriceps",
            "Hamstrings",
            "Glutes",
            "Calves",
            "Core",
            "Abs"
        };

        return Ok(muscleGroups);
    }
}