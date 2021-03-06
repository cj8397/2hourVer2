﻿using _2Hours_Ver2.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;


namespace _2Hours_Ver2.Models
{
    public class AccountRepo
    {

        public AspNetUser UpdateUser(string phone, string userName)
        {

            mergedEntities db = new mergedEntities();
            AspNetUser aspNetUser = db.AspNetUsers.Where(a => a.UserName == userName)
                            .FirstOrDefault();
            aspNetUser.PhoneNumber = phone;
            aspNetUser.UserName = userName;

            db.SaveChanges();
            return aspNetUser;
        }


        //public OrderDetail UpdateOrderDetail(string id,DateTime deliveryTime, int )
        //{

        //    mergedEntities db = new mergedEntities();
        //    OrderDetail orderDetail = db.OrderDetails.Where(o => o.Id == id)
        //                    .FirstOrDefault();
        //    orderDetail.orderNumber = productID;
        //    orderDetail.deliveryTime = deliveryTime;

        //    db.SaveChanges();
        //    return orderDetail;
        //}




        //Get profile details
        // change public AspNetUser to public RegisteredUser
        public RegisteredUser GetProfileDetail(Login login)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.UserName, login.Password);
            mergedEntities db = new mergedEntities();
            RegisteredUser USER = new RegisteredUser();

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
                USER.TelNumber = item.PhoneNumber;
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