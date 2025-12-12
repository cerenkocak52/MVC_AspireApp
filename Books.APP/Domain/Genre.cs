using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace Books.APP.Domain
{
    public class Genre : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = default!;

        // Many-to-many back reference to books
        public List<BookGenre> BookGenres { get; set; } = new();
    }
}