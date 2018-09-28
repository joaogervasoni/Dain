using Dain.DAL;
using Dain.Models;
using Dain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dain.Controllers
{
    public class PersonController : Controller
    {
        public ActionResult Register()
        {
            // Get the user from the session that the User Controller generated with the basic data from the user
            var user = (User)System.Web.HttpContext.Current.Session["user"];
            // System.Web.HttpContext.Current.Session["person"] = null;

            // If there is nothing in the user session, return to the user registration page
            if (user == null) return RedirectToAction("Register", "User");

            // Send user as a person model to the register pub view
            return View();
        }

        [HttpPost]
        public ActionResult Register(Person newPerson, HttpPostedFileBase upImage)
        {
            // Verify the if the model is valid
            if (ModelState.IsValid == false) return View(newPerson);

            // Get the user from the session that the User Controller generated with the basic data from the user
            var newUser = (User)System.Web.HttpContext.Current.Session["user"];

            // If there is nothing in the user session, return to the user registration page
            if (newUser == null) return RedirectToAction("Register", "User");

            // Get the coordinates of the bar location that the user has given
            Tuple<double, double> tuple = GoogleGeoLocation.GetCoordinates(newPerson.Address, newPerson.City, newPerson.State);

            newPerson.Lat = tuple.Item1;
            newPerson.Lng = tuple.Item2;

            newPerson.Photo = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
            newPerson.PhotoType = upImage.ContentType;

            newUser.RegistrationDate = DateTime.Now;
            newUser.UserType = nameof(Person);
            newUser.Password = CryptSharp.Crypter.MD5.Crypt(newUser.Password);

            // Insert in the database, if successful
            var returnedUser = UserDAO.Insert(newUser);

            newPerson.UserId = returnedUser.Id;
            var returnedPerson = PersonDAO.Insert(newPerson);
            if (returnedPerson == null || returnedUser == null) return View(newPerson);

            // Generate a session with the user database id
            UserSession.ReturnPersonId(returnedPerson.Id);
            UserSession.ReturnUserId(returnedPerson.UserId);

            System.Web.HttpContext.Current.Session["user"] = null;
            return RedirectToAction("Dashboard", "Person");
        }

        public ActionResult Dashboard()
        {
            var personSession = PersonDAO.Search(UserSession.ReturnPersonId(null));
            if (personSession == null) return RedirectToAction("Logout", "User");

            ViewBags(personSession);
            return View(personSession);
        }

        public ActionResult Account()
        {
            var returnedPerson = PersonDAO.Search(UserSession.ReturnPersonId(null));
            if (returnedPerson == null) return RedirectToAction("Logout", "User");

            ViewBags(returnedPerson);
            return View(returnedPerson);
        }


        public ActionResult Delete(string password)
        {
            var returnedPerson = PersonDAO.Search(UserSession.ReturnPersonId(null));
            var returnedUser = UserDAO.Search(returnedPerson.UserId);

            if (password != returnedUser.Password) return View("Account");

            PersonDAO.Delete(returnedPerson);
            UserDAO.Delete(returnedUser);
            
            return RedirectToAction("Logout", "User");
        }

        public ActionResult Update(Person personUpdate)
        {
            var returnedPerson = PersonDAO.Search(UserSession.ReturnPersonId(null));

            // Get the coordinates of the bar location that the user has given
            Tuple<double, double> tuple =
                GoogleGeoLocation.GetCoordinates(personUpdate.Address, personUpdate.City, personUpdate.State);

            // Set the remaining properties of the model
            returnedPerson.Lat = tuple.Item1;
            returnedPerson.Lng = tuple.Item2;
            returnedPerson.Name = personUpdate.Name;
            returnedPerson.Birthday = personUpdate.Birthday;
            returnedPerson.Address = personUpdate.Address;
            returnedPerson.City = personUpdate.City;
            returnedPerson.State = personUpdate.State;

            if (PersonDAO.Update(returnedPerson) != true) return View("Account");
            return View("Account");
        }

        public ActionResult UpdateLayout(string layout)
        {
            var update = PersonDAO.SearchByUserId(UserSession.ReturnUserId(null));
            update.LayoutStyle = layout;
            Update(update);
            return RedirectToAction("Account");
        }

        [HttpPost]
        public ActionResult UpdatePhoto(HttpPostedFileBase upImage)
        {
            if (upImage == null) return View("Account");

            var returnedPerson = PersonDAO.Search(UserSession.ReturnPersonId(null));
            if (returnedPerson == null) return RedirectToAction("Logout", "User");

            returnedPerson.Photo = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
            returnedPerson.PhotoType = upImage.ContentType;

            if (PersonDAO.Update(returnedPerson) != true) return View("Account");
            return RedirectToAction("Account");
        }

        public ActionResult Pubs()
        {
            var returnedPerson = PersonDAO.Search(UserSession.ReturnPersonId(null));
            ViewBags(returnedPerson);
            return View(PubDAO.ReturnList());
        }

        public ActionResult Pub(int id)
        {
            var pub = PubDAO.Search(id);
            var returnedPerson = PersonDAO.Search(UserSession.ReturnPersonId(null));
            ViewBag.ProductList = ProductDAO.ReturnList(pub.Id);
            ViewBag.Categories = new MultiSelectList(CategoryDAO.ReturnList(), "Id", "Name");
            if (RatingDAO.SearchByPersonAndPubId(UserSession.ReturnPersonId(null), id) != null)
            {
                ViewBag.Rate = "Unrate";
            }
            else
            {
                ViewBag.Rate = "Rate";
            }

            ViewBags(returnedPerson);
            return View(pub);
        }

        public ActionResult Rate(int id)
        {
            var pub = PubDAO.Search(id);
            var rating = RatingDAO.SearchByPersonAndPubId(UserSession.ReturnPersonId(null), pub.Id);

            if (rating == null)
            {
                rating = new Rating
                {
                    PersonId = UserSession.ReturnPersonId(null),
                    PubId = pub.Id
                };
                if (RatingDAO.Insert(rating) != null)
                {
                    pub.Rating = pub.Rating + 1;
                    PubDAO.Update(pub);
                }
            }
            else
            {
                if (RatingDAO.Delete(rating) == true)
                {
                    pub.Rating = pub.Rating - 1;
                    PubDAO.Update(pub);
                }
            }
            return RedirectToAction("Pub", new { id });
        }


        #region Helpers

        public void ViewBags(Person personSession)
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

            ViewBag.Name = personSession.Name;
            ViewBag.Profile = ImageHandler.PhotoBase64(personSession.Photo, personSession.PhotoType);
            ViewBag.Lng = personSession.Lng == 0 ? -49.276855 : personSession.Lng;
            ViewBag.Lat = personSession.Lat == 0 ? -25.441105 : personSession.Lat;
            ViewBag.PubsList = JsonConvert.SerializeObject(pubsList);
            ViewBag.LayoutStyle = personSession.LayoutStyle;
            ViewBag.Type = "person";
        }

        #endregion
    }

    public class GoogleMapPubData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public double Rating { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public DateTime FoundationDate { get; set; }
    }
}