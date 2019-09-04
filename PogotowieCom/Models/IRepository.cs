using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public interface IRepository
    {
        IQueryable<Appointment> Appointments { get; set; }
        IQueryable<Place> Places { get; set; }
        IQueryable<Specialization> Specializations { get; set; }
    }
}
