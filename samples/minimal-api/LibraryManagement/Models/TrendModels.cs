namespace LibraryManagement.Models;

public class CirculationTrend
{
    public string TrendName { get; set; } = string.Empty;
    public List<TrendDataPoint> DataPoints { get; set; } = new();
    public TrendAnalysis Analysis { get; set; } = new();
    public IEnumerable<TrendForecast> Forecasts { get; set; } = new List<TrendForecast>();
}

public class TrendDataPoint
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public List<DataPointContext> Context { get; set; } = new();
}

public class DataPointContext
{
    public string ContextType { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

public class TrendAnalysis
{
    public string Direction { get; set; } = string.Empty;
    public double Velocity { get; set; }
    public List<TrendInsight> Insights { get; set; } = new();
    public StatisticalSummary Statistics { get; set; } = new();
}

public class TrendInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public double Confidence { get; set; }
    public List<InsightEvidence> Evidence { get; set; } = new();
}

public class InsightEvidence
{
    public string EvidenceType { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
    public List<string> Sources { get; set; } = new();
}

public class StatisticalSummary
{
    public double Mean { get; set; }
    public double Median { get; set; }
    public double StandardDeviation { get; set; }
    public List<OutlierAnalysis> Outliers { get; set; } = new();
}

public class OutlierAnalysis
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public string Classification { get; set; } = string.Empty;
    public List<OutlierCause> PossibleCauses { get; set; } = new();
}

public class OutlierCause
{
    public string CauseType { get; set; } = string.Empty;
    public double Likelihood { get; set; }
    public List<string> Indicators { get; set; } = new();
}

public class TrendForecast
{
    public DateTime ForecastDate { get; set; }
    public double ForecastValue { get; set; }
    public double ConfidenceInterval { get; set; }
    public List<ForecastAssumption> Assumptions { get; set; } = new();
}

public class ForecastAssumption
{
    public string AssumptionType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> RiskFactors { get; set; } = new();
}
