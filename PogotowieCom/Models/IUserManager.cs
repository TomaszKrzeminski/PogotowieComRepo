using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public interface IUserManager
    {

        Task<AppUser> GetCurrentUserAsync();

    }


    public class Manager : IUserManager
    {
        public UserManager<AppUser> userManager;

        private HttpContext context;
        public Manager(HttpContext context)
        {
            this.context = context;
        }

        Task<AppUser> IUserManager.GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(context.User);
        }
    }



}
