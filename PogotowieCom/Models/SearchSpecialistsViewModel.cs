using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class SearchDoctorViewModel
    {
        public SearchDoctorViewModel()
        {
            Users = new List<AppUser>();
        }

        public List<AppUser> Users { get; set;}

        
    }
}
