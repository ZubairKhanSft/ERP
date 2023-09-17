using AccountErp.Dtos.CreditMemo;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;

namespace AccountErp.Dtos.Invoice
{
    public class InvoiceListItemDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? PoSoNumber { get; set; }
        public string StrInvoiceDate { get; set; }
        public string StrDueDate { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreaditMemoId { get; set; }

        public Constants.InvoiceStatus Status { get; set; }
        public Constants.InvoiceType InvoiceType { get; set; }
        public bool isCreditMemo { get; set; } 
       
    }
}
