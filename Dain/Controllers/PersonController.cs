using Dain.DAL;
using Dain.Models;
using Dain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dain.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Person person, HttpPostedFileBase upImage)
        {
            var user = (User)System.Web.HttpContext.Current.Session["person"];
            System.Web.HttpContext.Current.Session["person"] = null;
            Person newPerson = new Person(user, person)
            {
                PhotoUrl = ImageHandler.HttpPostedFileBaseToByteArray(upImage),
                PhotoType = upImage.ContentType
            };

            if (ModelState.IsValid == false) return View(newPerson);

            var returnedPerson = PersonDAO.Insert(newPerson);
            if (returnedPerson == null) return View(newPerson);

            UserSession.ReturnPubId(returnedPerson.Id);
            return View(returnedPerson);
        }

        public ActionResult Dashboard()
        {
            var pubSession = PubDAO.Search(UserSession.ReturnPubId(null));

            if (pubSession == null) RedirectToAction("Login", "User");

            return View(pubSession);
        }

        public ActionResult Account()
        {
            return View();
        }
    }
}