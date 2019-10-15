using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{

    public enum Check
    {
        Empty, City, Specialization, Date, Hour
    };


    public class Search
    {
        public IRepository repository;
        public List<AppUser> Users { get; set; }
        public AdvancedSearchViewModel model;
        public bool Set = false;
        public State Empty;
        public State City;
        public State Specialization;
        public State Date;
        public State Hour;

        private State state;


        public Search(AdvancedSearchViewModel model, IRepository repository)
        {
            this.Users = new List<AppUser>();
            this.model = model;
            this.repository = repository;
            Empty = new EmptySearch(this, repository);
            City = new CitySearch(this, repository);
            Specialization = new SpecializationSearch(this, repository);
            Date = new DateSearch(this, repository);
            Hour = new HourSearch(this, repository);
            state = Empty;
        }

        public void SetState(State state)
        {
            this.state = state;
        }


        public void Check()
        {
            state.Check();
        }

        public void Filtr()
        {
            state.Filtr();
        }


    }


    public abstract class State
    {

        protected IRepository repository;

        public State(IRepository repository)
        {
            this.repository = repository;
        }

        public abstract bool Check();

        public abstract void Filtr();


    }


    public class EmptySearch : State
    {
        Search search;
        public EmptySearch(Search search, IRepository repository) : base(repository)
        {
            this.search = search;
        }

        public override bool Check()
        {
            return true;
        }

        public override void Filtr()
        {
            search.SetState(search.City);
        }


    }

    public class CitySearch : State
    {

        Search search;
        public CitySearch(Search search, IRepository repository) : base(repository)
        {
            this.search = search;
        }

        public override bool Check()
        {
            return !String.IsNullOrWhiteSpace(search.model.City);
        }

        public override void Filtr()
        {
            if (Check())
            {

                if (search.Set == true)
                {
                    search.Users = repository.GetFilteredUsersCity(search.model.City,search.Users);
                    search.SetState(search.Specialization);
                }
                else if (search.Set == false)
                {
                    search.Users = repository.GetFilteredUsersCity(search.model.City);
                    search.Set = true;
                    //search.SetState(search.Specialization);
                }
            }

            search.SetState(search.Specialization);

        }


    }

    public class SpecializationSearch : State
    {

        Search search;
        public SpecializationSearch(Search search, IRepository repository) : base(repository)
        {
            this.search = search;
        }

        public override bool Check()
        {
            return !String.IsNullOrWhiteSpace(search.model.Specialization);
        }

        public override void Filtr()
        {
            if (Check())
            {
                if (search.Set == true)
                {
                    search.Users = repository.GetFilteredUsersSpecialization(search.model.Specialization,search.Users);
                   
                }
                else if (search.Set == false)
                {
                    search.Users = repository.GetFilteredUsersSpecialization(search.model.Specialization);
                    search.Set = true;
                   
                }
            }


            search.SetState(search.Date);
        }


    }

    public class DateSearch : State
    {

        Search search;
        public DateSearch(Search search, IRepository repository) : base(repository)
        {
            this.search = search;
        }

        public override bool Check()
        {
            if (search.model.Date == null)

            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void Filtr()
        {

            if (Check())
            {
                if (search.Set == true)
                {
                    search.Users = repository.GetFilteredUsersDate((DateTime)search.model.Date,search.Users);
                  
                }
                else if (search.Set == false)
                {
                    search.Users = repository.GetFilteredUsersDate((DateTime)search.model.Date);
                    search.Set = true;
                    
                }
            }
search.SetState(search.Hour);
        }


    }

    public class HourSearch : State
    {

        Search search;
        public HourSearch(Search search, IRepository repository) : base(repository)
        {
            this.search = search;
        }
        public override bool Check()
        {
            if (search.model.Hour == null)

            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void Filtr()
        {

            if (Check())
            {
                if (search.Set == true)
                {
                    search.Users = repository.GetFilteredUsersHour((DateTime)search.model.Hour,search.Users);
                }
                else if (search.Set == false)
                {
                    search.Users = repository.GetFilteredUsersHour((DateTime)search.model.Hour);
                    search.Set = true;
                }
            }

        }


    }


}
