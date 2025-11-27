using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CORE.Domain;

namespace Users.APP.Domain;

public class User : Entity
{
    [Required, StringLength(30)]
    public string UserName { get; set; } // Reference Type: null can't be assigned since Required is used

    [Required, StringLength(15)]
    public string Password { get; set; } // Reference Type: null can't be assigned since Required is used

    [StringLength(50)]
    public string FirstName { get; set; } // Reference Type: null can be assigned, default value is null if no assignment

    [StringLength(50)]
    public string LastName { get; set; } // Reference Type: null can be assigned, default value is null if no assignment

    public Genders Gender { get; set; } // Value Type: null can't be assigned, default value is the first element's value if no assignment

    public DateTime? BirthDate { get; set; } // Value Type: null can be assigned since ? is used, default value is null if no assignment

    public DateTime RegistrationDate { get; set; } // Value Type: null can't be assigned, default value is

    public decimal Score { get; set; } // Value Type: null can't be assigned, default value is 0.0M if no assignment

    public bool IsActive { get; set; } // Value Type: null can't be assigned, default value is false if no assignment

    public string Address { get; set; } // Reference Type: null can be assigned, default value is null if no assignment

    public int? CountryId { get; set; } // Value Type: null can be assigned since ? is used, default value is null if no assignment

    public int? CityId { get; set; } // Value Type: null can be assigned since ? is used, default value is null if no assignment
    public int? GroupId { get; set; } // foreign key that references to the Groups table's Id primary key,
    // Value Type: null can be assigned since ? is used, default value is null if no assignment,
    // change int? to int if each user must have a group

    public Group Group { get; set; } // navigation property for retrieving related Group entity data of the User entity data in queries,
    // Reference Type: null can be assigned, default reference is null if no assignment



    // for users-roles many to many relationship
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>(); // navigation property for retrieving related UserRole
    // entities data of the User entity data in queries,
    // initialized for preventing null reference exception

    [NotMapped] // no column in the Users table will be created for this property since NotMapped attribute is defined
    public List<int> RoleIds // helps to easily manage the UserRoles relational entities by Role Id values
    {
        // returns the Role Id values of the User entity
        get => UserRoles.Select(userRoleEntity => userRoleEntity.RoleId).ToList();

        // sets the UserRoles relational entities of the User entity by the assigned Role Id values
        set => UserRoles = value.Select(roleId => new UserRole() { RoleId = roleId }).ToList();
    }

    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
}