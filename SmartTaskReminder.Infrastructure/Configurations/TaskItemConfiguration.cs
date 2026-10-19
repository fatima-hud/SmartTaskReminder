using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SmartTaskReminder.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace SmartTaskReminder.Infrastructure.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(1000);

            builder.Property(t => t.Priority)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .IsRequired();
          
            builder.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(t => !t.IsDeleted);
        }

    }
}
