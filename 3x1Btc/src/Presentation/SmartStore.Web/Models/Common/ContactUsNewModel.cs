using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Web.Models.Common
{
	public partial class ContactUsNewModel : ModelBase
	{
		[AllowHtml]
		[SmartResourceDisplayName("ContactUs.Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public string Fname { get; set; }

		public string Lname { get; set; }

		public string Subject { get; set; }

		[AllowHtml]
		[SmartResourceDisplayName("ContactUs.Enquiry")]
		public string Enquiry { get; set; }
	}
}