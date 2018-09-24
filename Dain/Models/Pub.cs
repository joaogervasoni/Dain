using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    /// <summary>
    /// The data model of the pub type users that will access the system
    /// </summary>
    public class Pub : User
    {
        #region Public Properties

        /// <summary>
        /// The name of the pub
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// The address that the pub is located
        /// </summary>
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// The city where the pub is located
        /// </summary>
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        /// <summary>
        /// The state that the pub is located
        /// </summary>
        [Display(Name = "State")]
        public string State { get; set; }

        /// <summary>
        /// Latitude of the pub in the map
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Logitude of the pub in the map
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// The date of foundation of the pub
        /// </summary>
        [Required]
        [Display(Name = "Foundation Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FoundationDate { get; set; }

        /// <summary>
        /// The rating of the pub that persons have given
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// The path of the directory of the photos of the pub
        /// </summary>
        public byte[] PhotoUrl { get; set; }

        /// <summary>
        /// The type of the photo to add to the reponse headers to be send to the browser
        /// </summary>
        public string PhotoType { get; set; }
        
        #endregion

        #region Constructors
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Pub()
        {
            RegistrationDate = FoundationDate = DateTime.Now;
            Rating = 0;
            UserType = nameof(Pub);
        }

        /// <summary>
        /// Constructor that will initialize with the values of a <see cref="User"/> object
        /// </summary>
        /// <param name="user">The <see cref="User"/> object</param>
        public Pub(User user) : base(user)
        {
            RegistrationDate = FoundationDate = DateTime.Now;
            Rating = 0;
            UserType = nameof(Pub);
        }

        #endregion

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