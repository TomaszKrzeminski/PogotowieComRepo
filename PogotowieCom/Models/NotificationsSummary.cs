using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class NotificationsSummary : ViewComponent
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;

        private Task<AppUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public NotificationsSummary(IRepository repository,UserManager<AppUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke(string UserId)
        {
            AppUser user = GetCurrentUserAsync().Result;
            List<Notification> list = new List<Notification>();
            if (user.PatientId != null )
            {
                list = repository.GetNotifications((int)user.PatientId, false);
                return View(list);

            }
            else
            {                
                return View(list);
            }




        }


    }
}
