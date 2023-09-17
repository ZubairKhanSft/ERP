using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public  class UserScreenPermissionAccessConfiguration : IEntityTypeConfiguration<UserScreenPermissionAccess>
    {
        public void Configure(EntityTypeBuilder<UserScreenPermissionAccess> builder)
        {
            builder.ToTable("UserScreenPermissionAccess");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.RoleId).IsRequired();
            builder.Property(x => x.ScreenId).IsRequired();
            builder.Property(x => x.PermissionId).IsRequired();
            builder.Property(x => x.CompanyId).IsRequired();

            //  builder.Property(x => x.Status).IsRequired(false);
            builder.Property(x => x.CreatedOn).IsRequired(false);
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.Time_stamp).IsRequired(false);
        }
    }
}
