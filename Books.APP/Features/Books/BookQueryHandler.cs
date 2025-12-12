using CORE.APP.Models;
using CORE.APP.Services;
using Books.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Books.APP.Features.Books
{
    public class BookQueryRequest : Request, IRequest<IQueryable<BookQueryResponse>>
    {
    }
    public class BookQueryResponse : Response
    {
        public string Name { get; set; } = null!;
        public short? NumberOfPages { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal Price { get; set; }
        public bool IsTopSeller { get; set; }

        public string AuthorName { get; set; } = null!;
        public string GenreName  { get; set; } = null!;
    }

    public class BookQueryHandler
        : Service<Book>, IRequestHandler<BookQueryRequest, IQueryable<BookQueryResponse>>
    {
        public BookQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Book> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                       .Include(b => b.Author)
                       .Include(b => b.Genre)
                       .OrderBy(b => b.Name);
        }

        public Task<IQueryable<BookQueryResponse>> Handle(
            BookQueryRequest request,
            CancellationToken cancellationToken)
        {
            var query =
                Query().Select(b => new BookQueryResponse
                {
                    Id          = b.Id,
                    Guid        = b.Guid,
                    Name        = b.Name,
                    NumberOfPages = b.NumberOfPages,
                    PublishDate = b.PublishDate,
                    Price       = b.Price,
                    IsTopSeller = b.IsTopSeller,
                    AuthorName  = b.Author.FirstName + " " + b.Author.LastName,
                    GenreName   = b.Genre.Name
                });

            return Task.FromResult(query);
        }
    }
}