using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class HomePageViewModel
    {
        [Required]
        public string MedicalSpecialist { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public List<string> BodyParts { get; set; }
        
        public List<string> MedicalSpecialists { get; set; }

        public List<string> AllAilments { get; set; }

        public List<Tag> Ailments { get; set; }
    }

    public class MedicalSpecialistSummary:ViewComponent
    {
        IRepository repository;

      public  MedicalSpecialistSummary(IRepository repo)
        {
            repository = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(string City,string Country,string Specialist)
        {
            List<Doctor> doctorlist = new List<Doctor>();
            if (City == "Brak")
            {
                doctorlist.Add(new Doctor() { PriceForVisit = 666 });
            }
            return View(doctorlist);

        }



    }
}
