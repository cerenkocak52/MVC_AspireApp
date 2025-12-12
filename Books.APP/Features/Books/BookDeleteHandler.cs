using Books.APP.Domain;
using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Books.APP.Features.Books;

public class BookDeleteRequest : IRequest<CommandResponse>
{
    public int Id { get; set; }
}

public class BookDeleteHandler : IRequestHandler<BookDeleteRequest, CommandResponse>
{
    private readonly BooksDb _db;

    public BookDeleteHandler(BooksDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(
        BookDeleteRequest request,
        CancellationToken cancellationToken)
    {
        var entity = await _db.Books
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return NotFound(request.Id);

        _db.Books.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return Success(request.Id);
    }

    private static CommandResponse Success(int id)
        => new CommandResponse(isSuccessful: true, message: "Success", id: id);

    private static CommandResponse NotFound(int id)
        => new CommandResponse(isSuccessful: false, message: "Book not found.", id: id);
}