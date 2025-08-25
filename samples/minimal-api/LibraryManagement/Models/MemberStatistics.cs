public class MemberStatistics
{
    public int TotalBooksLoaned { get; set; }
    public int BooksCurrentlyLoaned { get; set; }
    public double AverageRating { get; set; }
    public string FavoriteGenre { get; set; } = string.Empty;
    public decimal LateFees { get; set; }
}
