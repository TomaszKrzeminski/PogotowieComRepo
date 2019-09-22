using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class SelectPlaceViewModel
    {
       [Required(ErrorMessage ="Podaj Kraj")]
       public string Country { get; set; }

        [Required(ErrorMessage = "Podaj miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Podaj ulicę")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Podaj numer budynku")]
        public int? BuildingNumber { get; set; }

        [Required(ErrorMessage = "Podaj numer pokoju")]
        public int? Room { get; set; }




    }
}
