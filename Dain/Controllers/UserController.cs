using Dain.DAL;
using Dain.Models;
using Dain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dain.Controllers
{
    public class UserController : Controller
    {
        #region User Login

        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var pub = PubDAO.Search(user.Email, user.Password);
            var person = PersonDAO.Search(user.Email, user.Password);

            if (pub != null)
            {
                UserSession.ReturnPubId(pub.Id);
                return RedirectToAction("Dashboard", "Pub");
            }
            else if(person != null)
            {
                UserSession.ReturnPubId(person.Id);
                return RedirectToAction("Dashboard", "Person");
            }

            return View("Login", user);
        }

        #endregion 

        #region User Registration

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

        #endregion
    }
}