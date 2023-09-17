using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class UserScreenAccessConfiguration : IEntityTypeConfiguration<UserScreenAccess>
    {
        public void Configure(EntityTypeBuilder<UserScreenAccess> builder)
        {
            builder.ToTable("UserScreenAccess");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.ScreenId).IsRequired();
            builder.Property(x => x.UserRoleId).IsRequired();
            builder.Property(x => x.CanAccess).IsRequired();
            builder.HasOne(x => x.Screen).WithMany().HasForeignKey(x => x.ScreenId);
        }
    }
}