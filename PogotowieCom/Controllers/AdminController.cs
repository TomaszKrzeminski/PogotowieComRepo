using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PogotowieCom.Models;

namespace PogotowieCom.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IRepository repository;

        public AdminController(UserManager<AppUser> usrMgr, IRepository repo)
        {
            userManager = usrMgr;
            this.repository = repo;
        }


        public ViewResult Index()
        {
            return View(userManager.Users);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ShowUsers");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika");
            }

            return View("Index", userManager.Users);




        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }


        public ViewResult AdminPanel()
        {
            return View();
        }

        public ViewResult ShowUsers()
        {

            ShowUsersViewModel model = new ShowUsersViewModel();
            model.Users = repository.GetAllUsers();

            return View(model);
        }


    }
}