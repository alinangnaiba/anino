namespace LibraryManagement.Models;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateOnly MembershipDate { get; set; }
    public string MembershipType { get; set; } = "Standard";
}
