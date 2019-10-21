using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class AddAppointmentViewModel
    {
        [Required(ErrorMessage ="Proszę uzupełnić inaczej dodanie wizyty będzie niemożliwe")]
        public Appointment Appointment{get;set;}
        public int DoctorId { get; set; }
        public int PlaceId { get; set; }
        [Required(ErrorMessage ="Błąd")]
        public Place place { get; set; }
    }
}
