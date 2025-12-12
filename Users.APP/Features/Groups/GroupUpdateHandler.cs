using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Groups;

public class GroupUpdateRequest : IRequest<CommandResponse>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
}

public class GroupUpdateHandler : IRequestHandler<GroupUpdateRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public GroupUpdateHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(GroupUpdateRequest request, CancellationToken cancellationToken)
    {
        var entity = await _db.Groups
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return NotFound(request.Id);

        entity.Title = request.Title.Trim();

        await _db.SaveChangesAsync(cancellationToken);

        return Success(entity.Id);
    }

    private static CommandResponse Success(int id)
        => new CommandResponse(true, "Group updated successfully.", id);

    private static CommandResponse NotFound(int id)
        => new CommandResponse(false, "Group not found.", id);
}