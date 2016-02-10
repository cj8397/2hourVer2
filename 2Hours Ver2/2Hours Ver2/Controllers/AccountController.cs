using _2Hours_Ver2.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using _2Hours_Ver2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using _2Hours_Ver2.Models;
using _2Hours_Ver2.BusinessLogic;

namespace _2Hours_Ver2.Controllers
{
    public class AccountController : Controller
    {
        private mergedEntities db = new mergedEntities();
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            // UserStore and UserManager manages data retreival.
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.UserName,
                                                             login.Password);

            if (ModelState.IsValid)
            {
                if (ValidLogin(login))
                {
                    IAuthenticationManager authenticationManager
                                           = HttpContext.GetOwinContext().Authentication;
                    authenticationManager
                   .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = new ClaimsIdentity(new[] {
                                            new Claim(ClaimTypes.Name, login.UserName),
                                        },
                                        DefaultAuthenticationTypes.ApplicationCookie,
                                        ClaimTypes.Name, ClaimTypes.Role);
                    // SignIn() accepts ClaimsIdentity and issues logged in cookie. 
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);
                    return RedirectToAction("UserArea", "Account");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisteredUser newUser) {
            var userStore         = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

            var identityUser      = new IdentityUser() { UserName = newUser.UserName, 
                                                         Email    = newUser.Email };
            IdentityResult result = manager.Create(identityUser, newUser.Password);

            if (result.Succeeded) {
                CreateTokenProvider(manager, EMAIL_CONFIRMATION);

                var code = manager.GenerateEmailConfirmationToken(identityUser.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Home",
                                                new { userId = identityUser.Id, code = code },
                                                    protocol: Request.Url.Scheme);

                string link = "Please confirm your account by clicking this link: <a href=\""
                                + callbackUrl + "\">Confirm Registration</a>";
                newUser.ConfirmLink = link;                
                // sending Email Start
                MailHelper mailer = new MailHelper();
                string response = mailer.EmailFromArvixe(
                                           new RegisteredUser(newUser.Email, newUser.UserName,newUser.ConfirmLink ));
                
                if (response != "Failure sending mail."){
                    ViewBag.Success = response;
                }else{
                    ViewBag.Failure = response;
                }

                // sending Email End
            }
            return View();
            }
        [Authorize]
        public ActionResult Welcome(string name) {
            //ViewBag.UserName = name;
            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(AspNetRole role)
        {
            db.AspNetRoles.Add(role);
            db.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult AddUserToRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUserToRole(string userName, string roleName)
        {
            AspNetUser user = db.AspNetUsers
                             .Where(u => u.UserName == userName).FirstOrDefault();
            AspNetRole role = db.AspNetRoles
                             .Where(r => r.Name == roleName).FirstOrDefault();

            user.AspNetRoles.Add(role);
            db.SaveChanges();
            return View();
        }



        [Authorize(Roles = "Admin")]        
        public ActionResult AdminOnly()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrderDetails()
        {
            var details = db.OrderDetails.ToList();
            return View(details);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrders()
        {
            var orders = db.OrderProducts.ToList();
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrderUpdate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProducts()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProductAdd()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProductDetails()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProductUpdate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSuppliers()
        {
            var suppliers = db.Suppliers.ToList();
            return View(suppliers);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSupplierAdd()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSupplierDetails()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSupplierUpdate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminViewUsers()
        {
            var users = db.AspNetUsers.ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminUserAdd()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminUserDetails()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminUserUpdate()
        {
            return View();
        }
        
        
        // Admin and customer have permission to go to these pages.

        [Authorize]
        public ActionResult UserArea()
        {
            return View();
        }

        [Authorize]
        public ActionResult UpdateProfile()
        {

            return View();
        }

        [Authorize]
        public ActionResult ProfileDetails()
        {
            return View();
        }

        [Authorize]
        public ActionResult PasswordReset()
        {
            return View();
        }

        bool ValidLogin(Login login)
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

        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        const string PASSWORD_RESET = "ResetPassword";

        void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
        }


        public ActionResult ConfirmEmail(string userID, string code)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(userID);
            CreateTokenProvider(manager, EMAIL_CONFIRMATION);
            try
            {
                IdentityResult result = manager.ConfirmEmail(userID, code);
                if (result.Succeeded)
                    ViewBag.Message = "<p class='alert alert-success'>Success! You are now registered.</p>";
            }
            catch
            {
                ViewBag.Message = "<p class='alert alert-danger>Error! Validation attempt failed.</p>";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindByEmail(email);
            CreateTokenProvider(manager, PASSWORD_RESET);

            var code = manager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account",
                                         new { userId = user.Id, code = code },
                                         protocol: Request.Url.Scheme);
            ViewBag.FakeEmailMessage = "Please reset your password by clicking <a href=\""
                                     + callbackUrl + "\">here</a>";
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string userID, string code)
        {
            ViewBag.PasswordToken = code;
            ViewBag.UserID = userID;
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string password, string passwordConfirm,
                                          string passwordToken, string userID)
        {

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(userID);
            CreateTokenProvider(manager, PASSWORD_RESET);

            IdentityResult result = manager.ResetPassword(userID, passwordToken, password);
            if (result.Succeeded)
                ViewBag.Result = "<p class='alert alert-success'>Success! Your password has been reset.</p>";
            else
                ViewBag.Result = "<p class='alert alert-danger'>Error! Your password has not been reset.</p>";
            return View();
        }



    }//end account controller
}