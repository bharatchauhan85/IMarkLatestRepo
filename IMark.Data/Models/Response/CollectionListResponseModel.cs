using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
    public class CollectionListNode
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public CollectionListVariants variants { get; set; }
    }

    public class CollectionListImage
    {
        public string id { get; set; }
        public string originalSrc { get; set; }
    }

    public class CollectionListNode2
    {
        public string id { get; set; }
        public string price { get; set; }
        public string title { get; set; }
        public List<CollectionSelectedOption> selectedOptions { get; set; }
        public CollectionListImage image { get; set; }
        public bool available { get; set; }
    }
    public class CollectionSelectedOption
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public class CollectionListEdge2
    {
        public CollectionListNode2 node { get; set; }
    }

    public class CollectionListVariants
    {
        public List<CollectionListEdge2> edges { get; set; }
    }

    public class CollectionListEdge
    {
        public string cursor { get; set; }
        public CollectionListNode node { get; set; }
    }

    public class CollectionListProducts
    {
        public PageInfoClass pageInfo { get; set; }
        public List<CollectionListEdge> edges { get; set; }
    }

    public class CollectionByHandle
    {
        public string title { get; set; }
        public CollectionListProducts products { get; set; }
    }

    public class CollectionListDataShop
    {
        public string name { get; set; }
        public CollectionByHandle collectionByHandle { get; set; }
    }

    public class CollectionListData
    {
        public CollectionListDataShop shop { get; set; }
    }

    public class CollectionListResponseModel
    {
        public CollectionListData data { get; set; }
    }
    public class PageInfoClass
    {
        public bool hasNextPage { get; set; }
        public bool hasPreviousPage { get; set; }
    }
}
