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
        public ActionResult Register(Product product, HttpPostedFileBase upImage, int? Categories)
        {
            var pub = PubDAO.Search(UserSession.ReturnPubId(null));
            Guid guid = Guid.NewGuid();
            product.PubId = UserSession.ReturnPubId(null);
            ViewBag.Categories = new SelectList(CategoryDAO.ReturnList(), "Id", "Name");
            product.Category = CategoryDAO.Search(Categories);

            if (ModelState.IsValid == true)
            {
                product.PhotoUrl = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
                product.PhotoType = upImage.ContentType;

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