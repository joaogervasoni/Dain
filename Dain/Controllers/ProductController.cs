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
    public class ProductController : Controller
    {
        public ActionResult Register(Product product, HttpPostedFileBase upImage)
        {
            var pub = PubDAO.Search(Sess.ReturnPubId(null));
            Guid guid = Guid.NewGuid();
            product.PubId = Sess.ReturnPubId(null);

            if (ModelState.IsValid == true)
            {
                if (upImage != null)
                {
                    string name = Path.GetFileName(upImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/Pub/" + pub.Name), ("Product_" + guid.ToString() + Path.GetExtension(upImage.FileName)));

                    upImage.SaveAs(path);
                    product.UriImage = "/Images/Pub/" + pub.Name + "/Product_" + guid.ToString() + Path.GetExtension(upImage.FileName);
                }
                else { product.UriImage = "/Content/Preview.png"; }

                ProductDAO.Insert(product);
                return RedirectToAction("Product", "Pub");
            }

            return RedirectToAction("Product", "Pub");
        }

        public ActionResult Delete(int id)
        {
            ProductDAO.Delete(id);
            return RedirectToAction("Product", "Pub");
        }
    }
}