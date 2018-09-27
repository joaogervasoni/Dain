using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dain.Models;
using Dain.DAL;
using Dain.Utils;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace Dain.Controllers
{
    public class PubController : Controller
    {
        #region Pub Registration
        
        public ActionResult Register()
        {
            // Get the user from the session that the User Controller generated with the basic data from the user
            var user = (User)System.Web.HttpContext.Current.Session["pub"];
            System.Web.HttpContext.Current.Session["pub"] = null;

            // If there is nothing in the user session, return to the user registration page
            if (user == null) return RedirectToAction("Register", "User");

            // Send user as a person model to the register pub view
            return View(new Pub(user));
        }
        
        [HttpPost]
        public ActionResult Register(Pub newPub, HttpPostedFileBase upImage)
        {
            if (ModelState.IsValid == true)
            {
                Tuple<double, double> tuple = GoogleGeoLocation.GetCoordinates(newPub.Address, newPub.State);
                if (upImage == null)
                {
                    newPub.PhotoUrl = null;
                    newPub.PhotoType = null;
                }
                else
                {
                    newPub.PhotoUrl = ImageHandler.HttpPostedFileBaseToByteArray(upImage);  
                    newPub.PhotoType = upImage.ContentType;
                }

                newPub.RegistrationDate = DateTime.Now;
                newPub.Lat = tuple.Item1;
                newPub.Lng = tuple.Item2;

                var returnedPub = PubDAO.Insert(newPub);
                if (returnedPub == null)
                {
                    ModelState.AddModelError("", "Error - Check information and try again");
                    return View(newPub);
                }
                else
                {
                    UserSession.ReturnPubId(returnedPub.Id);
                    return RedirectToAction("Dashboard", "Pub");
                }
            }
            else
            {
                ModelState.AddModelError("", "Error - Check information and try again");
                return View(newPub);
            }
        }

        #endregion

        [HttpPost]
        public ActionResult Login(string EmailLogin, string PasswordLogin)
        {
            var pub = PubDAO.Search(EmailLogin, PasswordLogin);
            if (pub != null)
            {
                UserSession.ReturnPubId(pub.Id);
                return RedirectToAction("Dashboard", "Pub");
            }
            return View("Login");
        }

        public ActionResult Product()
        {
            ViewBags();
            var pubSession = PubDAO.Search(UserSession.ReturnPubId(null));
            ViewBag.ProductList = ProductDAO.ReturnList(pubSession.Id);
            ViewBag.Categories = new MultiSelectList(CategoryDAO.ReturnList(),"Id", "Name");
            try
            {
                var product = (Product)System.Web.HttpContext.Current.Session["product"];
                if (product != null)
                {
                    ViewBag.Alter = "Yes";
                    return View(product);
                }
            }
            catch { }

            ViewBag.Alter = "No";
            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBags();
            var pubSession = PubDAO.Search(UserSession.ReturnPubId(null));

            if (pubSession == null) RedirectToAction("Login", "User");

            return View(pubSession);
        }

        public ActionResult Account()
        {
            var returnedPub = PubDAO.Search(UserSession.ReturnPubId(null));
            ViewBags();
            return View(returnedPub);
        }

        public ActionResult Delete(string Password)
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (Password == pub.Password)
            {
                PubDAO.Delete(pub);
                UserSession.ClearPubSession();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBags();
                ModelState.AddModelError("", "Error - Password does not match");
                return View("Account", pub);
            }
        }

        public ActionResult Update(Pub pubUpdate)
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));
            pubUpdate.Rating = pub.Rating;
            pubUpdate.RegistrationDate = pub.RegistrationDate;
            pubUpdate.UserType = pub.UserType;
            pubUpdate.State = pub.State;
            pubUpdate.PhotoUrl = pub.PhotoUrl;
            pubUpdate.Login = pub.Login;
            pubUpdate.Id = pub.Id;
            if (pubUpdate.Password == null) { pubUpdate.Password = pub.Password; }

            Tuple<double, double> tuple = GoogleGeoLocation.GetCoordinates(pubUpdate.Address, pubUpdate.State);
            pubUpdate.Lat = tuple.Item1;
            pubUpdate.Lng = tuple.Item2;

            if (PubDAO.Update(pubUpdate) == true)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBags();
                ModelState.AddModelError("", "Error - Check information and try again");
                return View("Account", pub);
            }
        }

        [HttpPost]
        public ActionResult UpdateImg(HttpPostedFileBase upImage)
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (upImage != null)
            {
                pub.PhotoUrl = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
                pub.PhotoType = upImage.ContentType;
                PubDAO.Update(pub);

                return RedirectToAction("Account", "Pub");
            }
            else
            {
                ViewBags();
                ModelState.AddModelError("", "Error - Image dont work");
                return View("Account", pub);
            }
        }

        public void ViewBags()
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));

            var pubsList = PubDAO.ReturnList().Select(x => new { x.Id, x.Name, x.Rating, x.Lat, x.Lng, x.Address, x.FoundationDate }).ToList();
            ViewBag.PubsList = JsonConvert.SerializeObject(pubsList);
            ViewBag.Name = pub.Name;
            ViewBag.Profile = pub.PhotoBase64();
        }
    }
}