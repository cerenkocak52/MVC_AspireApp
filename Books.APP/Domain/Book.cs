using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace Books.APP.Domain;

public class Book : Entity
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    public short? NumberOfPages { get; set; }
    public DateTime PublishDate { get; set; }
    public decimal Price { get; set; }
    public bool IsTopSeller { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public List<BookGenre> BookGenres { get; set; } = new();
}