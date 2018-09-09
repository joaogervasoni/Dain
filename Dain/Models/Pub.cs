using System;

namespace Dain.Models
{
    /// <summary>
    /// The data model of the pub type users that will access the system
    /// </summary>
    public class Pub : User
    {
        /// <summary>
        /// The name of the <see cref="Pub"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The address that the <see cref="Pub"/> is located
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The city where the <see cref="Pub"/> is located
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state that the <see cref="Pub"/> is located
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The date of foundation of the <see cref="Pub"/>
        /// </summary>
        public DateTime FoundationDate { get; set; }

        /// <summary>
        /// The rating of the <see cref="Pub"/> that persons have given
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// The path of the directory of the photos of the <see cref="Pub"/>
        /// </summary>
        public string UriGalery { get; set; }
    }
}