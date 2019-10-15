using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class AdvancedSearchViewModel
    {

        public List<AppUser> UserList { get; set; }


        public string City { get; set; }


        public string Specialization { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime? Hour { get; set; }


    }
}
