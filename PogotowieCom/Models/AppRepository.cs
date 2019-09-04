using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class AppRepository : IRepository
    {
        public IQueryable<Appointment> Appointments { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IQueryable<Place> Places { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IQueryable<Specialization> Specializations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
