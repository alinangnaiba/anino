namespace LibraryManagement.Models;

public class TotalCirculationData
{
    public int TotalLoans { get; set; }
    public int TotalReturns { get; set; }
    public List<LoanDetail> RecentLoans { get; set; } = new();
    public RenewalStatistics Renewals { get; set; } = new();
}
