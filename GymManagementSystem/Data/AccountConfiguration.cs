
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.Data;

public class AccountConfiguration: IEntityTypeConfiguration<AccountModel>
{
    public void Configure(EntityTypeBuilder<AccountModel> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey("Id");
        builder.Property("BranchId").HasColumnType("int").IsRequired();
        builder.Property("FirstName").HasColumnType("varchar").HasMaxLength(255).IsRequired();
        builder.Property("LastName").HasColumnType("varchar").HasMaxLength(255).IsRequired();
        builder.Property("Email").HasColumnType("varchar").HasMaxLength(255).IsRequired();
        builder.HasIndex(account => account.Email).IsUnique();
        builder.Property("PhoneNumber").HasColumnType("varchar").HasMaxLength(255).IsRequired();
        
    }
    
}