using BankAssignment.Models;
using BankAssignment.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankAssignment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccountType>()
            .Property(p => p.AccountType)
            .HasColumnType("varchar(30)");


            modelBuilder.Entity<ClientAccount>()
                .HasKey(ca => new { ca.ClientID, ca.AccountNum });

            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.Client)
                .WithMany(p => p.ClientAccounts)
                .HasForeignKey(fk => new { fk.ClientID })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientAccount>()
                .HasOne(p => p.BankAccount)
                .WithMany(p => p.ClientAccounts)
                .HasForeignKey(fk => new { fk.AccountNum })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BankAccount>()
                .Property(p => p.Balance)
                .HasColumnType("decimal(9,2)");

            modelBuilder.Entity<BankAccount>()
                .HasOne<BankAccountType>(s => s.BankAccountType)
                .WithMany(g => g.BankAccounts)
                .HasForeignKey(s => s.AccountType);


            modelBuilder.Entity<Client>()
                .Property(p => p.FirstName)
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Client>()
                .Property(p => p.LastName)
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Client>()
                .Property(p => p.Email)
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<BankAccountType>()
            .HasData(new BankAccountType { AccountType = "Chequing" }
                    , new BankAccountType { AccountType = "Savings" }
                    , new BankAccountType { AccountType = "Investment" }
                    , new BankAccountType { AccountType = "RRSP" }
                    , new BankAccountType { AccountType = "RESP" }
                    , new BankAccountType { AccountType = "Tax Free Savings" }
            );

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<ClientAccountVM> ClientAccountVM { get; set; }
        public DbSet<BankAccountType> BankAccountType { get; set; }
        public DbSet<BankAssignment.ViewModels.BankAccountVM> BankAccountVM { get; set; }
    }
}