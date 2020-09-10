using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Data.Models.Request
{
   public class PaymentCardRequestModel
    {
        public string checkoutId { get; set; }
        public Payment payment { get; set; }
    }

    public class PaymentAmount
    {
        public string amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class BillingAddress
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
    }

    public class Payment
    {
        public PaymentAmount paymentAmount { get; set; }
        public string idempotencyKey { get; set; }
        public BillingAddress billingAddress { get; set; }
        public string vaultId { get; set; }
    }

    

}
