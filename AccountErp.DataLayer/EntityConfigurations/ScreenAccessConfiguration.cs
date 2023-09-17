using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public  class ScreenAccessConfiguration : IEntityTypeConfiguration<ScreenAccess>
    {
        public void Configure(EntityTypeBuilder<ScreenAccess> builder)
        {
            builder.ToTable("ScreenAccess");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.ScreenId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired(false);
            builder.Property(x => x.CanAccess).IsRequired();
            builder.HasOne(x => x.Screen).WithMany().HasForeignKey(x => x.ScreenId);
            builder.HasOne(x => x.UserRole).WithMany().HasForeignKey(x => x.RoleId);
            builder.Property(x => x.Time_stamp).IsRequired(false);

        }
    }
}
