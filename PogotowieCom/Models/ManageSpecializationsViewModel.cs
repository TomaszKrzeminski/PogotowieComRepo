using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class ManageSpecializationsViewModel
    {
        public string UserId { get; set; }
        public string SpecializationName { get; set; }
        public int SpecializationId { get; set; }
        public List<Specialization> specializations { get; set; }

    }
}
