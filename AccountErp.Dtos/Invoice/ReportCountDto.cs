using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Invoice
{
    public class ReportCountDto
    {
        public int TotalPurchase { get; set; }
        public int TotalSales { get; set; }
        public int TotalApprovedInvoice { get; set;}
        public decimal PurchaseTotalCost { get; set;    }
        public int QtyInStock { get; set; }
    }
}
