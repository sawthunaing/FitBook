using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Stn.FitBook.Domain.Models.Entities;

#nullable disable

namespace Stn.FitBook.Repo.Ef.Repositories
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<ClassSchedule> ClassSchedules { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPackage> UserPackages { get; set; }
        public virtual DbSet<Waitlist> Waitlists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasIndex(e => e.ScheduleId, "ScheduleID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.BookingId)
                    .HasColumnType("int(11)")
                    .HasColumnName("BookingID");

                entity.Property(e => e.ScheduleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ScheduleID");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("Bookings_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Bookings_ibfk_1");
            });

            modelBuilder.Entity<ClassSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId)
                    .HasName("PRIMARY");

                entity.Property(e => e.ScheduleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ScheduleID");

                entity.Property(e => e.AvailableSlots).HasColumnType("int(11)");

                entity.Property(e => e.ClassName).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.RequiredCredits).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(e => e.PackageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PackageID");

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.Credits).HasColumnType("int(11)");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.PackageName).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.RefreshToken1)
                    .HasMaxLength(50)
                    .HasColumnName("RefreshToken");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "Email")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);
            });

            modelBuilder.Entity<UserPackage>(entity =>
            {
                entity.HasIndex(e => e.PackageId, "PackageID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.UserPackageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserPackageID");

                entity.Property(e => e.AvailableCredits).HasColumnType("int(11)");

                entity.Property(e => e.PackageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PackageID");

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.UserPackages)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("UserPackages_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPackages)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserPackages_ibfk_1");
            });

            modelBuilder.Entity<Waitlist>(entity =>
            {
                entity.ToTable("Waitlist");

                entity.HasIndex(e => e.ScheduleId, "ScheduleID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.WaitlistId)
                    .HasColumnType("int(11)")
                    .HasColumnName("WaitlistID");

                entity.Property(e => e.ScheduleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ScheduleID");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Waitlists)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("Waitlist_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Waitlists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Waitlist_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
