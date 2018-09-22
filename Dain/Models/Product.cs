using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dain.Models
{
    public class Product
    {
        /// <summary>
        /// The name of the <see cref="Product"/>
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the <see cref="Product"/>
        /// </summary>
        public int PubId { get; set; }

        /// <summary>
        /// The name of the <see cref="Product"/>
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// The name of the <see cref="Product"/>
        /// </summary>
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the <see cref="Product"/>
        /// </summary>
        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        /// <summary>
        /// The name of the <see cref="Product"/>
        /// </summary>
        public string UriImage { get; set; }
    }
}