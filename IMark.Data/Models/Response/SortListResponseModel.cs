using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
   public class SortListResponseModel
    {
        public SortData data { get; set; }
    }
    public class ShopImage
    {
        public string id { get; set; }
        public string originalSrc { get; set; }
    }
    public partial class ShopSelectedOption
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ShopNode2
    {
        public List<ShopSelectedOption> SelectedOptions { get; set; }
        public string Title { get; set; }
        public string id { get; set; }
        public string price { get; set; }
        public ShopImage image { get; set; }
    }

    public class ShopEdge2
    {
        public ShopNode2 node { get; set; }
    }

    public class ShopVariants
    {
        public List<ShopEdge2> edges { get; set; }
    }

    public class ShopNode
    {
        public string id { get; set; }
        public string title { get; set; }
        public ShopImages images { get; set; }
        public string productType { get; set; }
        public string description { get; set; }
        public ShopVariants variants { get; set; }
    }
    public class ShopsNode2
    {
        public string id { get; set; }
        public string src { get; set; }
    }

    public class ShopsEdge2
    {
        public ShopsNode2 node { get; set; }
    }

    public class ShopImages
    {
        public List<ShopsEdge2> edges { get; set; }
    }

    public class ShopEdge
    {
        public ShopNode node { get; set; }
    }

    public class ShopProducts
    {
        public List<ShopEdge> edges { get; set; }
    }

    public class SortShop
    {
        public ShopProducts products { get; set; }
    }

    public class SortData
    {
        public SortShop shop { get; set; }
    }

   


}
