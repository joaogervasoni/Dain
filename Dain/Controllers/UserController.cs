using Dain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dain.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            return View(user);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user, string button)
        {

            switch (button)
            {
                case "PubType":
                    System.Web.HttpContext.Current.Session["pub"] = user;
                    return RedirectToAction("Register", "Pub");

                case "PersonType":
                    System.Web.HttpContext.Current.Session["person"] = user;
                    return RedirectToAction ("Register", "Person");

                default: return View(user);
            }
        }
    }
}