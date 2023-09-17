using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermission");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Permissionid).IsRequired();
            builder.Property(x => x.Roleid).IsRequired();

            builder.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.Permissionid);
            builder.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.Roleid);
            builder.Property(x => x.Time_stamp).IsRequired(false);

        }
    }
}
