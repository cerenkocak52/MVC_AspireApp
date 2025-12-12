using CORE.APP.Models;
using MediatR;
using Users.APP.Domain;

namespace Users.APP.Features.Groups;

public class GroupCreateRequest : IRequest<CommandResponse>
{
    public string Title { get; set; } = null!;
}

public class GroupCreateHandler : IRequestHandler<GroupCreateRequest, CommandResponse>
{
    private readonly UsersDb _db;

    public GroupCreateHandler(UsersDb db)
    {
        _db = db;
    }

    public async Task<CommandResponse> Handle(GroupCreateRequest request, CancellationToken cancellationToken)
    {
        var entity = new Group
        {
            Title = request.Title.Trim()
        };

        _db.Groups.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return Success(entity.Id);
    }

    private static CommandResponse Success(int id)
        => new CommandResponse(isSuccessful: true, message: "Group created successfully.", id);
}