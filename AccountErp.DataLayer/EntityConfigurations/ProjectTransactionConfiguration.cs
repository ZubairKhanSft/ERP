using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class ProjectTransactionConfiguration : IEntityTypeConfiguration<ProjectTransaction>
    {
        public void Configure(EntityTypeBuilder<ProjectTransaction> builder)
        {
            builder.ToTable("ProjectTransactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceId).IsRequired(false);
            builder.Property(x => x.BillId).IsRequired(false);
            builder.Property(x => x.ProjectId).IsRequired();
            builder.Property(x => x.TransType).IsRequired();
            builder.HasOne(x => x.Invoice).WithMany().HasForeignKey(x => x.InvoiceId);
            builder.HasOne(x => x.Bill).WithMany().HasForeignKey(x => x.BillId);
        }
    }
}

