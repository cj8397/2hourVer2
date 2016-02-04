using _2Hours_Ver2.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using _2Hours_Ver2.ViewModels;
using shoppingCart.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using _2Hours_Ver2.Models;

namespace _2Hours_Ver2.Controllers
{
    public class HomeController : Controller
    {

        
        private const int DEFAULT_QTY = 1;
        private mergedEntities db = new mergedEntities();
        private ShoppingCart shoppingCart = new ShoppingCart();
        private Session session = new Session();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

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
                if (identityUser != null)
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
                    return RedirectToAction("SecureArea", "Home");
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
        public ActionResult Register(RegisteredUser newUser)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            var identityUser = new IdentityUser()
            {
                UserName = newUser.UserName,
                Email = newUser.Email
            };
            IdentityResult result = manager.Create(identityUser, newUser.Password);

            if (result.Succeeded)
            {
                var authenticationManager
                                  = HttpContext.Request.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(identityUser,
                                           DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { },
                                             userIdentity);
            }
            return View();
        }
        [Authorize]
        public ActionResult SecureArea()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Home");
        }
        /*
        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(AspNetRole role)
        {
            hoursLoginEntities context = new hoursLoginEntities();
            context.AspNetRoles.Add(role);
            context.SaveChanges();
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
            hoursLoginEntities context = new hoursLoginEntities();
            AspNetUser user = context.AspNetUsers
                             .Where(u => u.UserName == userName).FirstOrDefault();
            AspNetRole role = context.AspNetRoles
                             .Where(r => r.Name == roleName).FirstOrDefault();

            user.AspNetRoles.Add(role);
            context.SaveChanges();
            return View();
        }



        [Authorize(Roles = "Admin")]        
        public ActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrderDetail()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrders()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOrderUpdate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProduct()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProductAdd()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProductDetail()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminProductUpdate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSupplier()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSupplierAdd()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSupplierDetail()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminSupplierUpdate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminUserAdd()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminUserDetail()
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
        public ActionResult _Profile()
        {

            return View();
        }

        [Authorize]
        public ActionResult _User()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details()
        {
            return View();
        }

        [Authorize]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [Authorize]
        public ActionResult PasswordReset()
        {
            return View();
        }*/


        public ActionResult Add(int productID)
        {
            var item = shoppingCart.GetItem(productID, session.SessionID);
            if (item == null)
            {
                item = shoppingCart.NewCartItem(productID, session.SessionID, DEFAULT_QTY);
            }
            else {
                //default quantity to one
                item.Quantity = item.Quantity ?? DEFAULT_QTY;
                //shoppingCart.UpdateCartItem(item);
            }

            return View(item);
        }
        [HttpPost]
        public ActionResult Add(CartItem item)
        {
            using (var shoppingCart = new ShoppingCart())
            {
                if (ModelState.IsValid)
                {
                    string sessionId = session.SessionID;
                    shoppingCart.StoreItem(item.ProductID, item.Quantity, sessionId);
                    return RedirectToAction("ViewCart");
                }
                return View("Add");
            }
        }

        public ActionResult ViewCart()
        {
            var items = shoppingCart.GetAllItems(session.SessionID);

            if (items.Any())
            {
                Session["shoppingCart"] = items.Count();
            }
            else
            {
                Session["shoppingCart"] = null;
            }
            var order = new Order
            {
                CartItems = items,
                Tax = ShoppingCart.TAX_RATE
            };

            return View(order);
        }
    }
}