using Books.APP.Domain;
using CORE.APP.Models;
using MediatR;

namespace Books.APP.Features.Books;

public class BookCreateRequest : IRequest<CommandResponse>
{
    public string Name { get; set; } = null!;
    public short? NumberOfPages { get; set; }
    public DateTime PublishDate { get; set; }
    public decimal Price { get; set; }
    public bool IsTopSeller { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
}

public class BookCreateHandler : IRequestHandler<BookCreateRequest, CommandResponse>
{
    private readonly BooksDb _db;

    public BookCreateHandler(BooksDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(
        BookCreateRequest request,
        CancellationToken cancellationToken)
    {
        var entity = new Book
        {
            Name          = request.Name,
            NumberOfPages = request.NumberOfPages,
            PublishDate   = request.PublishDate,
            Price         = request.Price,
            IsTopSeller   = request.IsTopSeller,
            AuthorId      = request.AuthorId,
            GenreId       = request.GenreId
        };

        _db.Books.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return Success(entity.Id);
    }

    private static CommandResponse Success(int id)
        => new CommandResponse(isSuccessful: true, message: "Success", id: id);
}