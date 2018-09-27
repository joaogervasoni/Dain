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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var returnedUser = UserDAO.SearchByEmailPassword(user.Email, user.Password);
            if (returnedUser == null) return View(user);

            if (returnedUser.UserType.Equals(nameof(Pub)))
            {
                var pub = PubDAO.SearchByUserId(returnedUser.Id);

                UserSession.ReturnPubId(pub.Id);
                UserSession.ReturnUserId(pub.UserId);

                return RedirectToAction("Dashboard", "Pub");
            }
            else if (returnedUser.UserType.Equals(nameof(Person)))
            {
                var person = PersonDAO.SearchByUserId(returnedUser.Id);

                UserSession.ReturnPersonId(person.Id);
                UserSession.ReturnUserId(person.UserId);

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
            System.Web.HttpContext.Current.Session["user"] = null;
            System.Web.HttpContext.Current.Session["user"] = user;

            var returnedUser = UserDAO.SearchByEmailLogin(user.Email, user.Login);
            if (returnedUser != null) return View(user);

            switch (button)
            {
                case "PubType": return RedirectToAction("Register", "Pub");

                case "PersonType": return RedirectToAction("Register", "Person");

                default: return View(user);
            }
        }

        public ActionResult Update(User userUpdate)
        {
            var user = UserDAO.Search(UserSession.ReturnUserId(null));
            user.Email = userUpdate.Email;

            if (!string.IsNullOrEmpty(userUpdate.Password))
                user.Password = userUpdate.Password;

            if (UserDAO.Update(user) == true)
                return RedirectToAction("Dashboard", user.UserType);

            return RedirectToAction("Account", user.UserType);
        }

        #endregion
    }
}