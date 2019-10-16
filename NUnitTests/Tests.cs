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



            Assert.AreEqual(3,result.Users.Count);
            Assert.AreEqual(result.Users[1].ToString(), "User2");

        }













    }
}