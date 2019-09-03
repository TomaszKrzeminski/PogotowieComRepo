using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{




    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public decimal Price { get; set; }
        public int PhoneNumber { get; set; }

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
        public int AppointmentId { get; set; }
        public DateTime AppointmentStart { get; set; }
        public DateTime AppointmentEnd { get; set; }
        public int PlacesAvailable { get; set; }
        public int NumberOfPatients { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<Patient> Patients { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }

        

    }


    public class Patient
    {
        public int Id { get; set; }
        public int NumberInQueue { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PhoneNumber { get; set; }

       
        public ICollection<Appointment> Appointments { get; set; }
    }


    public class Place
    {
        public int PlaceId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Room { get; set; }

        ICollection<Appointment> Appointments { get; set; }

    }





}
