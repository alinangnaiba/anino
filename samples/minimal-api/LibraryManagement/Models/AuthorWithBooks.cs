public class AuthorWithBooks : Author
{
    public List<BookSummary> Books { get; set; } = new();
    public AuthorStatistics Statistics { get; set; } = new();
}
