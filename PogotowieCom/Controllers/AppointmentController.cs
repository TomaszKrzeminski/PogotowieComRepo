﻿using System;
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
        public AppointmentController(IRepository repo, UserManager<AppUser> usermgr)
        {
            repository = repo;
            userManager = usermgr;
        }


        public IActionResult AppointmentDetails(int AppointmentId)
        {
            ReserveAppointmentViewModel model;
            try
            {
                Appointment appointment = repository.GetAppointmentById(AppointmentId);
                AppUser user = GetCurrentUserAsync().Result;
                int PatientId =(int)user.PatientId;
                model = new ReserveAppointmentViewModel(repository,appointment, user.Id, AppointmentId, PatientId);

            }
            catch(Exception ex)
            {
                return View("Error",null);
            }
            return View(model);

        }
        [Authorize(Roles="Pacjent")]
        [HttpPost]
        public IActionResult ReserveAppointment(ReserveAppointmentViewModel model)
        {

           bool check= repository.ReserveAppointment(model);

            if(check)
            {
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
            List<Appointment> list = repository.GetUserAppointments(DoctorId).Where(a=>a.Place.Country==model.Country&&a.Place.City==model.City).ToList();

            return View(list);
        }

        [Authorize(Roles = "Doktor")]
        public IActionResult ManageAppointments()
        {
            int DoctorId =(int) GetCurrentUserAsync().Result.DoctorId;
            List<Appointment> list = repository.GetUserAppointments(DoctorId);
            return View(list);
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
            return View("ChoosePlace",model);
        }




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
                List<Place> PlacesList = new List<Place>();
               
               
                return View("ChoosePlace",model);
            }

        }

        [Authorize(Roles = "Doktor")]
        public IActionResult AddAppointment(int id = 0)
        {
            AppUser user = GetCurrentUserAsync().Result;
            int DoctorId =(int)user.DoctorId;

            AddAppointmentViewModel model = new AddAppointmentViewModel() { PlaceId = id, place = repository.GetPlaceById(id),DoctorId=DoctorId };
            return View(model);
        }
        [Authorize(Roles = "Doktor")]
        [HttpPost]
        public IActionResult AddAppointment(AddAppointmentViewModel model)
        {

            DateTime now = DateTime.Now;
            now = now.AddHours(1);

            if (model.Appointment.AppointmentDate.HasValue && model.Appointment.AppointmentStart.HasValue && model.Appointment.AppointmentEnd.HasValue)
            {

                if (now.Date > model.Appointment.AppointmentDate)
                {
                    ModelState.AddModelError("Appointment.AppointmentDate", "Podaj późniejszą datę");
                }
                else if (now.Date == model.Appointment.AppointmentDate)
                {

                    int value = DateTime.Compare(model.Appointment.AppointmentStart.Value, now);

                    if (value <= 0)
                    {
                        ModelState.AddModelError("Appointment.AppointmentStart", "Podaj późniejszą godzinę");
                    }




                }


            }



            if (ModelState.IsValid)
            {


                repository.AddAppointment(model);
                return RedirectToAction("ManageAppointments");
            }
            else
            {

                return View(model);
            }





        }





    }
}