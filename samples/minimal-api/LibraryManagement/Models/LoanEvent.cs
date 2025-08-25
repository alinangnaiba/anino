namespace LibraryManagement.Models;

public class LoanEvent
{
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = string.Empty; // Loan, Return, Renewal, Overdue
    public List<EventDetail> Details { get; set; } = new();
    public StaffMember ProcessedBy { get; set; } = new();
}
