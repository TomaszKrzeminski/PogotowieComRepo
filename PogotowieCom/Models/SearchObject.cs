using PogotowieCom.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public abstract class SearchObject
    {
        protected IRepository repository;



        public List<string> Filtering { get; set; }

     

        public virtual void Modification()
        {

        }

        public virtual Func<AppUser, bool> FiltrUser(Func<AppUser, bool> filtr=null)
        {

            Func<AppUser, bool> FilteringType=(AppUser user)=>true;

            return FilteringType;
          


        }

        public virtual bool Check()
        {
            return false;
        }

        public virtual bool Filtr(AppUser user)
        {
            return false;
        }

    }


    public class SearchDoctor : SearchObject
    {
        public SearchDoctor()
        {

        }

        public SearchDoctor(IRepository repository)
        {

        }



        public override void Modification()
        {
            Filtering.Add("Filtrowanie Specialistów");
        }


    }


    public class SearchPatient : SearchObject
    {

        public SearchPatient()
        {

        }


        public SearchPatient(IRepository repository)
        {

        }



        public override void Modification()
        {
            Filtering.Add("Filtrowanie Pacjentów");
        }


    }




    public abstract class SearchDecorator : SearchObject
    {
        SearchObject searchobj;
        public SearchDecorator()
        {

        }
        public SearchDecorator(IRepository repository, SearchObject searchobj)
        {
            this.searchobj = searchobj;
           
        }






    }







    public class SearchDecoratorCity : SearchDecorator
    {
        SearchObject searchobj;
        public string City { get; set; }


        public SearchDecoratorCity()
        {

        }

        public SearchDecoratorCity(IRepository repository, SearchObject obj, string City) : base(repository, obj)
        {
            this.City = City;
            this.repository = repository;
            searchobj = obj;

        }


        public override bool Filtr(AppUser user)
        {
            if (user.City == City)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override Func<AppUser, bool> FiltrUser(Func<AppUser, bool> filtr)
        {
            if (Check())
            {
                Func<AppUser, bool> FiltrUser= searchobj.FiltrUser();
                FiltrUser += Filtr;

                return FiltrUser;
            }
            else
            {
                return searchobj.FiltrUser();

            }
        }

        public override bool Check()
        {
            if (String.IsNullOrWhiteSpace(City))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }

    public class SearchDecoratorSpecialization : SearchDecorator
    {

        public SearchDecoratorSpecialization()
        {

        }

        SearchObject searchobj;
        public string Specialization { get; set; }


        public SearchDecoratorSpecialization(IRepository repository, SearchObject obj, string Specialization) : base(repository, obj)
        {
            this.Specialization = Specialization;
           
            this.repository = repository;
            searchobj = obj;

        }


        public override bool Filtr(AppUser user)
        {
            if (user.Doctor.DoctorSpecializations.Where(s => s.Specialization.Name == Specialization).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override Func<AppUser, bool> FiltrUser(Func<AppUser, bool> filtr)
        {

            if (Check())
            {
                Func<AppUser, bool> FiltrUser = searchobj.FiltrUser();
                FiltrUser += Filtr;

                return FiltrUser;
            }
            else
            {
                return searchobj.FiltrUser();

            }




        }


        public override bool Check()
        {

            if (String.IsNullOrWhiteSpace(Specialization))
            {
                return false;
            }
            else
            {
                return true;
            }


        }
    }

}

//public class SearchDecoratorAppointmentDate : SearchDecorator
//{
//    [DataType(DataType.Date)]
//    public DateTime? Date { get; set; }
//    SearchObject searchobj;



//    public SearchDecoratorAppointmentDate()
//    {

//    }



//    public SearchDecoratorAppointmentDate(IRepository repository, SearchObject obj, DateTime? Date) : base(repository, obj)
//    {
//        this.Date = Date;
//        FilteringType = Filtr;
//        this.repository = repository;
//        searchobj = obj;

//    }


//    public override bool Filtr(AppUser user)
//    {
//        if (true)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public override bool Check()
//    {
//        if (Date != null)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }




//    public override void UsersFiltering()
//    {
//        if (Check())
//        {
//            if (searchobj.Users != null && searchobj.Users.Count > 0)
//            {
//                searchobj.Users = searchobj.Users.Where(Filtr).ToList();
//            }
//            else
//            {
//                searchobj.Users = repository.GetFilteredUsers(this);
//            }
//        }



//    }
//}

//public class SearchDecoratorAppointmentHour : SearchDecorator
//{
//    [DataType(DataType.Time)]
//    public DateTime? Hour { get; set; }
//    SearchObject searchobj;



//    public SearchDecoratorAppointmentHour()
//    {

//    }

//    public SearchDecoratorAppointmentHour(IRepository repository, SearchObject obj, DateTime? Hour) : base(repository, obj)
//    {
//        this.Hour = Hour;
//        FilteringType = Filtr;
//        this.repository = repository;
//        searchobj = obj;

//    }


//    public override bool Filtr(AppUser user)
//    {
//        if (true)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }


//    public override void UsersFiltering()
//    {
//        if (Check())
//        {
//            if (searchobj.Users != null && searchobj.Users.Count > 0)
//            {
//                searchobj.Users = searchobj.Users.Where(Filtr).ToList();
//            }
//            else
//            {
//                searchobj.Users = repository.GetFilteredUsers(this);
//            }
//        }



//    }


//    public override bool Check()
//    {
//        if (Hour != null)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }



//}


