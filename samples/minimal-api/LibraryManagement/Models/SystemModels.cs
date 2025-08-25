namespace LibraryManagement.Models;

public class EventDetail
{
    public string DetailType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<string> Notes { get; set; } = new();
}

public class StaffMember
{
    public string EmployeeId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}

public class LoanContext
{
    public string Channel { get; set; } = string.Empty; // In-person, Online, Mobile
    public LibraryBranch Branch { get; set; } = new();
    public List<SystemFlag> Flags { get; set; } = new();
    public TransactionMetadata Metadata { get; set; } = new();
}

public class SystemFlag
{
    public string FlagType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<string> Implications { get; set; } = new();
}

public class TransactionMetadata
{
    public string TransactionId { get; set; } = string.Empty;
    public Dictionary<string, object> SystemData { get; set; } = new();
    public List<AuditEntry> AuditTrail { get; set; } = new();
}

public class AuditEntry
{
    public DateTime Timestamp { get; set; }
    public string Action { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public List<FieldChange> Changes { get; set; } = new();
}

public class FieldChange
{
    public string FieldName { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public List<string> Validators { get; set; } = new();
}
