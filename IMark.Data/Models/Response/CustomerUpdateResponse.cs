using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
    public class CustomerUpdateResponse
    {
        public CustomeData data { get; set; }
    }
    public class CustomerUpdateID
    {
        public string id { get; set; }
    }

    public class CustomerUpdate
    {
        public CustomerUpdateID customer { get; set; }
        public object customerAccessToken { get; set; }
        public List<object> customerUserErrors { get; set; }
    }

    public class CustomeData
    {
        public CustomerUpdate customerUpdate { get; set; }
    }
}
