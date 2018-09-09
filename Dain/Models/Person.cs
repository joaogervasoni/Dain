using System;

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
        public string Name { get; set; }

        /// <summary>
        /// The address that the <see cref="Person"/> lives
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The city where the <see cref="Person"/> lives
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state that the <see cref="Person"/> lives
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The date that the <see cref="Person"/> was born
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// The directory path of the <see cref="Person"/> photo
        /// </summary>
        public string UriPhoto { get; set; }

        /// <summary>
        /// The gender of the <see cref="Person"/>
        /// </summary>
        public string Gender { get; set; }
    }
}