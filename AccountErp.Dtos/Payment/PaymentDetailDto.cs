using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Payment
{
    public class PaymentDetailDto
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
