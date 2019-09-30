using Microsoft.AspNetCore.Identity;
using PogotowieCom.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{

    public class AppUser : IdentityUser
    {
        public int? PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public int? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }


        public string Surname { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string ChooseRole { get; set; }

        public void x()
        {
            AppUser user = new AppUser();
            
        }



    }

   
    
    

    public class PatientAppointment
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int? NumberInQueue { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }


    public class DoctorSpecialization
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
    }



    public class Patient
    {
        public int PatientId { get; set; }
        public Patient()
        {
            
            this.Notifications = new HashSet<Notification>();
        }

       

        
        public  AppUser AppUser;
        public IList<PatientAppointment> PatientAppointments { get; set; }
        public ICollection<Notification> Notifications { get; set; }



    }
   
    public class Doctor 
    {
        public int DoctorId { get; set; }
        public Doctor()
        {
            
            this.Appointments = new HashSet<Appointment>();
            this.DoctorSpecializations = new HashSet<DoctorSpecialization>();
          
        }
                         

        public decimal? PriceForVisit { get; set; }


        public AppUser AppUser;
        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

       
    }

    



    public class Specialization
    {

        public Specialization()
        {
            this.DoctorSpecializations = new HashSet<DoctorSpecialization>();
        }

        public int SpecializationId { get; set; }
        public string Name { get; set; }

        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
    }




    public class Appointment
    {


        public Appointment()
        {
            //this.Patients = new HashSet<Patient>();
            
           
        }

        public int AppointmentId { get; set; }
      
        [Required(ErrorMessage ="Proszę podać datę")]
        //[Display(Name="Data")]
        [DataType(DataType.Date)]
        public DateTime? AppointmentDate { get; set; }

        
        [Display(Name = "Czas rozpoczęcia ")]
        [Required(ErrorMessage = "Proszę podać czas rozpoczęcia przyjmowania pacjentów")]
        //[DataType(DataType.Time)]
        public DateTime? AppointmentStart { get; set; }


       
        [Required(ErrorMessage = "Proszę podać czas zakończenia przyjmowania pacjentów")]
        [TimeMustBeLater("AppointmentStart")]
        [DataType(DataType.Time)]
        public DateTime? AppointmentEnd { get; set; }

        [Display(Name = "Ilość Miejsc ")]
        [Required(ErrorMessage = "Proszę podać ilość miejsc")]
        public int? PlacesAvailable { get; set; }

       
        public int? NumberOfPatients { get; set; }

        [Display(Name = "Cena za wizytę ")]
        [Range(20,1000)]
        [Required(ErrorMessage = "Proszę podać cenę za wizytę")]
        public decimal? VisitPrice { get; set; }

        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        //public ICollection<Patient> Patients{ get; set; }
        public IList<PatientAppointment> PatientAppointments { get; set; }

        public int? PlaceId { get; set; }
        public Place Place { get; set; }


       


       
    }


    public class Notification
    {
        public int NotificationId { get; set; }
        public string NotificationText { get; set; }
        public bool Checked { get; set; }

        public int? PatientId { get; set; }
        public Patient Patient { get; set; }
    }


    public class Place
    {

        public Place()
        {
            this.Appointments = new HashSet<Appointment>();
        }

        public int PlaceId { get; set; }
        [Required(ErrorMessage ="Nazwa jest wymagana")]
        public string PlaceName { get; set; }
        [Required(ErrorMessage = "Nazwa kraju jest wymagana")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Nazwa miasta jest wymagana")]
        public string City { get; set; }
        [Required(ErrorMessage = "Nazwa ulicy jest wymagana")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Numer budynku jest wymagany")]
        public int? BuildingNumber { get; set; }
        [Required(ErrorMessage = "Numer pokoju jest wymagany")]
        public int? Room { get; set; }

       public ICollection<Appointment> Appointments { get; set; }

        //public int DoctorId { get; set; }
        //public Doctor Doctor { get; set; }

    }





}
