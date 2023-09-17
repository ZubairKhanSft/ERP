using System;

namespace AccountErp.Entities
{
    public class InvoiceService
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int? ServiceId { get; set; }
        public int? ProductId { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxPrice { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal LineAmount { get; set; }
        public int Quantity { get; set; }

       // public Item Service { get; set; }
        public Product Product { get; set; }
        public SalesTax Taxes { get; set; }
        public virtual Invoice Invoice { get; set; }

    }
}
