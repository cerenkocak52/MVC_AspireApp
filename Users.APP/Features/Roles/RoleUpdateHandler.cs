using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Roles;

public class RoleUpdateRequest : Request, IRequest<CommandResponse>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class RoleUpdateHandler : IRequestHandler<RoleUpdateRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public RoleUpdateHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(RoleUpdateRequest request, CancellationToken cancellationToken)
    {
        var entity = await _db.Roles
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return NotFound(request.Id);

        entity.Name = request.Name.Trim();

        await _db.SaveChangesAsync(cancellationToken);

        return Success(entity.Id);
    }

    private static CommandResponse Success(int id)
        => new(true, "Role updated successfully.", id);

    private static CommandResponse NotFound(int id)
        => new(false, "Role not found.", id);
}