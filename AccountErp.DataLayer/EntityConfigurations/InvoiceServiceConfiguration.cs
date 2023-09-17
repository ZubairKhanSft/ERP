using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class InvoiceServiceConfiguration : IEntityTypeConfiguration<InvoiceService>
    {
        public void Configure(EntityTypeBuilder<InvoiceService> builder)
        {
            builder.ToTable("InvoiceServices");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.InvoiceId).IsRequired();
            builder.Property(x => x.ServiceId).IsRequired(false);
            builder.Property(x => x.ProductId).IsRequired(false);
            builder.Property(x => x.Rate).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.Price).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.TaxPrice).IsRequired().HasColumnType("NUMERIC(12,2)");
            builder.Property(x => x.TaxId).IsRequired(false);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.TaxPercentage).IsRequired(false);
            builder.Property(x => x.LineAmount).IsRequired().HasColumnType("NUMERIC(12,2)");
           // builder.HasOne(x => x.Service).WithMany().HasForeignKey(x => x.ServiceId);
            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.Taxes).WithMany().HasForeignKey(x => x.TaxId);
            builder.HasOne(x => x.Invoice).WithMany().HasForeignKey(x => x.InvoiceId);

            builder.HasOne(x => x.Invoice)
        .WithMany(c => c.Services)
        .HasForeignKey(cf => cf.InvoiceId)
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
