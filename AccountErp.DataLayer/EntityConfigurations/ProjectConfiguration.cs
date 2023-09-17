using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {

                builder.ToTable("Projects");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();

                builder.Property(x => x.ProjectName).IsRequired().HasMaxLength(250);
                builder.Property(x => x.CustomerId).IsRequired();
                builder.Property(x => x.Status).IsRequired();
                builder.Property(x => x.CreatedOn).IsRequired();
                builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(40);
                builder.Property(x => x.UpdatedOn).IsRequired(false);
                builder.Property(x => x.UpdatedBy).HasMaxLength(40);

                builder.HasMany(x => x.ProjectTransaction).WithOne().HasForeignKey(x => x.ProjectId);
            
        }
    }
}
