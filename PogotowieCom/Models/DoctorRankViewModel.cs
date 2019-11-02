using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class DoctorRankViewModel
    {


         public DoctorRankViewModel()
        {

        }


        public DoctorRankViewModel(Doctor doctor, AppUser user, List<Comment> comments, List<Place> places, List<Appointment> appointments, List<Specialization> specializations)
        {
            this.UserId = user.Id;
            this.doctor = doctor;
            Name = user.UserName;
            Surname = user.Surname;

            if (comments == null || comments.Count() == 0)
            {
                this.comments = new List<Comment>() { new Comment() { Text = "Brak" } };
            }
            else
            {
                this.comments = comments;
            }

            if (places == null || places.Count() == 0)
            {
                this.places = new List<Place>() { new Place() { City = "Brak" } };
            }
            else
            {
                this.places = places;
            }



            if (specializations == null || specializations.Count() == 0)
            {
                this.specializations = new List<Specialization>() { new Specialization() { Name = "Brak" } };
            }
            else
            {
                this.specializations = specializations;
            }





            if (appointments == null || appointments.Count() == 0)
            {
                appointments = new List<Appointment>();
            }
            else
            {
                this.appointments = appointments;
            }

            SetRank(comments);
            CheckNearestAppoitmentDate(appointments);
        }

        public void CheckNearestAppoitmentDate(List<Appointment> list)
        {

            if (list != null && list.Count() > 0)
            {

                list = list.Where(d => d.AppointmentDate >= DateTime.Now).OrderBy(t => t.AppointmentStart).ToList();


                if (list != null && list.Count() > 0)
                {
                    string date = list.First().AppointmentStart.ToString();
                    NearestAppoitmentDate = date;
                }
                else
                {
                    NearestAppoitmentDate = "Brak wizyt w najbliższym czasie";
                }


            }
            else
            {
                NearestAppoitmentDate = "Brak wizyt w najbliższym czasie";

            }


        }


        public void SetRank(List<Comment> comments)
        {
            if (comments != null && comments.Count() > 0)
            {
                Points = 0;
                foreach (var comment in comments)
                {
                    Points += comment.Points;
                }


            }
            else
            {
                Points = 0;
            }
        }


        public string UserId { get; set; }
        public Doctor doctor { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Points { get; set; }
        public List<Comment> comments { get; set; }
        public List<Place> places { get; set; }
        public List<Specialization> specializations { get; set; }
        public List<Appointment> appointments { get; set; }
        public string NearestAppoitmentDate { get; set; }


    }
}
