namespace LibraryManagement.Models;

public class CirculationMetrics
{
    public TotalCirculationData TotalData { get; set; } = new();
    public List<CirculationTrend> Trends { get; set; } = new();
    public Dictionary<string, PopularityMetric> PopularityByCategory { get; set; } = new();
    public SeasonalAnalysis SeasonalPatterns { get; set; } = new();
}
