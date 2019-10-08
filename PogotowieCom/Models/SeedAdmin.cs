using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PogotowieCom.Models
//{
//    public class SeedAdmin
//    {
//        //public static void EnsurePopulated(IApplicationBuilder app)
//        public static void EnsurePopulated(AppIdentityDbContext context)
//        {
//            using (context)
//            {
//                context.Database.EnsureCreated();
//                context.Database.Migrate();

//                 void SeedSpecializations()
//                {
//                    if (context.Specializations.Any() == false )
//                    {
//                        List<string> list = new List<string>() { "Ginekolog", "Stomatolog", "Ortopeda", "Chirurg", "Dermatolog", "Psychiatra", "Psycholog", "Internista", "Laryngolog", "Okulista", "Neurolog", "Fizjoterapeuta", "Urolog", "Sexuolog", "Alergolog",  "Ortopeda", "Chirurg Szczękowy", "Lekarz Sportowy" };

//                        foreach (var item in list)
//                        {
//                            try
//                            {
                                                                                        
                               
//                                    Specialization specialization = new Specialization() { Name = item };
//                                 context.Specializations.Add(specialization);
//                                 context.SaveChanges();  
                                                             

//                            }
//                            catch (Exception ex)
//                            {
                                
//                            }
//                        }
//                    }

                    


//                }


//                 void SeedAdminUser()
//                {
//                    var user = new AppUser
//                    {
//                        UserName = "Tomek",
//                        Surname = "Krzemiński",
//                        City = "Świecie",
//                        ZipCode = "86-100",
//                        NormalizedUserName = "Tomek",
//                        Email = "tomek@example.com",
//                        NormalizedEmail = "tomek@example.com",
//                        EmailConfirmed = true,
//                        LockoutEnabled = false,
//                        SecurityStamp = Guid.NewGuid().ToString()
//                    };

//                    var roleStore = new RoleStore<IdentityRole>(context);

//                    if (!context.Roles.Any(r => r.Name == "Administrator"))
//                    {
//                         roleStore.CreateAsync(new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" });
//                    }
//                    if (!context.Roles.Any(r => r.Name == "Pacjent"))
//                    {
//                         roleStore.CreateAsync(new IdentityRole { Name = "Pacjent", NormalizedName = "Pacjent" });
//                    }
//                    if (!context.Roles.Any(r => r.Name == "Doktor"))
//                    {
//                        roleStore.CreateAsync(new IdentityRole { Name = "Doktor", NormalizedName = "Doktor" });
//                    }

//                    if (!context.Users.Any(u => u.UserName == user.UserName))
//                    {
//                        var password = new PasswordHasher<AppUser>();
//                        var hashed = password.HashPassword(user, "Sekret123@");
//                        user.PasswordHash = hashed;
//                        var userStore = new UserStore<AppUser>(context);
//                        userStore.CreateAsync(user);
//                         userStore.AddToRoleAsync(user, "Administrator");
//                    }

//                     context.SaveChangesAsync();
//                }




//                SeedAdminUser();
//                SeedSpecializations();


//                //context.SaveChanges();
//            }


//        }
//    }
//}

