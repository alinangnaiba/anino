namespace LibraryManagement.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public DateOnly PublishedDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Pages { get; set; }
    public bool Available { get; set; }
    public List<Author> Authors { get; set; } = new();
    public LibraryBranch Branch { get; set; } = new();
    public BookLoan? CurrentLoan { get; set; }
}
