using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    /// <summary>
    /// The data model of the person type users that will access the system
    /// </summary>
    public class Person
    {
        #region Public Properties

        [Key]
        /// <summary>
        /// The person ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user ID
        /// </summary>
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Name")]
        /// <summary>
        /// The name of the person
        /// </summary>
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        /// <summary>
        /// The address where the person lives
        /// </summary>
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        /// <summary>
        /// The city where the person lives
        /// </summary>
        public string City { get; set; }

        [Display(Name = "State")]
        /// <summary>
        /// The state where the person lives
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Latitude of the person house in the map
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Longitude of the person house in the map
        /// </summary>
        public double Lng { get; set; }

        [Required]
        [Display(Name = "Birthday Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        /// <summary>
        /// The date that the person was born
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// The person photo
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// The type of the photo to add to the reponse headers to be send to the browser
        /// </summary>
        public string PhotoType { get; set; }

        /// <summary>
        /// The gender of the person
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Layout color and style
        /// </summary>
        public string LayoutStyle { get; set; }

        #endregion
    }
}