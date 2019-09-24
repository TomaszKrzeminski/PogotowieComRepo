using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public interface IRepository
    {
        int GetDoctorIdByUserId(string UserId);
        bool AddPatientToUser(Patient patient, string Email);
        bool AddDoctorToUser(Doctor doctor, string Email);
         Task<bool> AddRoleToUser(string Email, string Role);
        bool AddSpecialization(string Name);
        bool AddSpecializationToDoctor(int DoctorId, string Name);
        bool AddPlace(Place place);
        bool AddAppointment(AddAppointmentViewModel model);
        List<Specialization> GetDoctorSpecializations(string UserId);
        List<Place> SelectPlaces(SelectPlaceViewModel model);
        List<Appointment> GetUserAppointments(int DoctorId);
        bool DeleteDoctorSpecialization(string UserId,int SpecializationId);
       SearchDoctorViewModel SearchForDoctor(HomePageViewModel model);
        Place GetPlaceById(int PlaceId);
        IQueryable<Appointment> Appointments { get; set; }
        IQueryable<Place> Places { get; set; }
        IQueryable<Specialization> Specializations { get;}
    }



    public class Repository : IRepository
    {
        public IQueryable<Appointment> Appointments { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IQueryable<Place> Places { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IQueryable<Specialization> Specializations { get =>context.Specializations.AsQueryable() ;  }
      

        private AppIdentityDbContext context;
        private UserManager<AppUser> userManager;
        public  Repository(AppIdentityDbContext context,UserManager<AppUser> userMgr)
        {
            this.context = context;
            userManager = userMgr;
        }

        public bool AddPatientToUser(Patient patient,string Email)
        {
            try
            {
                AppUser user = context.Users.Where(u => u.Email == Email).First();
                if(user!=null)
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
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool AddDoctorToUser(Doctor doctor, string Email)
        {
            try
            {
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
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddRoleToUser(string Email, string Role)
        {
            
            if(Email!=null&&Role!=null)
            {
                try
                {

                    AppUser user = context.Users.Where(u => u.Email == Email).First();
                    if (user != null)
                    {
                        IdentityResult result = await userManager.AddToRoleAsync(user, Role);

                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public bool AddSpecialization(string Name)
        {

            try
            {
                

               if(context.Specializations.Where(s=>s.Name!=null&&s.Name==Name).Any()==false)
                {
                    Specialization specialization = new Specialization() { Name = Name };
                    context.Specializations.Add(specialization);
                    return true;
                }

                return false;

            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public bool AddSpecializationToDoctor(int DoctorId, string Name)
        {
            try
            {

                Doctor doctor = context.Doctors.Find(DoctorId);

                if(doctor!=null)
                {

                    if(doctor.DoctorSpecializations!=null&&doctor.DoctorSpecializations.Where(d=>d.Specialization.Name==Name).Any())
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
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<Specialization> GetDoctorSpecializations(string UserId)
        {
            List<Specialization> list = new List<Specialization>();
            try
            {
                //AppUser user = context.Users.Find(UserId);
                //List<DoctorSpecialization> doctorSpecializations = user.Doctor.DoctorSpecializations.ToList();
                //list = doctorSpecializations.Select(x => x.Specialization).ToList();
                //return list;

                AppUser user = context.Users.Find(UserId);
                if(user.DoctorId!=null)
                {
                    Doctor doctor = context.Doctors.Include(p => p.DoctorSpecializations).ThenInclude(d => d.Specialization).Where(d => d.DoctorId == user.DoctorId).First();
                  
                    list = doctor.DoctorSpecializations.Select(s => s.Specialization).ToList();
                    return list;
                }

                return list;

            }
            catch(Exception ex)
            {
                return list;
            }



        }

        public bool DeleteDoctorSpecialization(string UserId,int  SpecializationId)
        {
            try
            {
                
                AppUser user = context.Users.Find(UserId);
                if (user.DoctorId != null)
                {
                    Doctor doctor = context.Doctors.Include(p => p.DoctorSpecializations).ThenInclude(d => d.Specialization).Where(d => d.DoctorId == user.DoctorId).First();
                    DoctorSpecialization doctorspecialization = doctor.DoctorSpecializations.Where(s => s.SpecializationId == SpecializationId).First();
                    doctor.DoctorSpecializations.Remove(doctorspecialization);
                    context.SaveChanges();
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public SearchDoctorViewModel SearchForDoctor(HomePageViewModel model)
        {
            SearchDoctorViewModel modelSearch =new SearchDoctorViewModel();
            try
            {
                List<AppUser> list = context.Users.Include(d => d.Doctor).ThenInclude(s=>s.DoctorSpecializations).Where(u=>u.Doctor.DoctorSpecializations.Where(s=>s.Specialization.Name==model.MedicalSpecialist).Any()).ToList();
                //List<Doctor>   list = context.Users.Include(d=>d.DoctorSpecializations).Where(d => d.DoctorSpecializations.Where(s => s.Specialization.Name == model.MedicalSpecialist).Any()).ToList();
                if (list != null)
                {
                    modelSearch.Users =list ;
                }
                modelSearch.City = model.City;
                modelSearch.Country = model.Country;
                return modelSearch;


            }
            catch(Exception ex)
            {
                return modelSearch;
            }




        }

        public bool AddPlace(Place place)
        {
            try
            {
                if(context.Places.Where(p=>p.Country==place.Country&&p.City==place.City&&p.Street==place.Street&&p.BuildingNumber==place.BuildingNumber&&p.Room==place.Room).Any())
                {
                    return false;
                }

                context.Places.Add(place);
                context.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<Place> SelectPlaces(SelectPlaceViewModel model)
        {
            List<Place> list = new List<Place>();
            try
            {

                list = context.Places.Where(p => p.Country == model.Country&&p.City==model.City&&p.Street==model.Street).ToList();

                if(model.BuildingNumber!=0)
                {
                    list = list.Where(p => p.BuildingNumber == model.BuildingNumber).ToList();
                }

                if(model.Room!=0)
                {
                    list = list.Where(p => p.Room == model.Room).ToList();
                }

                return list;

            }
            catch(Exception ex)
            {
                return list;
            }
        }

        public Place GetPlaceById(int PlaceId)
        {
            Place place = new Place();
            try
            {
                place = context.Places.Find(PlaceId);
                return place;
            }
            catch(Exception ex)
            {
                return place;
            }
        }

        public bool AddAppointment(AddAppointmentViewModel model)
        {

            try
            {

                Doctor doctor = context.Doctors.Find(model.DoctorId);
                Place place = context.Places.Find(model.PlaceId);
                Appointment appointment = model.Appointment;
                appointment.NumberOfPatients = 0;
                context.Appointments.Add(appointment);
                context.SaveChanges();




                doctor.Appointments.Add(appointment);
                place.Appointments.Add(appointment);
                context.SaveChanges();
                return true;




            }
            catch(Exception ex)
            {
                return false;
            }




        }

        public List<Appointment> GetUserAppointments(int DoctorId)
        {
            try
            {
                
                List<Appointment> list = context.Appointments.Include(p=>p.Place).Where(a => a.DoctorId == DoctorId).ToList();
                if(list==null)
                {
                    return new List<Appointment>();
                }
                return list;

            }
            catch(Exception ex)
            {
                return new List<Appointment>();
            }
        }

        public int GetDoctorIdByUserId(string UserId)
        {
            try
            {

                int Id = (int)context.Users.Find(UserId).DoctorId;
                return Id;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
    }


}
