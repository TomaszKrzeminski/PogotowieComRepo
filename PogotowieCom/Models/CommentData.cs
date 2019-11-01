using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class CommentData
    {
        public AppUser user { get; set; }
        public Appointment appointment { get; set; }
    }
}
