using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(u => u.LastName)
            .IsRequired(false)
            .HasMaxLength(50);
        
        builder.Property(u => u.Phone)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(u => u.Address).IsRequired();
        
        // 1-to-1 relationship between User and Account
        builder.HasOne(u => u.Account)
            .WithOne(a => a.User)
            .HasForeignKey<User>(u => u.AccountId);
    }
}