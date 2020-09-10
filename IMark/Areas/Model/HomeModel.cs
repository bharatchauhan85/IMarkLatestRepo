using IMark.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Areas.Model
{
   public class HomeModel
    {
        public string CarouselImages { get; set; }
        public string CategoriesImages { get; set; }
        public string CategoriesColor { get; set; }
        public string CategoriesName { get; set; }
        public string ProductImages { get; set; }
        public string product_type { get; set; }
        public string ProductForwardImages { get; set; }                      
        public string productName { get; set; }        
        public string Rating { get; set; }        
        public string ProductPrice { get; set; }
        public string ProductColor { get; set; }
        public string star1 { get; set; }
        public string star2 { get; set; }
        public string star3 { get; set; }
        public string star4 { get; set; }
        public string star5 { get; set; }
        //public IList<Images> images { get; set; }
        //public Image image { get; set; }
    }
}
