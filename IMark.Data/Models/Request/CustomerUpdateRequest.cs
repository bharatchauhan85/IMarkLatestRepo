using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
  public  class CustomerUpdateRequest
    {
        public string customerAccessToken { get; set; }
        public UpdateCustomer customer { get; set; }
    }
    public class UpdateCustomer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
