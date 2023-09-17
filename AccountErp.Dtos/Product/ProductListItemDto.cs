using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Product
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        /* public decimal Percentage { get; set; }
         public string Name { get; set; }
         public decimal Rate { get; set; }
         public string Description { get; set; }

         public string TaxCode { get; set; }
         public decimal? TaxPercentage { get; set; }
         public int? BankAccountId { get; set; }
         public decimal SellingPrice { get; set; }
         public decimal BuyingPrice { get; set; }
         public string CayegoryName { get; set; }
         public int? ProductCategoryId { get; set; }
         public int? WarehouseId { get; set; }
         public string WarehouseName { get; set; }
         public Constants.RecordStatus Status { get; set; }

         public DateTime CreatedOn { get; set; }*/
        public int? ProductCategoryId { get; set; }
        public string Model { get; set; }
        public string Specification { get; set; }
        public string Brands { get; set; }
        public int Units { get; set; }
        public double RateUSD { get; set; }
        public double RateAED { get; set; }
        public int PartNumber { get; set; }
        public string UAN { get; set; }
        public string CategoryName { get; set; }
        public int SupplierCode { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public int? InitialStock { get; set; }
    }
}
