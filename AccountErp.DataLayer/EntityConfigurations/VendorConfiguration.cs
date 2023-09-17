using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("Vendors");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.ShopName).IsRequired();
            builder.Property(x => x.ShopAddress).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(250);
            builder.Property(x => x.City).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ZipCode).IsRequired(false);
            builder.Property(x => x.State).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Country).IsRequired(false);

           /* builder.Property(x => x.AccountNumber).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.BankName).IsRequired(false).HasMaxLength(250);
            builder.Property(x => x.BankBranch).IsRequired(false).HasMaxLength(250);
            builder.Property(x => x.Ifsc).IsRequired(false).HasMaxLength(50);

            builder.Property(x => x.Discount).IsRequired(false).HasColumnType("NUMERIC(5,2)");*/

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            /*builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(40);
            builder.Property(x => x.UpdatedBy).HasMaxLength(40);

            builder.HasMany(x => x.Contacts).WithOne().HasForeignKey(x => x.VendorId);*/
        }
    }
}
