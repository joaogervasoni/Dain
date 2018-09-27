using Dain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dain.Utils
{
    public class UserSession
    {
        #region Get Session Methods

        public static int ReturnUserId(int? id)
        {
            if (HttpContext.Current.Session["UserID"] == null)
            {
                HttpContext.Current.Session["UserID"] = (id);
            }
            try { return int.Parse(HttpContext.Current.Session["UserID"].ToString()); }
            catch { return -1; }
        }

        public static int ReturnPubId(int? id)
        {
            if (HttpContext.Current.Session["PubID"] == null)
            {
                HttpContext.Current.Session["PubID"] = (id);
            }
            try { return int.Parse(HttpContext.Current.Session["PubID"].ToString()); }
            catch { return -1; }
        }

        public static int ReturnPersonId(int? id)
        {
            if (HttpContext.Current.Session["PersonID"] == null)
            {
                HttpContext.Current.Session["PersonID"] = (id);
            }

            try { return int.Parse(HttpContext.Current.Session["PersonID"].ToString()); }
            catch { return -1; }
        }

        #endregion

        #region Clear Session Methods

        public static void ClearUserSession()
        {
            HttpContext.Current.Session["UserID"] = null;
        }

        public static void ClearPubSession()
        {
            HttpContext.Current.Session["PubID"] = null;
        }

        public static void ClearPersonSession()
        {
            HttpContext.Current.Session["PersonID"] = null;
        }

        #endregion

    }
}