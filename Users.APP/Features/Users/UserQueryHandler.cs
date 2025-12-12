using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CORE.APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    public class UserQueryResponse : Response
    {
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName  { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }

        public decimal Score  { get; set; }
        public bool IsActive  { get; set; }

        public string? Address { get; set; }

        public int? CountryId { get; set; }
        public int? CityId    { get; set; }
        public int? GroupId   { get; set; }

        public List<int> RoleIds { get; set; } = new();
    }

    public class UserQueryHandler : IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        private readonly UsersDb _db;

        public UserQueryHandler(UsersDb db)
        {
            _db = db;
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var query =
                _db.Users
                    .Include(u => u.UserRoles)
                    .OrderBy(u => u.UserName)
                    .Select(u => new UserQueryResponse
                    {
                        Id   = u.Id,
                        Guid = u.Guid,

                        UserName = u.UserName,
                        FirstName = u.FirstName,
                        LastName  = u.LastName,

                        Gender = u.Gender,

                        BirthDate        = u.BirthDate,
                        RegistrationDate = u.RegistrationDate,

                        Score    = u.Score,
                        IsActive = u.IsActive,

                        Address  = u.Address,
                        CountryId = u.CountryId,
                        CityId    = u.CityId,
                        GroupId   = u.GroupId,

                        RoleIds = u.UserRoles
                            .Select(ur => ur.RoleId)
                            .ToList()
                    });

            return Task.FromResult(query);
        }
    }
}