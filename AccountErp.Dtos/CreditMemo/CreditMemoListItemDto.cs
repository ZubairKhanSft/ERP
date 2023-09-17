using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.CreditMemo
{
   public class CreditMemoListItemDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public Decimal OldAmmount { get; set; }
        public Decimal NewAmmount { get; set; }
        public Decimal DiffAmmount { get; set; }
        public string Remark { get; set; }
        public Constants.InvoiceStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
       // public Customer Customer { get; set; }
        public string StrInvoiceDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string StrDueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? PoSoNumber { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? LineAmountSubTotal { get; set; }
        public string CustomerName { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public string CreditMemoNumber { get; set; }


        public int? InvoiceId { get; set; }
      //  public Invoice Invoice { get; set; }

    }
}
