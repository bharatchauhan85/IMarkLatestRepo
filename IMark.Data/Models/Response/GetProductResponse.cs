using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
   public class GetProductResponse
    {
        public ProductData Data { get; set; }
    }
    public partial class ProductData
    {
        public Shop Shop { get; set; }
    }

    public partial class Shop
    {
        public Products Products { get; set; }
    }

    public partial class Products
    {
        public PageInfoClass pageInfo { get; set; }
        public List<ProductsEdge> Edges { get; set; }
    }

    public partial class ProductsEdge
    {

        public string cursor { get; set; }
        public PurpleNode Node { get; set; }
    }

    public partial class PurpleNode
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public object OnlineStoreUrl { get; set; }
        public string ProductType { get; set; }
        public string Title { get; set; }
        public List<string> tags { get; set; }
        public Images Images { get; set; }
        public Variants Variants { get; set; }
    }

    public partial class Images
    {
        public List<ImagesEdge> Edges { get; set; }
    }

    public partial class ImagesEdge
    {
        public FluffyNode Node { get; set; }
    }

    public partial class FluffyNode
    {
        public string Id { get; set; }
        public Uri Src { get; set; }
    }

    public partial class Variants
    {
        public List<VariantsEdge> Edges { get; set; }
    }

    public partial class VariantsEdge
    {
        public TentacledNode Node { get; set; }
    }

    public partial class TentacledNode
    {
        public List<SelectedOption> SelectedOptions { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public string price { get; set; }
        public bool available { get; set; }
        
        public Image1 image { get; set; }
    }

    public partial class SelectedOption
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Image1
    {
        public string id { get; set; }
        public string originalSrc { get; set; }
    }
}
