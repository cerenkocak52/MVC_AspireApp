using CORE.APP.Models;
using MediatR;
using Users.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace Users.APP.Features.Tokens;

public class TokenCreateRequest : Request, IRequest<CommandResponse>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class TokenCreateHandler : IRequestHandler<TokenCreateRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public TokenCreateHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(TokenCreateRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.UserName == request.UserName && x.Password == request.Password, cancellationToken);

        if (user == null)
            return new CommandResponse(false, "Invalid username or password.");

        // Generate refresh token
        user.RefreshToken = Guid.NewGuid().ToString();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        await _db.SaveChangesAsync(cancellationToken);

        return new CommandResponse(true, "Token generated successfully.", user.Id);
    }
}