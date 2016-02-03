using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace _2Hours_Ver2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
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
        public ActionResult Profile_A()
        {
            return View();
        }

        [Authorize]
        public ActionResult User_A()
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
        }

    }
}