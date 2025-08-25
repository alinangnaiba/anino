namespace FitnessTracker.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public double Height { get; set; } // in cm
    public double CurrentWeight { get; set; } // in kg
    public string FitnessLevel { get; set; } = "Beginner"; // Beginner, Intermediate, Advanced
    public DateTime JoinDate { get; set; }
    public UserGoals Goals { get; set; } = new();
    public UserPreferences Preferences { get; set; } = new();
}

public class UserGoals
{
    public double? TargetWeight { get; set; }
    public int? WeeklyWorkoutTarget { get; set; }
    public int? DailyStepTarget { get; set; }
    public string PrimaryGoal { get; set; } = string.Empty; // Weight Loss, Muscle Gain, Endurance, etc.
}

public class UserPreferences
{
    public List<string> PreferredWorkoutTypes { get; set; } = new();
    public int PreferredWorkoutDuration { get; set; } // in minutes
    public TimeOnly PreferredWorkoutTime { get; set; }
    public bool NotificationsEnabled { get; set; }
}

public class Workout
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Strength, Cardio, Flexibility, Sports
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Duration { get; set; } // in minutes
    public int CaloriesBurned { get; set; }
    public string Intensity { get; set; } = "Medium"; // Low, Medium, High
    public List<WorkoutExercise> Exercises { get; set; } = new();
    public WorkoutMetrics Metrics { get; set; } = new();
    public string? Notes { get; set; }
}

public class WorkoutExercise
{
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = new();
    public List<ExerciseSet> Sets { get; set; } = new();
    public int RestTime { get; set; } // in seconds
    public string? Notes { get; set; }
}

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Push, Pull, Legs, Core, Cardio
    public string MuscleGroup { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Beginner";
    public string Description { get; set; } = string.Empty;
    public List<string> Instructions { get; set; } = new();
    public ExerciseMetadata Metadata { get; set; } = new();
}

public class ExerciseSet
{
    public int SetNumber { get; set; }
    public int? Reps { get; set; }
    public double? Weight { get; set; } // in kg
    public int? Duration { get; set; } // in seconds for timed exercises
    public double? Distance { get; set; } // in km for cardio
    public int? RestTime { get; set; } // in seconds
    public bool Completed { get; set; }
}

public class ExerciseMetadata
{
    public double CaloriesPerMinute { get; set; }
    public List<string> Tags { get; set; } = new();
    public int PopularityRank { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class WorkoutMetrics
{
    public double AverageHeartRate { get; set; }
    public double MaxHeartRate { get; set; }
    public int TotalSets { get; set; }
    public int TotalReps { get; set; }
    public double TotalWeight { get; set; } // total weight lifted
    public double TotalDistance { get; set; } // for cardio workouts
    public int RecoveryTime { get; set; } // estimated recovery time in hours
}

public class UserProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime RecordDate { get; set; }
    public double Weight { get; set; }
    public double? BodyFatPercentage { get; set; }
    public double? MuscleMass { get; set; }
    public Dictionary<string, double> Measurements { get; set; } = new(); // chest, waist, arms, etc.
    public PerformanceMetrics Performance { get; set; } = new();
    public string? Notes { get; set; }
    public List<string>? Photos { get; set; } // photo URLs
}

public class PerformanceMetrics
{
    public int? BenchPressMax { get; set; } // 1RM in kg
    public int? SquatMax { get; set; }
    public int? DeadliftMax { get; set; }
    public TimeSpan? RunTime5k { get; set; }
    public int? PushUpMax { get; set; }
    public int? PullUpMax { get; set; }
    public TimeSpan? PlankMax { get; set; }
}

public class WorkoutPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Beginner";
    public int DurationWeeks { get; set; }
    public int WorkoutsPerWeek { get; set; }
    public string Goal { get; set; } = string.Empty;
    public List<WorkoutTemplate> Workouts { get; set; } = new();
    public List<string> Equipment { get; set; } = new();
}

public class WorkoutTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DayOfWeek { get; set; } // 1-7
    public string Type { get; set; } = string.Empty;
    public int EstimatedDuration { get; set; }
    public List<TemplateExercise> Exercises { get; set; } = new();
}

public class TemplateExercise
{
    public Exercise Exercise { get; set; } = new();
    public int Sets { get; set; }
    public string RepsRange { get; set; } = string.Empty; // e.g., "8-12", "30 seconds"
    public int RestTime { get; set; }
    public string? Notes { get; set; }
}

public class CreateWorkoutRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<CreateWorkoutExercise> Exercises { get; set; } = new();
}

public class CreateWorkoutExercise
{
    public int ExerciseId { get; set; }
    public List<CreateExerciseSet> Sets { get; set; } = new();
}

public class CreateExerciseSet
{
    public int? Reps { get; set; }
    public double? Weight { get; set; }
    public int? Duration { get; set; }
}

public class WorkoutSummary
{
    public int TotalWorkouts { get; set; }
    public int TotalDuration { get; set; } // in minutes
    public int TotalCalories { get; set; }
    public int CurrentStreak { get; set; } // days
    public int BestStreak { get; set; }
    public Dictionary<string, int> WorkoutsByType { get; set; } = new();
    public PerformanceImprovements Improvements { get; set; } = new();
}

public class PerformanceImprovements
{
    public double WeightChange { get; set; } // in kg
    public double StrengthIncrease { get; set; } // percentage
    public double EnduranceIncrease { get; set; } // percentage
    public int WorkoutFrequencyIncrease { get; set; } // workouts per week
}

// Complex nested models for advanced analytics - 3+ levels deep
public class ComprehensiveUserAnalytics
{
    public int UserId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public AdvancedUserMetrics Metrics { get; set; } = new();
    public List<GoalProgressReport> GoalProgress { get; set; } = new();
    public IEnumerable<WorkoutTypeAnalysis> WorkoutAnalysis { get; set; } = new List<WorkoutTypeAnalysis>();
    public Dictionary<string, double> CustomMetrics { get; set; } = new();
    public ComprehensiveBenchmarks Benchmarks { get; set; } = new();
}

public class AdvancedUserMetrics
{
    public PersonalRecords Records { get; set; } = new();
    public List<PerformanceTrend> Trends { get; set; } = new();
    public HealthIndicators Health { get; set; } = new();
    public IEnumerable<CompetitiveComparison> Comparisons { get; set; } = new List<CompetitiveComparison>();
}

public class PersonalRecords
{
    public List<ExerciseRecord> StrengthRecords { get; set; } = new();
    public CardioRecord? BestCardioSession { get; set; }
    public Dictionary<string, RecordDetail> CustomRecords { get; set; } = new();
    public AchievementHistory Achievements { get; set; } = new();
}

public class ExerciseRecord
{
    public Exercise Exercise { get; set; } = new();
    public double MaxWeight { get; set; }
    public int MaxReps { get; set; }
    public DateTime AchievedDate { get; set; }
    public List<RecordAttempt> AttemptHistory { get; set; } = new();
    public RecordContext Context { get; set; } = new();
}

public class RecordAttempt
{
    public DateTime Date { get; set; }
    public double Weight { get; set; }
    public int Reps { get; set; }
    public bool Successful { get; set; }
    public List<PerformanceNote> Notes { get; set; } = new();
    public EnvironmentalFactors Environment { get; set; } = new();
}

public class PerformanceNote
{
    public string Category { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public int Severity { get; set; } // 1-10 scale
    public List<string> Tags { get; set; } = new();
}

public class EnvironmentalFactors
{
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public string Equipment { get; set; } = string.Empty;
    public List<string> Conditions { get; set; } = new();
}

public class RecordContext
{
    public int WorkoutId { get; set; }
    public string TrainingPhase { get; set; } = string.Empty;
    public List<TrainingPartner> Partners { get; set; } = new();
    public SupplementationInfo Supplements { get; set; } = new();
}

public class TrainingPartner
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Spotter, Coach, etc.
    public List<string> Contributions { get; set; } = new();
}

public class SupplementationInfo
{
    public List<Supplement> PreWorkout { get; set; } = new();
    public List<Supplement> PostWorkout { get; set; } = new();
    public NutritionalTiming Timing { get; set; } = new();
}

public class Supplement
{
    public string Name { get; set; } = string.Empty;
    public double Dosage { get; set; }
    public string Unit { get; set; } = string.Empty;
    public List<string> ActiveIngredients { get; set; } = new();
}

public class NutritionalTiming
{
    public DateTime? LastMeal { get; set; }
    public List<MacroIntake> RecentIntake { get; set; } = new();
    public HydrationLevel Hydration { get; set; } = new();
}

public class MacroIntake
{
    public string MacroType { get; set; } = string.Empty; // Protein, Carbs, Fats
    public double Grams { get; set; }
    public DateTime ConsumedAt { get; set; }
    public List<string> Sources { get; set; } = new();
}

public class HydrationLevel
{
    public double WaterIntake { get; set; } // in liters
    public List<FluidIntake> Fluids { get; set; } = new();
    public string Status { get; set; } = string.Empty; // Optimal, Dehydrated, etc.
}

public class FluidIntake
{
    public string FluidType { get; set; } = string.Empty;
    public double Volume { get; set; }
    public DateTime ConsumedAt { get; set; }
    public List<string> Electrolytes { get; set; } = new();
}

public class CardioRecord
{
    public string ActivityType { get; set; } = string.Empty;
    public double Distance { get; set; }
    public TimeSpan Duration { get; set; }
    public double AverageHeartRate { get; set; }
    public List<SplitTime> Splits { get; set; } = new();
    public WeatherConditions Weather { get; set; } = new();
}

public class SplitTime
{
    public double Distance { get; set; }
    public TimeSpan Time { get; set; }
    public double Pace { get; set; }
    public List<string> Observations { get; set; } = new();
}

public class WeatherConditions
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Conditions { get; set; } = string.Empty;
    public List<string> Challenges { get; set; } = new();
}

public class RecordDetail
{
    public string RecordType { get; set; } = string.Empty;
    public double Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public List<RecordValidation> Validations { get; set; } = new();
}

public class RecordValidation
{
    public string ValidatorName { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public List<string> Criteria { get; set; } = new();
}

public class AchievementHistory
{
    public List<Achievement> Unlocked { get; set; } = new();
    public IEnumerable<Achievement> InProgress { get; set; } = new List<Achievement>();
    public Dictionary<string, AchievementCategory> Categories { get; set; } = new();
}

public class Achievement
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? UnlockedDate { get; set; }
    public List<AchievementCriteria> Criteria { get; set; } = new();
    public AchievementRewards Rewards { get; set; } = new();
}

public class AchievementCriteria
{
    public string Type { get; set; } = string.Empty;
    public double TargetValue { get; set; }
    public double CurrentValue { get; set; }
    public List<string> Requirements { get; set; } = new();
}

public class AchievementRewards
{
    public int Points { get; set; }
    public List<string> Badges { get; set; } = new();
    public Dictionary<string, object> CustomRewards { get; set; } = new();
}

public class AchievementCategory
{
    public string CategoryName { get; set; } = string.Empty;
    public List<Achievement> Achievements { get; set; } = new();
    public CategoryProgress Progress { get; set; } = new();
}

public class CategoryProgress
{
    public int CompletedCount { get; set; }
    public int TotalCount { get; set; }
    public List<Milestone> Milestones { get; set; } = new();
}

public class Milestone
{
    public string Name { get; set; } = string.Empty;
    public int RequiredAchievements { get; set; }
    public bool IsReached { get; set; }
    public List<string> Rewards { get; set; } = new();
}

public class PerformanceTrend
{
    public string MetricName { get; set; } = string.Empty;
    public List<TrendDataPoint> DataPoints { get; set; } = new();
    public TrendAnalysis Analysis { get; set; } = new();
    public IEnumerable<TrendPrediction> Predictions { get; set; } = new List<TrendPrediction>();
}

public class TrendDataPoint
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public List<ContextualFactor> Factors { get; set; } = new();
}

public class ContextualFactor
{
    public string FactorType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public double Impact { get; set; } // -1.0 to 1.0
    public List<string> Notes { get; set; } = new();
}

public class TrendAnalysis
{
    public string Direction { get; set; } = string.Empty; // Improving, Declining, Stable
    public double SlopeValue { get; set; }
    public List<TrendInsight> Insights { get; set; } = new();
    public StatisticalSummary Statistics { get; set; } = new();
}

public class TrendInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public double Confidence { get; set; }
    public List<string> SupportingData { get; set; } = new();
}

public class StatisticalSummary
{
    public double Mean { get; set; }
    public double Median { get; set; }
    public double StandardDeviation { get; set; }
    public List<OutlierDetection> Outliers { get; set; } = new();
}

public class OutlierDetection
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public string Reason { get; set; } = string.Empty;
    public List<string> PossibleCauses { get; set; } = new();
}

public class TrendPrediction
{
    public DateTime PredictionDate { get; set; }
    public double PredictedValue { get; set; }
    public double ConfidenceInterval { get; set; }
    public List<PredictionFactor> Factors { get; set; } = new();
}

public class PredictionFactor
{
    public string FactorName { get; set; } = string.Empty;
    public double Weight { get; set; }
    public List<string> Assumptions { get; set; } = new();
}

public class HealthIndicators
{
    public VitalSigns Vitals { get; set; } = new();
    public List<BiometricMeasurement> Biometrics { get; set; } = new();
    public RecoveryMetrics Recovery { get; set; } = new();
    public IEnumerable<HealthRisk> RiskFactors { get; set; } = new List<HealthRisk>();
}

public class VitalSigns
{
    public HeartRateData HeartRate { get; set; } = new();
    public BloodPressureReading BloodPressure { get; set; } = new();
    public List<VitalTrend> Trends { get; set; } = new();
}

public class HeartRateData
{
    public int RestingHR { get; set; }
    public int MaxHR { get; set; }
    public List<HRZone> Zones { get; set; } = new();
    public HRVariability Variability { get; set; } = new();
}

public class HRZone
{
    public string ZoneName { get; set; } = string.Empty;
    public int MinHR { get; set; }
    public int MaxHR { get; set; }
    public List<ZoneMetric> Metrics { get; set; } = new();
}

public class ZoneMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Benchmarks { get; set; } = new();
}

public class HRVariability
{
    public double RMSSD { get; set; }
    public double SDNN { get; set; }
    public List<HRVTrend> Trends { get; set; } = new();
}

public class HRVTrend
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<string> Factors { get; set; } = new();
}

public class BloodPressureReading
{
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public DateTime MeasuredAt { get; set; }
    public List<BPContext> Context { get; set; } = new();
}

public class BPContext
{
    public string ContextType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<string> Influences { get; set; } = new();
}

public class VitalTrend
{
    public string VitalType { get; set; } = string.Empty;
    public List<VitalDataPoint> History { get; set; } = new();
    public TrendDirection Direction { get; set; } = new();
}

public class VitalDataPoint
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class TrendDirection
{
    public string Direction { get; set; } = string.Empty;
    public double Magnitude { get; set; }
    public List<string> Influences { get; set; } = new();
}

public class BiometricMeasurement
{
    public string MeasurementType { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime MeasuredAt { get; set; }
    public List<MeasurementContext> Context { get; set; } = new();
    public QualityIndicators Quality { get; set; } = new();
}

public class MeasurementContext
{
    public string ContextType { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
    public List<string> Notes { get; set; } = new();
}

public class QualityIndicators
{
    public double Accuracy { get; set; }
    public string ReliabilityScore { get; set; } = string.Empty;
    public List<QualityFlag> Flags { get; set; } = new();
}

public class QualityFlag
{
    public string FlagType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public List<string> Reasons { get; set; } = new();
}

public class RecoveryMetrics
{
    public SleepAnalysis Sleep { get; set; } = new();
    public List<StressIndicator> StressLevels { get; set; } = new();
    public InflammationMarkers Inflammation { get; set; } = new();
    public IEnumerable<RecoveryRecommendation> Recommendations { get; set; } = new List<RecoveryRecommendation>();
}

public class SleepAnalysis
{
    public TimeSpan Duration { get; set; }
    public double Quality { get; set; }
    public List<SleepStage> Stages { get; set; } = new();
    public SleepDisturbances Disturbances { get; set; } = new();
}

public class SleepStage
{
    public string StageName { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public List<StageMetric> Metrics { get; set; } = new();
}

public class StageMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Indicators { get; set; } = new();
}

public class SleepDisturbances
{
    public List<Disturbance> Events { get; set; } = new();
    public int TotalDisturbances { get; set; }
    public ImpactAssessment Impact { get; set; } = new();
}

public class Disturbance
{
    public DateTime Time { get; set; }
    public string Type { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public List<string> Causes { get; set; } = new();
}

public class ImpactAssessment
{
    public string SeverityLevel { get; set; } = string.Empty;
    public List<ImpactArea> Areas { get; set; } = new();
    public RecoveryTime EstimatedRecovery { get; set; } = new();
}

public class ImpactArea
{
    public string AreaName { get; set; } = string.Empty;
    public double ImpactScore { get; set; }
    public List<string> Symptoms { get; set; } = new();
}

public class RecoveryTime
{
    public TimeSpan Estimated { get; set; }
    public List<RecoveryFactor> Factors { get; set; } = new();
}

public class RecoveryFactor
{
    public string FactorName { get; set; } = string.Empty;
    public double Influence { get; set; }
    public List<string> Recommendations { get; set; } = new();
}

public class StressIndicator
{
    public string IndicatorType { get; set; } = string.Empty;
    public double Level { get; set; }
    public List<StressSource> Sources { get; set; } = new();
    public MitigationStrategies Mitigation { get; set; } = new();
}

public class StressSource
{
    public string SourceType { get; set; } = string.Empty;
    public double Contribution { get; set; }
    public List<string> Manifestations { get; set; } = new();
}

public class MitigationStrategies
{
    public List<Strategy> Recommended { get; set; } = new();
    public IEnumerable<Strategy> Alternative { get; set; } = new List<Strategy>();
    public EffectivenessTracking Tracking { get; set; } = new();
}

public class Strategy
{
    public string StrategyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ImplementationStep> Steps { get; set; } = new();
}

public class ImplementationStep
{
    public string StepDescription { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public List<string> Resources { get; set; } = new();
}

public class EffectivenessTracking
{
    public List<EffectivenessMetric> Metrics { get; set; } = new();
    public TrackingPeriod Period { get; set; } = new();
}

public class EffectivenessMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double BaselineValue { get; set; }
    public double CurrentValue { get; set; }
    public List<string> Improvements { get; set; } = new();
}

public class TrackingPeriod
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<CheckPoint> CheckPoints { get; set; } = new();
}

public class CheckPoint
{
    public DateTime Date { get; set; }
    public Dictionary<string, double> Measurements { get; set; } = new();
    public List<string> Observations { get; set; } = new();
}

public class InflammationMarkers
{
    public List<Biomarker> Markers { get; set; } = new();
    public InflammationLevel OverallLevel { get; set; } = new();
    public IEnumerable<InflammationTrigger> Triggers { get; set; } = new List<InflammationTrigger>();
}

public class Biomarker
{
    public string MarkerName { get; set; } = string.Empty;
    public double Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public List<ReferenceRange> Ranges { get; set; } = new();
}

public class ReferenceRange
{
    public string RangeType { get; set; } = string.Empty;
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    public List<string> Categories { get; set; } = new();
}

public class InflammationLevel
{
    public string Level { get; set; } = string.Empty;
    public double Score { get; set; }
    public List<InflammationIndicator> Indicators { get; set; } = new();
}

public class InflammationIndicator
{
    public string IndicatorName { get; set; } = string.Empty;
    public bool IsElevated { get; set; }
    public List<string> Implications { get; set; } = new();
}

public class InflammationTrigger
{
    public string TriggerType { get; set; } = string.Empty;
    public double Likelihood { get; set; }
    public List<TriggerSource> Sources { get; set; } = new();
}

public class TriggerSource
{
    public string SourceName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Interventions { get; set; } = new();
}

public class RecoveryRecommendation
{
    public string RecommendationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; }
    public List<ActionItem> ActionItems { get; set; } = new();
    public ExpectedOutcome Outcome { get; set; } = new();
}

public class ActionItem
{
    public string Action { get; set; } = string.Empty;
    public TimeSpan TimeFrame { get; set; }
    public List<string> Resources { get; set; } = new();
}

public class ExpectedOutcome
{
    public string Description { get; set; } = string.Empty;
    public TimeSpan Timeline { get; set; }
    public List<SuccessMetric> Metrics { get; set; } = new();
}

public class SuccessMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double TargetValue { get; set; }
    public List<string> MeasurementMethods { get; set; } = new();
}

public class HealthRisk
{
    public string RiskType { get; set; } = string.Empty;
    public double RiskLevel { get; set; }
    public List<RiskFactor> Factors { get; set; } = new();
    public PreventionPlan Prevention { get; set; } = new();
}

public class RiskFactor
{
    public string FactorName { get; set; } = string.Empty;
    public double Contribution { get; set; }
    public bool IsModifiable { get; set; }
    public List<string> InterventionOptions { get; set; } = new();
}

public class PreventionPlan
{
    public List<PreventionStrategy> Strategies { get; set; } = new();
    public MonitoringSchedule Monitoring { get; set; } = new();
    public IEnumerable<PreventionMilestone> Milestones { get; set; } = new List<PreventionMilestone>();
}

public class PreventionStrategy
{
    public string StrategyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<StrategyComponent> Components { get; set; } = new();
}

public class StrategyComponent
{
    public string ComponentName { get; set; } = string.Empty;
    public List<ComponentDetail> Details { get; set; } = new();
}

public class ComponentDetail
{
    public string DetailType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<string> Notes { get; set; } = new();
}

public class MonitoringSchedule
{
    public List<MonitoringTask> Tasks { get; set; } = new();
    public string Frequency { get; set; } = string.Empty;
    public AlertCriteria Alerts { get; set; } = new();
}

public class MonitoringTask
{
    public string TaskName { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public List<TaskParameter> Parameters { get; set; } = new();
}

public class TaskParameter
{
    public string ParameterName { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
}

public class AlertCriteria
{
    public List<AlertCondition> Conditions { get; set; } = new();
    public NotificationSettings Notifications { get; set; } = new();
}

public class AlertCondition
{
    public string ConditionType { get; set; } = string.Empty;
    public double Threshold { get; set; }
    public List<string> Actions { get; set; } = new();
}

public class NotificationSettings
{
    public List<NotificationChannel> Channels { get; set; } = new();
    public string Urgency { get; set; } = string.Empty;
}

public class NotificationChannel
{
    public string ChannelType { get; set; } = string.Empty;
    public Dictionary<string, string> Configuration { get; set; } = new();
    public List<string> Recipients { get; set; } = new();
}

public class PreventionMilestone
{
    public string MilestoneName { get; set; } = string.Empty;
    public DateTime TargetDate { get; set; }
    public List<MilestoneMetric> Metrics { get; set; } = new();
}

public class MilestoneMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double TargetValue { get; set; }
    public double CurrentValue { get; set; }
    public List<string> Actions { get; set; } = new();
}

public class CompetitiveComparison
{
    public string ComparisonType { get; set; } = string.Empty;
    public CompetitorGroup Group { get; set; } = new();
    public List<PerformanceComparison> Comparisons { get; set; } = new();
    public RankingInfo Ranking { get; set; } = new();
}

public class CompetitorGroup
{
    public string GroupName { get; set; } = string.Empty;
    public List<CompetitorProfile> Competitors { get; set; } = new();
    public GroupStatistics Statistics { get; set; } = new();
}

public class CompetitorProfile
{
    public string CompetitorId { get; set; } = string.Empty;
    public Demographics Demographics { get; set; } = new();
    public List<PerformanceMetric> Performance { get; set; } = new();
}

public class Demographics
{
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public double Weight { get; set; }
    public List<string> Categories { get; set; } = new();
}

public class PerformanceMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime RecordedAt { get; set; }
    public List<string> Conditions { get; set; } = new();
}

public class GroupStatistics
{
    public Dictionary<string, double> Averages { get; set; } = new();
    public List<StatisticalDistribution> Distributions { get; set; } = new();
}

public class StatisticalDistribution
{
    public string MetricName { get; set; } = string.Empty;
    public double Mean { get; set; }
    public double StandardDeviation { get; set; }
    public List<Percentile> Percentiles { get; set; } = new();
}

public class Percentile
{
    public double PercentileValue { get; set; }
    public double MetricValue { get; set; }
}

public class PerformanceComparison
{
    public string MetricName { get; set; } = string.Empty;
    public double UserValue { get; set; }
    public double GroupAverage { get; set; }
    public ComparisonResult Result { get; set; } = new();
}

public class ComparisonResult
{
    public string Status { get; set; } = string.Empty; // Above, Below, Average
    public double Difference { get; set; }
    public List<ImprovementSuggestion> Suggestions { get; set; } = new();
}

public class ImprovementSuggestion
{
    public string SuggestionType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ActionPlan> Plans { get; set; } = new();
}

public class ActionPlan
{
    public string PlanName { get; set; } = string.Empty;
    public List<PlanPhase> Phases { get; set; } = new();
    public ExpectedResults Results { get; set; } = new();
}

public class PlanPhase
{
    public string PhaseName { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public List<PhaseActivity> Activities { get; set; } = new();
}

public class PhaseActivity
{
    public string ActivityName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Requirements { get; set; } = new();
}

public class ExpectedResults
{
    public List<ResultMetric> Metrics { get; set; } = new();
    public TimeSpan Timeline { get; set; }
}

public class ResultMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double ExpectedImprovement { get; set; }
    public List<string> Assumptions { get; set; } = new();
}

public class RankingInfo
{
    public int CurrentRank { get; set; }
    public int TotalParticipants { get; set; }
    public List<RankingCategory> Categories { get; set; } = new();
}

public class RankingCategory
{
    public string CategoryName { get; set; } = string.Empty;
    public int Rank { get; set; }
    public List<RankingFactor> Factors { get; set; } = new();
}

public class RankingFactor
{
    public string FactorName { get; set; } = string.Empty;
    public double Weight { get; set; }
    public double Score { get; set; }
}

public class GoalProgressReport
{
    public string GoalType { get; set; } = string.Empty;
    public GoalDetails Details { get; set; } = new();
    public List<ProgressMilestone> Milestones { get; set; } = new();
    public IEnumerable<GoalAdjustment> Adjustments { get; set; } = new List<GoalAdjustment>();
}

public class GoalDetails
{
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime TargetDate { get; set; }
    public List<GoalCriteria> Criteria { get; set; } = new();
}

public class GoalCriteria
{
    public string CriteriaType { get; set; } = string.Empty;
    public double TargetValue { get; set; }
    public double CurrentValue { get; set; }
    public List<CriteriaMetric> Metrics { get; set; } = new();
}

public class CriteriaMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Influences { get; set; } = new();
}

public class ProgressMilestone
{
    public string MilestoneName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public List<MilestoneTask> Tasks { get; set; } = new();
}

public class MilestoneTask
{
    public string TaskName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<TaskDependency> Dependencies { get; set; } = new();
}

public class TaskDependency
{
    public string DependencyType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Requirements { get; set; } = new();
}

public class GoalAdjustment
{
    public DateTime AdjustmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public List<AdjustmentDetail> Changes { get; set; } = new();
}

public class AdjustmentDetail
{
    public string FieldName { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public List<string> Rationale { get; set; } = new();
}

public class WorkoutTypeAnalysis
{
    public string WorkoutType { get; set; } = string.Empty;
    public WorkoutStats Statistics { get; set; } = new();
    public List<WorkoutPattern> Patterns { get; set; } = new();
    public IEnumerable<WorkoutRecommendation> Recommendations { get; set; } = new List<WorkoutRecommendation>();
}

public class WorkoutStats
{
    public int TotalSessions { get; set; }
    public TimeSpan TotalDuration { get; set; }
    public List<StatsPeriod> PeriodStats { get; set; } = new();
}

public class StatsPeriod
{
    public string Period { get; set; } = string.Empty;
    public Dictionary<string, double> Metrics { get; set; } = new();
    public List<string> Highlights { get; set; } = new();
}

public class WorkoutPattern
{
    public string PatternType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<PatternElement> Elements { get; set; } = new();
}

public class PatternElement
{
    public string ElementType { get; set; } = string.Empty;
    public Dictionary<string, object> Properties { get; set; } = new();
    public List<string> Observations { get; set; } = new();
}

public class WorkoutRecommendation
{
    public string RecommendationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<RecommendationStep> Steps { get; set; } = new();
}

public class RecommendationStep
{
    public string StepDescription { get; set; } = string.Empty;
    public int Priority { get; set; }
    public List<StepResource> Resources { get; set; } = new();
}

public class StepResource
{
    public string ResourceType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> AccessMethods { get; set; } = new();
}

public class ComprehensiveBenchmarks
{
    public List<BenchmarkCategory> Categories { get; set; } = new();
    public OverallBenchmark Overall { get; set; } = new();
    public IEnumerable<BenchmarkComparison> Comparisons { get; set; } = new List<BenchmarkComparison>();
}

public class BenchmarkCategory
{
    public string CategoryName { get; set; } = string.Empty;
    public List<IndividualBenchmark> Benchmarks { get; set; } = new();
    public CategorySummary Summary { get; set; } = new();
}

public class IndividualBenchmark
{
    public string BenchmarkName { get; set; } = string.Empty;
    public double UserScore { get; set; }
    public double BenchmarkScore { get; set; }
    public List<ScoreBreakdown> Breakdown { get; set; } = new();
}

public class ScoreBreakdown
{
    public string Component { get; set; } = string.Empty;
    public double Score { get; set; }
    public double Weight { get; set; }
    public List<string> Factors { get; set; } = new();
}

public class CategorySummary
{
    public double OverallScore { get; set; }
    public string Performance { get; set; } = string.Empty;
    public List<CategoryInsight> Insights { get; set; } = new();
}

public class CategoryInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<string> ActionItems { get; set; } = new();
}

public class OverallBenchmark
{
    public double CompositeScore { get; set; }
    public string Grade { get; set; } = string.Empty;
    public List<StrengthArea> Strengths { get; set; } = new();
    public List<ImprovementArea> ImprovementAreas { get; set; } = new();
}

public class StrengthArea
{
    public string AreaName { get; set; } = string.Empty;
    public double Score { get; set; }
    public List<string> Contributors { get; set; } = new();
}

public class ImprovementArea
{
    public string AreaName { get; set; } = string.Empty;
    public double CurrentScore { get; set; }
    public double PotentialScore { get; set; }
    public List<ImprovementAction> Actions { get; set; } = new();
}

public class ImprovementAction
{
    public string ActionName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double ExpectedImpact { get; set; }
    public List<string> Prerequisites { get; set; } = new();
}

public class BenchmarkComparison
{
    public string ComparisonType { get; set; } = string.Empty;
    public string ReferenceGroup { get; set; } = string.Empty;
    public List<ComparisonMetric> Metrics { get; set; } = new();
}

public class ComparisonMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double UserValue { get; set; }
    public double ReferenceValue { get; set; }
    public List<MetricInsight> Insights { get; set; } = new();
}

public class MetricInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Implications { get; set; } = new();
}