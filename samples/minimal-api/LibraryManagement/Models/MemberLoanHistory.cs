public class MemberLoanHistory
{
    public Member Member { get; set; } = new();
    public List<BookLoan> ActiveLoans { get; set; } = new();
    public List<CompletedLoan> LoanHistory { get; set; } = new();
    public MemberStatistics Statistics { get; set; } = new();
}
