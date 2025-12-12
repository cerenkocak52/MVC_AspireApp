using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Groups;

public class GroupDeleteRequest : IRequest<CommandResponse>
{
    public int Id { get; set; }
}

public class GroupDeleteHandler : IRequestHandler<GroupDeleteRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public GroupDeleteHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(GroupDeleteRequest request, CancellationToken cancellationToken)
    {
        var entity = await _db.Groups
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return NotFound(request.Id);

        _db.Groups.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return Success(request.Id);
    }

    private static CommandResponse Success(int id)
        => new CommandResponse(true, "Group deleted successfully.", id);

    private static CommandResponse NotFound(int id)
        => new CommandResponse(false, "Group not found.", id);
}