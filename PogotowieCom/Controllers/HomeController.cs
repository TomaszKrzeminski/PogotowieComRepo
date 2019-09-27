using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PogotowieCom.Models;


namespace PogotowieCom.Controllers
{
    public class HomeController : Controller
    {

        IRepository repository;

        public HomeController(IRepository repo)
        {
            repository = repo;
        }


        public ViewResult HomePage()
        {
            HomePageViewModel model = new HomePageViewModel() { Country = "Polska", City = "Świecie", MedicalSpecialist ="Stomatolog" /*"Wyszukaj specialistę" */};
            return View(model);
        }
        [HttpPost]
        public ViewResult HomePage(HomePageViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                return View(model);
            }

        }

        [Authorize]
        public ViewResult UsersPanel()
        {
            return View();
        }







        public PartialViewResult FindSpecialist(HomePageViewModel model)
        {

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



        public ViewResult FindAilment(string Ailment)
        {
            return View();
        }


        public ViewResult MedicalSpecialists()
        {
            return View();
        }

        public ViewResult ShowAilment()
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
            return View();
        }




    }
}