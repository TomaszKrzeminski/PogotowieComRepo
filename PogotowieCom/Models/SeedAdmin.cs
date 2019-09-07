using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class SeedAdmin
    {
        //public static void EnsurePopulated(IApplicationBuilder app)
        public static void EnsurePopulated(AppIdentityDbContext context)
        {
            using (context)
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();


                async void SeedAdminUser()
                {
                    var user = new AppUser
                    {
                        UserName = "Tomek",
                        Surname = "Krzemiński",
                        City = "Świecie",
                        ZipCode = "86-100",
                        NormalizedUserName = "Tomek",
                        Email = "tomek@example.com",
                        NormalizedEmail = "tomek@example.com",
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var roleStore = new RoleStore<IdentityRole>(context);

                    if (!context.Roles.Any(r => r.Name == "Administrator"))
                    {
                        await roleStore.CreateAsync(new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" });
                    }
                    if (!context.Roles.Any(r => r.Name == "PatientRole"))
                    {
                        await roleStore.CreateAsync(new IdentityRole { Name = "PatientRole", NormalizedName = "PatientRole" });
                    }
                    if (!context.Roles.Any(r => r.Name == "DoctorRole"))
                    {
                        await roleStore.CreateAsync(new IdentityRole { Name = "DoctorRole", NormalizedName = "DoctorRole" });
                    }

                    if (!context.Users.Any(u => u.UserName == user.UserName))
                    {
                        var password = new PasswordHasher<AppUser>();
                        var hashed = password.HashPassword(user, "Sekret123@");
                        user.PasswordHash = hashed;
                        var userStore = new UserStore<AppUser>(context);
                        await userStore.CreateAsync(user);
                        await userStore.AddToRoleAsync(user, "Administrator");
                    }

                    await context.SaveChangesAsync();
                }


                SeedAdminUser();



                context.SaveChanges();
            }

              
        }
    }
}

