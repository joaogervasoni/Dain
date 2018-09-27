using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Dain.Controllers
{
    public static class GoogleGeoLocation
    {
        public static Tuple<double, double> GetCoordinates(string address, string city, string state)
        {
            address = address.Replace(" ", "+");
            state = state.Replace(" ", "+");

            string url = "https://maps.googleapis.com/maps/api/geocode/json?address="
                         + address + "+" + city + "+" + state
                         + "&key=AIzaSyAq0VfrA_iDhSsQFW-wHlZ3X78rZ68GngI";

            WebClient client = new WebClient();
            string json = client.DownloadString(url);

            byte[] bytes = Encoding.Default.GetBytes(json);
            json = Encoding.UTF8.GetString(bytes);

            JToken location = JObject.Parse(json)["results"][0]["geometry"]["location"];
            double lat = location["lat"].Value<double>();
            double lng = location["lng"].Value<double>();

            return Tuple.Create(lat, lng);
        }
    }
}