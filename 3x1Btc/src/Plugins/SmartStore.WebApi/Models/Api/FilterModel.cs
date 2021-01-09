using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.WebApi.Models.Api
{
    public class FilterModel
    {
        public string categoryId { get; set; }
        public string AuthorId { get; set; }
        public string Language { get; set; }
        public string Format { get; set; }
        public string price { get; set; }
        public string Popular { get; set; }
		public int pageIndex { get; set; }		
	}
}