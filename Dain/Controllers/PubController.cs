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
            pub.Rating = 0;
            pub.RegistrationDate = DateTime.Now;
            pub.UriGalery = "No";
            pub.State = "Paraná";
            pub.UserType = "Default";
            pub = Geo(pub);

            if (ModelState.IsValid == true)
            {
                pub.Email.ToLower();
                if (PubDAO.Insert(pub) == true)
                {
                    Sess.ReturnPubId(pub.Id);
                    return RedirectToAction("Dashboard", "Pub");
                }
                else { return View("Login", pub); }
            }
            return View("Login", pub);
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
                return RedirectToAction("Login");
            }
            else { return View("Account"); }
        }

        public ActionResult Update(Pub pubUpdate)
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            pubUpdate.Rating = pub.Rating;
            pubUpdate.RegistrationDate = pub.RegistrationDate;
            pubUpdate.UserType = pub.UserType;
            pubUpdate.State = pub.State;
            pubUpdate.UriGalery = pub.UriGalery;
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

        public void ViewBags()
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            if (pub == null){ ViewBag.Name = "Null"; } else { ViewBag.Name = pub.Name; };
        }
    }
}