using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PogotowieCom.Controllers;
using PogotowieCom.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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

            AdminController controller = new AdminController(mockUserManager.Object, mockRepo.Object);

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

            AdminController controller = new AdminController(mockUserManager.Object, mockRepo.Object);

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

            mockRepo.Setup(r => r.GetAppointmentById(It.IsAny<int>())).Returns(() => new Appointment() { AppointmentId = 1, AppointmentDate = DateTime.Now, AppointmentStart = new DateTime(2019, 10, 18, 11, 00, 00), AppointmentEnd = new DateTime(2019, 10, 18, 12, 00, 00), NumberOfPatients = 1, PlacesAvailable = 5 });
            mockRepo.Setup(r => r.GetBookedAppointments(It.IsAny<int>())).Returns<int>((a) => new List<int> { 2, 3, 4, 5 });

            AppUser appUser = new AppUser() { UserName = "Check User", Patient = new Patient() { PatientId = 1 } };

            async Task<AppUser> GetUser()
            {
                return new AppUser() { Id = "1", UserName = "Test User", PatientId = 1, Patient = new Patient() { PatientId = 1 } };
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
            RedirectToActionResult result = controller.ReserveAppointment(new ReserveAppointmentViewModel() { PatientId = 1 }) as RedirectToActionResult;

            Assert.That(result.ActionName == "AppointmentDetails");


        }

        [Test]
        public void ShowAppointments_Returns_List_With_4_Elements()
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

            List<Appointment> result = (List<Appointment>)(controller.ShowAppointments(new ShowAppointmentViewModel() { City = "City 1", Country = "Poland" }) as ViewResult).Model;

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

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate(), GetUser);

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
            PatientAppointment patientAppointment = new PatientAppointment() { AppointmentId = 1, PatientId = 2, Patient = patient };
            mockRepo.Setup(r => r.GetAppointmentByIdAllData(It.IsAny<int>())).Returns(() => new Appointment() { AppointmentId = 1, PatientAppointments = new List<PatientAppointment>() { new PatientAppointment() { PatientId = 2, AppointmentId = 1 } } });
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            Mock<UserManager<AppUser>> mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);


            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate(), GetUser);

            ViewResult result = controller.RemoveAppointment(1) as ViewResult;

            Assert.That(result.ViewName == "Error");
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
            mockRepo.Setup(r => r.GetPatientById(It.IsAny<int>())).Returns(() => new Patient() { PatientId = 2, Notifications = new List<Notification>() { } });
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

            Place result = (Place)(controller.AddPlace(new Place() { City = "Test City" }) as ViewResult).Model;

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

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate());


            controller.ModelState.AddModelError("City", "City is required");

            ViewResult result = controller.ShowPlaces(new SelectPlaceViewModel()) as ViewResult;

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


            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, new TimeAndDate(), GetUser);

            AddAppointmentViewModel result = (AddAppointmentViewModel)(controller.AddAppointment(1) as ViewResult).Model;

            Assert.That(result.PlaceId == 1);

        }

        [Test]
        public void AddAppointment_Model_State_Isnt_Valid_Returns_View()
        {
            Mock<ITimeAndDate> mockTime = new Mock<ITimeAndDate>();
            mockTime.Setup(t => t.GetTime()).Returns(() => new DateTime(2019, 10, 21, 10, 0, 0));

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddAppointment(It.IsAny<AddAppointmentViewModel>())).Returns(false);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, mockTime.Object);

            ViewResult result = (ViewResult)(controller.AddAppointment(new AddAppointmentViewModel() { DoctorId = 1, PlaceId = 1, Appointment = new Appointment() { AppointmentDate = new DateTime(2019, 10, 21), AppointmentStart = new DateTime(2019, 10, 20, 11, 0, 0), AppointmentEnd = new DateTime(2019, 10, 20, 12, 0, 0) } }) as ViewResult);

            Assert.That(result.ViewName == "AddAppointment");




        }


        [Test]
        public void AddAppointment_Appointment_lasts_two_days_returns_Model()
        {
            Mock<ITimeAndDate> mockTime = new Mock<ITimeAndDate>();
            mockTime.Setup(t => t.GetTime()).Returns(() => new DateTime(2018, 10, 21, 10, 0, 0));

            Mock<IRepository> mockRepo = new Mock<IRepository>();
        mockRepo.Setup(r => r.AddAppointment(It.IsAny<AddAppointmentViewModel>())).Returns(false);
        var mockUserStore = new Mock<IUserStore<AppUser>>();
        var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

        AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, mockTime.Object);

        ViewResult result = (ViewResult)(controller.AddAppointment(new AddAppointmentViewModel() { DoctorId = 1, PlaceId = 1, Appointment = new Appointment() { AppointmentDate = new DateTime(2019, 10, 20), AppointmentStart = new DateTime(2019, 10, 20, 11, 0, 0), AppointmentEnd = new DateTime(2019, 10, 21, 12, 0, 0) } }) as ViewResult);


            Assert.That(result.ViewName == "AddAppointment");
            


        }


        [Test]
        public void AddAppointment_Time_Of_Reservation_Is_Blocked_Returns_View()
        {
            Mock<ITimeAndDate> mockTime = new Mock<ITimeAndDate>();
            mockTime.Setup(t => t.GetTime()).Returns(() => new DateTime(2018, 10, 21, 10, 0, 0));

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddAppointment(It.IsAny<AddAppointmentViewModel>())).Returns(false);
            mockRepo.Setup(r => r.CheckIfAppointmentExists(It.IsAny<AddAppointmentViewModel>())).Returns(true);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, mockTime.Object);

            ViewResult result = (ViewResult)(controller.AddAppointment(new AddAppointmentViewModel() { DoctorId = 1, PlaceId = 1, Appointment = new Appointment() { AppointmentDate = new DateTime(2019, 10, 20), AppointmentStart = new DateTime(2019, 10, 20, 11, 0, 0), AppointmentEnd = new DateTime(2019, 10, 21, 12, 0, 0) } }) as ViewResult);


            Assert.That(result.ViewName == "AddAppointment");



        }


        [Test]
        public void AddAppointment_Time_Of_Reservation_Isnt_Blocked_Redirects()
        {
            Mock<ITimeAndDate> mockTime = new Mock<ITimeAndDate>();
            mockTime.Setup(t => t.GetTime()).Returns(() => new DateTime(2018, 10, 21, 10, 0, 0));

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddAppointment(It.IsAny<AddAppointmentViewModel>())).Returns(false);
            mockRepo.Setup(r => r.CheckIfAppointmentExists(It.IsAny<AddAppointmentViewModel>())).Returns(false);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, mockTime.Object);


            RedirectToActionResult result = controller.AddAppointment(new AddAppointmentViewModel() { DoctorId = 1, PlaceId = 1, Appointment = new Appointment() { AppointmentDate = null, AppointmentStart = new DateTime(2019, 10, 20, 19, 0, 0), AppointmentEnd = new DateTime(2019, 10, 20, 19, 0, 0) } }) as RedirectToActionResult;

            Assert.That(result.ActionName == "ManageAppointments");



        }



        [Test]
        public void AddAppointment_Model_State_Is_Valid_Redirects()
        {
            Mock<ITimeAndDate> mockTime = new Mock<ITimeAndDate>();
            mockTime.Setup(t => t.GetTime()).Returns(() => new DateTime(2019, 10, 21, 10, 0, 0));

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddAppointment(It.IsAny<AddAppointmentViewModel>())).Returns(false);
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            AppointmentController controller = new AppointmentController(mockRepo.Object, mockUserManager.Object, mockTime.Object);

            RedirectToActionResult result = controller.AddAppointment(new AddAppointmentViewModel() { DoctorId = 1, PlaceId = 1, Appointment = new Appointment() { AppointmentDate = null, AppointmentStart = new DateTime(2019, 10, 20, 19, 0, 0), AppointmentEnd = new DateTime(2019, 10, 20, 19, 0, 0) } }) as RedirectToActionResult;

            Assert.That(result.ActionName == "ManageAppointments");


        }



    }





    public class RoleAdminTests
    {
        //[Test]
        //public void Edit_Returns_RoleEditModel()
        //{


        //    async Task<bool> CheckRole(AppUser user, string role)
        //    {
        //        if (user.ChooseRole == "User")
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    var mockUserStore = new Mock<IUserStore<AppUser>>();
        //    var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);


        //    List<AppUser> list = new List<AppUser>() { new AppUser() {Id="1",UserName="User 1" ,ChooseRole="User"},
        //        new AppUser() {Id="2",UserName="User 2",ChooseRole="User" },
        //        new AppUser() {Id="3",UserName="User 3",ChooseRole="None" },
        //        new AppUser() {Id="4",UserName="User 4",ChooseRole="None" },
        //        new AppUser() {Id="5",UserName="User 5" ,ChooseRole="User"}


        //    };

        //    mockUserManager.Setup(m => m.Users).Returns(list.AsQueryable());

        //    mockUserManager.Setup(m => m.IsInRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Returns<AppUser, string>((a, b) => CheckRole(a, b));

        //    var roleStore = new Mock<IRoleStore<IdentityRole>>();
        //    Mock<RoleManager<IdentityRole>> roleManager = new Mock<RoleManager<IdentityRole>>(
        //                 roleStore.Object, null, null, null, null);

        //    async Task<IdentityRole> GetRole(string role)
        //    {
        //        return new IdentityRole() { Name = "User", Id = "Id" };
        //    }

        //    roleManager.Setup(r => r.FindByIdAsync(It.IsAny<string>())).Returns<string>((a) => GetRole(a));

        //    RoleAdminController controller = new RoleAdminController(roleManager.Object, mockUserManager.Object);

        //    RoleEditModel result = (RoleEditModel)(controller.Edit("Id").Result as ViewResult).Model;



        //    Assert.That(result.Members.ToList().Count == 3);
        //    Assert.That(result.NonMembers.ToList().Count == 2);

        //}


        //[Test]
        //public void When_Model_Isnt_Valid_Edit_Returns_Model_String()
        //{

        //    RoleModificationModel model = new RoleModificationModel() { RoleId = "User Role" };

        //    var mockUserStore = new Mock<IUserStore<AppUser>>();
        //    var mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

        //    var roleStore = new Mock<IRoleStore<IdentityRole>>();
        //    Mock<RoleManager<IdentityRole>> roleManager = new Mock<RoleManager<IdentityRole>>(
        //                 roleStore.Object, null, null, null, null);



        //    RoleAdminController controller = new RoleAdminController(roleManager.Object,mockUserManager.Object);

        //    controller.ModelState.AddModelError("RoleName", "RoleName is required");

        //    var result = controller.Edit(model).Result as OkObjectResult;


        //    Assert.That(result.Value.ToString() == "User Role");

        //}








    }


    public class UserControllerTests
    {


        class IdentityResultMock : SignInResult
        {
            public IdentityResultMock(bool succeeded = false)
            {
                this.Succeeded = succeeded;
            }
        }





        [Test]
        public void ChooseRole_Returns_List()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            List<IdentityRole> Roles = new List<IdentityRole>();
            Roles.Add(new IdentityRole() { Id = "Doctor", Name = "Doctor" });
            Roles.Add(new IdentityRole() { Id = "User", Name = "User" });
            Roles.Add(new IdentityRole() { Id = "Patient", Name = "Patient" });
            Roles.Add(new IdentityRole() { Id = "Administrator", Name = "Administrator" });


            _mockRoleManager.Setup(r => r.Roles).Returns(() => Roles.AsQueryable());


            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            var result = (string[])(controller.ChooseRole() as ViewResult).Model;


            Assert.That(result.ToList<string>().Count == 3);
        }

        [Test]
        public void RemoveSpecialization_Redirects_To_ManageSpecializations()
        {

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(m => m.DeleteDoctorSpecialization(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);


            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            ManageSpecializationsViewModel model = new ManageSpecializationsViewModel() { SpecializationId = 1, UserId = "User" };

            var result = (controller.RemoveSpecialization(model) as RedirectToActionResult).ActionName;

            Assert.That(result == "ManageSpecializations");


        }



        [Test]
        public void ManageSpecializations_Returns_Model()
        {

            async Task<AppUser> GetUser()
            {
                return new AppUser() { Id = "User666" };
            }

          

          


            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(m => m.GetDoctorSpecializations(It.IsAny<string>())).Returns(() => new List<Specialization>() { new Specialization() { Name = "Dentysta" } });

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);


          


            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object, GetUser);



            var result = (ManageSpecializationsViewModel)(controller.ManageSpecializations() as ViewResult).Model;

            Assert.That(result.SpecializationName == "None");
            Assert.That(result.UserId == "User666");
            Assert.That(result.specializations[0].Name == "Dentysta");







        }


        [TestCase("Pacjent", "PatientView")]
        [TestCase("Doktor", "DoctorView")]
        [TestCase("None", "Error")]
        public void Create_Returns_View(string Role, string resultViewName)
        {

            Mock<IRepository> mockRepo = new Mock<IRepository>();

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);






            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            ViewResult result = controller.Create(Role) as ViewResult;




            Assert.That(result.ViewName == resultViewName);
        }


        [Test]
        public void CreatePatient_Redirects_To_Action_When_Succes()
        {
            //Mock<IRepository> mockRepo = new Mock<IRepository>();

            //var userStoreMock = new Mock<IUserStore<AppUser>>();

            //var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
            //     null, null, null, null, null, null, null, null);

            //IdentityResult identity = Mock.Of<IdentityResult>(i => i.Succeeded == true);

            //async   Task<IdentityResult> Create(AppUser user,string Password)
            //{
            //    return identity;
            //}

            //_mockUserManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(),It.IsAny<string>())).Returns<Task<IdentityResult>>((a,b) =>Create(a,b) );

            //var contextAccessor = new Mock<IHttpContextAccessor>();
            //var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            //var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
            //    contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            //var roleStore = new Mock<IRoleStore<IdentityRole>>();
            //var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
            //             roleStore.Object, null, null, null, null);






            //UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);









        }



        [Test]
        public void CreatePatient_Returns_PartialView_When_Create_Fails()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            IdentityResult identity = Mock.Of<IdentityResult>(i => i.Succeeded == false);

            async Task<IdentityResult> Create(AppUser user, string Password)
            {
                return identity;
            }

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Returns<AppUser, string>((a, b) => Create(a, b));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            ViewResult result = controller.CreatePatient(new CreatePatientModel() { Password = "Password", Name = "User", Surname = "Surname User" }).Result as ViewResult;


            Assert.That(result.ViewName == "PatientView");


        }

        [Test]
        public void CreatePatient_Redirects_To_HomePage_Action()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddPatientToUser(It.IsAny<Patient>(), It.IsAny<string>())).Returns(true);
            mockRepo.Setup(r => r.AddRoleToUser(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => AddRole(a, b));

            async Task<bool> AddRole(string user, string Password)
            {
                return true;
            }

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            IdentityResult identity = Mock.Of<IdentityResult>(i => i.Succeeded == true);

            async Task<IdentityResult> Create(AppUser user, string Password)
            {
                return identity;
            }

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Returns<AppUser, string>((a, b) => Create(a, b));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            RedirectToActionResult result = controller.CreatePatient(new CreatePatientModel() { Password = "Password", Name = "User", Surname = "Surname User", ChooseRole = "Patient" }).Result as RedirectToActionResult;


            Assert.That(result.ActionName == "HomePage");


        }




        [Test]
        public void CreateDoctor_Returns_View_When_Create_Fails()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            IdentityResult identity = Mock.Of<IdentityResult>(i => i.Succeeded == false);

            async Task<IdentityResult> Create(AppUser user, string Password)
            {
                return identity;
            }

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Returns<AppUser, string>((a, b) => Create(a, b));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            ViewResult result = controller.CreateDoctor(new CreateDoctorModel() { Password = "Password", Name = "User", Surname = "Surname User", PriceForVisit = 100 }).Result as ViewResult;


            Assert.That(result.ViewName == "DoctorView");


        }

        [Test]
        public void CreateDoctor_Redirects_To_AddDoctorDetails_Action()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddDoctorToUser(It.IsAny<Doctor>(), It.IsAny<string>())).Returns(true);
            mockRepo.Setup(r => r.AddRoleToUser(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => AddRole(a, b));

            async Task<bool> AddRole(string user, string Password)
            {
                return true;
            }

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            IdentityResult identity = Mock.Of<IdentityResult>(i => i.Succeeded == true);

            async Task<IdentityResult> Create(AppUser user, string Password)
            {
                return identity;
            }

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Returns<AppUser, string>((a, b) => Create(a, b));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            RedirectToActionResult result = controller.CreateDoctor(new CreateDoctorModel() { Password = "Password", Name = "User", Surname = "Surname User", PriceForVisit = 100 }).Result as RedirectToActionResult;


            Assert.That(result.ActionName == "AddDoctorDetails");


        }

        [Test]
        public void AddDoctorDetails_Returns_DoctorDetailsViewModel()
        {

            List<Specialization> GetSpecializations()
            {
                return new List<Specialization>() { new Specialization() { Name = "Specialization1" }, new Specialization() { Name = "Specialization2" }, new Specialization() { Name = "Specialization3" } };
            }


            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.Specializations).Returns(() => GetSpecializations().AsQueryable());

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);


            async Task<AppUser> GetUser()
            {
                return new AppUser() { DoctorId = 1 };
            }

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object, GetUser);


            DoctorDetailsViewModel result = (DoctorDetailsViewModel)(controller.AddDoctorDetails(new AppUser()) as ViewResult).Model;



            Assert.That(result.SpecializationList.Count == 3);
            Assert.That(result.DoctorId == 1);








        }

        [Test]
        public void AddDoctorDetails_Redirects_To_ManageSpecializations()
        {
            List<Specialization> GetSpecializations()
            {
                return new List<Specialization>() { new Specialization() { Name = "Specialization1" }, new Specialization() { Name = "Specialization2" }, new Specialization() { Name = "Specialization3" } };
            }


            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.AddSpecializationToDoctor(It.IsAny<int>(), It.IsAny<string>())).Returns(() => true);

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);




            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);


            RedirectToActionResult result = controller.AddDoctorDetails(new DoctorDetailsViewModel() { DoctorId = 1, Specialization = new Specialization() { Name = "Spec Name" } }) as RedirectToActionResult;



            Assert.That(result.ActionName == "ManageSpecializations");


        }

        [Test]
        public void Login_Returns_Model_Login_When_Model_Isnt_Valid()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();

            var userStoreMock = new Mock<IUserStore<AppUser>>();

            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                 null, null, null, null, null, null, null, null);



            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);


            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            LoginModel result = (LoginModel)(controller.Login(new LoginModel() { Email = "Email" }, "Return Url").Result as ViewResult).Model;


            Assert.That(result.Email == "Email");




        }



        [Test]
        public void Login_Redirects_When_Model_Is_Valid()
        {
            Mock<IRepository> mockRepo = new Mock<IRepository>();





            async Task<AppUser> FindByEmail(string email)
            {
                return new AppUser() { UserName = "User", Id = "UserId", };
            }

            async Task<AppUser> SignOut()
            {
                return new AppUser() { UserName = "User SignOut", Id = "UserId", };
            }
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            var _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                    null, null, null, null, null, null, null, null);
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).Returns<string>((e) => FindByEmail(e));

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();

            var _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);

            _mockSignInManager.Setup(s => s.SignOutAsync()).Returns(() => SignOut());





            async Task<Microsoft.AspNetCore.Identity.SignInResult> GetSignInResult(AppUser appuser, string password, bool one, bool two)
            {
                return new IdentityResultMock(true);
            }

            _mockSignInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns<AppUser, string, bool, bool>((a, b, c, d) => GetSignInResult(a, b, c, d));



            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                         roleStore.Object, null, null, null, null);

            UserController controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, mockRepo.Object);

            RedirectResult result = controller.Login(new LoginModel() { Email = "Email", Password = "Password" }, "Return Url").Result as RedirectResult;




            Assert.That(result.Url == "Return Url");





        }



        public class CommentControllerTests
        {

            [Test]
            public void DoctorRank_Returns_Model_Count_1()
            {

                Mock<ITimeAndDate> timeMock = new Mock<ITimeAndDate>();


                Mock<IRepository> mockRepo = new Mock<IRepository>();
                DoctorRankViewModel model = new DoctorRankViewModel();
                List<DoctorRankViewModel> list = new List<DoctorRankViewModel>();
                list.Add(model);
                mockRepo.Setup(x => x.GetCommentDetails()).Returns(() => list);

                var userStoreMock = new Mock<IUserStore<AppUser>>();

                var mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                     null, null, null, null, null, null, null, null);


                CommentController controller = new CommentController(mockRepo.Object, mockUserManager.Object, timeMock.Object);

                List<DoctorRankViewModel> result = (List<DoctorRankViewModel>)(controller.DoctorRank() as ViewResult).Model;

                Assert.That(result.Count() == 1);



            }

            [Test]
            public void ShowComments_Returns_Model()
            {

                Mock<ITimeAndDate> timeMock = new Mock<ITimeAndDate>();

                Mock<IRepository> mockRepo = new Mock<IRepository>();
                DoctorRankViewModel model = new DoctorRankViewModel();
                List<DoctorRankViewModel> list = new List<DoctorRankViewModel>();
                list.Add(model);

                ShowCommentsViewModel ShowComments(string Id)
                {
                    if (Id == "UserId")
                    {
                        return new ShowCommentsViewModel() { };
                    }
                    else
                    {
                        return null;
                    }
                }

                mockRepo.Setup(x => x.GetCommentsAndDoctorData(It.IsAny<string>())).Returns<string>((s) => ShowComments(s));

                var userStoreMock = new Mock<IUserStore<AppUser>>();

                var mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                     null, null, null, null, null, null, null, null);


                CommentController controller = new CommentController(mockRepo.Object, mockUserManager.Object, timeMock.Object);

                ShowCommentsViewModel result = (ShowCommentsViewModel)(controller.ShowComments("UserId") as ViewResult).Model;

                Assert.That(result != null);



            }



            [Test]
            public void AddCommentAndVote_Returns_View()
            {
                Mock<ITimeAndDate> timeMock = new Mock<ITimeAndDate>();

                Mock<IRepository> mockRepo = new Mock<IRepository>();


                var userStoreMock = new Mock<IUserStore<AppUser>>();

                var mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                     null, null, null, null, null, null, null, null);

                CommentController controller = new CommentController(mockRepo.Object, mockUserManager.Object, timeMock.Object);

                AddVoteCommentViewModel model = new AddVoteCommentViewModel() { Message = "Text is Empty" };

                AddVoteCommentViewModel result = (AddVoteCommentViewModel)(controller.AddCommentAndVote(model) as ViewResult).Model;

                Assert.That(result.Message == "Text is Empty");




            }

            [Test]
            public void AddCommentAndVote_Returns_View_Error()
            {
                Mock<ITimeAndDate> timeMock = new Mock<ITimeAndDate>();

                Mock<IRepository> mockRepo = new Mock<IRepository>();
                mockRepo.Setup(r => r.ChangeComment(It.IsAny<Comment>())).Returns(false);

                var userStoreMock = new Mock<IUserStore<AppUser>>();

                var mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                     null, null, null, null, null, null, null, null);

                CommentController controller = new CommentController(mockRepo.Object, mockUserManager.Object, timeMock.Object);

                AddVoteCommentViewModel model = new AddVoteCommentViewModel() { Message = "Text is Empty", Comment = new Comment() { Text = "Text" } };

                ViewResult result = controller.AddCommentAndVote(model) as ViewResult;

                Assert.That(result.ViewName == "Error");





            }


            [Test]
            public void AddCommentAndVote_Redirects_To_Action()
            {
                Mock<ITimeAndDate> timeMock = new Mock<ITimeAndDate>();
                timeMock.Setup(t => t.GetTime()).Returns(new DateTime(2019, 11, 11));

                Mock<IRepository> mockRepo = new Mock<IRepository>();
                mockRepo.Setup(r => r.ChangeComment(It.IsAny<Comment>())).Returns(true);

                var userStoreMock = new Mock<IUserStore<AppUser>>();

                var mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                     null, null, null, null, null, null, null, null);

                CommentController controller = new CommentController(mockRepo.Object, mockUserManager.Object, timeMock.Object);

                AddVoteCommentViewModel model = new AddVoteCommentViewModel() { Message = "Text is Empty", Comment = new Comment() { Text = "Text" } };

                RedirectToActionResult result = controller.AddCommentAndVote(model) as RedirectToActionResult;

                Assert.That(result.ActionName == "UsersPanel");
                Assert.That(result.ControllerName == "Home");


            }

            [Test]
            public void AddCommentAndVote_Returns_Model()
            {
                Mock<ITimeAndDate> timeMock = new Mock<ITimeAndDate>();

                Mock<IRepository> mockRepo = new Mock<IRepository>();
                mockRepo.Setup(r => r.GetUserByDoctorId(It.IsAny<int>())).Returns<int>((i) => GetUserInt(i).Result);

                CommentData data = new CommentData() { user = new AppUser() { Id = "AppUserId", UserName = "User1" }, appointment = new Appointment() { AppointmentId = 99, AppointmentEnd = new DateTime(2019, 11, 2), Doctor = new Doctor() { DoctorId = 2 }, DoctorId = 2 } };


                async Task<AppUser> GetUserInt(int i)
                {
                    return new AppUser() { Id = "1", UserName = "Test User", Surname = "Surname", PatientId = 1, Patient = new Patient() { PatientId = 1 } };
                }

                async Task<AppUser> GetUser()
                {
                    return new AppUser() { Id = "1", UserName = "Test User", Surname = "Surname", PatientId = 1, Patient = new Patient() { PatientId = 1 } };
                }


                mockRepo.Setup(r => r.CommentAndVoteCheck(It.IsAny<AppUser>())).Returns<AppUser>((u) => data);

                var userStoreMock = new Mock<IUserStore<AppUser>>();

                var mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object,
                     null, null, null, null, null, null, null, null);

                CommentController controller = new CommentController(mockRepo.Object, mockUserManager.Object, timeMock.Object, GetUser);

                AddVoteCommentViewModel model = (AddVoteCommentViewModel)(controller.AddCommentAndVote() as ViewResult).Model;



                string compare = "Prosimy o komentarz i ocenę dotyczącą wizyty u Test User Surname z dnia 2019-11-02";



                Assert.That(model.Message == compare);



            }




        }






    }












}