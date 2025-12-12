using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace Books.APP.Domain
{
    public class Book : Entity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = default!;

        public short? NumberOfPages { get; set; }

        public DateTime PublishDate { get; set; }

        public decimal Price { get; set; }

        public bool IsTopSeller { get; set; }

        // Author (many books -> one author)
        public int AuthorId { get; set; }
        public Author Author { get; set; } = default!;

        // Many-to-many with Genre through BookGenre
        public List<BookGenre> BookGenres { get; set; } = new();
        
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}