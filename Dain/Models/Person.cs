using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    /// <summary>
    /// The data model of the person type users that will access the system
    /// </summary>
    public class Person : User
    {
        /// <summary>
        /// The name of the <see cref="Person"/>
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// The address that the <see cref="Person"/> lives
        /// </summary>
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// The city where the <see cref="Person"/> lives
        /// </summary>
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        /// <summary>
        /// The state that the <see cref="Person"/> lives
        /// </summary>
        [Display(Name = "State")]
        public string State { get; set; }

        /// <summary>
        /// The date that the <see cref="Person"/> was born
        /// </summary>
        [Required]
        [Display(Name = "Birthday Date")]
        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// The directory path of the <see cref="Person"/> photo
        /// </summary>
        public string UriPhoto { get; set; }

        /// <summary>
        /// The gender of the <see cref="Person"/>
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; }
    }
}