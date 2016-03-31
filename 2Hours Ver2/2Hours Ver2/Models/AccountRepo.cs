using _2Hours_Ver2.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;


namespace _2Hours_Ver2.Models
{
    public class AccountRepo
    {

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

        //Get profile details
        public AspNetUser GetProfileDetail(Login login)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.UserName, login.Password);
            mergedEntities db = new mergedEntities();
            AspNetUser USER = new AspNetUser();

            var query =
            from a in db.AspNetUsers
            where (a.Id == identityUser.Id)
             select new
            {
                ID = a.Id,
                UserName = a.UserName,
                PhoneNumber = a.PhoneNumber,
                Email = a.Email,           
            };

            foreach (var item in query)
            {
                USER.Id = item.ID;
                USER.UserName = item.UserName;
                USER.PhoneNumber = item.PhoneNumber;
                USER.Email = item.Email;
            }

            return USER;
        }

        public bool ValidLogin(Login login)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 1, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };
            var user = userManager.FindByName(login.UserName);

            if (user == null)
                return false;

            // User is locked out.
            if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
                return false;

            // Validated user was locked out but now can be reset.
            if (userManager.CheckPassword(user, login.Password)
                      && userManager.IsEmailConfirmed(user.Id))

            {
                if (userManager.SupportsUserLockout
                 && userManager.GetAccessFailedCount(user.Id) > 0)
                {
                    userManager.ResetAccessFailedCount(user.Id);
                }
            }
            // Login is invalid so increment failed attempts.
            else {
                bool lockoutEnabled = userManager.GetLockoutEnabled(user.Id);
                if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                {
                    userManager.AccessFailed(user.Id);
                    return false;
                }
            }
            return true;
        }
    }
}