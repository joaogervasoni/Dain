using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    /// <summary>
    /// The model of user that will access the system
    /// </summary>
    public class User
    {
        #region Public Properties

        [Key]
        /// <summary>
        /// The user ID
        /// </summary>
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        /// <summary>
        /// The E-mail of the user
        /// </summary>
        public string Email { get; set; }

        [Required]
        [Display(Name = "Login")]
        /// <summary>
        /// The login name of the user that will access the system
        /// </summary>
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        /// <summary>
        /// The password that the user will use to access the system
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The type of user accessing the system
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// The date that the user was registered on the system
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        #endregion
    }
}