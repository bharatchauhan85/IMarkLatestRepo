using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
   public class CheckoutResponseModel
    {
        public CheckoutData data { get; set; }
    }
    public class Checkout
    {
        public string id { get; set; }
    }
    public class CheckoutUserError
    {
        public string code { get; set; }
        public List<string> field { get; set; }
        public string message { get; set; }
    }

    public class CheckoutCreate
    {
        public Checkout checkout { get; set; }
        public List<CheckoutUserError> checkoutUserErrors { get; set; }
    }

    public class CheckoutData
    {
        public CheckoutCreate checkoutCreate { get; set; }
    }

   
}
