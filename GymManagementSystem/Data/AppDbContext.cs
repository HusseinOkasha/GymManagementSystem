
using GymManagementSystem.Data;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gym_Management_System.Data;

public class AppDbContext : IdentityDbContext<AccountModel>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<AccountModel> Accounts { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        new AccountConfiguration().Configure(modelBuilder.Entity<AccountModel>());
    }
    
}