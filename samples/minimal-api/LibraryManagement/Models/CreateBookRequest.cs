public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public DateOnly PublishedDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Pages { get; set; }
    public List<int> AuthorIds { get; set; } = new();
    public int BranchId { get; set; }
}
