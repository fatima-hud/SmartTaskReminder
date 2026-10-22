using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SmartTaskReminder.Core.Entities;
namespace SmartTaskReminder.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.HasIndex(r => r.Name).IsUnique();
            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}
