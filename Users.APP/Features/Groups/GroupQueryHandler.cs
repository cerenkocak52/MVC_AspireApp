using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Groups;

public class GroupQueryRequest : Request, IRequest<IQueryable<GroupQueryResponse>>
{
}

public class GroupQueryResponse : Response
{
    public string Title { get; set; } = null!;
    public int UserCount { get; set; }
}

public class GroupQueryHandler : IRequestHandler<GroupQueryRequest, IQueryable<GroupQueryResponse>>
{
    private readonly UsersDb _db;

    public GroupQueryHandler(UsersDb db)
    {
        _db = db;
    }

    public Task<IQueryable<GroupQueryResponse>> Handle(GroupQueryRequest request, CancellationToken cancellationToken)
    {
        var query =
            _db.Groups
                .Include(g => g.Users)
                .OrderBy(g => g.Title)
                .Select(g => new GroupQueryResponse
                {
                    Id        = g.Id,      // from Response base
                    Guid      = g.Guid,    // from Response base
                    Title     = g.Title,
                    UserCount = g.Users.Count
                });

        return Task.FromResult(query);
    }
}