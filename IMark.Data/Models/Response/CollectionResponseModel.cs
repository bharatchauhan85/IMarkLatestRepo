using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{

    public class Node2
    {
        public string id { get; set; }
        public CollectionShopVariants variants { get; set; }
        public string title { get; set; }
        
        public string productType { get; set; }
    }
    public class CollectionShopImage2
    {
        public string id { get; set; }
        public string originalSrc { get; set; }
    }

    public class CollectionShopNode3
    {

      
        public string price { get; set; }
      
        public string id { get; set; }
        public bool available { get; set; }
        public string title { get; set; }
        public List<SelectedOption> selectedOptions { get; set; }
        public CollectionShopImage2 image { get; set; }
    }

    public class CollectionShopEdge3
    {
        public CollectionShopNode3 node { get; set; }
    }

    public class CollectionShopVariants
    {
        public List<CollectionShopEdge3> edges { get; set; }
    }


    public class Edge2
    {
        public Node2 node { get; set; }
    }

    public class CollectionProducts
    {
        public List<Edge2> edges { get; set; }
    }

    public class CollectionNode
    {
        public string id { get; set; }
        public string title { get; set; }
        public CollectionProductImage image { get; set; }
        public CollectionProducts products { get; set; }
    }
    public class CollectionProductImage
    {
        public string id { get; set; }
        public string originalSrc { get; set; }
    }

    public class CollectionEdge
    {
        public CollectionNode  node { get; set; }
    }

    public class Collections
    {
        public List<CollectionEdge> edges { get; set; }
    }

    public class CollectionShop
    {
        public string name { get; set; }
        public Collections collections { get; set; }
    }

    public class CollectionData
    {
        public CollectionShop shop { get; set; }
    }

    public class CollectionResponseModel
    {
        public CollectionData data { get; set; }
    }
}
