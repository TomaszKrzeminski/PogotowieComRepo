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
    public class UserController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        public UserController(UserManager<AppUser> usrMgr, SignInManager<AppUser> signinMgr)
        {
            userManager = usrMgr;
            signInManager = signinMgr;
        }


        public ViewResult Create()
        {
            return View();
        }





        [HttpPost]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user,
                            model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
            }

        }
       
  


[HttpPost]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Surname = model.Surname,
                    City = model.City,
                    ZipCode = model.ZipCode,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber


                };


                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("HomePage","Home",null);
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            return View(model);
        }






        public ViewResult ShowUsers()
        {
            return View(new Dictionary<string, object> { ["Miejsce zarezerwowane"] = "Miejsce zarezerwowane" });
        }


        ///Login
        ///
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }



        public async Task<IActionResult> LogOut()
        {
            try
            {
                await signInManager.SignOutAsync();
            }
            catch
            {

            }


            return RedirectToAction("HomePage", "Home", null);
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }

                ModelState.AddModelError(nameof(LoginModel.Email), "Nieprawidłowa nazwa użytkownika lub hasłó");
            }

            return View(details);
        }



    }
}