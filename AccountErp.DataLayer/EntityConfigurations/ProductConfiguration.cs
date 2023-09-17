using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

         //   builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
         //   builder.Property(x => x.SellingPrice).IsRequired().HasColumnType("NUMERIC(12,2)");
        //    builder.Property(x => x.BuyingPrice).IsRequired().HasColumnType("NUMERIC(12,2)");
         //   builder.Property(x => x.Description).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.Model).IsRequired();
            builder.Property(x => x.Specification).IsRequired();
            builder.Property(x => x.Brands).IsRequired();
            builder.Property(x => x.Units).IsRequired();
            builder.Property(x => x.RateAED).IsRequired();
            builder.Property(x => x.RateUSD).IsRequired();
            builder.Property(x => x.PartNumber).IsRequired();
            builder.Property(x => x.UAN).IsRequired();
            builder.Property(x => x.SupplierCode).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(40);
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.UpdatedBy).HasMaxLength(40);
            builder.Property(x => x.ProductCategoryId).IsRequired(false);
            builder.Property(x => x.AttachmentName).IsRequired(false);
            //   builder.Property(x => x.FileName).IsRequired(false);

            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.ProductCategoryId);
            //builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(x => x.WareHouseId);

        }
    }
}