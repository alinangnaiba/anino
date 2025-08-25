namespace LibraryManagement.Models;

public class MembershipAnalytics
{
    public MembershipMetrics Metrics { get; set; } = new();
    public List<MemberSegment> Segments { get; set; } = new();
    public RetentionAnalysis Retention { get; set; } = new();
    public IEnumerable<MemberBehaviorPattern> BehaviorPatterns { get; set; } = new List<MemberBehaviorPattern>();
}

public class MembershipMetrics
{
    public int TotalMembers { get; set; }
    public int NewMembers { get; set; }
    public List<MembershipGrowth> Growth { get; set; } = new();
    public MembershipDistribution Distribution { get; set; } = new();
}

public class MembershipGrowth
{
    public DateTime Period { get; set; }
    public int NewRegistrations { get; set; }
    public int Cancellations { get; set; }
    public List<GrowthFactor> Factors { get; set; } = new();
}

public class GrowthFactor
{
    public string FactorType { get; set; } = string.Empty;
    public double Impact { get; set; }
    public List<string> Details { get; set; } = new();
}

public class MembershipDistribution
{
    public Dictionary<string, int> ByType { get; set; } = new();
    public List<DistributionInsight> Insights { get; set; } = new();
}

public class DistributionInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Finding { get; set; } = string.Empty;
    public List<string> Recommendations { get; set; } = new();
}

public class MemberSegment
{
    public string SegmentName { get; set; } = string.Empty;
    public int MemberCount { get; set; }
    public List<SegmentCharacteristic> Characteristics { get; set; } = new();
    public SegmentValue Value { get; set; } = new();
}

public class SegmentCharacteristic
{
    public string CharacteristicType { get; set; } = string.Empty;
    public Dictionary<string, object> Attributes { get; set; } = new();
    public List<string> Behaviors { get; set; } = new();
}

public class SegmentValue
{
    public double LifetimeValue { get; set; }
    public List<ValueComponent> Components { get; set; } = new();
}

public class ValueComponent
{
    public string ComponentName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Drivers { get; set; } = new();
}

public class RetentionAnalysis
{
    public double RetentionRate { get; set; }
    public List<RetentionCohort> Cohorts { get; set; } = new();
    public IEnumerable<ChurnRisk> ChurnRisks { get; set; } = new List<ChurnRisk>();
}

public class RetentionCohort
{
    public string CohortName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public List<CohortMetric> Metrics { get; set; } = new();
}

public class CohortMetric
{
    public int MonthsAfterStart { get; set; }
    public double RetentionRate { get; set; }
    public List<string> Observations { get; set; } = new();
}

public class ChurnRisk
{
    public Member Member { get; set; } = new();
    public double RiskScore { get; set; }
    public List<RiskFactor> RiskFactors { get; set; } = new();
}

public class RiskFactor
{
    public string FactorName { get; set; } = string.Empty;
    public double Weight { get; set; }
    public List<string> Indicators { get; set; } = new();
}

public class MemberBehaviorPattern
{
    public string PatternName { get; set; } = string.Empty;
    public List<BehaviorMetric> Metrics { get; set; } = new();
    public PatternSignificance Significance { get; set; } = new();
}

public class BehaviorMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<MetricContext> Context { get; set; } = new();
}

public class MetricContext
{
    public string ContextType { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
}

public class PatternSignificance
{
    public double StatisticalSignificance { get; set; }
    public List<SignificanceTest> Tests { get; set; } = new();
}

public class SignificanceTest
{
    public string TestName { get; set; } = string.Empty;
    public double PValue { get; set; }
    public List<string> Assumptions { get; set; } = new();
}
