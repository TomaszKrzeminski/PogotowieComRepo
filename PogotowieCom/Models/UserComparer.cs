using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    class UserComparer : EqualityComparer<AppUser>
    {
        public override bool Equals(AppUser x, AppUser y)
        {
            return x.Email == y.Email;
        }

        public override int GetHashCode(AppUser obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}
