using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class DoctorDetailsViewModel
    {
        public int DoctorId { get; set; }
        public Specialization Specialization { get; set; }
        public List<Specialization> SpecializationList { get; set; }
    }
}
