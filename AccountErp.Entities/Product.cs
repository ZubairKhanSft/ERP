using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class Product
    {
        public int Id { get; set; }
       /* public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTaxable { get; set; }
        public int? SalesTaxId { get; set; }
       
        public int? InitialStock { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal BuyingPrice { get; set; }*/
       // public int? BankAccountId { get; set; }
        public int? ProductCategoryId { get; set; }
        //public int? WareHouseId { get; set; }
       // public WareHouse Warehouse { get; set; } 
        public ProductCategory Category { get; set; }
        // public SalesTax SalesTax { get; set; }

        public string Model { get; set; }
        public string Specification { get; set; }
        public string Brands { get; set; }
        public int Units { get; set; }
        public double RateUSD { get; set; }
        public double RateAED { get; set; }
        public int PartNumber { get; set; }
        public string UAN { get; set; }
        public int SupplierCode { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string AttachmentName { get; set; }
        //  public string FileName { get; set; }
    }
}
