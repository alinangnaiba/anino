namespace LibraryManagement.Models;

public class LoanDetail
{
    public BookSummary Book { get; set; } = new();
    public Member Member { get; set; } = new();
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public List<LoanEvent> Events { get; set; } = new();
    public LoanContext Context { get; set; } = new();
}
