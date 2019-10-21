using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PogotowieCom.Controllers;
using PogotowieCom.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tests
{
    public class HomeControllerTests
    {
        private Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();

            return new Mock<UserManager<AppUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }


        [Test]
        public void Method_Returns_HomePageViewModel_()
        {

            IRepository repo = Mock.Of<IRepository>();
            UserManager<AppUser> userManager = GetMockUserManager().Object;



            HomeController controller = new HomeController(repo, userManager);

            HomePageViewModel model = (controller.HomePage() as ViewResult).ViewData.Model as HomePageViewModel;

            HomePageViewModel modelToCompare = new HomePageViewModel() { Country = "Polska", City = "Świecie", MedicalSpecialist = "Stomatolog" /*"Wyszukaj specialistę" */};

            Assert.AreEqual(modelToCompare.Country, model.Country);
            Assert.AreEqual(modelToCompare.City, model.City);
            Assert.AreEqual(modelToCompare.MedicalSpecialist, model.MedicalSpecialist);

        }



        //public string City { get; set; }

        //public string Specialization { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? Date { get; set; }

        //[DataType(DataType.Time)]
        //public DateTime? Hour { get; set; }


        [Test]
        public void AdvancedSearch_returns_Empty_UserList()
        {
            IRepository repo = Mock.Of<IRepository>();
            UserManager<AppUser> userManager = GetMockUserManager().Object;

            HomeController controller = new HomeController(repo, userManager);

            AdvancedSearchViewModel modelTry = new AdvancedSearchViewModel();

            AdvancedSearchViewModel model = (controller.AdvancedSearch(modelTry) as ViewResult).ViewData.Model as AdvancedSearchViewModel;


            Assert.That(model.UserList.Count, Is.EqualTo(0));

        }


        //List<AppUser> GetFilteredUsersCity(string City, List<AppUser> list = null);
        //List<AppUser> GetFilteredUsersSpecialization(string Specialization, List<AppUser> list = null);
        //List<AppUser> GetFilteredUsersDate(DateTime Date, List<AppUser> list = null);
        //List<AppUser> GetFilteredUsersHour(DateTime Date, List<AppUser> list = null);



        [Test]
        public void AdvancedSearch_returns_4_AppUsers_HourList()
        {
            Mock<IRepository> mock = new Mock<IRepository>();

            List<AppUser> listCity = new List<AppUser>() { new AppUser() { UserName = "City1" }, new AppUser() { UserName = "City2" }, new AppUser() { UserName = "City3" }, new AppUser() { UserName = "City4" } };
            List<AppUser> listSpecialization = new List<AppUser>() { new AppUser() { UserName = "Specialization1" }, new AppUser() { UserName = "Specialization2" }, new AppUser() { UserName = "Specialization3" }, new AppUser() { UserName = "Specialization4" } };
            List<AppUser> listDate = new List<AppUser>() { new AppUser() { UserName = "Date1" }, new AppUser() { UserName = "Date2" }, new AppUser() { UserName = "Date3" }, new AppUser() { UserName = "Date4" } };
            List<AppUser> listHour = new List<AppUser>() { new AppUser() { UserName = "Hour1" }, new AppUser() { UserName = "Hour2" }, new AppUser() { UserName = "Hour3" }, new AppUser() { UserName = "Hour4" } };



            mock.Setup(r => r.GetFilteredUsersCity(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listCity);
            mock.Setup(r => r.GetFilteredUsersSpecialization(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listSpecialization);
            mock.Setup(r => r.GetFilteredUsersDate(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listDate);
            mock.Setup(r => r.GetFilteredUsersHour(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listHour);



            UserManager<AppUser> userManager = GetMockUserManager().Object;

            HomeController controller = new HomeController(mock.Object, userManager);

            AdvancedSearchViewModel modelTry = new AdvancedSearchViewModel() { City = "Świecie", Specialization = null, Date = DateTime.Now.Date, Hour = DateTime.Now.Date };


            AdvancedSearchViewModel model = (controller.AdvancedSearch(modelTry) as ViewResult).ViewData.Model as AdvancedSearchViewModel;


            Assert.That(model.UserList[0].ToString(), Is.EqualTo("Hour1"));
            Assert.That(model.UserList[1].ToString(), Is.EqualTo("Hour2"));
            Assert.That(model.UserList[2].ToString(), Is.EqualTo("Hour3"));
            Assert.That(model.UserList[3].ToString(), Is.EqualTo("Hour4"));

        }

        [Test]
        public void AdvancedSearch_returns_4_AppUsers_DateList()
        {
            Mock<IRepository> mock = new Mock<IRepository>();

            List<AppUser> listCity = new List<AppUser>() { new AppUser() { UserName = "City1" }, new AppUser() { UserName = "City2" }, new AppUser() { UserName = "City3" }, new AppUser() { UserName = "City4" } };
            List<AppUser> listSpecialization = new List<AppUser>() { new AppUser() { UserName = "Specialization1" }, new AppUser() { UserName = "Specialization2" }, new AppUser() { UserName = "Specialization3" }, new AppUser() { UserName = "Specialization4" } };
            List<AppUser> listDate = new List<AppUser>() { new AppUser() { UserName = "Date1" }, new AppUser() { UserName = "Date2" }, new AppUser() { UserName = "Date3" }, new AppUser() { UserName = "Date4" } };
            List<AppUser> listHour = new List<AppUser>() { new AppUser() { UserName = "Hour1" }, new AppUser() { UserName = "Hour2" }, new AppUser() { UserName = "Hour3" }, new AppUser() { UserName = "Hour4" } };



            mock.Setup(r => r.GetFilteredUsersCity(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listCity);
            mock.Setup(r => r.GetFilteredUsersSpecialization(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listSpecialization);
            mock.Setup(r => r.GetFilteredUsersDate(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listDate);
            mock.Setup(r => r.GetFilteredUsersHour(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listHour);



            UserManager<AppUser> userManager = GetMockUserManager().Object;

            HomeController controller = new HomeController(mock.Object, userManager);

            AdvancedSearchViewModel modelTry = new AdvancedSearchViewModel() { City = "Świecie", Specialization = "Stomatolog", Date = DateTime.Now.Date };


            AdvancedSearchViewModel model = (controller.AdvancedSearch(modelTry) as ViewResult).ViewData.Model as AdvancedSearchViewModel;


            Assert.That(model.UserList[0].ToString(), Is.EqualTo("Date1"));
            Assert.That(model.UserList[1].ToString(), Is.EqualTo("Date2"));
            Assert.That(model.UserList[2].ToString(), Is.EqualTo("Date3"));
            Assert.That(model.UserList[3].ToString(), Is.EqualTo("Date4"));

        }


        [Test]
        public void AdvancedSearch_returns_4_AppUsers_CityList()
        {
            Mock<IRepository> mock = new Mock<IRepository>();

            List<AppUser> listCity = new List<AppUser>() { new AppUser() { UserName = "City1" }, new AppUser() { UserName = "City2" }, new AppUser() { UserName = "City3" }, new AppUser() { UserName = "City4" } };
            List<AppUser> listSpecialization = new List<AppUser>() { new AppUser() { UserName = "Specialization1" }, new AppUser() { UserName = "Specialization2" }, new AppUser() { UserName = "Specialization3" }, new AppUser() { UserName = "Specialization4" } };
            List<AppUser> listDate = new List<AppUser>() { new AppUser() { UserName = "Date1" }, new AppUser() { UserName = "Date2" }, new AppUser() { UserName = "Date3" }, new AppUser() { UserName = "Date4" } };
            List<AppUser> listHour = new List<AppUser>() { new AppUser() { UserName = "Hour1" }, new AppUser() { UserName = "Hour2" }, new AppUser() { UserName = "Hour3" }, new AppUser() { UserName = "Hour4" } };



            mock.Setup(r => r.GetFilteredUsersCity(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listCity);
            mock.Setup(r => r.GetFilteredUsersSpecialization(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listSpecialization);
            mock.Setup(r => r.GetFilteredUsersDate(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listDate);
            mock.Setup(r => r.GetFilteredUsersHour(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listHour);



            UserManager<AppUser> userManager = GetMockUserManager().Object;

            HomeController controller = new HomeController(mock.Object, userManager);

            AdvancedSearchViewModel modelTry = new AdvancedSearchViewModel() { City = "Świecie" };


            AdvancedSearchViewModel model = (controller.AdvancedSearch(modelTry) as ViewResult).ViewData.Model as AdvancedSearchViewModel;


            Assert.That(model.UserList[0].ToString(), Is.EqualTo("City1"));
            Assert.That(model.UserList[1].ToString(), Is.EqualTo("City2"));
            Assert.That(model.UserList[2].ToString(), Is.EqualTo("City3"));
            Assert.That(model.UserList[3].ToString(), Is.EqualTo("City4"));

        }

        [Test]
        public void AdvancedSearch_Empty_params_returns_4_AppUsers_HourList()
        {
            Mock<IRepository> mock = new Mock<IRepository>();

            List<AppUser> listCity = new List<AppUser>() { new AppUser() { UserName = "City1" }, new AppUser() { UserName = "City2" }, new AppUser() { UserName = "City3" }, new AppUser() { UserName = "City4" } };
            List<AppUser> listSpecialization = new List<AppUser>() { new AppUser() { UserName = "Specialization1" }, new AppUser() { UserName = "Specialization2" }, new AppUser() { UserName = "Specialization3" }, new AppUser() { UserName = "Specialization4" } };
            List<AppUser> listDate = new List<AppUser>() { new AppUser() { UserName = "Date1" }, new AppUser() { UserName = "Date2" }, new AppUser() { UserName = "Date3" }, new AppUser() { UserName = "Date4" } };
            List<AppUser> listHour = new List<AppUser>() { new AppUser() { UserName = "Hour1" }, new AppUser() { UserName = "Hour2" }, new AppUser() { UserName = "Hour3" }, new AppUser() { UserName = "Hour4" } };



            mock.Setup(r => r.GetFilteredUsersCity(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listCity);
            mock.Setup(r => r.GetFilteredUsersSpecialization(It.IsAny<string>(), null)).Returns<string, List<AppUser>>((a, b) => listSpecialization);
            mock.Setup(r => r.GetFilteredUsersDate(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listDate);
            mock.Setup(r => r.GetFilteredUsersHour(It.IsAny<DateTime>(), null)).Returns<DateTime, List<AppUser>>((a, b) => listHour);



            UserManager<AppUser> userManager = GetMockUserManager().Object;

            HomeController controller = new HomeController(mock.Object, userManager);

            AdvancedSearchViewModel modelTry = new AdvancedSearchViewModel() { Hour = DateTime.Now.Date };


            AdvancedSearchViewModel model = (controller.AdvancedSearch(modelTry) as ViewResult).ViewData.Model as AdvancedSearchViewModel;


            Assert.That(model.UserList[0].ToString(), Is.EqualTo("Hour1"));
            Assert.That(model.UserList[1].ToString(), Is.EqualTo("Hour2"));
            Assert.That(model.UserList[2].ToString(), Is.EqualTo("Hour3"));
            Assert.That(model.UserList[3].ToString(), Is.EqualTo("Hour4"));

        }

        [Test]
        public void NotificationChecked_Redirects_To_HomePage_View_In_HomeController()
        {

            Mock<IRepository> mock = new Mock<IRepository>();
            mock.Setup(r => r.ChangeNotificationToChecked(It.IsAny<int>(), It.IsAny<string>())).Returns(() => true);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            AppUser appUser = new AppUser();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
  {
    new Claim(ClaimTypes.Name, "example name"),
    new Claim(ClaimTypes.NameIdentifier, "1"),
    new Claim("custom-claim", "example claim value"),
  }, "mock"));


            mockUserManager
    .Setup(x => x.GetUserAsync(user))
    .ReturnsAsync(appUser);


            HomeController controller = new HomeController(mock.Object, mockUserManager.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            RedirectToActionResult result = (RedirectToActionResult)controller.NotificationChecked(1);




            Assert.AreEqual("HomePage", result.ActionName);

        }


        [Test]
        public void Find_Specialist_returns_4_Users_When_Ailments_Texts_Are_Null()
        {
            List<AppUser> listUsers = new List<AppUser>() { new AppUser() { UserName = "User1" }, new AppUser() { UserName = "User2" }, new AppUser() { UserName = "User3" } };
            SearchDoctorViewModel model = new SearchDoctorViewModel();
            HomePageViewModel homepageviewmodel = new HomePageViewModel();
            homepageviewmodel.Ailments = new List<Tag>() { new Tag() { Text = null }, new Tag() { Text = null }, new Tag() { Text = null }, new Tag() { Text = null } };
            model.Users = listUsers;
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.SearchForDoctor(It.IsAny<HomePageViewModel>())).Returns(() => model);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            HomeController controller = new HomeController(mockRepo.Object, mockUserManager.Object);


            SearchDoctorViewModel result = (SearchDoctorViewModel)(controller.FindSpecialist(homepageviewmodel) as PartialViewResult).Model;



            Assert.AreEqual(3, result.Users.Count);
            Assert.AreEqual(result.Users[1].ToString(), "User2");

        }

        [Test]
        public void Find_Specialist_returns_2_Users_When_Ailments_Texts_Arent_Null()
        {
            List<Tag> GetTags(Specialist specialist)
            {

                if (specialist is Ginekolog)
                {
                    return new List<Tag>() { new Tag() { Text = "złe samopoczucie " }, new Tag() { Text = "niepłodność" }, new Tag() { Text = "bezpłodność " } };
                }
                else if (specialist is Stomatolog)
                {
                    return new List<Tag>() { new Tag() { Text = "złe samopoczucie " }, new Tag() { Text = "ból zęba" } };
                }
                else if (specialist is Ortopeda)
                {
                    return new List<Tag>() { new Tag() { Text = "bóle mięśni " }, new Tag() { Text = "napięcie mięśni " }, new Tag() { Text = "złe samopoczucie " } };
                }

                return new List<Tag>();

            }

            List<AppUser> listUsersG = new List<AppUser>() { new AppUser() { UserName = "UserG" }, new AppUser() { UserName = "UserG" } };
            SearchDoctorViewModel modelG = new SearchDoctorViewModel();
            modelG.Users = listUsersG;

            List<AppUser> listUsersS = new List<AppUser>() { new AppUser() { UserName = "UserS" }, new AppUser() { UserName = "UserS" } };
            SearchDoctorViewModel modelS = new SearchDoctorViewModel();
            modelS.Users = listUsersS;

            List<AppUser> listUsersO = new List<AppUser>() { new AppUser() { UserName = "UserO" }, new AppUser() { UserName = "UserO" } };
            SearchDoctorViewModel modelO = new SearchDoctorViewModel();
            modelO.Users = listUsersO;
            SearchDoctorViewModel GetViewModel(HomePageViewModel viewmodel)
            {

                if (viewmodel.MedicalSpecialist == "Ginekolog")
                {
                    return modelG;
                }
                else if (viewmodel.MedicalSpecialist == "Ortopeda")
                {
                    return modelO;
                }
                else if (viewmodel.MedicalSpecialist == "Stomatolog")
                {
                    return modelS;
                }


                return null;


            }





            List<AppUser> listUsers = new List<AppUser>() { new AppUser() { UserName = "User1" }, new AppUser() { UserName = "User2" } };
            SearchDoctorViewModel model = new SearchDoctorViewModel();
            model.Users = listUsers;
            HomePageViewModel homepageviewmodel = new HomePageViewModel();
            homepageviewmodel.Ailments = new List<Tag>() { new Tag() { Text = "ból zęba" }, new Tag() { Text = null }, new Tag() { Text = "bezpłodność" }, new Tag() { Text = "bóle mięśni " } };

            Mock<IRepository> mockRepo = new Mock<IRepository>();

            mockRepo.Setup(r => r.GetTagsSpecialist(It.IsAny<Specialist>())).Returns<Specialist>((s) => GetTags(s));





            mockRepo.Setup(r => r.SearchForDoctor(It.IsAny<HomePageViewModel>())).Returns<HomePageViewModel>((m) => GetViewModel(m));


            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            HomeController controller = new HomeController(mockRepo.Object, mockUserManager.Object);


            SearchDoctorViewModel result = (SearchDoctorViewModel)(controller.FindSpecialist(homepageviewmodel) as PartialViewResult).Model;



            Assert.AreEqual(6, result.Users.Count);


        }











    }
    public class AdminTests
    {

        [Test]
        public void Delete_Method_Returns_Index_View_When_User_Is_Null()
        {
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(m => m.Users).Returns(() => null);

            Mock<IRepository> mockRepo = new Mock<IRepository>();

            AdminController controller = new AdminController(mockUserManager.Object);

            List<AppUser> result = (List<AppUser>)(controller.Delete("Empty").Result as ViewResult).Model;
            ViewResult viewResult = controller.Delete("Empty").Result as ViewResult;

            Assert.That(viewResult.ViewName == "Index");


        }




        [Test]
        public void Delete_Method_Redirects_To_Index_When_User_Isnt_Null()
        {
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            IdentityResult identity = Mock.Of<IdentityResult>(i => i.Succeeded == true);
            AppUser user = new AppUser() { UserName = "Test User" };

            async Task<AppUser> GetAppUser(string Id)
            {
                return user;
            }


            async Task<IdentityResult> GetIdentityResult(AppUser appUser)
            {
                return identity;
            }

            mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).Returns<string>((a) => GetAppUser(a));
            mockUserManager.Setup(m => m.DeleteAsync(It.IsAny<AppUser>())).Returns<AppUser>((a) => GetIdentityResult(a));

            Mock<IRepository> mockRepo = new Mock<IRepository>();

            AdminController controller = new AdminController(mockUserManager.Object);

            RedirectToActionResult Result = controller.Delete("Empty").Result as RedirectToActionResult;

            Assert.That(Result.ActionName == "Index");


        }



    }




    public class AppointmentTests
    {


        [Test]
        public void AppointmentDetails_Returns_Error_When_GetAppointmentById_Throws_Exception()
        {

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockRepo.Setup(r => r.GetAppointmentById(It.IsAny<int>())).Throws(new Exception("Exception from GetAppointmentById") { });

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());


            var result = (controller.AppointmentDetails(1) as ViewResult).ViewName;

            Assert.That(result == "Error");


        }

        [Test]
        public void AppointmentDetails_Returns_Model_When_GetAppointmentById_Returns_Appointment()
        {

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockRepo.Setup(r => r.GetAppointmentById(It.IsAny<int>())).Returns(() => new Appointment() { AppointmentId = 1,AppointmentDate=DateTime.Now,AppointmentStart=new DateTime(2019,10,18,11,00,00),AppointmentEnd= new DateTime(2019, 10, 18, 12, 00, 00), NumberOfPatients=1,PlacesAvailable=5 });
            mockRepo.Setup(r => r.GetBookedAppointments(It.IsAny<int>())).Returns<int>((a) => new List<int> {2,3,4,5 });

            AppUser appUser = new AppUser() {UserName="Check User",Patient=new Patient() {PatientId=1 } };

            async Task<AppUser> GetUser()
            {
                return new AppUser() { Id = "1", UserName = "Test User",PatientId=1, Patient = new Patient() { PatientId = 1 } };
            }

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate(), GetUser);


            ReserveAppointmentViewModel result = (ReserveAppointmentViewModel)(controller.AppointmentDetails(1) as ViewResult).Model;

            Assert.That(result.AppointmentId == 1);


        }



        [Test]
        public void ReserveAppointment_Returns_Error_View_When_ReserveAppointment_Doesnt_Complete()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockRepo.Setup(r => r.ReserveAppointment(It.IsAny<ReserveAppointmentViewModel>())).Returns(false);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());
            ViewResult result = controller.ReserveAppointment(new ReserveAppointmentViewModel()) as ViewResult;

            Assert.That(result.ViewName == "Error");

        }

        [Test]
        public void ReserveAppointment_Redirects_To_AppointmentDetails_When_ReserveAppointment_Is_Completed()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            mockRepo.Setup(r => r.ReserveAppointment(It.IsAny<ReserveAppointmentViewModel>())).Returns(true);
            mockRepo.Setup(r => r.GetPatientById(It.IsAny<int>())).Returns(new Patient() { PatientId = 1 });
            mockRepo.Setup(r => r.AddNotificationToPatient(It.IsAny<int>(), It.IsAny<Notification>())).Returns(true);
            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());
            RedirectToActionResult result = controller.ReserveAppointment(new ReserveAppointmentViewModel() {PatientId=1 }) as RedirectToActionResult;

            Assert.That(result.ActionName == "AppointmentDetails");


        }

        [Test]
        public void ShowAppointments_Returns_List_With_4_Elements()
        {
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            
            mockRepo.Setup(r => r.GetDoctorIdByUserId(It.IsAny<string>())).Returns<string>((a)=>1);

            Place Place1 = new Place() { PlaceId = 1, PlaceName = "Place 1", City = "City 1", Country = "Poland" };

            Place Place2 = new Place() { PlaceId = 1, PlaceName = "Place 2", City = "City 2", Country = "Poland" };

            List<Appointment> list = new List<Appointment>();

          
            list.Add(new Appointment() { AppointmentId = 1, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 2, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 3, Place = Place2 });
            list.Add(new Appointment() { AppointmentId = 4, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 5, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 6, Place = Place2 });

            mockRepo.Setup(r => r.GetUserAppointments(It.IsAny<int>())).Returns<int>((i) => list);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());

            List<Appointment> result=  (List<Appointment>)(controller.ShowAppointments(new ShowAppointmentViewModel() { City = "City 1", Country = "Poland" }) as ViewResult).Model;

            Assert.That(result.Count == 4);
            
        }

        [Test]
        public void ShowAppointments_Returns_List_With_0_Elements()
        {
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            Mock<IRepository> mockRepo = new Mock<IRepository>();

            mockRepo.Setup(r => r.GetDoctorIdByUserId(It.IsAny<string>())).Returns<string>((a) => 1);

            Place Place1 = new Place() { PlaceId = 1, PlaceName = "Place 1", City = "City 1", Country = "Poland" };

            Place Place2 = new Place() { PlaceId = 1, PlaceName = "Place 2", City = "City 2", Country = "Poland" };

            List<Appointment> list = new List<Appointment>();


            list.Add(new Appointment() { AppointmentId = 1, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 2, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 3, Place = Place2 });
            list.Add(new Appointment() { AppointmentId = 4, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 5, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 6, Place = Place2 });

            mockRepo.Setup(r => r.GetUserAppointments(It.IsAny<int>())).Returns<int>((i) => list);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());

            List<Appointment> result = (List<Appointment>)(controller.ShowAppointments(new ShowAppointmentViewModel() { City = "x", Country = "y" }) as ViewResult).Model;

            Assert.That(result.Count == 0);

        }

      

        [Test]
        public void ShowAppointmentsAdvanced_Returns_List_With_6_Elements()
        {
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            Mock<IRepository> mockRepo = new Mock<IRepository>();

            mockRepo.Setup(r => r.GetDoctorIdByUserId(It.IsAny<string>())).Returns<string>((a) => 1);

            Place Place1 = new Place() { PlaceId = 1, PlaceName = "Place 1", City = "City 1", Country = "Poland" };

            Place Place2 = new Place() { PlaceId = 1, PlaceName = "Place 2", City = "City 2", Country = "Poland" };

            List<Appointment> list = new List<Appointment>();


            list.Add(new Appointment() { AppointmentId = 1, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 2, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 3, Place = Place2 });
            list.Add(new Appointment() { AppointmentId = 4, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 5, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 6, Place = Place2 });

            mockRepo.Setup(r => r.GetUserAppointments(It.IsAny<int>())).Returns<int>((i) => list);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());

            List<Appointment> result = (List<Appointment>)(controller.ShowAppointmentsAdvanced(new ShowAppointmentViewModel() { City = "City 1", Country = "Poland" }) as ViewResult).Model;

            Assert.That(result.Count == 6);

        }

        [Test]
        public void ManageAppointments_Returns_List_With_4_Elements()
        {

           async Task<AppUser> GetUser()
            {
                return new AppUser() { UserName = "User", Doctor = new Doctor() { DoctorId = 1 }, DoctorId = 1 };
              }
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            Mock<IRepository> mockRepo = new Mock<IRepository>();

            mockRepo.Setup(r => r.GetDoctorIdByUserId(It.IsAny<string>())).Returns<string>((a) => 1);

            Place Place1 = new Place() { PlaceId = 1, PlaceName = "Place 1", City = "City 1", Country = "Poland" };

            Place Place2 = new Place() { PlaceId = 1, PlaceName = "Place 2", City = "City 2", Country = "Poland" };

            List<Appointment> list = new List<Appointment>();


            list.Add(new Appointment() { AppointmentId = 1, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 2, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 3, Place = Place2 });
            list.Add(new Appointment() { AppointmentId = 4, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 5, Place = Place1 });
            list.Add(new Appointment() { AppointmentId = 6, Place = Place2 });

            mockRepo.Setup(r => r.GetUserAppointments(It.IsAny<int>())).Returns<int>((i) => list);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate(), GetUser );

            List<Appointment> result = (List<Appointment>)(controller.ManageAppointments() as ViewResult).Model;

            Assert.That(result.Count == 6);

        }


        [Test]
        public void RemoveAppointment_Returns_Error_View_When_Remove_Of_Appointment_Faild()
        {

            async Task<AppUser> GetUser()
            {
                return new AppUser() { UserName = "User", Doctor = new Doctor() { DoctorId = 1 }, DoctorId = 1 };
            }

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.RemoveAppointment(It.IsAny<int>())).Returns(false);
            Patient patient = new Patient() { PatientId = 2 };
            PatientAppointment patientAppointment = new PatientAppointment() { AppointmentId = 1, PatientId = 2,Patient=patient };
            mockRepo.Setup(r => r.GetAppointmentByIdAllData(It.IsAny<int>())).Returns(() => new Appointment() {AppointmentId=1,PatientAppointments=new List<PatientAppointment>() { new PatientAppointment() {PatientId=2,AppointmentId=1 } } });
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);


            AppointmentController controller = new AppointmentController(mockRepo.Object,mockUserManager.Object, new TimeAndDate(), GetUser);

            ViewResult result = controller.RemoveAppointment(1) as ViewResult;

            Assert.That(result.ViewName=="Error");
        }

        [Test]
        public void RemoveAppointment_Redirects_To_ManageAppointments_When_Remove_Of_Appointment_Ends_Succes()
        {

            async Task<AppUser> GetUser()
            {
                return new AppUser() { UserName = "User", Doctor = new Doctor() { DoctorId = 1 }, DoctorId = 1 };
            }

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.RemoveAppointment(It.IsAny<int>())).Returns(true);
            mockRepo.Setup(r => r.GetPatientById(It.IsAny<int>())).Returns(() => new Patient() {PatientId=2,Notifications=new List<Notification>() { } });
            mockRepo.Setup(r => r.AddNotificationToPatient(It.IsAny<int>(), It.IsAny<Notification>())).Returns(true);
            Patient patient = new Patient() { PatientId = 2 };
            PatientAppointment patientAppointment = new PatientAppointment() { AppointmentId = 1, PatientId = 2, Patient = patient };
            mockRepo.Setup(r => r.GetAppointmentByIdAllData(It.IsAny<int>())).Returns(() => new Appointment() { AppointmentId = 1, PatientAppointments = new List<PatientAppointment>() { new PatientAppointment() { PatientId = 2, AppointmentId = 1 } } });
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);


            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate(), GetUser);

            RedirectToActionResult result = controller.RemoveAppointment(1) as RedirectToActionResult;

            Assert.That(result.ActionName == "ManageAppointments");
        }


        [Test]
        public void AddPlace_Returns_View_ChoosePlace_When_Model_Is_Valid()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddPlace(It.IsAny<Place>())).Returns(true);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());

            RedirectToActionResult result = controller.AddPlace(new Place() { }) as RedirectToActionResult;

            Assert.That(result.ActionName == "ChoosePlace");

        }



        [Test]
        public void AddPlace_Returns_Model_When_Model_Is_Not_Valid()
        {

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddPlace(It.IsAny<Place>())).Returns(true);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());

            controller.ModelState.AddModelError("Country", "Country is required");

            Place result = (Place)(controller.AddPlace(new Place() {City="Test City" }) as ViewResult).Model;

            Assert.That(result.City == "Test City");



        }


        [Test]
        public void ChoosePlace_Returns_ChoosePlace_View()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();

            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());

            ViewResult result = controller.ChoosePlace() as ViewResult;

            Assert.That(result.ViewName == "ChoosePlace");

        }

        [Test]
        public void ShowPlaces_Returns_List_Of_Places_When_Model_State_Is_Valid()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();

            List<Place> list = new List<Place>();
            list.Add(new Place() { PlaceId = 1, PlaceName = "Place 1" });
            list.Add(new Place() { PlaceId = 2, PlaceName = "Place 2" });
            list.Add(new Place() { PlaceId = 3, PlaceName = "Place 3" });
            list.Add(new Place() { PlaceId = 4, PlaceName = "Place 4" });

            mockRepo.Setup(r => r.SelectPlaces(It.IsAny<SelectPlaceViewModel>())).Returns(() => list);

            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());

            List<Place> result = (List<Place>)(controller.ShowPlaces(new SelectPlaceViewModel()) as PartialViewResult).Model;

            Assert.That(result.Count == 4);


        }

        [Test]
        public void ShowPlaces_Returns_View_ChoosePlace_When_Model_State_Isnt_Valid()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();

            List<Place> list = new List<Place>();
            list.Add(new Place() { PlaceId = 1, PlaceName = "Place 1" });
            list.Add(new Place() { PlaceId = 2, PlaceName = "Place 2" });
            list.Add(new Place() { PlaceId = 3, PlaceName = "Place 3" });
            list.Add(new Place() { PlaceId = 4, PlaceName = "Place 4" });

            mockRepo.Setup(r => r.SelectPlaces(It.IsAny<SelectPlaceViewModel>())).Returns(() => list);

            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object,new TimeAndDate());
           

            controller.ModelState.AddModelError("City", "City is required");

            ViewResult result =controller.ShowPlaces(new SelectPlaceViewModel()) as ViewResult;

            Assert.That(result.ViewName == "ChoosePlace");
        }


        [Test]
        public void AddAppointment_Returns_Model()
        {

            async Task<AppUser> GetUser()
            {
                return new AppUser() { UserName = "User", Doctor = new Doctor() { DoctorId = 1 }, DoctorId = 1 };
            }

            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            Mock<IRepository> mockRepo = new Mock<IRepository>();


            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object,new TimeAndDate(), GetUser);

            AddAppointmentViewModel result = (AddAppointmentViewModel)(controller.AddAppointment(1) as ViewResult).Model;

           Assert.That(result.PlaceId == 1);

        }

        [Test]
        public void AddAppointment_Model_State_Is_Valid_Returns_Model()
        {
            Mock<ITimeAndDate> mockTime = new Mock<ITimeAndDate>();
            mockTime.Setup(t => t.GetTime()).Returns(() => new DateTime(2019, 10, 21, 10, 0, 0));

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddAppointment(It.IsAny<AddAppointmentViewModel>())).Returns(false);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, mockTime.Object);

            AddAppointmentViewModel result = (AddAppointmentViewModel)(controller.AddAppointment(new AddAppointmentViewModel() { DoctorId = 1, PlaceId = 1, Appointment = new Appointment() { AppointmentDate = new DateTime(2019, 10, 21), AppointmentStart = new DateTime(2019, 10, 20, 11, 0, 0), AppointmentEnd = new DateTime(2019, 10, 20, 12, 0, 0) } }) as ViewResult).Model;

            Assert.That(result.PlaceId == 1);




        }


        [Test]
        public void AddAppointment_Model_State_Isnt_Valid_Returns_Model()
        {
            Mock<ITimeAndDate> mockTime = new Mock<ITimeAndDate>();
            mockTime.Setup(t => t.GetTime()).Returns(() => new DateTime(2019, 10, 21, 10, 0, 0));

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddAppointment(It.IsAny<AddAppointmentViewModel>())).Returns(false);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, mockTime.Object);

            RedirectToActionResult result = controller.AddAppointment(new AddAppointmentViewModel() { DoctorId = 1, PlaceId = 1, Appointment = new Appointment() { AppointmentDate = null, AppointmentStart = new DateTime(2019, 10, 20, 19, 0, 0), AppointmentEnd = new DateTime(2019, 10, 20, 19, 0, 0) } } ) as RedirectToActionResult;

            Assert.That(result.ActionName == "ManageAppointments");


        }



    }





    public class RoleAdminTests
    {












    }















}