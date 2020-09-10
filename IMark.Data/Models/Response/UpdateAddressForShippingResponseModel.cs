using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
    public class UpdateAddressForShippingResponseModel
    {
        public UpdateShippingAddressData data { get; set; }
    }
    public class UpdateShippingAddressShippingAddress
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }

    public class UpdateShippingAddressCheckout
    {
        public string id { get; set; }
        public UpdateShippingAddressShippingAddress shippingAddress { get; set; }
    }

    public class UpdateShippingAddressCheckoutShippingAddressUpdateV2
    {
        public List<object> userErrors { get; set; }
        public UpdateShippingAddressCheckout checkout { get; set; }
    }

    public class UpdateShippingAddressData
    {
        public UpdateShippingAddressCheckoutShippingAddressUpdateV2 checkoutShippingAddressUpdateV2 { get; set; }
    }
}
