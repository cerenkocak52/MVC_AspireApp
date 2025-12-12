using CORE.APP.Models;
using MediatR;
using Users.APP.Domain;

namespace Users.APP.Features.Roles;

public class RoleCreateRequest : Request, IRequest<CommandResponse>
{
    public string Name { get; set; } = null!;
}

public class RoleCreateHandler : IRequestHandler<RoleCreateRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public RoleCreateHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(RoleCreateRequest request, CancellationToken cancellationToken)
    {
        var entity = new Role
        {
            Name = request.Name.Trim()
        };

        _db.Roles.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return Success(entity.Id);
    }

    private static CommandResponse Success(int id)
        => new(true, "Role created successfully.", id);
}