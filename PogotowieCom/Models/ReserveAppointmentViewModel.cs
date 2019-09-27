using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class ReserveAppointmentViewModel
    {


        public ReserveAppointmentViewModel(Appointment appointment,string UserId,int AppointmentId,int PatientId)
        {
            this.AppointmentId = AppointmentId;
            this.appointment = appointment;
            this.UserId = UserId;
            this.PatientId = PatientId;
            GetTimes();
        }


        public ReserveAppointmentViewModel(IRepository repository,Appointment appointment, string UserId, int AppointmentId, int PatientId)
        {
            this.repository = repository;
            this.AppointmentId = AppointmentId;
            this.appointment = appointment;
            this.UserId = UserId;
            this.PatientId = PatientId;
            BookedAppointments = CheckIfBooked();
            GetTimes();
        }


        public ReserveAppointmentViewModel()
        {
           
        }

        private IRepository repository;
        public int AppointmentId { get; set; }
        public string UserId { get; set; }
        public int PatientId { get; set; }
        [DataType(DataType.Time)]
        public DateTime? timeSelected { get; set; } = new DateTime();
        public int NumberInQueue { get; set; }
        public Appointment appointment { get; set; }
        public List<TimeOfVisit> timesofvisit { get; set; } = new List<TimeOfVisit>();
        public List<int> BookedAppointments { get; set; } = new List<int>();


        List<int> CheckIfBooked()
        {
            List<int> list = new List<int>();
            try
            {
                list = repository.GetBookedAppointments(AppointmentId);
                return list;
            }
            catch(Exception ex)
            {
                return list;
            }

        }
        

        public void GetTimes()
        {
            int NumberOfPatients =(int)appointment.PlacesAvailable;
            TimeSpan AppointmentTime = (TimeSpan)(appointment.AppointmentEnd - appointment.AppointmentStart);
            int OneVisitTime = (int)(AppointmentTime.TotalMinutes / NumberOfPatients);
            DateTime startTime =(DateTime)appointment.AppointmentStart;
            for (int i = 0; i < NumberOfPatients; i++)
            {
                TimeOfVisit timeofvisit = new TimeOfVisit();
                timeofvisit.NumberInQueue = i + 1;
                if(i>0)
                {
                    startTime += new TimeSpan(0, OneVisitTime, 0);
                }
                timeofvisit.time = startTime;

                foreach (var item in BookedAppointments)
                {
                    if(item==timeofvisit.NumberInQueue)
                    {
                        timeofvisit.Booked = true;
                    }
                }

                timesofvisit.Add(timeofvisit);

            }

        }


    }


    public class TimeOfVisit
    {
        [DataType(DataType.Time)]
        public DateTime time { get; set; }
        public int NumberInQueue { get; set; }
        public bool Booked { get; set; } = false;
    }


}
