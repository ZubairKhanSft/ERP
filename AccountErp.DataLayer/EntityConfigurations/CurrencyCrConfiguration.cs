using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class CurrencyCrConfiguration : IEntityTypeConfiguration<CurrencyCr>
    {
        public void Configure(EntityTypeBuilder<CurrencyCr> builder)
        {
            builder.ToTable("CurrencyCr");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.Name).IsRequired();
          /*  builder.Property(x => x.StreetName).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.City).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.State).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.PostalCode).IsRequired(false).HasMaxLength(50);
*/
          builder.Property(x=> x.Symbol).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
         
        }
    }
}
