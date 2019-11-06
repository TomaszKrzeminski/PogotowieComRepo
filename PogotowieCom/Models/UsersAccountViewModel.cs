using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class UsersAccountViewModel
    {
       public List<Notification> NotificationList { get; set; }

       public bool Comment { get; set; }

      public void OrderByDesceding()
        {
            if(NotificationList!=null&&NotificationList.Count()>0)
            {
                NotificationList = NotificationList.OrderByDescending(x => x.NotificationId).ToList();
            }
        }


    }
}
