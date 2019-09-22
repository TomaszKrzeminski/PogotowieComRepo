using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class CreateUserModel
    {
        [Required]
        public string ChooseRole { get; set; }

        [Required]
        public string Name { get; set; }

        //[Required]
        public string Surname { get; set; }

        //[Required]
        public string City { get; set; }

        //[Required]
        public string ZipCode { get; set; }

        [Required]
        public string Email { get; set; }

        //[Required]
        public string PhoneNumber { get; set; }


        [Required]
        public string Password { get; set; }
    }

    public class CreateDoctorModel:CreateUserModel
    {
        public decimal PriceForVisit { get; set; }
    }

    public class CreatePatientModel:CreateUserModel
    {
        
    }

    public class LoginModel
    {
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }

    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string [] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }




}
