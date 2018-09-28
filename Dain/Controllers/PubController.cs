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
            return RedirectToAction("Dashboard", "Pub");
        }
        

        public ActionResult Product()
        {
            var pubSession = PubDAO.Search(UserSession.ReturnPubId(null));
            if (pubSession == null) return RedirectToAction("Logout", "User");
            ViewBags(pubSession);
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
            catch (Exception ex) { ModelState.AddModelError("", $"Error - {ex.Message}"); }

            ViewBag.Alter = "No";
            return View();
        }

        public ActionResult Dashboard()
        {
            var pubSession = PubDAO.Search(UserSession.ReturnPubId(null));
            if (pubSession == null) return RedirectToAction("Logout", "User");

            ViewBags(pubSession);
            return View(pubSession);
        }

        public ActionResult Account()
        {
            var returnedPub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (returnedPub == null) return RedirectToAction("Logout", "User");

            ViewBags(returnedPub);
            return View(returnedPub);
        }

        public ActionResult Delete(string password)
        {
            var returnedPub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (returnedPub == null) return RedirectToAction("Logout", "User");
            var returnedUser = UserDAO.Search(returnedPub.UserId);

            if (password != returnedUser.Password)
            {
                ModelState.AddModelError("", "Error - Password does not match");
                return View("Account");
            }

            PubDAO.Delete(returnedPub);
            UserDAO.Delete(returnedUser);

            return RedirectToAction("Logout", "User");
        }

        public ActionResult Update(Pub pubUpdate)
        {
            var returnedPub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (returnedPub == null) return RedirectToAction("Logout", "User");

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
            if (update == null) return RedirectToAction("Logout", "User"); 
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

        public void ViewBags(Pub returnedPub)
        {
            var pubsList = PubDAO.ReturnList().Select(pub => new GoogleMapPubData()
            {
                Id = pub.Id,
                Name = pub.Name,
                Photo = ImageHandler.PhotoBase64(pub.Photo, pub.PhotoType),
                Rating = pub.Rating,
                Lat = pub.Lat,
                Lng = pub.Lng,
                Address = pub.Address,
                FoundationDate = pub.FoundationDate
            }).ToList();

            ViewBag.PubsList = JsonConvert.SerializeObject(pubsList);
            ViewBag.Lng = returnedPub.Lng == 0 ? -49.276855 : returnedPub.Lng;
            ViewBag.Lat = returnedPub.Lat == 0 ? -25.441105 : returnedPub.Lat;
            ViewBag.Name = returnedPub.Name;
            ViewBag.Profile = ImageHandler.PhotoBase64(returnedPub.Photo, returnedPub.PhotoType);
            ViewBag.LayoutStyle = returnedPub.LayoutStyle;
            ViewBag.Type = "pub";
        }

        #endregion
    }
}