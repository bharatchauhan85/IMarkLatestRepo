using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Response
{
   public class CustomerDeleteAddressResponse
    {
        public Data_De data { get; set; }
    }

    public class CustomerAddressDelete
    {
        public IList<CustomerUserErrors> customerUserErrors { get; set; }
        public string deletedCustomerAddressId { get; set; }

    }
    public class Data_De
    {
        public CustomerAddressDelete customerAddressDelete { get; set; }

    }
   
}
