using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.Payment
{
    public class AddPaymentModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string PaymentType { get; set; }
        public string CardNumber { get; set; }
    }
}
