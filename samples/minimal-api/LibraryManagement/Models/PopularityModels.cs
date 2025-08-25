namespace LibraryManagement.Models;

public class PopularityMetric
{
    public string Category { get; set; } = string.Empty;
    public int TotalLoans { get; set; }
    public List<PopularItem> TopItems { get; set; } = new();
    public PopularityTrends Trends { get; set; } = new();
}

public class PopularItem
{
    public BookSummary Book { get; set; } = new();
    public int LoanCount { get; set; }
    public List<PopularityFactor> Factors { get; set; } = new();
    public DemographicBreakdown Demographics { get; set; } = new();
}

public class PopularityFactor
{
    public string FactorType { get; set; } = string.Empty;
    public double Impact { get; set; }
    public List<string> Attributes { get; set; } = new();
}

public class DemographicBreakdown
{
    public Dictionary<string, int> AgeGroups { get; set; } = new();
    public List<DemographicInsight> Insights { get; set; } = new();
}

public class DemographicInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Implications { get; set; } = new();
}

public class PopularityTrends
{
    public List<TrendPeriod> Periods { get; set; } = new();
    public SeasonalityAnalysis Seasonality { get; set; } = new();
    public IEnumerable<EmergingTrend> EmergingTrends { get; set; } = new List<EmergingTrend>();
}

public class TrendPeriod
{
    public string PeriodName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<PeriodMetric> Metrics { get; set; } = new();
}

public class PeriodMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Influences { get; set; } = new();
}

public class SeasonalityAnalysis
{
    public List<SeasonalPattern> Patterns { get; set; } = new();
    public SeasonalForecast Forecast { get; set; } = new();
}

public class SeasonalPattern
{
    public string PatternName { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public double Intensity { get; set; }
    public List<PatternDriver> Drivers { get; set; } = new();
}

public class PatternDriver
{
    public string DriverType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Triggers { get; set; } = new();
}

public class SeasonalForecast
{
    public List<SeasonalPrediction> Predictions { get; set; } = new();
    public ForecastAccuracy Accuracy { get; set; } = new();
}

public class SeasonalPrediction
{
    public string Season { get; set; } = string.Empty;
    public double PredictedChange { get; set; }
    public List<string> KeyCategories { get; set; } = new();
}

public class ForecastAccuracy
{
    public double HistoricalAccuracy { get; set; }
    public List<AccuracyMetric> Metrics { get; set; } = new();
}

public class AccuracyMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Limitations { get; set; } = new();
}

public class EmergingTrend
{
    public string TrendName { get; set; } = string.Empty;
    public DateTime DetectedDate { get; set; }
    public double GrowthRate { get; set; }
    public List<TrendIndicator> Indicators { get; set; } = new();
}

public class TrendIndicator
{
    public string IndicatorName { get; set; } = string.Empty;
    public double Strength { get; set; }
    public List<string> Manifestations { get; set; } = new();
}
