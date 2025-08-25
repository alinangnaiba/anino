public class BranchInventory
{
    public LibraryBranch Branch { get; set; } = new();
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public Dictionary<string, int> BooksByGenre { get; set; } = new();
    public List<BookSummary> PopularBooks { get; set; } = new();
    public BranchActivity RecentActivity { get; set; } = new();
}
