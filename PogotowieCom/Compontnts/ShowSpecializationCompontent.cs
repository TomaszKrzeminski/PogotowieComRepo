using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using PogotowieCom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Compontnts
{
    public class ShowSpecialization:ViewComponent
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;

        public ShowSpecialization(IRepository repo, UserManager<AppUser> usrMgr)
        {
            repository = repo;
            userManager = usrMgr;
        }

        public IViewComponentResult Invoke()
        {
            AppUser user = userManager.GetUserAsync(HttpContext.User).Result;
           List<Specialization> list= repository.GetDoctorSpecializations(user.Id);
            return View(list);
            //string Name = "";
            //foreach (var item in list)
            //{
            //    Name += "<tr><td>"+item.Name+"</td></tr>";
            //}
            //string Table = @"<table class=""table table-sm table - bordered"">" + Name + "</ table > ";
            //return new HtmlContentViewComponentResult(new HtmlString(Table));
        }




    }
}
