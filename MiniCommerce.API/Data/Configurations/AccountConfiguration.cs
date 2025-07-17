using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(a => a.Email)
            .IsUnique(); // Unique constraint for Email
        
        builder.Property(a => a.Password)
            .IsRequired();
        
        builder.Property(a => a.Role)
            .IsRequired();
        
        builder.Property(a => a.RefreshToken)
            .IsRequired(false);
    }
}