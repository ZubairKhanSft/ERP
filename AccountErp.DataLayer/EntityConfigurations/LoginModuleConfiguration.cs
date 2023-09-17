using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class LoginModuleConfiguration : IEntityTypeConfiguration<LoginModule>
    {
        public void Configure(EntityTypeBuilder<LoginModule> builder)
        {
            {

                builder.Property(x => x.Id).IsRequired();
                builder.Property(x => x.UserId).IsRequired();
                builder.Property(x => x.createdOn).IsRequired(false);
                builder.Property(x => x.status).IsRequired(false);
                builder.Property(x => x.RoleId).IsRequired(false);
                builder.HasOne(x => x.user).WithMany().HasForeignKey(x => x.UserId);
            }
        }
    }
}
