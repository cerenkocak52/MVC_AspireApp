using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserCreateRequest : Request, IRequest<CommandResponse>
    {
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

        // Role ids that will populate UserRoles via the RoleIds helper
        public List<int> RoleIds { get; set; } = new();
    }

    public class UserCreateHandler : IRequestHandler<UserCreateRequest, CommandResponse>
    {
        private readonly UsersDb _db;

        public UserCreateHandler(UsersDb db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            var entity = new User
            {
                UserName = request.UserName,
                Password = request.Password,

                FirstName = request.FirstName,
                LastName  = request.LastName,

                Gender    = request.Gender,
                BirthDate = request.BirthDate,

                RegistrationDate = DateTime.UtcNow,
                Score     = 0m,
                IsActive  = true,

                Address   = request.Address,
                CountryId = request.CountryId,
                CityId    = request.CityId,
                GroupId   = request.GroupId,

                RoleIds   = request.RoleIds
            };

            await _db.Users.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return Success(entity.Id);
        }

        private static CommandResponse Success(int id)
            => new CommandResponse(isSuccessful: true, message: "User created successfully.", id: id);
    }
}