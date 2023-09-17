using System;

namespace AccountErp.Dtos.Bill
{
    public class BillServiceDto
    {
        public Guid GId { get; set; }
        public int Id { get; set; }
        public int BillId { get; set; }
        public string Type { get; set; }
        public decimal Rate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxPercentage { get; set; }
        public string RefrenceNumber { get; set; }
        public int Quantity { get; set; }
        public decimal? TaxPrice { get; set; }
        public decimal LineAmount { get; set; }
        public int? TaxBankAccountId { get; set; }
        public int? BankAccountId { get; set; }

        public int? ProductId { get; set; }
        public string ProductBrand { get; set; }
        public string ProductSpecification { get; set; }
        public string ProductModel{ get; set; }
    }
}
