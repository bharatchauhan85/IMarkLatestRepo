using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
   public class DeleteCustomerRequest
    {
        public string id { get; set; }
        public string customerAccessToken { get; set; }
    }
}
