using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public abstract class SearchObject
    {
        Repository repository;
        public SearchObject(Repository repository)
        {
            this.repository = repository;
        }
            

        public List<AppUser> Users { get; set; }

        public List<string> Filtering { get; set; }

        public Func<AppUser, bool> FilteringType;

        public abstract void Modification();

        public  List<AppUser> GetUsers()
        {
            return Users;
        }

        public abstract void UsersFiltering();
       

    }


    public class SearchDoctor : SearchObject
    {
       
        public SearchDoctor(Repository repository):base(repository)
        {

        }

        public override void Modification()
        {
            Filtering.Add("Filtrowanie Specialistów");
        }

        public override void UsersFiltering()
        {
           
        }
    }


    public class SearchPatient : SearchObject
    {
        public SearchPatient(Repository repository) : base(repository)
        {

        }


        public override void Modification()
        {
            Filtering.Add("Filtrowanie Pacjentów");
        }

        public override void UsersFiltering()
        {
            
        }
    }




    public abstract class SearchDecorator : SearchObject
    {

        SearchPatient

        public override void Modification()
        {
            throw new NotImplementedException();
        }


       

        public override void UsersFiltering()
        {
            
        }
       
    }







    public class SearchDecoratorCity:SearchDecorator
    {
        SearchObject searchobj;
        string City;

        public SearchDecoratorCity(SearchObject obj,string City)
        {
            searchobj = obj;
            this.City = City;
        }


        bool Filtr(AppUser user)
        {
            if(user.City==City)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override void UsersFiltering()
        {

            if (Users != null && Users.Count > 0)
            {
                Users = Users.Where(Filtr).ToList();
            }


          
        }


    }

    public class SearchDecoratorSpecialization
    {

    }

    public class SearchDecoratorAppointmentDate
    {

    }

    public class SearchDecoratorAppointmentHour
    {

    }

}
