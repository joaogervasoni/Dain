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
        // GET: Pub
        public ActionResult Register()
        {
            var user = (User)System.Web.HttpContext.Current.Session["pub"];
            return View(new Pub(user));
        }

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

        [HttpPost]
        public ActionResult Register(Pub pub, HttpPostedFileBase upImage)
        {
            var user = (User)System.Web.HttpContext.Current.Session["pub"];
            System.Web.HttpContext.Current.Session["pub"] = null;

            Pub newPub = new Pub(user, pub)
            {
                PhotoUrl = ImageHandler.HttpPostedFileBaseToByteArray(upImage),
                PhotoType = upImage.ContentType
            };

            if (ModelState.IsValid == false) return View(newPub);
            var returnedPub = PubDAO.Insert(newPub);
            if (returnedPub == null) return View(newPub);
                
            UserSession.ReturnPubId(returnedPub.Id);
            return RedirectToAction("Dashboard", "Pub");

        }

        public ActionResult Product()
        {
            ViewBags();
            ViewBag.ProductList = ProductDAO.ReturnList(UserSession.ReturnPubId(null));
            return View();
        }

        [HttpPost]
        public Pub Geo(Pub pub)
        {
            string Address = pub.Address.Replace(" ", "+");
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + Address + "+" + pub.State + "&key=AIzaSyAq0VfrA_iDhSsQFW-wHlZ3X78rZ68GngI";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);

            byte[] bytes = Encoding.Default.GetBytes(json);
            json = Encoding.UTF8.GetString(bytes);

            JToken location = JObject.Parse(json)["results"][0]["geometry"]["location"];
            pub.Lat = location["lat"].Value<double>();
            pub.Lng = location["lng"].Value<double>();

            return pub;
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
            else { return View("Account"); }
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
            pubUpdate = Geo(pubUpdate);

            if (PubDAO.Update(pubUpdate) == true)
            {
                return RedirectToAction("Dashboard");
            }
            else { return View("Account"); }
        }

        [HttpPost]
        public ActionResult UpdateImg(HttpPostedFileBase upImage)
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (upImage != null)
            {
                if(!Directory.Exists(Server.MapPath("~/Images/Pub/" + pub.Name)))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Images/Pub/" + pub.Name));
                }

                string name = Path.GetFileName(upImage.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/Pub/" + pub.Name), "Profile_Image.jpg");

                upImage.SaveAs(path);
                return RedirectToAction("Account", "Pub");

            } else { return View("Account"); }

        }

        public void ViewBags()
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));
            if (pub == null){ ViewBag.Name = "Null"; } else { ViewBag.Name = pub.Name; };
            // ViewBag.Image = pub.PhotoUrl;
            //if (pub.PhotoUrl != "No")
            //{
              //  ViewBag.Image = pub.PhotoUrl + "/Profile_Image.jpg";
            //}
            //else { ViewBag.Image ="~/Content/Preview.png"; }

            var pubsList = PubDAO.ReturnList().Select(x => new { x.Id, x.Name, x.Rating, x.Lat, x.Lng, x.Address, x.FoundationDate }).ToList();
            ViewBag.PubsList = JsonConvert.SerializeObject(pubsList);
        }
    }
}