using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dain.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int PubId { get; set; }
        public int PersonId { get; set; }
    }
}