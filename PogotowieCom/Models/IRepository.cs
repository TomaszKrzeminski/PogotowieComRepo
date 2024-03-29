﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public interface IRepository
    {
        ShowCommentsViewModel GetCommentsAndDoctorData(string UserId);
        AppUser GetUserByDoctorId(int Id);
        bool AddCommentToComplete(int PatientId, Appointment appointment);
        bool ChangeComment(Comment comment);
        CommentData CommentAndVoteCheck(AppUser user);
        List<DoctorRankViewModel> GetCommentDetails();
        List<Tag> GetTagsSpecialist(Specialist specialist);
        int GetDoctorIdByUserId(string UserId);
        bool AddPatientToUser(Patient patient, string Email);
        bool AddDoctorToUser(Doctor doctor, string Email);
        Task<bool> AddRoleToUser(string Email, string Role);
        bool AddSpecialization(string Name);
        bool AddSpecializationToDoctor(int DoctorId, string Name);
        bool AddPlace(Place place);
        bool AddAppointment(AddAppointmentViewModel model);
        bool CheckIfAppointmentExists(AddAppointmentViewModel model);
        Appointment GetAppointmentById(int Id);
        List<Specialization> GetDoctorSpecializations(string UserId);
        List<Place> SelectPlaces(SelectPlaceViewModel model);
        List<Appointment> GetUserAppointments(int DoctorId);
        bool DeleteDoctorSpecialization(string UserId, int SpecializationId);
        bool ReserveAppointment(ReserveAppointmentViewModel model);
        SearchDoctorViewModel SearchForDoctor(HomePageViewModel model);
        Place GetPlaceById(int PlaceId);
        IQueryable<Tag> Tags { get; }
        IQueryable<Appointment> Appointments { get; set; }
        IQueryable<Place> Places { get; set; }
        IQueryable<Specialization> Specializations { get; }
        List<int> GetBookedAppointments(int AppointmentId);
        bool AddNotificationToPatient(int PatientId, Notification notification);
        List<Notification> GetNotifications(int PatientId, bool Checked);
        Patient GetPatientById(int PatientId);
        bool ChangeNotificationToChecked(int NotificationId, string UserId);
        bool RemoveAppointment(int AppointmentId);
        Appointment GetAppointmentByIdAllData(int AppointmentId);
        List<AppUser> GetFilteredUsersCity(string City, List<AppUser> list = null);
        List<AppUser> GetFilteredUsersSpecialization(string Specialization, List<AppUser> list = null);
        List<AppUser> GetFilteredUsersDate(DateTime Date, List<AppUser> list = null);
        List<AppUser> GetFilteredUsersHour(DateTime Date, List<AppUser> list = null);
        List<AppUser> GetAllUsers();


    }



    public class Repository : IRepository
    {
        public IQueryable<Appointment> Appointments { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IQueryable<Place> Places { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IQueryable<Specialization> Specializations { get => context.Specializations.AsQueryable(); }
        public IQueryable<Tag> Tags { get => context.Tags.AsQueryable(); }

        private AppIdentityDbContext context;
        private UserManager<AppUser> userManager;
        public Repository(AppIdentityDbContext context, UserManager<AppUser> userMgr)
        {
            this.context = context;
            userManager = userMgr;
        }


        public bool ChangeNotificationToChecked(int NotificationId, string UserId)
        {
            try
            {
                Notification notification = context.Users.Include(p => p.Patient).ThenInclude(n => n.Notifications).Where(u => u.Id == UserId).First().Patient.Notifications.Where(n => n.NotificationId == NotificationId).First();
                notification.Checked = true;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool AddPatientToUser(Patient patient, string Email)
        {
            try
            {
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddRoleToUser(string Email, string Role)
        {

            if (Email != null && Role != null)
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


                if (context.Specializations.Where(s => s.Name != null && s.Name == Name).Any() == false)
                {
                    Specialization specialization = new Specialization() { Name = Name };
                    context.Specializations.Add(specialization);
                    return true;
                }

                return false;

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
                if (user.DoctorId != null)
                {
                    Doctor doctor = context.Doctors.Include(p => p.DoctorSpecializations).ThenInclude(d => d.Specialization).Where(d => d.DoctorId == user.DoctorId).First();

                    list = doctor.DoctorSpecializations.Select(s => s.Specialization).ToList();
                    return list;
                }

                return list;

            }
            catch (Exception ex)
            {
                return list;
            }



        }

        public bool DeleteDoctorSpecialization(string UserId, int SpecializationId)
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
            SearchDoctorViewModel modelSearch = new SearchDoctorViewModel();
            try
            {
                //List<AppUser> list = context.Users.Include(d => d.Doctor).ThenInclude(s => s.DoctorSpecializations).Where(u => u.Doctor.DoctorSpecializations.Where(s => s.Specialization.Name == model.MedicalSpecialist).Any()).ToList();
                List<AppUser> list = context.Users.Include(d => d.Doctor).ThenInclude(s => s.DoctorSpecializations).Where(u => u.Doctor.DoctorSpecializations.Where(s => s.Specialization.Name == model.MedicalSpecialist).Any()).Where(x => x.City == model.City).ToList();
                if (list != null)
                {
                    modelSearch.Users = list;
                }
                modelSearch.City = model.City;
                modelSearch.Country = model.Country;
                return modelSearch;


            }
            catch (Exception ex)
            {
                return modelSearch;
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

        public List<Place> SelectPlaces(SelectPlaceViewModel model)
        {
            List<Place> list = new List<Place>();
            try
            {

                list = context.Places.Where(p => p.Country == model.Country && p.City == model.City && p.Street == model.Street).ToList();

                if (model.BuildingNumber != 0)
                {
                    list = list.Where(p => p.BuildingNumber == model.BuildingNumber).ToList();
                }

                if (model.Room != 0)
                {
                    list = list.Where(p => p.Room == model.Room).ToList();
                }

                return list;

            }
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }




        }




        public List<Appointment> GetUserAppointments(int DoctorId)
        {
            try
            {

                List<Appointment> list = context.Appointments.Include(p => p.Place).Where(a => a.DoctorId == DoctorId).ToList();
                if (list == null)
                {
                    return new List<Appointment>();
                }
                return list;

            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Appointment GetAppointmentById(int Id)
        {
            try
            {
                Appointment data = context.Appointments.Include(p => p.Place).Where(a => a.AppointmentId == Id).First();
                return data;
            }
            catch (Exception ex)
            {
                return new Appointment();
            }
        }

        public bool ReserveAppointment(ReserveAppointmentViewModel model)
        {
            try
            {
                Appointment appointment = context.Appointments.Include(d => d.PatientAppointments).Where(a => a.AppointmentId == model.AppointmentId).First();
                appointment.NumberOfPatients += 1;
                Patient patient = context.Patients.Where(a => a.PatientId == model.PatientId).First();
                PatientAppointment patientappointment = new PatientAppointment();
                patientappointment.Appointment = appointment;
                patientappointment.Patient = patient;
                patientappointment.NumberInQueue = model.NumberInQueue;
                appointment.PatientAppointments.Add(patientappointment);


                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<int> GetBookedAppointments(int AppointmentId)
        {
            List<int> list = new List<int>();

            try
            {

                Appointment appointment = context.Appointments.Include(x => x.PatientAppointments).Where(a => a.AppointmentId == AppointmentId).First();
                foreach (var a in appointment.PatientAppointments)
                {
                    list.Add((int)a.NumberInQueue);
                }
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }



        }

        public bool AddNotificationToPatient(int PatientId, Notification notification)
        {
            try
            {

                Patient patient = context.Patients.Include(n => n.Notifications).Where(p => p.PatientId == PatientId).First();
                patient.Notifications.Add(notification);
                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Notification> GetNotifications(int PatientId, bool Checked)
        {
            try
            {
                List<Notification> list = context.Patients.Include(n => n.Notifications).Where(p => p.PatientId == PatientId).First().Notifications.ToList();
                if (Checked)
                {

                    return list;
                }
                else
                {
                    list = list.Where(n => n.Checked == Checked).ToList();
                    return list;
                }

            }
            catch (Exception ex)
            {
                return new List<Notification>();
            }
        }

        public Patient GetPatientById(int PatientId)
        {
            try
            {

                return context.Patients.Find(PatientId);

            }
            catch (Exception ex)
            {
                return new Patient();
            }
        }

        public bool RemoveAppointment(int AppointmentId)
        {
            try
            {

                Appointment appointment = context.Appointments.Find(AppointmentId);
                context.Appointments.Remove(appointment);
                context.SaveChanges();



                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Appointment GetAppointmentByIdAllData(int AppointmentId)
        {
            try
            {
                Appointment appointment = context.Appointments.Include(pa => pa.PatientAppointments).Where(a => a.AppointmentId == AppointmentId).First();
                return appointment;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Tag> GetTagsSpecialist(Specialist specialist)
        {
            List<Tag> TagList = new List<Tag>();
            string name = specialist.GetNameOfSpecialization();
            try
            {
                Specialization specialization = context.Specializations.Include(s => s.TagSpecializations).ThenInclude(s => s.Tag).Where(x => x.Name == name).First();
                TagList = specialization.TagSpecializations.Select(z => z.Tag).ToList();

                return TagList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public List<AppUser> GetExistingUsers(List<AppUser> listfromrepo, List<AppUser> listToCompare)
        {
            List<AppUser> returnList = new List<AppUser>();

            if (listToCompare.Count() == 0 || listfromrepo.Count() == 0)
            {
                return returnList;
            }

            foreach (var compareU in listToCompare)
            {
                foreach (var repoU in listfromrepo)
                {
                    if (repoU.Id == compareU.Id)
                    {
                        returnList.Add(repoU);
                    }
                }
            }

            return returnList;

        }


        public List<AppUser> GetFilteredUsersCity(string City, List<AppUser> listCompare = null)
        {
            List<AppUser> list = new List<AppUser>();

            try
            {
                list = context.Users.Where(c => c.ChooseRole != null && c.ChooseRole == "Doktor").Include(b => b.Doctor).ThenInclude(b => b.Appointments).ToList();
                List<Place> places = context.Places.Where(c => c.City == City).Include(a => a.Appointments).ToList();
                List<AppUser> UserList = new List<AppUser>();
                foreach (var p in places)
                {


                    foreach (var l in list)
                    {
                        if (l.Doctor.Appointments.Where(x => x.PlaceId == x.PlaceId).Any())
                        {
                            UserList.Add(l);
                        }
                    }


                }

                if (listCompare != null)
                {
                    UserList = GetExistingUsers(listCompare, UserList);
                }


                return UserList;
            }
            catch (Exception ex)
            {
                return list;
            }



        }


        public List<AppUser> GetFilteredUsersSpecialization(string Specialization, List<AppUser> listCompare = null)
        {
            List<AppUser> list = new List<AppUser>();

            try
            {

                int Id = context.Specializations.Where(x => x.Name == Specialization).Select(a => a.SpecializationId).First();




                list = context.Users.Where(c => c.ChooseRole != null && c.ChooseRole == "Doktor").Include(b => b.Doctor).ThenInclude(b => b.DoctorSpecializations).Where(x => x.Doctor.DoctorSpecializations.Where(i => i.SpecializationId == Id).Any()).ToList();


                if (listCompare != null)
                {
                    list = GetExistingUsers(listCompare, list);
                }

                return list;
            }
            catch (Exception ex)
            {
                return list;
            }



        }

        public List<AppUser> GetFilteredUsersDate(DateTime Date, List<AppUser> listCompare = null)
        {
            List<AppUser> list = new List<AppUser>();

            try
            {


                list = context.Users.Where(c => c.ChooseRole != null && c.ChooseRole == "Doktor").Include(d => d.Doctor).ThenInclude(a => a.Appointments).Where(x => x.Doctor.Appointments.Where(c => c.AppointmentDate >= Date).Any()).ToList();


                if (listCompare != null)
                {
                    list = GetExistingUsers(listCompare, list);
                }

                return list;
            }
            catch (Exception ex)
            {
                return list;
            }



        }

        public List<AppUser> GetFilteredUsersHour(DateTime Date, List<AppUser> listCompare = null)
        {
            List<AppUser> list = new List<AppUser>();

            try
            {

                list = context.Users.Where(c => c.ChooseRole != null && c.ChooseRole == "Doktor").Include(d => d.Doctor).ThenInclude(a => a.Appointments).Where(x => x.Doctor.Appointments.Where(c => c.AppointmentStart >= Date).Any()).ToList();

                if (listCompare != null)
                {
                    list = GetExistingUsers(listCompare, list);
                }

                return list;
            }
            catch (Exception ex)
            {
                return list;
            }



        }

        public List<AppUser> GetAllUsers()
        {
            List<AppUser> users = new List<AppUser>();

            try
            {
                users = context.Users.Where(r => r.ChooseRole != null && r.ChooseRole == "Pacjeln" || r.ChooseRole == "Doktor").ToList();
                return users;
            }
            catch (Exception ex)
            {
                return users;
            }
        }

        public List<DoctorRankViewModel> GetCommentDetails()
        {
            List<DoctorRankViewModel> model = new List<DoctorRankViewModel>();
            try
            {
                List<DoctorRankViewModel> listRank = new List<DoctorRankViewModel>();
                List<AppUser> Doctors = context.Users.Include(x=>x.Doctor).Where(d => d.Doctor != null).ToList();


                foreach (var doctor in Doctors)
                {
                    //List<Comment> comments = context.Comments.Where(d => d.DoctorId == doctor.DoctorId ).ToList();
                    List<Comment> comments = context.Comments.Where(d => d.DoctorId == doctor.DoctorId&&d.Done==true).ToList();
                    List<Place> places = context.Appointments.Where(d => d.DoctorId == doctor.DoctorId).Select(p => p.Place).ToList();
                    List<Appointment> appointments = context.Appointments.Where(d =>d.DoctorId==doctor.DoctorId&& d.AppointmentEnd > DateTime.Now).ToList();

                    List<Specialization> specializations = context.Specializations.Include(d=>d.DoctorSpecializations).Where(s => s.DoctorSpecializations.Any(d => d.DoctorId == doctor.DoctorId)).ToList();

                    DoctorRankViewModel rankmodel = new DoctorRankViewModel(doctor.Doctor,doctor,comments,places,appointments,specializations) ;

                    listRank.Add(rankmodel);

                }


          IEnumerable<DoctorRankViewModel> query=listRank.OrderByDescending(x => x.Points);
               

                return query.ToList();
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public CommentData CommentAndVoteCheck(AppUser user)
        {
            CommentData data = new CommentData();
            try
            {
                DateTime now = DateTime.Now;
                Comment comment = context.Comments.Where(x => x.PatientId == user.PatientId && x.Done == false && x.CommentAvailable < now).First();
                if (comment != null)
                {
                    AppUser userDoctor = context.Users.Where(d => d.DoctorId != null && d.DoctorId == comment.DoctorId).First();
                    Appointment appointment = context.Appointments.Where(a => a.AppointmentId == comment.AppointmentId).First();

                    data.appointment = appointment;
                    data.user = userDoctor;

                    return data;

                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddCommentToComplete(int PatientId, Appointment appointment)
        {
            Comment comment = new Comment();
            try
            {
                string Email = context.Users.Where(x => x.Patient != null && x.PatientId == PatientId).First().Email;

                comment.AppointmentId = appointment.AppointmentId;
                comment.CommentAvailable = (DateTime)appointment.AppointmentEnd;
                comment.Done = false;
                comment.PatientId = PatientId;
                comment.Points = 0;
                comment.PatientEmail = Email;

                Doctor doctor = context.Doctors.Find(appointment.DoctorId);
                doctor.Comments.Add(comment);
                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool ChangeComment(Comment comment)
        {

            try
            {
                Comment commentToChange = context.Comments.Where(c => c.AppointmentId == comment.AppointmentId).First();
                commentToChange.CommentDate = comment.CommentDate;
                commentToChange.Done = true;
                commentToChange.Points = comment.Points;
                commentToChange.Text = comment.Text;

                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public AppUser GetUserByDoctorId(int Id)
        {
            try
            {
                AppUser user = context.Users.Where(u => u.DoctorId != null && u.DoctorId == Id).First();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ShowCommentsViewModel GetCommentsAndDoctorData(string UserId)
        {
            ShowCommentsViewModel model = new ShowCommentsViewModel();

            try
            {
                AppUser user = context.Users.Include(d => d.Doctor).Where(u => u.Id == UserId).First();
                //List<Comment> list = context.Comments.Where(c => c.DoctorId == user.DoctorId).ToList();
                List<Comment> list = context.Comments.Where(c => c.DoctorId == user.DoctorId&&c.Done==true).ToList();

                model.user = user;
                model.Comments = list;

                return model;


            }
            catch(Exception ex)
            {
                return model;
            }

        }

        public bool CheckIfAppointmentExists(AddAppointmentViewModel model)
        {


            Dictionary<int,int> Minuts = new Dictionary<int, int>();

            for (int i = 0; i < 1441; i++)
            {
                Minuts.Add(i, 0);
            }
                  
            
            DateTime day = new DateTime(((DateTime)model.Appointment.AppointmentDate).Year, ((DateTime)model.Appointment.AppointmentDate).Month, ((DateTime)model.Appointment.AppointmentDate).Day, ((DateTime)model.Appointment.AppointmentDate).Hour, 0, 0);

            




            try
            {
              List<Appointment> list = context.Appointments.Where(a => a.DoctorId == model.DoctorId && a.AppointmentDate==model.Appointment.AppointmentDate ).ToList();

               
             if(list!=null)
                {
                    foreach (var appointment in list)
                    {

                        int Start =((DateTime)appointment.AppointmentStart).Hour*60+ (((DateTime)appointment.AppointmentStart).Minute);
                        int End =((DateTime)appointment.AppointmentEnd).Hour * 60 + (((DateTime)appointment.AppointmentEnd).Minute);


                        for (int i = (int)Start; i < ((int)End)+1; i++)
                        {
                            Minuts[i] = 1;
                        }
                                               

                    }


                    int start = ((DateTime)model.Appointment.AppointmentStart).Hour * 60 + (((DateTime)model.Appointment.AppointmentStart).Minute);
                    int end = ((DateTime)model.Appointment.AppointmentEnd).Hour * 60 + (((DateTime)model.Appointment.AppointmentEnd).Minute);


                    for (int i =start ; i < end+1; i++)
                    {


                        if(Minuts[i]==1)
                        {
                            return true;
                        }



                    }



                    return false;

                                                                          
                }
             else
                {
                    return false;
                }




            }
            catch(Exception ex)
            {
                return true;
            }







        }
    }


}


