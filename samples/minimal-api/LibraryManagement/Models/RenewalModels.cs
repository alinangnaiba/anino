namespace LibraryManagement.Models;

public class RenewalStatistics
{
    public int TotalRenewals { get; set; }
    public List<RenewalPattern> Patterns { get; set; } = new();
    public IEnumerable<RenewalAnalysis> Analysis { get; set; } = new List<RenewalAnalysis>();
}

public class RenewalPattern
{
    public string PatternType { get; set; } = string.Empty;
    public Dictionary<string, double> Metrics { get; set; } = new();
    public List<PatternObservation> Observations { get; set; } = new();
}

public class PatternObservation
{
    public string ObservationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SupportingData> Evidence { get; set; } = new();
}

public class SupportingData
{
    public string DataType { get; set; } = string.Empty;
    public Dictionary<string, object> Values { get; set; } = new();
    public List<string> Sources { get; set; } = new();
}

public class RenewalAnalysis
{
    public string AnalysisType { get; set; } = string.Empty;
    public double Score { get; set; }
    public List<RenewalFactor> Factors { get; set; } = new();
    public PredictiveModel Predictions { get; set; } = new();
}

public class RenewalFactor
{
    public string FactorName { get; set; } = string.Empty;
    public double Weight { get; set; }
    public List<FactorComponent> Components { get; set; } = new();
}

public class FactorComponent
{
    public string ComponentName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Influences { get; set; } = new();
}
