using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    public class Product
    {
        [Key]
        /// <summary>
        /// The name of the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the product
        /// </summary>
        public int PubId { get; set; }

        [Required]
        [Display(Name = "Name")]
        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        /// <summary>
        /// The name of the product
        /// </summary>
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        /// <summary>
        /// The name of the product
        /// </summary>
        public double Price { get; set; }

        [Display(Name = "Category")]
        public Category Category { get; set; }

        /// <summary>
        /// The path of the directory of the photos of the pub
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// The type of the photo to add to the reponse headers to be send to the browser
        /// </summary>
        public string PhotoType { get; set; }


        public string PhotoBase64()
        {
            if (Photo == null || string.IsNullOrEmpty(PhotoType)) return null;

            return string.Format($"data:{PhotoType};base64,{Convert.ToBase64String(Photo)}");
        }
    }
}