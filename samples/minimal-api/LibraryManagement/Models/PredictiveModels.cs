namespace LibraryManagement.Models;

public class PredictiveModel
{
    public string ModelType { get; set; } = string.Empty;
    public double Accuracy { get; set; }
    public List<ModelPrediction> Predictions { get; set; } = new();
    public ModelValidation Validation { get; set; } = new();
}

public class ModelPrediction
{
    public DateTime PredictionDate { get; set; }
    public double PredictedValue { get; set; }
    public double Confidence { get; set; }
    public List<PredictionVariable> Variables { get; set; } = new();
}

public class PredictionVariable
{
    public string VariableName { get; set; } = string.Empty;
    public double Coefficient { get; set; }
    public List<string> Assumptions { get; set; } = new();
}

public class ModelValidation
{
    public List<ValidationMetric> Metrics { get; set; } = new();
    public ValidationResults Results { get; set; } = new();
    public IEnumerable<ValidationIssue> Issues { get; set; } = new List<ValidationIssue>();
}

public class ValidationMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public string Interpretation { get; set; } = string.Empty;
    public List<string> Benchmarks { get; set; } = new();
}

public class ValidationResults
{
    public bool IsValid { get; set; }
    public List<ResultComponent> Components { get; set; } = new();
    public double OverallScore { get; set; }
}

public class ResultComponent
{
    public string ComponentName { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public List<string> Details { get; set; } = new();
}

public class ValidationIssue
{
    public string IssueType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RemediationAction> Resolution { get; set; } = new();
}

public class RemediationAction
{
    public string ActionType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; }
    public List<string> Steps { get; set; } = new();
}
