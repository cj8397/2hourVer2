﻿using _2Hours_Ver2.ViewModel;
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
            TempData["Login"] = login;
            if (ModelState.IsValid)
            {
                AccountRepo accountRepo = new AccountRepo();
                if (accountRepo.ValidLogin(login))
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
                    
                    if (identityUser.Roles.Count == 1)
                    {                        
                        return RedirectToAction("AdminOnly", "Account");
                    }
                    if (identityUser.Roles.Count == 0)
                    {                        
                        return RedirectToAction("UserArea", "Account");
                    }
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
                var callbackUrl = Url.Action("ConfirmEmail", "Account",
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
            TempData["orders"]    = db.OrderDetails.ToList();
            TempData["products"]  = db.Products.ToList();
            TempData["suppliers"] = db.Suppliers.ToList();
            TempData["users"]     = db.AspNetUsers.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrders()
        {
            var orders = db.OrderDetails.ToList();
            return PartialView("_AdminOrders",orders);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrderDetails()
        {
            OrderRepo orderRepo = new OrderRepo();
            OrderDetail orderDetail = orderRepo.GetOrderDetail();
            return View("AdminOrderDetails", orderDetail);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrderUpdate()
        {
            return View();
        }

        public ActionResult AdminOrderDelete()
        {
            return RedirectToAction("DeleteProduct", "ProductController");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProducts()
        {
            var products = db.Products.ToList();
            return PartialView("_AdminProducts",products);
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
        public ActionResult AdminProductDelete()
        {
            return RedirectToAction("DeleteProduct", "ProductController");
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
            AccountRepo accountRepo = new AccountRepo();
            var login = TempData["Login"];
            TempData["orders"] = db.OrderDetails.ToList();
            TempData["profile"] = accountRepo.GetProfileDetail((Login)login);
            return View();
        }
        [Authorize]
        public ActionResult UserOrderDetails()
        {
            var details = db.OrderDetails.ToList();
            return PartialView("_UserOrderDetails", details);
        }

        [Authorize]
        public ActionResult ProfileDetails()
        {
            AccountRepo accountRepo = new AccountRepo();
            var login = TempData["Login"];
            RegisteredUser aspNetUser = accountRepo.GetProfileDetail((Login)login);
            return PartialView("_ProfileDetails",aspNetUser);
        }

        [Authorize]
        public ActionResult PasswordReset()
        {
            return View();
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
                ViewBag.Message = "<p class='alert alert-danger'>Error! Validation attempt failed.</p>";
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
            string link = "Please reset your password by clicking <a href=\""
                                     + callbackUrl + "\">here</a>";


            MailHelper mailer = new MailHelper();
            string response = mailer.EmailFromArvixe(
                                       new RegisteredUser(email, PASSWORD_RESET, link));
            if (response != "Failure sending mail.")
            {
                ViewBag.Success = response;
            }
            else {
                ViewBag.Failure = response;
            }

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
        
        public ActionResult Create()
        {            
            return View();
        }


      

        public ActionResult Update(RegisteredUser registeredUser)
        {
            AccountRepo accountRepo = new AccountRepo();
            accountRepo.UpdateUser(registeredUser.TelNumber, registeredUser.UserName);
            
            return View();
        }

        // Returning order's details
        public ActionResult DetailsOrderProduct(int id)
        {
            ProductRepo productRepo = new ProductRepo();
            return View(productRepo.GetDetail(id));
        }

        // Editing an order
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditOrderDetail(int id) // "id" is "id order number".
        {
            OrderRepo orderRepo = new OrderRepo();
            OrderDetails orderDetails = new OrderDetails();


            orderDetails = orderRepo.GetOrderDetail(id);
            return View(orderDetails);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditOrderDetail(OrderDetails orderDetails)
        {
            OrderRepo orderRepo = new OrderRepo();
            OrderDetails editedOrderDetails = new OrderDetails();

            if (ModelState.IsValid)
            {
                editedOrderDetails = orderRepo.UpdateOrder(orderDetails);
                return RedirectToAction("AdminOnly");
            }
            else
            {
                ViewBag.ErrorMessage = "This entry is invalid.";
                return View(orderDetails);
            }
        }


        [HttpGet]
        public ActionResult EditOrderProduct(int id,int productId) // "id" is "id order number".
        {
            ProductRepo productRepo = new ProductRepo();
            OrderProductVM orderProductVM = new OrderProductVM();


            orderProductVM = productRepo.GetOrderProductDetail(id, productId);
            return View(orderProductVM);
        }

        [HttpPost]
        public ActionResult EditOrderProduct(OrderProductVM orderProductVM)
        {
            ProductRepo productRepo = new ProductRepo();
            OrderProductVM editedOrderProductVM = new OrderProductVM();

            if (ModelState.IsValid)
            {
                editedOrderProductVM = productRepo.UpdateOrderProduct(orderProductVM);
                return RedirectToAction("DetailsOrderProduct", new { id = orderProductVM.OrderNumber });
            }
            else
            {
                ViewBag.ErrorMessage = "This entry is invalid.";
                return View(orderProductVM);
            }

        }

        public ActionResult DeleteOrderProduct(int id, int productId)
        {
            ProductRepo productRepo = new ProductRepo();            

            if (ModelState.IsValid)
            {
                productRepo.DeleteOrderProduct(id,productId);                 
            }
            return RedirectToAction("DetailsOrderProduct", new { id = id });

        }
        
        [HttpGet]
        public ActionResult EditProducts(int id)
        {
            ProductRepo productRepo = new ProductRepo();
            ProductVM productVM = new ProductVM();

            productVM = productRepo.GetProductDetail(id);
            return View(productVM);
        }


        [HttpPost]
        public ActionResult EditProducts(ProductVM productVM)
        {
            ProductRepo productRepo = new ProductRepo();
            ProductVM editedProductVM = new ProductVM();

            if (ModelState.IsValid)
            {
                editedProductVM = productRepo.UpdateProduct(productVM);
                return RedirectToAction("AdminOnly");
            }
            else
            {
                ViewBag.ErrorMessage = "This entry is invalid.";
                return View(productVM);
            }

        }

    }//end account controller
}