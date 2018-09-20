using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    /// <summary>
    /// The abstract model of user that will access the system
    /// </summary>
    public abstract class User
    {
        [Key]
        /// <summary>
        /// The <see cref="User"/> ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The E-mail of the <see cref="User"/>
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// The login name of the <see cref="User"/> that will access the system
        /// </summary>
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        /// <summary>
        /// The password that the <see cref="User"/> will use to access the system
        /// </summary>
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// The type of <see cref="User"/> accessing the system
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// The date that the <see cref="User"/> was registered on the system
        /// </summary>
        public DateTime RegistrationDate { get; set; }
    }
}