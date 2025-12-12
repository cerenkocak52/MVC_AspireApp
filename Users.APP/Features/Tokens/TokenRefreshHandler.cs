using CORE.APP.Models;
using MediatR;
using Users.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace Users.APP.Features.Tokens;

public class TokenRefreshRequest : Request, IRequest<CommandResponse>
{
    public string RefreshToken { get; set; } = null!;
}

public class TokenRefreshHandler : IRequestHandler<TokenRefreshRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public TokenRefreshHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(TokenRefreshRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken, cancellationToken);

        if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
            return new CommandResponse(false, "Invalid or expired refresh token.");

        user.RefreshToken = Guid.NewGuid().ToString();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        await _db.SaveChangesAsync(cancellationToken);
        return new CommandResponse(true, "Token refreshed successfully.", user.Id);
    }
}