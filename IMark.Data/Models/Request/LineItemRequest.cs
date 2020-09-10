using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
   public class LineItemRequest
    {
        public List<LineItems> lineItems { get; set; }
        public string checkoutId { get; set; }
    }
    public class LineItems
    {
        public int quantity { get; set; }
        public string variantId { get; set; }
    }

}
