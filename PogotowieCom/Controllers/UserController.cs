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
        private RoleManager<IdentityRole> roleManager;
        private IRepository repository;
        private Task<AppUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
        public UserController(UserManager<AppUser> usrMgr, SignInManager<AppUser> signinMgr,RoleManager<IdentityRole> roleMgr,IRepository repo)
        {
            userManager = usrMgr;
            signInManager = signinMgr;
            roleManager = roleMgr;
            repository = repo;
        }


        public ViewResult ChooseRole()
        {
            string[] list = GetRoles().ToArray();
            return View(list);
        }

        public ViewResult Settings()
        {
            return View("Settings"/*,new ViewResult {Controller=nameof(User),Action=nameof(Settings) }*/);
        }

       


        public ActionResult RemoveSpecialization(ManageSpecializationsViewModel model)
        {
            repository.DeleteDoctorSpecialization(model.UserId, model.SpecializationId);
            return RedirectToAction("ManageSpecializations");
        }
        [Authorize]
        public ViewResult ManageSpecializations()
        {
            AppUser user = GetCurrentUserAsync().Result;
            ManageSpecializationsViewModel model =new ManageSpecializationsViewModel() {specializations=repository.GetDoctorSpecializations(user.Id),UserId=user.Id,SpecializationName="None" };

            return View(model);
        }

        public ViewResult Create(string Role)
        {

            switch (Role)
            {

                case "Pacjent":
                    {
                        return View("PatientView",new CreatePatientModel() { ChooseRole = Role });
                        break;
                    }
                case "Doktor":
                    {
                        return View("DoctorView",new CreateDoctorModel() { ChooseRole = Role });
                        break;
                    }
                default:
                    return View("Error");
                    break;
            }


           
        }


        public List<string> GetRoles()
        {
            List<string> list = new List<string>();

           IList<IdentityRole> Roles=    roleManager.Roles.ToList();


            if(Roles!=null&&Roles.Count>0)
            {
               
                try
                {
                  IdentityRole roleAdmin = Roles.Where(r => r.Name == "Administrator").First();
                  if(roleAdmin!=null)
                    {
                        Roles.Remove(roleAdmin);
                    }
                }
                catch
                {

                }
               
                foreach (var role in Roles)
                {
                    list.Add(role.Name);
                }
            }

            return list;
        }


               
  


[HttpPost]
        public async Task<IActionResult> CreatePatient(CreatePatientModel model)
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
                    PhoneNumber = model.PhoneNumber,
                    ChooseRole = model.ChooseRole



                };


                IdentityResult result = await userManager.CreateAsync(user, model.Password);

               

                if (result.Succeeded)
                {
                    Patient patient = new Patient() { NumberInQueue = 0 };
                    repository.AddPatientToUser(patient, user.Email);
                   await repository.AddRoleToUser(user.Email, user.ChooseRole);
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

            return View("PatientView",model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(CreateDoctorModel model)
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
                    PhoneNumber = model.PhoneNumber,
                    ChooseRole = model.ChooseRole



                };


                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                
                
                if (result.Succeeded)
                {
                    Doctor doctor = new Doctor() { PriceForVisit = model.PriceForVisit };
                    repository.AddDoctorToUser(doctor, user.Email);
                    await repository.AddRoleToUser(user.Email, user.ChooseRole);
                    //return RedirectToAction("HomePage", "Home", null);
                    return RedirectToAction("AddDoctorDetails", "User", user);
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            return View("DoctorView",model);
        }


        public ViewResult AddDoctorDetails(AppUser user)
        {

            if(user.DoctorId==null)
            {
                user = GetCurrentUserAsync().Result;
            }
            List<Specialization> list = repository.Specializations.ToList();
            DoctorDetailsViewModel model = new DoctorDetailsViewModel() { DoctorId =(int)user.DoctorId,SpecializationList=list };
           
            return View(model);
        }
        [HttpPost]
        public IActionResult AddDoctorDetails(DoctorDetailsViewModel model)
        {
            repository.AddSpecializationToDoctor(model.DoctorId, model.Specialization.Name);
            return RedirectToAction("ManageSpecializations");
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