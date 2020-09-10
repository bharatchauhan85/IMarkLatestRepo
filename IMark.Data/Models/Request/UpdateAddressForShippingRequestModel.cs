using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
    public class UpdateAddressForShippingRequestModel
    {
        public UpdateShippingAddress shippingAddress { get; set; }
        public string checkoutId { get; set; }
    }
    public class UpdateShippingAddress
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string address1 { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
    }
}
