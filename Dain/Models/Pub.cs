using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    /// <summary>
    /// The data model of the pub type users that will access the system
    /// </summary>
    public class Pub
    {
        #region Public Properties

        [Key]
        /// <summary>
        /// The pub ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user ID
        /// </summary>
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Name")]
        /// <summary>
        /// The name of the pub
        /// </summary>
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        /// <summary>
        /// The address where the pub is located
        /// </summary>
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        /// <summary>
        /// The city where the pub is located
        /// </summary>
        public string City { get; set; }

        [Display(Name = "State")]
        /// <summary>
        /// The state where the pub is located
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Latitude of the pub in the map
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Logitude of the pub in the map
        /// </summary>
        public double Lng { get; set; }

        [Required]
        [Display(Name = "Foundation Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        /// <summary>
        /// The date of foundation of the pub
        /// </summary>
        public DateTime FoundationDate { get; set; }

        /// <summary>
        /// The rating of the pub that persons have given
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// The path of the directory of the photos of the pub
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// The type of the photo to add to the reponse headers to be send to the browser
        /// </summary>
        public string PhotoType { get; set; }

        /// <summary>
        /// Layout color and style
        /// </summary>
        public string LayoutStyle { get; set; }

        #endregion

        public string PhotoBase64()
        {
            if (Photo == null || string.IsNullOrEmpty(PhotoType)) return null;

            return string.Format($"data:{PhotoType};base64,{Convert.ToBase64String(Photo)}");
        }
    }
}