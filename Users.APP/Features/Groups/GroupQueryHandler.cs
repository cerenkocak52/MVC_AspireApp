using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Users.APP.Domain;

namespace Users.APP.Features;

public class GroupQueryRequest : Request, IRequest<IQueryable<GroupQueryResponse>>
{
}
public class GroupQueryResponse : Response
{
    public string Title { get; set; }
}

public class GroupQueryHandler : ServiceBase, IRequestHandler<GroupQueryRequest, IQueryable<GroupQueryResponse>>
{
    private readonly UsersDb _db; 
    
    public GroupQueryHandler(UsersDb db)
    {
        _db = db;
    }
    
    public Task<IQueryable<GroupQueryResponse>> Handle(GroupQueryRequest request, CancellationToken cancellationToken)
    {
        // Query the Groups DbSet and project each group entity to a GroupQueryResponse response.
        // Here, projection means mapping the values of the entity properties to the corresponding properties of the response model.

        // Way 1: types can be used with variables for declarations
        //IQueryable<GroupQueryResponse> query = Db.Groups.Select(groupEntity => new GroupQueryResponse()
        // Way 2: var can also be used therefore the type of the variable (IQueryable<GroupQueryResponse>)
        // will be known dynamically if an assignment is provided, if no assignment, types must be used
        var query = _db.Groups.Select(groupEntity => new GroupQueryResponse()
        {
            Id = groupEntity.Id,         // Maps the entity's integer ID.
            Guid = groupEntity.Guid,     // Maps the entity's GUID.
            Title = groupEntity.Title    // Maps the entity's title to the response.
        });

        // Return the query as a Task result for MediatR compatibility.
        return Task.FromResult(query);
    }
}

