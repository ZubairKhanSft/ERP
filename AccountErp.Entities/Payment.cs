using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string PaymentType { get; set; }
        public string CardNumber { get; set; }
      
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
