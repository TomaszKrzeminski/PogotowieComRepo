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



        public HomeController(IRepository repo, IUserManager userMgr)
        {
            
            repository = repo;
        }



        private Task<AppUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public ViewResult HomePage()
        {
            HomePageViewModel model = new HomePageViewModel() { Country = "Polska", City = "Świecie", MedicalSpecialist = "Stomatolog" /*"Wyszukaj specialistę" */};
            return View(model);
        }
        //[HttpPost]
        //public ViewResult HomePage(HomePageViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    else
        //    {
        //        return View(model);
        //    }

        //}



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


            if (model.Ailments != null && model.Ailments.Count() > 0)
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



        public ViewResult ChooseBodyPart(string[] BodyParts)
        {
            return View();
        }

        public ViewResult FindBodyPart(string FindBodyPart)
        {
            return View();
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


        //        public ViewResult MedicalSpecialists()
        //     /*   {*/
        //            return View();
        //        }

        //        public ViewResult ShowAilment()
        //        {

        //            List<string> AilmentList = new List<string>() {
        //"bezpłodność ",
        //"bezsenność ",
        //"białkomocz ",
        //"biegunka ",
        //"ból brzucha ",
        //"bóle mięśni ",
        //"bóle stawów ",
        //"ból gardła ",
        //"ból głowy ",
        //"ból twarzy ",
        //"ból ucha ",
        //"ból w klatce piersiowej ",
        //"ból zęba ",
        //"brak apetytu ",
        //"brzydki zapach ",
        //"chrapanie ",
        //"chrypka ",
        //"chudnięcie ",
        //"częste oddawanie moczu ",
        //"depresja ",
        //"drgawki ",
        //"drżenie rąk ",
        //"duszność ",
        //"dysfagia ",
        //"gorączka ",
        //"guz ",
        //"hiperglikemia ",
        //"infekcje ",
        //"kaszel ",
        //"katar sienny ",
        //"kichanie ",
        //"kołatanie serca ",
        //"kolka ",
        //"krew w kale ",
        //"krew w moczu ",
        //"krwawienia międzymiesiączkowe ",
        //"krwawienie ",
        //"krwawienie z nosa ",
        //"krwioplucie ",
        //"łamliwość włosów ",
        //"lęk ",
        //"łzawienie oczu ",
        //"migrena ",
        //"mroczki przed oczami ",
        //"mrużenie oczu ",
        //"nadciśnienie ",
        //"nadmierny apetyt ",
        //"napięcie mięśni ",
        //"nerwowość ",
        //"niedotlenienie ",
        //"niedowład ",
        //"niedożywienie ",
        //"nieostre widzenie ",
        //"niepłodność ",
        //"niestrawność ",
        //"niewydolność nerek ",
        //"nieżyt nosa ",
        //"nudności ",
        //"obrzęk ",
        //"obrzęk jądra ",
        //"odwodnienie ",
        //"omamy ",
        //"omdlenia ",
        //"opryszczka ",
        //"osłabienie ",
        //"osteoporoza ",
        //"otępienie ",
        //"owrzodzenie ",
        //"pobudzenie ",
        //"podwójne widzenie ",
        //"pokrzywka ",
        //"potliwość ",
        //"powiększenie śledziony ",
        //"powiększenie wątroby ",
        //"rozdrażnienie ",
        //"rozstępy ",
        //"rumień ",
        //"senność ",
        //"sinica ",
        //"skurcz mięśni ",
        //"smutek ",
        //"spadek libido ",
        //"stan zapalny ",
        //"sucha skóra ",
        //"swędzenie ",
        //"świąd ",
        //"trudności w oddychaniu ",
        //"uczulenie ",
        //"upławy ",
        //"urojenia ",
        //"utrata przytomności ",
        //"utrata wagi ",
        //"wrzody ",
        //"wydzielina z nosa ",
        //"wymioty ",
        //"wysypka ",
        //"wzdęcia ",
        //"zaburzenia miesiączkowania ",
        //"zaburzenia mowy ",
        //"zaburzenia słuchu ",
        //"zaburzenia widzenia ",
        //"zaburzenia wzwodu ",
        //"zaczerwienienie ",
        //"zakażenie",
        //"zakrzepica ",
        //"zaparcia ",
        //"zatkany nos ",
        //"zawał ",
        //"zawał płuca ",
        //"zawroty głowy ",
        //"zgaga ",
        //"złe samopoczucie ",
        //"zmęczenie ",
        //" żółtaczka" };
        //            return View();
        //        }




    }
}