using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
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
        public string AttachmentName { get; set; }
        public int initialStock { get; set; }
    }
}
