using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{




    public abstract class Specialist
    {
        protected bool Check { get; set; } = false;
        protected Specialist specialist;
        protected IRepository repository;

        public List<Specialist> specialists { get; set; }

        public Specialist(IRepository repository)
        {
            this.repository = repository;
            this.specialists = new List<Specialist>();
        }


        public List<string> GetSpecialistsNames()
        {
            List<string> Names = new List<string>();
            if (specialists != null && specialists.Count() > 0)
            {
                foreach (var specialist in specialists)
                {
                    Names.Add(specialist.GetNameOfSpecialization());
                }
            }
            return Names;
        }

        public abstract string GetNameOfSpecialization();

        public void setNumber(Specialist specialist)
        {
            this.specialist = specialist;
        }

        public void ForwardRequest(List<Tag> taglist)
        {

            taglist.RemoveAll(t => t.Text == null);

            List<Tag> SpecialistListOfTags = repository.GetTagsSpecialist(this);


          


            Check = CompareTags(taglist, SpecialistListOfTags);

            if (Check)
            {
                specialists.Add(this);
            }

            if (specialist != null)
            {
                specialist.specialists = specialists;
                specialist.ForwardRequest(taglist);
            }


        }




        bool CompareTags(List<Tag> userTags, List<Tag> specialistTags)
        {


            if (userTags != null && userTags.Count > 0 && specialistTags != null && specialistTags.Count > 0)
            {
                foreach (var TagU in userTags)
                {

                    foreach (var TagS in specialistTags)
                    {

                        string s1 = TagU.Text;
                        string s2 = TagS.Text;

                        string normalized1 = Regex.Replace(s1, @"\s", "");
                        string normalized2 = Regex.Replace(s2, @"\s", "");

                        bool stringEquals = String.Equals(
                            normalized1,
                            normalized2,
                            StringComparison.OrdinalIgnoreCase);

                        if (stringEquals == true)
                        {
                            return true;
                        }


                    }

                }
            }
            return false;
        }




    }

    //"Ginekolog", "Stomatolog", "Ortopeda", "Chirurg", "Dermatolog", "Psychiatra", "Psycholog", "Internista", "Laryngolog", "Okulista", "Neurolog", "Fizjoterapeuta", "Urolog", "Sexuolog", "Alergolog",  "Ortopeda", "Chirurg Szczękowy", "Lekarz Sportowy" 

    public class Ginekolog : Specialist
    {
        public Ginekolog(IRepository repository) : base(repository)
        {

        }



        public override string GetNameOfSpecialization()
        {
            return "Ginekolog";
        }
    }


    public class Stomatolog : Specialist
    {
        public Stomatolog(IRepository repository) : base(repository)
        {

        }



        public override string GetNameOfSpecialization()
        {
            return "Stomatolog";
        }
    }


    public class Ortopeda : Specialist
    {
        public Ortopeda(IRepository repository) : base(repository)
        {

        }



        public override string GetNameOfSpecialization()
        {
            return "Ortopeda";
        }
    }


}

