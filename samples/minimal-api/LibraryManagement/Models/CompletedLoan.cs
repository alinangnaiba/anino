public class CompletedLoan
{
    public string BookTitle { get; set; } = string.Empty;
    public DateOnly LoanDate { get; set; }
    public DateOnly ReturnDate { get; set; }
    public int? Rating { get; set; }
}
