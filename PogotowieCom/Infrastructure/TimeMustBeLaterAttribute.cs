using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Infrastructure
{
    public class TimeMustBeLaterAttribute : ValidationAttribute
    {
           public bool IsRequired => true;
          
        private readonly string _comparisonProperty;

        public TimeMustBeLaterAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
              try          
            {

            if (value==null)
            {
                return ValidationResult.Success;
            }
            
            var currentValue = (DateTime)value;

            var startTime = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (startTime == null)
                throw new ArgumentException("Błąd nie znaleziono takiej właściwości podczas kontroli poprawności modelu Appointment");

            DateTime? timeToCompare = (DateTime)startTime.GetValue(validationContext.ObjectInstance);

                if(timeToCompare==null)
                {
                    return ValidationResult.Success;
                }

            if (currentValue <= timeToCompare)
                return new ValidationResult("Czas zakończenia nie może być wcześniejszy od czasu rozpoczęcia");
            }
            catch(Exception ex)
            {
                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }

        
    }
}
