using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Infrastructure
{
    public class DateShouldBeLaterThanNowAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;
        public string ErrorMessage { get; set; } = "Czas ustalenia wizyty to co najmniej 1 godzina wcześniej";



        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            DateTime? time = context.Model as DateTime?;
            DateTime timeToCheck = DateTime.Now;
            timeToCheck.AddHours(1);

            if(!time.HasValue||time.Value==null)
            {
                return Enumerable.Empty<ModelValidationResult>();
            }
            else if(time.HasValue&&timeToCheck<=time)
            {
                return new List<ModelValidationResult> { new ModelValidationResult("", ErrorMessage) };
            }

             return Enumerable.Empty<ModelValidationResult>();

        }
    }
















}
