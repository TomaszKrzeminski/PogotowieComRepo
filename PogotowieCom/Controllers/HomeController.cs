using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PogotowieCom.Models;
using static PogotowieCom.Models.Specialist;

namespace PogotowieCom.Controllers
{
    public class HomeController : Controller
    {

        IRepository repository;
        private UserManager<AppUser> userManager;



        public HomeController(IRepository repo, UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            repository = repo;
        }






        private Task<AppUser> GetCurrentUserAsync() =>
            userManager.GetUserAsync(HttpContext.User);



        public ViewResult HomePage()
        {
            HomePageViewModel model = new HomePageViewModel() { Country = "Polska", City = "Świecie", MedicalSpecialist = "Stomatolog" /*"Wyszukaj specialistę" */};

            List<string> ListTags = new List<string>();
            ListTags.Add("");
            ListTags.AddRange(  repository.Tags.Select(t=>t.Text).ToList());
            model.AllAilments = ListTags;

            return View(model);
        }
      
        public IActionResult AdvancedSearch()
        {
            AdvancedSearchViewModel model = new AdvancedSearchViewModel();


            return View(model);
        }

        [HttpPost]
        public IActionResult AdvancedSearch(AdvancedSearchViewModel model)
        {

            if (ModelState.IsValid)
            {
                Search search = new Search(model, repository);
                search.Check();
                search.Filtr();
                search.Check();
                search.Filtr();
                search.Check();
                search.Filtr();
                search.Check();
                search.Filtr();
                search.Check();
                search.Filtr();

                model.UserList = search.Users;

                return View(model);
            }
            else
            {
                return View(model);
            }



        }


        public IActionResult NotificationChecked(int id)
        {
            AppUser user = GetCurrentUserAsync().Result;

            repository.ChangeNotificationToChecked(id, user.Id);

            return RedirectToAction("HomePage");
        }



        [Authorize]
        public ViewResult UsersPanel()
        {
            UsersAccountViewModel model = new UsersAccountViewModel();
            AppUser user = GetCurrentUserAsync().Result;
            model.NotificationList = repository.GetNotifications((int)user.PatientId, true);

            return View(model);
        }







        public PartialViewResult FindSpecialist(HomePageViewModel model)
        {

            bool CheckAilmentsText(List<Tag> list)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item.Text != null)
                        {
                            return true;
                        }
                    }
                }
                               
                return false;
            }





            if (CheckAilmentsText(model.Ailments))
            {
                Specialist ginekolg = new Ginekolog(repository);
                Specialist stomatolog = new Stomatolog(repository);
                Specialist ortopeda = new Ortopeda(repository);


                ginekolg.setNumber(stomatolog);
                stomatolog.setNumber(ortopeda);



                ginekolg.ForwardRequest(model.Ailments);
                List<string> listofspecialist = ortopeda.GetSpecialistsNames().Distinct().ToList();

                SearchDoctorViewModel DoctorModel = new SearchDoctorViewModel();
                HomePageViewModel Model = model;

                for (int i = 0; i < listofspecialist.Count(); i++)
                {
                    Model.MedicalSpecialist = listofspecialist[i];
                    DoctorModel.Users.AddRange(repository.SearchForDoctor(Model).Users);
                }

                DoctorModel.Users.Distinct(new UserComparer());

                return PartialView(DoctorModel);


            }





            SearchDoctorViewModel doctormodel = repository.SearchForDoctor(model);


            return PartialView(doctormodel);
        }



     




        public List<string> MakeTags(string Ailment)
        {
            List<string> list = new List<string>();




            return list;
        }






        public PartialViewResult FindAilment(List<Tag> Ailments)

        {


            Specialist ginekolg = new Ginekolog(repository);
            Specialist stomatolog = new Stomatolog(repository);
            Specialist ortopeda = new Ortopeda(repository);


            ginekolg.setNumber(stomatolog);
            stomatolog.setNumber(ortopeda);



            ginekolg.ForwardRequest(Ailments);
            List<string> listofspecialist = ortopeda.GetSpecialistsNames();



            SearchDoctorViewModel doctormodel = repository.SearchForDoctor(new HomePageViewModel());
            return PartialView("FindSpecialist", doctormodel);

        }


    


    }
}