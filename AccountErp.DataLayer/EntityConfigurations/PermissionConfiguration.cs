using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Permisions).IsRequired();
            builder.Property(x => x.PermisionDescription).IsRequired();
          //  builder.Property(x => x.CompanyId).IsRequired();

            builder.Property(x => x.PermissionCode).IsRequired();

            builder.Property(x => x.ScreenId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ScreenCode).IsRequired();
            builder.Property(x => x.ScreenUrl).IsRequired();

            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(40);
            builder.Property(x => x.UpdatedOn).IsRequired(false); //  builder.Property(x => x.updatedDate).IsRequired(false);
            builder.Property(x => x.ScreenUrl).IsRequired();
            builder.Property(x => x.Permisions).IsRequired();
            builder.Property(x => x.ScreenCode).IsRequired();
            builder.Property(x => x.PermisionDescription).IsRequired();
            builder.HasOne(x => x.Screen).WithMany().HasForeignKey(x => x.ScreenId);
            builder.Property(x => x.Time_stamp).IsRequired(false);
        }
    }
}
