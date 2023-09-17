using AccountErp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.DataLayer.EntityConfigurations
{
    public class ScreenDetailConfiguration : IEntityTypeConfiguration<ScreenDetail>
    {
        public void Configure(EntityTypeBuilder<ScreenDetail> builder)
        {
            builder.ToTable("ScreenDetail");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.ScreenName).IsRequired();

            builder.Property(x => x.ScreenCode).IsRequired();
        }
    }
}

