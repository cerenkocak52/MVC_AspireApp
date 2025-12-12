using System.Threading;
using System.Threading.Tasks;
using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserDeleteRequest : Request, IRequest<CommandResponse>
    {
        public int Id { get; set; }
    }

    public class UserDeleteHandler : IRequestHandler<UserDeleteRequest, CommandResponse>
    {
        private readonly UsersDb _db;

        public UserDeleteHandler(UsersDb db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
                return NotFound(request.Id);

            _db.Users.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success(request.Id);
        }

        private static CommandResponse Success(int id)
            => new CommandResponse(isSuccessful: true, message: "User deleted successfully.", id: id);

        private static CommandResponse NotFound(int id)
            => new CommandResponse(isSuccessful: false, message: "User not found.", id: id);
    }
}