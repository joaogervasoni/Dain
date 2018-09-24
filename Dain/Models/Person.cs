using System;
using System.ComponentModel.DataAnnotations;

namespace Dain.Models
{
    /// <summary>
    /// The data model of the person type users that will access the system
    /// </summary>
    public class Person : User
    {
        
        public Person() {}

        public Person(User user) : base(user) {}

        public Person(User user, Person person) : base(user)
        {
            Name = person.Name;
            Address = person.Address;
            City = person.City;
            State = person.State;
            Birthday = person.Birthday;
            PhotoUrl = person.PhotoUrl;
            Gender = person.Gender;
            UserType = nameof(Person);
        }

        /// <summary>
        /// The name of the person
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// The address that the person lives
        /// </summary>
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// The city where the person lives
        /// </summary>
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        /// <summary>
        /// The state that the person lives
        /// </summary>
        [Display(Name = "State")]
        public string State { get; set; }

        /// <summary>
        /// The date that the person was born
        /// </summary>
        [Required]
        [Display(Name = "Birthday Date")]
        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// The person photo
        /// </summary>
        public byte[] PhotoUrl { get; set; }

        public string PhotoType { get; set; }
        

        /// <summary>
        /// The gender of the person
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public string PhotoBase64()
        {
            if (PhotoUrl != null) return string.Format($"data:{PhotoType};base64,{Convert.ToBase64String(PhotoUrl)}");
            return null;

        }
    }
}