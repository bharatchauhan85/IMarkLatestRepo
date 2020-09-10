using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
   public class CustomerInfoResponse
    {
        public DataInfo data { get; set; }
    }
    public class Customer
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class DataInfo
    {
        public Customer customer { get; set; }
    }

    

}
