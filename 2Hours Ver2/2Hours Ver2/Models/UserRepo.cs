using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Hours_Ver2.Models
{
    class UserRepo
    {

        //  [HttpPost]
        public AspNetUser UpdateUser(string id, string phone, string userName)
        {

            mergedEntities db = new mergedEntities();
            AspNetUser aspNetUser = db.AspNetUsers.Where(a => a.Id == id)
                            .FirstOrDefault();
            aspNetUser.PhoneNumber = phone;
            aspNetUser.UserName = userName;

            db.SaveChanges();
            return aspNetUser;
        }
    }
}
