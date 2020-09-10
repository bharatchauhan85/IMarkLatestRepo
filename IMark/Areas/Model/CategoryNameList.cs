using IMark.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Areas.Model
{
   public class CategoryNameList
    {
       
        public string CategoriesImages { get; set; }
        public long id { get; set; }
        public string title { get; set; }
        public string body_html { get; set; }
        public string vendor { get; set; }
        public string product_type { get; set; }
        public string created_at { get; set; }
        public string handle { get; set; }
        public string updated_at { get; set; }
        public string published_at { get; set; }
        public string template_suffix { get; set; }
        public string published_scope { get; set; }
        public string tags { get; set; }
        public string admin_graphql_api_id { get; set; }

        //public IList<Images> images { get; set; }
        //public Image image { get; set; }
    }
}
