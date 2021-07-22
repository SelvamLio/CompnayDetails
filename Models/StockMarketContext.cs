using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CompnayDetails.Models
{
    public partial class StockMarketContext : DbContext
    {
        public StockMarketContext()
        {
        }

        public StockMarketContext(DbContextOptions<StockMarketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompanyDetail> CompanyDetails { get; set; }
        public virtual DbSet<StockExchange> StockExchanges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=StockMarket;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CompanyDetail>(entity =>
            {
                entity.HasKey(e => new { e.CompanyId, e.CompanyCode })
                    .HasName("PK_CompanyId");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyCeo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CompanyCEO");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Turnover).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Website)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.StockExchangeNavigation)
                    .WithMany(p => p.CompanyDetails)
                    .HasForeignKey(d => d.StockExchange)
                    .HasConstraintName("Fk_StockExchange_ExchangeID");
            });

            modelBuilder.Entity<StockExchange>(entity =>
            {
                entity.HasKey(e => e.ExchangeId)
                    .HasName("PK_ExchangeId");

                entity.ToTable("StockExchange");

                entity.Property(e => e.ExchangeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ExchangeID");

                entity.Property(e => e.ExchangeName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
