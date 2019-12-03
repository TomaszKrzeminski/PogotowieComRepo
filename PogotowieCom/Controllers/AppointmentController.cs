using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PogotowieCom.Models;

namespace PogotowieCom.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;
        private Task<AppUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
        private Func<Task<AppUser>> GetUser;
        private ITimeAndDate time;






        public AppointmentController(IRepository repo, UserManager<AppUser> usermgr, ITimeAndDate time, Func<Task<AppUser>> GetUser = null)
        {

            if (GetUser == null)
            {
                this.GetUser = () => userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                this.GetUser = GetUser;
            }


            repository = repo;
            userManager = usermgr;
            this.time = time;
        }

        public IActionResult AppointmentDetails(int AppointmentId)
        {
            ReserveAppointmentViewModel model;
            try
            {
                Appointment appointment = repository.GetAppointmentById(AppointmentId);
                AppUser user = GetUser().Result;
                int PatientId = (int)user.PatientId;
                model = new ReserveAppointmentViewModel(repository, appointment, user.Id, AppointmentId, PatientId);

            }
            catch (Exception ex)
            {
                return View("Error", null);
            }
            return View(model);

        }
        [Authorize(Roles = "Pacjent")]
        [HttpPost]
        public IActionResult ReserveAppointment(ReserveAppointmentViewModel model)
        {

            bool check = repository.ReserveAppointment(model);

            if (check)
            {
                Appointment appointment = repository.GetAppointmentByIdAllData(model.AppointmentId);

                repository.AddCommentToComplete(model.PatientId, appointment);
                Subject subject = new Subject();
                Observer obserwer = new Observer(subject, repository, model.PatientId);
                subject.MakeNotificationReservedAppointment(model.timeSelected);
                subject.notifyObservers();

                return RedirectToAction("AppointmentDetails", new { AppointmentId = model.AppointmentId });
            }
            else
            {
                return View("Error", null);
            }


        }


        public IActionResult ShowAppointments(ShowAppointmentViewModel model)
        {

            int DoctorId = repository.GetDoctorIdByUserId(model.DoctorId);
            List<Appointment> list = new List<Appointment>();

            if (String.IsNullOrEmpty(model.Country) && String.IsNullOrEmpty(model.City))
            {
                
                 list = repository.GetUserAppointments(DoctorId).Where(x=>x.AppointmentDate>=time.GetTime()).OrderByDescending(d=>d.AppointmentDate).ToList();
            }
            else
            {
                 list = repository.GetUserAppointments(DoctorId).Where(a => a.Place.Country == model.Country && a.Place.City == model.City&&a.AppointmentDate>=time.GetTime()).ToList();
            }


            return View(list);
        }

        public IActionResult ShowAppointmentsAdvanced(ShowAppointmentViewModel model)
        {

            int DoctorId = repository.GetDoctorIdByUserId(model.DoctorId);
            List<Appointment> list = repository.GetUserAppointments(DoctorId);

            return View("ShowAppointments", list);
        }

        [Authorize(Roles = "Doktor")]
        public IActionResult ManageAppointments()
        {
            int DoctorId = (int)GetUser().Result.DoctorId;
            List<Appointment> list = repository.GetUserAppointments(DoctorId);
            return View(list);
        }

        [HttpPost]
        [Authorize(Roles = "Doktor")]
        public IActionResult RemoveAppointment(int AppointmentId)
        {
            Appointment appointment = repository.GetAppointmentByIdAllData(AppointmentId);
            AppUser user = GetUser().Result;
            List<IObserver> observers = new List<IObserver>();
            List<int> observeresId = appointment.PatientAppointments.Where(a => a.AppointmentId == AppointmentId).Select(id => id.PatientId).ToList();
            bool check = repository.RemoveAppointment(AppointmentId);

            if (check)
            {
                SubjectRemoveAppointment subject = new SubjectRemoveAppointment(appointment, user);

                foreach (var ID in observeresId)
                {
                    observers.Add(new Observer(subject, repository, ID));
                }


                subject.MakeNotificationRemoveAppointment(appointment);
                subject.notifyObservers();

                return RedirectToAction("ManageAppointments");
            }
            else
            {
                return View("Error", null);
            }



        }


        [Authorize(Roles = "Doktor")]
        public IActionResult AddPlace()
        {
            Place place = new Place();
            return View(place);
        }

        [Authorize(Roles = "Doktor")]
        [HttpPost]
        public IActionResult AddPlace(Place model)
        {
            if (ModelState.IsValid)
            {
                repository.AddPlace(model);
                return RedirectToAction("ChoosePlace");
            }
            else
            {
                return View(model);
            }
        }

        [Authorize(Roles = "Doktor")]
        public IActionResult ChoosePlace()
        {
            SelectPlaceViewModel model = new SelectPlaceViewModel();
            return View("ChoosePlace", model);
        }




        //[Authorize]
        //public IActionResult ShowPlaces(SelectPlaceViewModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        List<Place> PlacesList = repository.SelectPlaces(model);

        //        return PartialView(PlacesList);
        //    }
        //    else
        //    {
        //        List<Place> PlacesList = new List<Place>();


        //        return View("ChoosePlace", model);
        //    }

        //}


        [Authorize]
        public IActionResult ShowPlaces(SelectPlaceViewModel model)
        {

            if (ModelState.IsValid)
            {
                List<Place> PlacesList = repository.SelectPlaces(model);

                return PartialView(PlacesList);
            }
            else
            {

                return View("Empty");

               
            }

        }



        [Authorize(Roles = "Doktor")]
        public IActionResult AddAppointment(int id = 0)
        {
            AppUser user = GetUser().Result;
            int DoctorId = (int)user.DoctorId;

            AddAppointmentViewModel model = new AddAppointmentViewModel() { PlaceId = id, place = repository.GetPlaceById(id), DoctorId = DoctorId };
            return View(model);
        }
        [Authorize(Roles = "Doktor")]
        [HttpPost]
        public IActionResult AddAppointment(AddAppointmentViewModel model)
        {


            DateTime now = time.GetTime();
            

            if (model.Appointment.AppointmentDate.HasValue && model.Appointment.AppointmentStart.HasValue && model.Appointment.AppointmentEnd.HasValue)
            {

                if (now.Date > model.Appointment.AppointmentDate)
                {
                    ModelState.AddModelError("Appointment.AppointmentDate", "Podaj późniejszą datę");
                }
                else if (now.Date == model.Appointment.AppointmentDate)
                {
                    now = now.AddHours(1);
                    int value = DateTime.Compare(model.Appointment.AppointmentStart.Value, now);

                    if (value <= 0)
                    {
                        ModelState.AddModelError("Appointment.AppointmentStart", "Podaj późniejszą godzinę");
                    }




                }
                else if (repository.CheckIfAppointmentExists(model))
                {


                    ModelState.AddModelError("Appointment.AppointmentStart", "Posiadasz już wizytę w tych godzinach");

                }
                else if (model.Appointment != null && ((DateTime)model.Appointment.AppointmentStart).Day < ((DateTime)model.Appointment.AppointmentEnd).Day || ((DateTime)model.Appointment.AppointmentStart).Day > ((DateTime)model.Appointment.AppointmentEnd).Day)
                {

                    ModelState.AddModelError("Appointment.AppointmentStart", "Wizyta może trwać tylko jeden dzień");

                }


            }



            if (ModelState.IsValid)
            {


                repository.AddAppointment(model);
                return RedirectToAction("ManageAppointments");
            }
            else
            {

                return View("AddAppointment", model);
            }





        }





    }
}