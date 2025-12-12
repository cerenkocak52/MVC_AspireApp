using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserUpdateRequest : Request, IRequest<CommandResponse>
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string? FirstName { get; set; }
        public string? LastName  { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Address { get; set; }

        public int? CountryId { get; set; }
        public int? CityId    { get; set; }

        public int? GroupId   { get; set; }

        public bool IsActive  { get; set; }

        // Full replacement of role ids for this user
        public List<int> RoleIds { get; set; } = new();
    }

    public class UserUpdateHandler : IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        private readonly UsersDb _db;

        public UserUpdateHandler(UsersDb db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
                return NotFound(request.Id);

            entity.UserName = request.UserName;
            entity.Password = request.Password;

            entity.FirstName = request.FirstName;
            entity.LastName  = request.LastName;

            entity.Gender    = request.Gender;
            entity.BirthDate = request.BirthDate;

            entity.Address   = request.Address;
            entity.CountryId = request.CountryId;
            entity.CityId    = request.CityId;

            entity.GroupId   = request.GroupId;

            entity.IsActive  = request.IsActive;

            entity.RoleIds = request.RoleIds;

            await _db.SaveChangesAsync(cancellationToken);

            return Success(entity.Id);
        }

        private static CommandResponse Success(int id)
            => new CommandResponse(isSuccessful: true, message: "User updated successfully.", id: id);

        private static CommandResponse NotFound(int id)
            => new CommandResponse(isSuccessful: false, message: "User not found.", id: id);
    }
}