using System;
using System.Collections.Generic;
using System.Text;
using static IMark.Data.Models.Response.CustomerLoginResponse;

namespace IMark.Data.Models.Response
{
   public class GraphqlResponse
    {
        public Data_TR data { get; set; }
    }

    public class Data_TR
    {
        public CustomerCreate_ customerCreate { get; set; }
    }
    public class Customer_T
    {
        public string id { get; set; }

    }
   
    public class CustomerCreate_
    {
        public Customer_T customer { get; set; }
        public IList<CustomerUserErrors> customerUserErrors { get; set; }

    }

}
