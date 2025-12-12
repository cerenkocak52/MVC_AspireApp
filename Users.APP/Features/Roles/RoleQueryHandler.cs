using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Users.APP.Domain;

namespace Users.APP.Features.Roles;

public class RoleQueryRequest : Request, IRequest<IQueryable<RoleQueryResponse>>
{
}

public class RoleQueryResponse : Response
{
    public string Name { get; set; } = null!;
}

public class RoleQueryHandler 
    : Service<Role>, IRequestHandler<RoleQueryRequest, IQueryable<RoleQueryResponse>>
{
    public RoleQueryHandler(UsersDb db) : base(db)
    {
    }

    public Task<IQueryable<RoleQueryResponse>> Handle(RoleQueryRequest request, CancellationToken cancellationToken)
    {
        var query = Query()
            .OrderBy(r => r.Name)
            .Select(r => new RoleQueryResponse
            {
                Id = r.Id,
                Guid = r.Guid,
                Name = r.Name
            });

        return Task.FromResult(query);
    }
}