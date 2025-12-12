using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Roles;

public class RoleDeleteRequest : Request, IRequest<CommandResponse>
{
    public int Id { get; set; }
}

public class RoleDeleteHandler : IRequestHandler<RoleDeleteRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public RoleDeleteHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(RoleDeleteRequest request, CancellationToken cancellationToken)
    {
        var entity = await _db.Roles
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return NotFound(request.Id);

        _db.Roles.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return Success(request.Id);
    }

    private static CommandResponse Success(int id)
        => new(true, "Role deleted successfully.", id);

    private static CommandResponse NotFound(int id)
        => new(false, "Role not found.", id);
}