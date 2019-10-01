using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{

    public class Tag
    {
        public int TagId { get; set; }
        public string Text { get; set; }


    }


    public abstract class Specialist
    {
        protected bool Check { get; set; } = false;
        protected Specialist specialist;
        protected Repository repository;

        public Specialist(Repository repository)
        {
            this.repository = repository;
        }

        

        public void setNumber(Specialist specialist)
        {
            this.specialist = specialist;
        }

        public abstract void ForwardRequest(List<Tag> taglist,List<string> Names);

        public bool CompareTags(List<Tag> list, List<Tag> listToCompare)
        {


            if (list != null && list.Count > 0 && listToCompare != null && listToCompare.Count > 0)
            {
                foreach (var Tag in list)
                {

                    foreach (var TagCompare in listToCompare)
                    {

                        if (Tag.Text.ToLower() == TagCompare.Text.ToLower())
                        {
                            return true;
                        }

                    }

                }
            }
            return false;
        }
    }

    "Ginekolog", "Stomatolog", "Ortopeda", "Chirurg", "Dermatolog", "Psychiatra", "Psycholog", "Internista", "Laryngolog", "Okulista", "Neurolog", "Fizjoterapeuta", "Urolog", "Sexuolog", "Alergolog",  "Ortopeda", "Chirurg Szczękowy", "Lekarz Sportowy" 

    public class Ginekolog : Specialist
    {
        public Ginekolog(Repository repository) : base(repository)
        {

        }

        public int GinekologId { get; set; }


        

        public override void  ForwardRequest(List<Tag> taglist,List<string> names) 
        {

            List<Tag> SpecialistListOfTags = repository.GetTagsSpecialist();


          Check=CompareTags(taglist, SpecialistListOfTags);

            if (Check)
            {
                names.Add(this.GetType().ToString());
            }
           
            specialist.ForwardRequest(taglist,names);
        }
    }




}

