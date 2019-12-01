using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class SeedAll
    {

        AppIdentityDbContext context;

        public SeedAll(AppIdentityDbContext context)
        {
            this.context = context;
        }


        public void EnsurePopulated(AppIdentityDbContext context)
        {



            bool check;
            check = context.Database.EnsureCreated();



            if (check)
            {



                SeedAdminAndRoles();

                SeedPatient1();

                SeedDoctor1();

                SeedDoctor2();

                SeedDoctor3();

                AddDoctorAndPatientToUser();

                SeedPlaces();

                SeedSpecializations();

                SeedTags();

                SeedSpecializationsToTags();

                SeddDoctroWithSpecializations();

                SeedAppointments();




            }



        }

        void SeddDoctroWithSpecializations()
        {
            try
            {
                AddSpecializationToDoctor(1, "Stomatolog");
                AddSpecializationToDoctor(2, "Stomatolog");
                AddSpecializationToDoctor(3, "Stomatolog");
            }
            catch (Exception ex)
            {

            }




        }
        void SeedDoctor1()
        {


            try
            {


                var doctor1 = new AppUser
                {
                    UserName = "DoctorStom1",
                    Surname = "Kowalski1",
                    City = "Świecie",
                    ZipCode = "86-100",
                    NormalizedUserName = "doctor1",
                    Email = "doctor1@example.com",
                    NormalizedEmail = "doctor1@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                if (!context.Users.Any(u => u.UserName == doctor1.UserName))
                {
                    var password = new PasswordHasher<AppUser>();
                    var hashed = password.HashPassword(doctor1, "Sekret123@");
                    doctor1.PasswordHash = hashed;

                    UserStore<AppUser> userStore;


                    userStore = new UserStore<AppUser>(context);




                    userStore.CreateAsync(doctor1).Wait();


                    userStore.AddToRoleAsync(doctor1, "Doktor").Wait();
                }




                context.SaveChanges();

            }
            catch (Exception ex)
            {

            }






        }

        void SeedDoctor2()
        {


            try
            {


                var doctor2 = new AppUser
                {
                    UserName = "DoctorStom2",
                    Surname = "Kowalski2",
                    City = "Świecie",
                    ZipCode = "86-100",
                    NormalizedUserName = "doctor2",
                    Email = "doctor2@example.com",
                    NormalizedEmail = "doctor2@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };


                if (!context.Users.Any(u => u.UserName == doctor2.UserName))
                {
                    var password = new PasswordHasher<AppUser>();
                    var hashed = password.HashPassword(doctor2, "Sekret123@");
                    doctor2.PasswordHash = hashed;
                    UserStore<AppUser> userStore;



                    userStore = new UserStore<AppUser>(context);


                    userStore.CreateAsync(doctor2).Wait();

                    userStore.AddToRoleAsync(doctor2, "Doktor").Wait();
                }




                context.SaveChanges();







            }
            catch (Exception ex)
            {

            }








        }

        void SeedDoctor3()
        {




            try
            {

                var doctor3 = new AppUser
                {
                    UserName = "DoctorStom3",
                    Surname = "Kowalski3",
                    City = "Świecie",
                    ZipCode = "86-100",
                    NormalizedUserName = "doctor3",
                    Email = "doctor3@example.com",
                    NormalizedEmail = "doctor3@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                if (!context.Users.Any(u => u.UserName == doctor3.UserName))
                {
                    var password = new PasswordHasher<AppUser>();
                    var hashed = password.HashPassword(doctor3, "Sekret123@");
                    doctor3.PasswordHash = hashed;

                    UserStore<AppUser> userStore;

                    userStore = new UserStore<AppUser>(context);



                    userStore.CreateAsync(doctor3).Wait();

                    userStore.AddToRoleAsync(doctor3, "Doktor").Wait();
                }


                context.SaveChanges();







            }
            catch (Exception ex)
            {

            }

        }             

        void SeedPatient1()
        {




            try
            {

                var patient1 = new AppUser
                {
                    UserName = "Patient1",
                    Surname = "Kowalska1",
                    City = "Świecie",
                    ZipCode = "86-100",
                    NormalizedUserName = "patient1",
                    Email = "patient1@example.com",
                    NormalizedEmail = "patient1@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };




                if (!context.Users.Any(u => u.UserName == "Patient1"))
                {
                    var userStore = new UserStore<AppUser>(context);


                    var password = new PasswordHasher<AppUser>();
                    var hashed = password.HashPassword(patient1, "Sekret123@");
                    patient1.PasswordHash = hashed;

                    userStore.CreateAsync(patient1).Wait();
                    userStore.AddToRoleAsync(patient1, "Pacjent").Wait();
                }

                context.SaveChanges();


            }
            catch (Exception ex)
            {

            }




        }

        void SeedAdminAndRoles()
        {




            try
            {
                var admin = new AppUser
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

                if (!context.Users.Any(u => u.UserName == admin.UserName))
                {
                    var password = new PasswordHasher<AppUser>();
                    var hashed = password.HashPassword(admin, "Sekret123@");
                    admin.PasswordHash = hashed;
                    var userStore = new UserStore<AppUser>(context);
                    userStore.CreateAsync(admin).Wait();
                    userStore.AddToRoleAsync(admin, "Administrator").Wait();
                }




                context.SaveChanges();



            }
            catch (Exception ex)
            {

            }
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

            List<string> list = new List<string>() { "Ginekolog", "Stomatolog", "Ortopeda", "Chirurg", "Dermatolog", "Psychiatra", "Psycholog", "Internista", "Laryngolog", "Okulista", "Neurolog", "Fizjoterapeuta", "Urolog", "Sexuolog", "Alergolog", "Chirurg Szczękowy", "Lekarz Sportowy" };


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

        void AddDoctorAndPatientToUser()
        {


            try
            {

                AddDoctorToUser("doctor1@example.com");
                AddDoctorToUser("doctor2@example.com");
                AddDoctorToUser("doctor3@example.com");

                AddPatientToUser("patient1@example.com");



            }
            catch (Exception ex)
            {

            }





        }

        void SeedPlaces()
        {
            try
            {
                Place place = new Place() { PlaceName = "Przychodnia", Country = "Polska", City = "Świecie", Street = "Wojska Polskiego", BuildingNumber = 10, Room = 1 };
                AddPlace(place);
                Place place2 = new Place() { PlaceName = "Przychodnia Prywatna", Country = "Polska", City = "Świecie", Street = "Aleja Jana Pawła II", BuildingNumber = 15, Room = 1 };
                AddPlace(place2);

            }
            catch (Exception ex)
            {

            }



        }

        public bool AddPlace(Place place)
        {
            try
            {
                if (context.Places.Where(p => p.Country == place.Country && p.City == place.City && p.Street == place.Street && p.BuildingNumber == place.BuildingNumber && p.Room == place.Room).Any())
                {
                    return false;
                }

                context.Places.Add(place);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void SeedAppointments()
        {
            try
            {

                Appointment appointment1 = new Appointment() { AppointmentDate = new DateTime(2020, 12, 1), AppointmentStart = new DateTime(2020, 12, 1, 12, 0, 0), AppointmentEnd = new DateTime(2020, 12, 1, 13, 0, 0), PlacesAvailable = 4, VisitPrice = 100 };
                Appointment appointment2 = new Appointment() { AppointmentDate = new DateTime(2020, 12, 1), AppointmentStart = new DateTime(2020, 12, 1, 13, 0, 0), AppointmentEnd = new DateTime(2020, 12, 1, 14, 0, 0), PlacesAvailable = 6, VisitPrice = 100 };
                Appointment appointment3 = new Appointment() { AppointmentDate = new DateTime(2020, 12, 1), AppointmentStart = new DateTime(2020, 12, 1, 14, 0, 0), AppointmentEnd = new DateTime(2020, 12, 1, 16, 0, 0), PlacesAvailable = 7, VisitPrice = 100 };

                AddAppointment(1, 1, appointment1);
                AddAppointment(2, 1, appointment2);
                AddAppointment(3, 2, appointment3);

            }
            catch(Exception ex)
            {

            }

           

           }

       public bool AddPatientToUser(string Email)
    {
        try
        {
            Patient patient = new Patient();
            AppUser user = context.Users.Where(u => u.Email == Email).First();
            if (user != null)
            {
                user.Patient = patient;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

       public bool AddDoctorToUser(string Email)
    {
        try
        {
            Doctor doctor = new Doctor();
            AppUser user = context.Users.Where(u => u.Email == Email).First();
            if (user != null)
            {
                user.Doctor = doctor;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

     

        public bool AddAppointment(int DoctorId, int PlaceId, Appointment appointment)
    {


        try
        {

            Doctor doctor = context.Doctors.Find(DoctorId);
            Place place = context.Places.Find(PlaceId);
           
            appointment.NumberOfPatients = 0;
            context.Appointments.Add(appointment);
            context.SaveChanges();




            doctor.Appointments.Add(appointment);
            place.Appointments.Add(appointment);
            context.SaveChanges();
            return true;




        }
        catch (Exception ex)
        {
            return false;
        }




    }

        public bool AddSpecializationToDoctor(int DoctorId, string Name)
        {
            try
            {

                Doctor doctor = context.Doctors.Find(DoctorId);

                if (doctor != null)
                {

                    if (doctor.DoctorSpecializations != null && doctor.DoctorSpecializations.Where(d => d.Specialization.Name == Name).Any())
                    {
                        return false;
                    }
                    else
                    {
                        Specialization specialization = context.Specializations.Where(s => s.Name == Name).First();

                        DoctorSpecialization doctorSpecialization = new DoctorSpecialization() { Doctor = doctor, Specialization = specialization };

                        specialization.DoctorSpecializations.Add(doctorSpecialization);

                        context.SaveChanges();
                    }


                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }










    }


}





