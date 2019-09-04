using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{

    public class AppUser : IdentityUser
    {
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }


        public string Surname { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }


    

    public class PatientAppointment
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }


    
    public class Patient 
    {
        public int PatientId { get; set; }
        public Patient()
        {
            //this.Appointments = new HashSet<Appointment>();
        }

        public int NumberInQueue { get; set; }


        //public ICollection<Appointment> Appointments{ get; set; }
        public IList<PatientAppointment> PatientAppointments { get; set; }

    }
   
    public class Doctor 
    {
        public int DoctorId { get; set; }
        public Doctor()
        {
            this.Specializations = new HashSet<Specialization>();
            this.Appointments = new HashSet<Appointment>();
        }
                         

        public decimal PriceForVisit { get; set; }
     


        public ICollection<Specialization> Specializations { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }

    



    public class Specialization
    {
        public int SpecializationId { get; set; }
        public int Name { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }




    public class Appointment
    {


        public Appointment()
        {
            //this.Patients = new HashSet<Patient>();
        }

        public int AppointmentId { get; set; }
        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set; }
        public int PlacesAvailable { get; set; }
        public int NumberOfPatients { get; set; }
        public decimal VisitPrice { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<Patient> Patients{ get; set; }
        public IList<PatientAppointment> PatientAppointments { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }



    }





    public class Place
    {

        public Place()
        {
            this.Appointments = new HashSet<Appointment>();
        }

        public int PlaceId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Room { get; set; }

        ICollection<Appointment> Appointments { get; set; }

    }





}
