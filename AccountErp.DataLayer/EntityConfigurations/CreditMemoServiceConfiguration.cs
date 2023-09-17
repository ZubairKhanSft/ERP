using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class CreditMemoServiceConfiguration : IEntityTypeConfiguration<CreditMemoService>
    {
        public void Configure(EntityTypeBuilder<CreditMemoService> builder)
        {
            builder.ToTable("CreditMemoService");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreditMemoId).IsRequired();
            builder.Property(x => x.ServiceId).IsRequired(false);
            builder.Property(x => x.ProductId).IsRequired(false);
            builder.Property(x => x.Rate).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.Price).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.TaxPrice).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.TaxId).IsRequired(false);
            builder.Property(x => x.OldQuantity).IsRequired();
            builder.Property(x => x.NewQuantity).IsRequired();
            builder.Property(x => x.TaxPercentage).IsRequired(false);
            builder.Property(x => x.LineAmount).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.HasOne(x => x.Item).WithMany().HasForeignKey(x => x.ServiceId);
            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.Taxes).WithMany().HasForeignKey(x => x.TaxId);

        }
    }
}

