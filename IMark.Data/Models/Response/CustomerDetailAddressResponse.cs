using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
  public  class CustomerDetailAddressResponse
    {
        public Data_Cus data { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string company { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public int quantity { get; set; }
        public string title { get; set; }
        public Variant variant { get; set; }

    }
    public class Variant
    {
        public string id { get; set; }
        public VariantImage image { get; set; }
       
             public string price { get; set; }
        public List<SelectedOption> selectedOptions { get; set; }
    }
    public class VariantImage
    {
        public string id { get; set; }
        public string originalSrc { get; set; }
    }

    public class Edges
    {
        public Node node { get; set; }

    }
    public class Addresses
    {
        public IList<Edges> edges { get; set; }

    }

    public class Customer_Cus
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
        public LastIncompleteCheckout lastIncompleteCheckout { get; set; }
        public Addresses addresses { get; set; }
        public Orders orders { get; set; }
    }

    public class LastIncompleteCheckout
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public string totalPrice { get; set; }
        public string currencyCode { get; set; }
        public string webUrl { get; set; }
        public LineItems lineItems { get; set; }
    }

    public class LineItems
    {
        public List<Edge> edges { get; set; }
    }
    public class Edge
    {
        public Node node { get; set; }
    }
    public class Image2
    {
        public string originalSrc { get; set; }
    }

    public class Variant2
    {
        public string id { get; set; }
        public Image2 image { get; set; }
        public string price { get; set; }
        public string title { get; set; }
        public List<LineItemSelectedOption> selectedOptions { get; set; }
    }

    public class Node4
    {
        public int quantity { get; set; }
        public string title { get; set; }
        
        public Variant2 variant { get; set; }
    }
    public partial class LineItemSelectedOption
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Edge4
    {
        public Node4 node { get; set; }
    }

    public class LineItems2
    {
        public List<Edge4> edges { get; set; }
    }

    public class Node3
    {
        public string id { get; set; }
        public int orderNumber { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public List<SuccessfulFulfillment> successfulFulfillments { get; set; }
        public LineItems2 lineItems { get; set; }
    }
    public class SuccessfulFulfillment
    {
        public FulfillmentLineItems fulfillmentLineItems { get; set; }
        public string trackingCompany { get; set; }
        public List<TrackingInfo> trackingInfo { get; set; }
    }
    public class TrackingInfo
    {
        public string number { get; set; }
        public string url { get; set; }
    }
    public class FulfillmentLineItems
    {
        public List<Edge5> edges { get; set; }
    }

    public class FullfilVariant
    {
        public string id { get; set; }
    }

    public class FullfilLineItem
    {
        public string title { get; set; }
        public FullfilVariant variant { get; set; }
    }

    public class Node5
    {
        public FullfilLineItem lineItem { get; set; }
    }

    public class Edge5
    {
        public Node5 node { get; set; }
    }
    public class Edge3
    {
        public Node3 node { get; set; }
    }

    public class Orders
    {
        public List<Edge3> edges { get; set; }
    }
    public class Data_Cus
    {
        public Customer_Cus customer { get; set; }

    }
    
}
