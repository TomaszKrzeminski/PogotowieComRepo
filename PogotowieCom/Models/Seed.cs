using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class Seed
    {
        //public static void EnsurePopulated(IApplicationBuilder app)
        public static void EnsurePopulated(AppIdentityDbContext context)
        {
            using (context)
            {

                //context.Database.Migrate();


                if (!context.Database.EnsureCreated())
                {
                    SeedAdminUser();
                    SeedSpecializations();
                    SeedTags();
                    SeedSpecializationsToTags();
                }


                void SeedSpecializations()
                {
                    if (context.Specializations.Any() == false)
                    {
                        List<string> list = new List<string>() { "Ginekolog", "Stomatolog", "Ortopeda", "Chirurg", "Dermatolog", "Psychiatra", "Psycholog", "Internista", "Laryngolog", "Okulista", "Neurolog", "Fizjoterapeuta", "Urolog", "Sexuolog", "Alergolog", "Ortopeda", "Chirurg Szczękowy", "Lekarz Sportowy" };

                        foreach (var item in list)
                        {
                            try
                            {


                                Specialization specialization = new Specialization() { Name = item };
                                context.Specializations.Add(specialization);
                                context.SaveChanges();


                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }




                }

                void SeedTags()
                {

                    List<string> AilmentList = new List<string>() {
"bezpłodność ",
"bezsenność ",
"białkomocz ",
"biegunka ",
"ból brzucha ",
"bóle mięśni ",
"bóle stawów ",
"ból gardła ",
"ból głowy ",
"ból twarzy ",
"ból ucha ",
"ból w klatce piersiowej ",
"ból zęba ",
"brak apetytu ",
"brzydki zapach ",
"chrapanie ",
"chrypka ",
"chudnięcie ",
"częste oddawanie moczu ",
"depresja ",
"drgawki ",
"drżenie rąk ",
"duszność ",
"dysfagia ",
"gorączka ",
"guz ",
"hiperglikemia ",
"infekcje ",
"kaszel ",
"katar sienny ",
"kichanie ",
"kołatanie serca ",
"kolka ",
"krew w kale ",
"krew w moczu ",
"krwawienia międzymiesiączkowe ",
"krwawienie ",
"krwawienie z nosa ",
"krwioplucie ",
"łamliwość włosów ",
"lęk ",
"łzawienie oczu ",
"migrena ",
"mroczki przed oczami ",
"mrużenie oczu ",
"nadciśnienie ",
"nadmierny apetyt ",
"napięcie mięśni ",
"nerwowość ",
"niedotlenienie ",
"niedowład ",
"niedożywienie ",
"nieostre widzenie ",
"niepłodność ",
"niestrawność ",
"niewydolność nerek ",
"nieżyt nosa ",
"nudności ",
"obrzęk ",
"obrzęk jądra ",
"odwodnienie ",
"omamy ",
"omdlenia ",
"opryszczka ",
"osłabienie ",
"osteoporoza ",
"otępienie ",
"owrzodzenie ",
"pobudzenie ",
"podwójne widzenie ",
"pokrzywka ",
"potliwość ",
"powiększenie śledziony ",
"powiększenie wątroby ",
"rozdrażnienie ",
"rozstępy ",
"rumień ",
"senność ",
"sinica ",
"skurcz mięśni ",
"smutek ",
"spadek libido ",
"stan zapalny ",
"sucha skóra ",
"swędzenie ",
"świąd ",
"trudności w oddychaniu ",
"uczulenie ",
"upławy ",
"urojenia ",
"utrata przytomności ",
"utrata wagi ",
"wrzody ",
"wydzielina z nosa ",
"wymioty ",
"wysypka ",
"wzdęcia ",
"zaburzenia miesiączkowania ",
"zaburzenia mowy ",
"zaburzenia słuchu ",
"zaburzenia widzenia ",
"zaburzenia wzwodu ",
"zaczerwienienie ",
"zakażenie",
"zakrzepica ",
"zaparcia ",
"zatkany nos ",
"zawał ",
"zawał płuca ",
"zawroty głowy ",
"zgaga ",
"złe samopoczucie ",
"zmęczenie ",
" żółtaczka" };


                    foreach (var item in AilmentList)
                    {

                        try
                        {


                            Tag tag = new Tag() { Text = item };
                            context.Tags.Add(tag);
                            context.SaveChanges();


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                void SeedSpecializationsToTags()
                {

                    List<string> list = new List<string>() { "Ginekolog", "Stomatolog", "Ortopeda", "Chirurg", "Dermatolog", "Psychiatra", "Psycholog", "Internista", "Laryngolog", "Okulista", "Neurolog", "Fizjoterapeuta", "Urolog", "Sexuolog", "Alergolog",  "Chirurg Szczękowy", "Lekarz Sportowy" };


                    try
                    {

                        foreach (var item in list)
                        {
                            CreateTagsToSpecialization(item, "złe samopoczucie ");

                        }
                        CreateTagsToSpecialization("Ginekolog", "bezpłodność ");
                        CreateTagsToSpecialization("Ginekolog", "niepłodność ");


                        CreateTagsToSpecialization("Stomatolog", "ból zęba ");


                        CreateTagsToSpecialization("Ortopeda", "bóle mięśni ");
                        CreateTagsToSpecialization("Ortopeda", "napięcie mięśni ");




                    }
                    catch (Exception ex)
                    {

                    }
                }


                void CreateTagsToSpecialization(string SpecializationName, string TagName)
                {


                    Tag tag = context.Tags.Where(n => n.Text == TagName).First();
                    Specialization specialization = context.Specializations.Where(s => s.Name == SpecializationName).First();
                    TagSpecialization tagspecialization = new TagSpecialization() { Tag = tag, Specialization = specialization };
                    specialization.TagSpecializations.Add(tagspecialization);

                    context.SaveChanges();




                }

                void SeedAdminUser()
                {

                    try
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
                            roleStore.CreateAsync(new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" });
                        }
                        if (!context.Roles.Any(r => r.Name == "Pacjent"))
                        {
                            roleStore.CreateAsync(new IdentityRole { Name = "Pacjent", NormalizedName = "Pacjent" });
                        }
                        if (!context.Roles.Any(r => r.Name == "Doktor"))
                        {
                            roleStore.CreateAsync(new IdentityRole { Name = "Doktor", NormalizedName = "Doktor" });
                        }

                        if (!context.Users.Any(u => u.UserName == user.UserName))
                        {
                            var password = new PasswordHasher<AppUser>();
                            var hashed = password.HashPassword(user, "Sekret123@");
                            user.PasswordHash = hashed;
                            var userStore = new UserStore<AppUser>(context);
                            userStore.CreateAsync(user);
                            userStore.AddToRoleAsync(user, "Administrator");
                        }

                        context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }

                }







                //context.SaveChanges();
            }


        }
    }
}
