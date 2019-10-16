using System;
using System.Collections.Generic;
using System.Linq;
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

                          
                    //if (String.IsNullOrEmpty(item.Text))
                    //{
                    //  Tag tag=   taglist.Where(t => t.Text == item.Text).First();
                    //    taglist.Remove(tag);
                    //}
               


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

                            string[] userStrings = PrepareTag(TagU);
                            string[] specialistString = PrepareTag(TagS);

                            if (userStrings.All(us => specialistString.Contains(us)))
                            {
                                return true;
                            }

                        }

                    }
                }
                return false;
            }

            string[] PrepareTag(Tag tag)
            {
                string[] Text = tag.Text.ToLower().Split();

                return Text;


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

