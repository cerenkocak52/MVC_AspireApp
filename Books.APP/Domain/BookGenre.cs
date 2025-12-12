namespace Books.APP.Domain
{
    // Join table between Book and Genre
    public class BookGenre
    {
        public int BookId { get; set; }
        public Book Book { get; set; } = default!;

        public int GenreId { get; set; }
        public Genre Genre { get; set; } = default!;
    }
}