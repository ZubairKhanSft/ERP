using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
    {
        public void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {
            builder.ToTable("ShippingAddress");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            //   builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            //   builder.Property(x => x.SellingPrice).IsRequired().HasColumnType("NUMERIC(12,2)");
            //    builder.Property(x => x.BuyingPrice).IsRequired().HasColumnType("NUMERIC(12,2)");
            //   builder.Property(x => x.Description).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.CustomerId).IsRequired();
            builder.Property(x => x.Address).IsRequired(false);
            builder.Property(x => x.City).IsRequired(false) ;
            builder.Property(x => x.State).IsRequired(false);
            builder.Property(x => x.PostalCode).IsRequired(false);
            builder.Property(x => x.ShippingMethod).IsRequired(false);

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
           
            //builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(x => x.WareHouseId);

        }

    }
}
