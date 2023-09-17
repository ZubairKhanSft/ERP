using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountErp.DataLayer.EntityConfigurations
{
   public class ReconciliationConfigurations : IEntityTypeConfiguration<Reconciliation>
    {
        public void Configure(EntityTypeBuilder<Reconciliation> builder)
        {
            builder.ToTable("Reconciliations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.ReconciliationDate).IsRequired(false);
            builder.Property(x => x.StatementBalance).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.IcloseBalance).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.IsReconciliation).IsRequired();
            builder.Property(x => x.ReconciliationStatus).IsRequired();
            builder.HasOne(x => x.bank).WithMany().HasForeignKey(x => x.BankAccountId);

        }
    }
}
