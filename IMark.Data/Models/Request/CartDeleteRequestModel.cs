using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
    public class CartDeleteRequestModel
    {
        public string checkoutId { get; set; }
        public List<string> lineItemIds { get; set; }
    }
}
