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

        [HttpPost]
        public ActionResult Register(Pub pub, HttpPostedFileBase upImage)
        {
            pub.Rating = 0;
            pub.RegistrationDate = DateTime.Now;
            pub.State = "Paraná";
            pub.UserType = "Default";
            pub = Geo(pub);
            pub.Email.ToLower();

            if (ModelState.IsValid == true)
            {
                if (upImage != null)
                {
                    Directory.CreateDirectory(Server.MapPath("~/Images/Pub/" + pub.Name));
                    string name = Path.GetFileName(upImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/Pub/" + pub.Name), ("Profile_Image" + Path.GetExtension(upImage.FileName)));

                    upImage.SaveAs(path);
                    pub.UriGalery = "/Images/Pub/" + pub.Name;
                }
                else { pub.UriGalery = "No"; }

                if (PubDAO.Insert(pub) == true)
                {
                    Sess.ReturnPubId(pub.Id);
                    return RedirectToAction("Dashboard", "Pub");
                }
                else { return View("Login", pub); }
            }
            return View("Login", pub);
        }

        public ActionResult Product()
        {
            ViewBags();
            ViewBag.ProductList = ProductDAO.ReturnList(Sess.ReturnPubId(null));
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

        [HttpPost]
        public ActionResult UpdateImg(HttpPostedFileBase upImage)
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
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
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            if (pub == null){ ViewBag.Name = "Null"; } else { ViewBag.Name = pub.Name; };

            if (pub.UriGalery != "No")
            {
                ViewBag.Image = pub.UriGalery + "/Profile_Image.jpg";
            }
            else { ViewBag.Image ="~/Content/Preview.png"; }

            var pubsList = PubDAO.ReturnList().Select(x => new { x.Id, x.Name, x.Rating, x.Lat, x.Lng, x.Address, x.FoundationDate }).ToList();
            ViewBag.PubsList = JsonConvert.SerializeObject(pubsList);
        }
    }
}