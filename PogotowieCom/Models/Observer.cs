using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{

   

    public interface IObserver
    {

        void update(Notification notification);
        //List<Notification> GetAllNotifications();
        //List<Notification> GetNotCheckedNotifications();
    }

    public interface ISubject
    {
        void AddObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void notifyObservers();

    }


    public class Observer : IObserver
    {
        private IRepository repository;
        private ISubject subject;
        private Patient Patient;
       
       
        public Observer(ISubject subject,IRepository repository,int PatientId)
        {
            
            this.subject = subject;
            this.repository = repository;
            Patient = repository.GetPatientById(PatientId);
            subject.AddObserver(this);
           
        }
        public void update(Notification notification)
        {
            repository.AddNotificationToPatient(Patient.PatientId, notification);
        }

       //public List<Notification>  GetAllNotifications()
       // {
       //     return repository.GetNotifications(Patient.PatientId, false);
       // }

       //public   List<Notification> GetNotCheckedNotifications()
       // {
       //     return repository.GetNotifications(Patient.PatientId, true);
       // }

    }

    


    public class Subject : ISubject
    {

       
        private Appointment appointment;
        protected Notification notification;
        List<IObserver> observers { get; set; }

        public Subject(Appointment appointment)
        {
            this.appointment = appointment;
            observers = new List<IObserver>();
        }

        public Subject()
        {
            observers = new List<IObserver>();
        }


        public void AddObserver(IObserver o)
        {
            observers.Add(o);
        }

        public void notifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.update( notification);
            }
        }

        public void RemoveObserver(IObserver o)
        {
            observers.Remove(o);
        }

        public void MakeNotificationcountdownTime(string Text)
        {
            notification = new Notification() { Checked = false, NotificationText = "Czas pozostały do wizyty "+Text };
        }

        public void MakeNotificationReservedAppointment(DateTime? time)
        {
            notification = new Notification() {Checked=false, NotificationText = "Zarezerwowano wizytę " + time };
        }

    }


    public class SubjectRemoveAppointment:Subject
    {
        private AppUser user { get; set; }

        public SubjectRemoveAppointment(Appointment appointment,AppUser user):base(appointment)
        {
            this.user = user;
        }

        public void MakeNotificationRemoveAppointment(Appointment appointment)
        {
            
            notification = new Notification() { Checked = false, NotificationText = "Usunięto wizytę z  " + appointment.AppointmentDate+"u doktora "+user.UserName+" "+user.Surname+"  Przepraszamy" };
        }
    }




}
