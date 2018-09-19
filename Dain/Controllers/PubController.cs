using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dain.Models;
using Dain.DAL;
using Dain.Utils;

namespace Dain.Controllers
{
    public class PubController : Controller
    {
        // GET: Pub
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string EmailLogin, string PasswordLogin)
        {
            var pub = PubDAO.Search(EmailLogin, PasswordLogin);
            if (pub != null)
            {
                Sess.ReturnPubId(pub.Id);
                return RedirectToAction("Dashboard", "Pub");
            }
            return View("Login");
        }

        public ActionResult Register(Pub pub)
        {
            pub.AccessLevel = '1';
            pub.Rating = 0;
            pub.RegistrationDate = DateTime.Now;
            pub.UriGalery = "No";
            pub.State = "Paraná";
            pub.UserType = "Default";

            if (ModelState.IsValid == true)
            {
                if (PubDAO.Insert(pub) == true)
                {
                    Sess.ReturnPubId(pub.Id);
                    return RedirectToAction("Dashboard", "Pub");
                }
                else { return View("Login", pub); }
            }
            return View("Login", pub);
        }

        public ActionResult Dashboard()
        {
            ViewBags();
            return View();
        }

        public ActionResult Account()
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            ViewBags();
            return View(pub);
        }

        public ActionResult Delete(string Password)
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            if (Password == pub.Password)
            {
                PubDAO.Delete(pub);
                Sess.ClearPubSession();
                return View("Login");
            }
            else { return View("Account"); }
        }

        public ActionResult Update(Pub pubUpdate)
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            pubUpdate.AccessLevel = pub.AccessLevel;
            pubUpdate.Rating = pub.Rating;
            pubUpdate.RegistrationDate = pub.RegistrationDate;
            pubUpdate.UserType = pub.UserType;
            pubUpdate.State = pub.State;
            pubUpdate.UriGalery = pub.UriGalery;
            pubUpdate.Login = pub.Login;
            pubUpdate.Id = pub.Id;
            if (pubUpdate.Password == null) { pubUpdate.Password = pub.Password; }

            if (PubDAO.Update(pubUpdate) == true)
            {
                return View("Dashboard");
            }
            else { return View("Account"); }
        }

        public void ViewBags()
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            if (pub == null)
            {
                ViewBag.Name = "Null";
            }
            else { ViewBag.Name = pub.Name; }

            //ViewBag.Image = pub.UriGalery;
        }
    }
}