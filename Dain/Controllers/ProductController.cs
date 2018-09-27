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
            else
            {
                return RedirectToAction("Product", "Pub");
            }
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            if (id != null)
            {
                var product = ProductDAO.Search(id);
                System.Web.HttpContext.Current.Session["product"] = product;
            }
            
            return RedirectToAction("Product", "Pub", null);
        }

        [HttpPost]
        public ActionResult Update(Product productUpdate, HttpPostedFileBase upImage, int? Categories)
        {
            var product = (Product)System.Web.HttpContext.Current.Session["product"];
            System.Web.HttpContext.Current.Session["product"] = null;

            productUpdate.Id = product.Id;
            productUpdate.PubId = product.PubId;
            if (Categories == null)
            {
                productUpdate.Category = null;
            }
            else
            {
                productUpdate.Category = CategoryDAO.Search(Categories);
            }
            
            if (upImage == null)
            {
                productUpdate.PhotoUrl = product.PhotoUrl;
                productUpdate.PhotoType = product.PhotoType;
            }
            else
            {
                productUpdate.PhotoUrl = ImageHandler.HttpPostedFileBaseToByteArray(upImage);
                productUpdate.PhotoType = upImage.ContentType;
            }
            
            ProductDAO.Update(productUpdate);
            return RedirectToAction("Product", "Pub");
        }

        public ActionResult Delete(int id)
        {
            ProductDAO.Delete(id);
            return RedirectToAction("Product", "Pub");
        }
    }
}