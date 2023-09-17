using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
  public class EndingStatementBalanceConfiguration : IEntityTypeConfiguration<EndingStatementBalance>
    {
        public void Configure(EntityTypeBuilder<EndingStatementBalance> builder)
        {
            builder.ToTable("EndingStatementBalance");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.EndingBalanceDate).IsRequired(false);
            builder.Property(x => x.EndingBalanceAmount).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.HasOne(x => x.bank).WithMany().HasForeignKey(x => x.BankAccountId);

        }
    }
}
