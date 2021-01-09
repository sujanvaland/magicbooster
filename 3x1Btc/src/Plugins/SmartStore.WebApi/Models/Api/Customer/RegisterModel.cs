using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.WebApi.Models.Api.Customer
{
    public partial class RegisterModel
    {
		public string SponsorsName { get; set; }

		public string PlacementUserName { get; set; }

		public string Position { get; set; }

		public int PlacementId { get; set; }

		public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
        public string Username { get; set; }
		public int AffliateId { get; set; }
		public bool CheckUsernameAvailabilityEnabled { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }
        public string Gender { get; set; }

        public bool FirstNameRequired { get; set; }
        public bool LastNameRequired { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool DateOfBirthEnabled { get; set; }
        public int? DateOfBirthDay { get; set; }
        public int? DateOfBirthMonth { get; set; }
        public int? DateOfBirthYear { get; set; }

         
        public bool CountryEnabled { get; set; }
        public int CountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }
         
        public bool PhoneEnabled { get; set; }
        public bool PhoneRequired { get; set; }
         
        public string Phone { get; set; }

         
        public string TimeZoneId { get; set; }
        public bool AllowCustomersToSetTimeZone { get; set; }
        public IList<SelectListItem> AvailableTimeZones { get; set; }

       
    }
}