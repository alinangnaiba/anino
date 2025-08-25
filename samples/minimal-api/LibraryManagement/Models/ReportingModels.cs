namespace LibraryManagement.Models;

public class AdvancedReportingData
{
    public List<ReportMetadata> Reports { get; set; } = new();
    public DataQualityMetrics DataQuality { get; set; } = new();
    public IEnumerable<ExportOption> ExportOptions { get; set; } = new List<ExportOption>();
}

public class ReportMetadata
{
    public string ReportName { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public List<ReportSection> Sections { get; set; } = new();
    public ReportConfiguration Configuration { get; set; } = new();
}

public class ReportSection
{
    public string SectionName { get; set; } = string.Empty;
    public List<SectionContent> Content { get; set; } = new();
    public SectionSummary Summary { get; set; } = new();
}

public class SectionContent
{
    public string ContentType { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
    public List<string> Annotations { get; set; } = new();
}

public class SectionSummary
{
    public List<KeyFinding> KeyFindings { get; set; } = new();
    public Dictionary<string, double> Metrics { get; set; } = new();
}

public class KeyFinding
{
    public string FindingType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Impact { get; set; }
    public List<string> SupportingEvidence { get; set; } = new();
}

public class ReportConfiguration
{
    public Dictionary<string, object> Parameters { get; set; } = new();
    public List<ConfigurationOption> Options { get; set; } = new();
}

public class ConfigurationOption
{
    public string OptionName { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<string> Alternatives { get; set; } = new();
}

public class DataQualityMetrics
{
    public double OverallQuality { get; set; }
    public List<QualityDimension> Dimensions { get; set; } = new();
    public IEnumerable<QualityIssue> Issues { get; set; } = new List<QualityIssue>();
}

public class QualityDimension
{
    public string DimensionName { get; set; } = string.Empty;
    public double Score { get; set; }
    public List<QualityMeasure> Measures { get; set; } = new();
}

public class QualityMeasure
{
    public string MeasureName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Criteria { get; set; } = new();
}

public class QualityIssue
{
    public string IssueType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ResolutionStep> Resolution { get; set; } = new();
}

public class ResolutionStep
{
    public string StepDescription { get; set; } = string.Empty;
    public string ResponsibleRole { get; set; } = string.Empty;
    public List<string> Resources { get; set; } = new();
}

public class ExportOption
{
    public string ExportType { get; set; } = string.Empty;
    public List<ExportFormat> Formats { get; set; } = new();
    public ExportConfiguration Configuration { get; set; } = new();
}

public class ExportFormat
{
    public string FormatName { get; set; } = string.Empty;
    public List<FormatOption> Options { get; set; } = new();
}

public class FormatOption
{
    public string OptionName { get; set; } = string.Empty;
    public string DefaultValue { get; set; } = string.Empty;
    public List<string> PossibleValues { get; set; } = new();
}

public class ExportConfiguration
{
    public Dictionary<string, object> Settings { get; set; } = new();
    public List<ExportFilter> Filters { get; set; } = new();
}

public class ExportFilter
{
    public string FilterType { get; set; } = string.Empty;
    public Dictionary<string, object> Criteria { get; set; } = new();
}
