using Books.APP.Domain;
using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Books.APP.Features.Books;

public class BookUpdateRequest : IRequest<CommandResponse>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public short? NumberOfPages { get; set; }
    public DateTime PublishDate { get; set; }
    public decimal Price { get; set; }
    public bool IsTopSeller { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
}

public class BookUpdateHandler : IRequestHandler<BookUpdateRequest, CommandResponse>
{
    private readonly BooksDb _db;

    public BookUpdateHandler(BooksDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(
        BookUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var entity = await _db.Books
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return NotFound(request.Id);

        entity.Name          = request.Name;
        entity.NumberOfPages = request.NumberOfPages;
        entity.PublishDate   = request.PublishDate;
        entity.Price         = request.Price;
        entity.IsTopSeller   = request.IsTopSeller;
        entity.AuthorId      = request.AuthorId;
        entity.GenreId       = request.GenreId;

        await _db.SaveChangesAsync(cancellationToken);

        return Success(entity.Id);
    }
    private static CommandResponse Success(int id)
        => new CommandResponse(isSuccessful: true, message: "Success", id: id);

    private static CommandResponse NotFound(int id)
        => new CommandResponse(isSuccessful: false, message: "Book not found.", id: id);
}