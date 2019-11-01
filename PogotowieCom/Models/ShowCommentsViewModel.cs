using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class ShowCommentsViewModel
    {
               

        public List<Comment> Comments { get; set; }
        public AppUser user { get; set; }

    }
}
