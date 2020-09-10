using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
   public class UpdateAddressResponse
    {
        public Datas data { get; set; }
    }
    public class CustomerAddressUpdate
    {
        public CustomerAddres customerAddress { get; set; }
        public List<object> customerUserErrors { get; set; }
    }


    public class CustomerAddres
    {
        public string id { get; set; }
    }

    public class CustomerAddressUpdates
    {
        public CustomerAddress customerAddress { get; set; }
        public List<object> customerUserErrors { get; set; }
    }

    public class Datas
    {
        public CustomerAddressUpdates customerAddressUpdate { get; set; }
    }

   



}
