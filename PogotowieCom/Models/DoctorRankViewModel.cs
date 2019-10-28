using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class DoctorRankViewModel
    {

        public DoctorRankViewModel(Doctor doctor,AppUser user,List<Comment> comments,int NumberInRank,List<Place> places,List<Appointment> appointments,List<Specialization>specializations)
        {
            RankNumber = NumberInRank;
            Name = user.UserName;
            Surname = user.Surname;
            this.comments = comments;
            this.places = places;
            this.appointments = appointments;

        }

        public string UserId { get; set; }
        public int RankNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Comment> comments { get; set; }
        public List<Place> places { get; set; }
        public List<Specialization> specializations { get; set; }
        public List<Appointment> appointments { get; set; }
       
    }
}
