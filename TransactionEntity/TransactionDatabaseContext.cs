using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TransactionEntity.Models
{
    public partial class TransactionDatabaseContext : DbContext
    {
        public TransactionDatabaseContext()
        {
        }

        public TransactionDatabaseContext(DbContextOptions<TransactionDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionFile> TransactionFile { get; set; }
        public virtual DbSet<TransactionImport> TransactionImport { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = (localdb)\\ProjectsV13; Database = TransactionDatabase; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.TransactionIdentificator)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TransactionImport>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
