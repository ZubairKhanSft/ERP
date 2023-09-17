using AccountErp.Dtos.Customer;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;

namespace AccountErp.Dtos.Invoice
{
    public class InvoiceDetailDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal? Tax { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remark { get; set; }
        public Constants.InvoiceStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? PoSoNumber { get; set; }
        public string StrInvoiceDate { get; set; }
        public string StrDueDate { get; set; }
        public string CreatedBy { get; set; }
        public decimal? SubTotal { get; set; }
        public Constants.InvoiceType InvoiceType { get; set; }
        public CustomerDetailDto Customer { get; set; }

        public IEnumerable<InvoiceServiceDto> Items { get; set; }
        public IEnumerable<InvoiceAttachmentDto> Attachments { get; set; }
        public InvoiceServiceDto InvoiceServiceDto { get; set; }
        public bool IsApproved { get; set; }
        public int AppovedId { get; set; }
        public string AppoverName { get; set; }
        public decimal VatAmount { get; set; }
        public string SalesOrderNumber { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public double ConversionRate { get; set; }
        public double TotalAmountAfterConversion { get; set; }

        /*  public string ProductModel { get; set; }
          public string ProductSpecification { get; set; }
          public string ProductBrands { get; set; }
          public int ProductUnits { get; set; }
          public double ProductRateUSD { get; set; }
          public double ProductRateAED { get; set; }
          public int ProductPartNumber { get; set; }
          public string ProductUAN { get; set; }
          public int ProductSupplierCode { get; set; }
          public int ProductId { get; set; }*/
    }
}
