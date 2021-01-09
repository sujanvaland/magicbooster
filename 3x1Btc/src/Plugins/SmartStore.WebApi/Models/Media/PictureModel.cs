using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.WebApi.Models.Media
{
    public class PictureModel
    {
        public int PictureId { get; set; }
        public int? Size { get; set; }
        public string ThumbImageUrl { get; set; }
        public string ImageUrl { get; set; }
        public string FullSizeImageUrl { get; set; }
        public int? FullSizeImageWidth { get; set; }
        public int? FullSizeImageHeight { get; set; }
        public string Title { get; set; }
        public string AlternateText { get; set; }
    }
}