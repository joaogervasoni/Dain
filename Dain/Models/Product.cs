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

        [Display(Name = "Category")]
        public Category Category { get; set; }

        /// <summary>
        /// The path of the directory of the photos of the pub
        /// </summary>
        public byte[] PhotoUrl { get; set; }

        /// <summary>
        /// The type of the photo to add to the reponse headers to be send to the browser
        /// </summary>
        public string PhotoType { get; set; }

        #region Methods

        /// <summary>
        /// This is a helper method to encode the image byte array to a format that the browser will understand
        /// </summary>
        /// <returns>A image in base64</returns>
        public string PhotoBase64()
        {
            if (PhotoUrl == null) return null;
            return string.Format($"data:{PhotoType};base64,{Convert.ToBase64String(PhotoUrl)}");
        }

        #endregion
    }
}