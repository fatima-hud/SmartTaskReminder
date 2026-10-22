using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartTaskReminder.Core.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace SmartTaskReminder.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(254)
                .IsRequired();

            builder.HasQueryFilter(u => !u.IsDeleted);

            builder.HasIndex(u => u.Email)
                .IsUnique().HasFilter("[IsDeleted] = 0");

            builder.HasIndex(u => u.UserName)
               .IsUnique()
               .HasFilter("[IsDeleted] = 0");

            builder.HasOne(u=>u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.Gender)
             .HasConversion<string>();


        }
    }
}
