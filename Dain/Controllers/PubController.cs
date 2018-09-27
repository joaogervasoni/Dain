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
using System.Web.Security;

namespace Dain.Controllers
{
    public class PubController : Controller
    {
        
        public ActionResult Register()
        {
            // Get the user from the session that the User Controller generated with the basic data from the user
            var user = (User)System.Web.HttpContext.Current.Session["user"];
            // System.Web.HttpContext.Current.Session["user"] = null;

            // If there is nothing in the user session, return to the user registration page
            if (user == null) return RedirectToAction("Register", "User");

            // Send user as a person model to the register pub view
            return View();
        }

        [HttpPost]
        public ActionResult Register(Pub newPub, HttpPostedFileBase upImage)
        {
            // Verify the if the model is valid
            if (ModelState.IsValid == false)
            {
                ModelState.AddModelError("", "Error - Check information and try again");
                return View(newPub);
            }


            // Get the user from the session that the User Controller generated with the basic data from the user
            var newUser = (User)System.Web.HttpContext.Current.Session["user"];

            // If there is nothing in the user session, return to the user registration page
            if (newUser == null) return RedirectToAction("Register", "User");

            // Get the coordinates of the bar location that the user has given
            Tuple<double, double> tuple = GoogleGeoLocation.GetCoordinates(newPub.Address, newPub.City, newPub.State);

            // Set the remaining properties of the model
            newPub.Lat = tuple.Item1;
            newPub.Lng = tuple.Item2;
            newPub.Photo = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
            newPub.PhotoType = upImage.ContentType;
            newPub.LayoutStyle = "dark";
            newUser.RegistrationDate = DateTime.Now;
            newUser.UserType = nameof(Pub);
            newUser.Password = CryptSharp.Crypter.MD5.Crypt(newUser.Password);
            
            // Insert in the database, if successful
            var returnedUser = UserDAO.Insert(newUser);

            newPub.UserId = returnedUser.Id;
            var returnedPub = PubDAO.Insert(newPub);
            if (returnedPub == null || returnedUser == null)
            {
                ModelState.AddModelError("", "Error - Check information and try again");
                return View(newPub);
            }

            // Generate a session with the user database id
            UserSession.ReturnPubId(returnedPub.Id);
            UserSession.ReturnUserId(returnedPub.UserId);

            System.Web.HttpContext.Current.Session["user"] = null;
            return RedirectToAction("Account", "Pub");
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
            var pubSession = PubDAO.Search(UserSession.ReturnPubId(null));
            if (pubSession == null) RedirectToAction("Login", "User");

            ViewBags();
            return View(pubSession);
        }

        public ActionResult Account()
        {
            var returnedPub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (returnedPub == null) RedirectToAction("Login", "User");

            ViewBags();
            return View(returnedPub);
        }

        public ActionResult Delete(string password)
        {
            var returnedPub = PubDAO.Search(UserSession.ReturnPubId(null));
            var returnedUser = UserDAO.Search(returnedPub.UserId);

            if (password != returnedUser.Password)
            {
                ModelState.AddModelError("", "Error - Password does not match");
                return View("Account");
            }

            PubDAO.Delete(returnedPub);
            UserDAO.Delete(returnedUser);
            UserSession.ClearPubSession();
            UserSession.ClearUserSession();

            return RedirectToAction("Login", "User");
        }

        public ActionResult Update(Pub pubUpdate)
        {
            var returnedPub = PubDAO.Search(UserSession.ReturnPubId(null));
            

            // Set the remaining properties of the model
            
            returnedPub.Name = pubUpdate.Name ?? returnedPub.Name;
            returnedPub.FoundationDate = pubUpdate.FoundationDate == null ? returnedPub.FoundationDate : pubUpdate.FoundationDate;
            returnedPub.Address = pubUpdate.Address ?? returnedPub.Address;
            returnedPub.City = pubUpdate.City ?? returnedPub.City;
            returnedPub.State = pubUpdate.State ?? returnedPub.State;


            // Get the coordinates of the bar location that the user has given
            Tuple<double, double> tuple =
                GoogleGeoLocation.GetCoordinates(returnedPub.Address, returnedPub.City, returnedPub.State);
            returnedPub.Lat = tuple.Item1;
            returnedPub.Lng = tuple.Item2;

            if (PubDAO.Update(returnedPub) != true)
            {
                ModelState.AddModelError("", "Error - Check information and try again");
                return View("Account");
            }

            return RedirectToAction("Account");
        }

        public ActionResult UpdateLayout(string layout)
        {
            var update = PubDAO.SearchByUserId(UserSession.ReturnUserId(null));
            update.LayoutStyle = layout;
            Update(update);
            return RedirectToAction("Account");
        }

        [HttpPost]
        public ActionResult UpdateProfile(HttpPostedFileBase upImage)
        {
            if (upImage == null)
            {
                ModelState.AddModelError("", "Error - Image dont work");
                return View("Account");
            }

            var pub = PubDAO.Search(UserSession.ReturnPubId(null));

            pub.Photo = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
            pub.PhotoType = upImage.ContentType;

            if (PubDAO.Update(pub) != true)
            {
                ModelState.AddModelError("", "Error - Database update image error!");
                return View("Account");
            }
            return RedirectToAction("Dashboard");
        }

        #region Helpers

        public void ViewBags()
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));

            var pubsList = PubDAO.ReturnList().Select(x => new { x.Id, x.Name, x.Rating, x.Lat, x.Lng, x.Address, x.FoundationDate }).ToList();
            ViewBag.PubsList = JsonConvert.SerializeObject(pubsList);
            ViewBag.Name = pub.Name;
            ViewBag.Profile = ImageHandler.PhotoBase64(pub.Photo, pub.PhotoType);
            ViewBag.LayoutStyle = pub.LayoutStyle;
            ViewBag.Type = "pub";
        }

        #endregion
    }
}