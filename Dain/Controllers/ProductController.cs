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
            product.PubId = pub.Id;

            ViewBag.Categories = new SelectList(CategoryDAO.ReturnList(), "Id", "Name");
            product.Category = CategoryDAO.Search(Categories);

            if (ModelState.IsValid != true) return RedirectToAction("Product", "Pub");

            product.Photo = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
            product.PhotoType = upImage.ContentType;

            ProductDAO.Insert(product);
            return RedirectToAction("Product", "Pub");
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {

            if (id != null)
            {
                var product = ProductDAO.Search(id);
                if(product != null)
                    System.Web.HttpContext.Current.Session["product"] = product;
            }
            
            return RedirectToAction("Product", "Pub");
        }

        [HttpPost]
        public ActionResult Update(Product productUpdate, HttpPostedFileBase upImage, int? Categories)
        {
            var product = (Product)System.Web.HttpContext.Current.Session["product"];
            System.Web.HttpContext.Current.Session["product"] = null;


            product.Name = productUpdate.Name ?? product.Name;
            product.Price = productUpdate.Price;
            product.Description = productUpdate.Description ?? product.Description;


            product.Category = Categories == null ? null : CategoryDAO.Search(Categories);

            product.Photo = upImage == null ? product.Photo : ImageHandler.HttpPostedFileBaseToByteArray(upImage);
            product.PhotoType = upImage == null ? product.PhotoType : upImage.ContentType;
            
            // TODO: put an error handler here
            ProductDAO.Update(product);
            return RedirectToAction("Product", "Pub");
        }

        public ActionResult Delete(int id)
        {
            var product = ProductDAO.Search(id);
            ProductDAO.Delete(product);
            return RedirectToAction("Product", "Pub");
        }
    }
}