using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
   public class CheckOutCustomerRequestModel
    {
        public string checkoutId { get; set; }
        public string customerAccessToken { get; set; }
    }
}
