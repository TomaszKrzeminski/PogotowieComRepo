using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{

    public interface ITimeAndDate
    {
        DateTime GetTime();
       
    }


    public class TimeAndDate:ITimeAndDate
    {

       

        public DateTime GetTime()
        {
            return DateTime.Now;
        }


    }
}
