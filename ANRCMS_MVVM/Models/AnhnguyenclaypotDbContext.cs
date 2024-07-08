using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ANRCMS_MVVM.Models;

public partial class AnhnguyenclaypotDbContext : DbContext
{
    public static AnhnguyenclaypotDbContext INSTANCE = new AnhnguyenclaypotDbContext();
    public AnhnguyenclaypotDbContext()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
    }

    public AnhnguyenclaypotDbContext(DbContextOptions<AnhnguyenclaypotDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-VSTI326\\VKHOANG;Initial Catalog=ANHNGUYENCLAYPOT_DB; Trusted_Connection=SSPI;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branch__A1682FA505184C60");

            entity.ToTable("Branch");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.BranchName).HasMaxLength(255);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B84E5CBC5B");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.CustomerPhone, "UQ__Customer__390618B38687A716").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.CustomerPhone).HasMaxLength(11);
            entity.Property(e => e.Password).HasMaxLength(30);
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CB4E1363BB");

            entity.ToTable("Food");

            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.FoodEnglishName).HasMaxLength(150);
            entity.Property(e => e.FoodVietnameseName).HasMaxLength(150);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF0CBF0AAC");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

            entity.HasOne(d => d.Branch).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__BranchID__44FF419A");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Order__CustomerI__440B1D61");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.FoodId }).HasName("PK__OrderDet__6BC68093290F2FAE");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");

            entity.HasOne(d => d.Food).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__FoodI__46E78A0C");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__45F365D3");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF76E4A1FA4");

            entity.HasIndex(e => e.StaffPhone, "UQ__Staff__143C00B3216596EB").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.StaffName).HasMaxLength(100);
            entity.Property(e => e.StaffPhone).HasMaxLength(11);

            entity.HasOne(d => d.Branch).WithMany(p => p.Staff)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__BranchID__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
