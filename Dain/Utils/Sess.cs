using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dain.Utils
{
    public class Sess
    {
        public static int ReturnPubId(int? id)
        {
            if (HttpContext.Current.Session["PubID"] == null)
            {
                HttpContext.Current.Session["PubID"] = (id);
            }

            return int.Parse(HttpContext.Current.Session["PubID"].ToString());
        }

        public static int ReturnPersonId(int? id)
        {
            if (HttpContext.Current.Session["PersonID"] == null)
            {
                HttpContext.Current.Session["PersonID"] = (id);
            }

            return int.Parse(HttpContext.Current.Session["PersonID"].ToString());
        }

        public static void ClearPubSession()
        {
            HttpContext.Current.Session["UserID"] = null;
        }

        public static void ClearPersonSession()
        {
            HttpContext.Current.Session["PersonID"] = null;
        }
    }
}