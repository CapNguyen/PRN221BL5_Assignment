using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assignment_Project.Models
{
    public partial class PRN221_FapProjectContext : DbContext
    {
        public PRN221_FapProjectContext()
        {
        }

        public PRN221_FapProjectContext(DbContextOptions<PRN221_FapProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=CAPZDAPOET;database=PRN221_FapProject;uid=sa;pwd=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Class)
                    .HasMaxLength(50)
                    .HasColumnName("class");

                entity.Property(e => e.Room)
                    .HasMaxLength(50)
                    .HasColumnName("room");

                entity.Property(e => e.Subject)
                    .HasMaxLength(50)
                    .HasColumnName("subject");

                entity.Property(e => e.Teacher)
                    .HasMaxLength(50)
                    .HasColumnName("teacher");

                entity.Property(e => e.TimeslotId).HasColumnName("timeslot_id");

                entity.Property(e => e.Week).HasColumnName("week");

                entity.Property(e => e.Weekday).HasColumnName("weekday");

                entity.HasOne(d => d.Timeslot)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.TimeslotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lesson_TimeSlot");
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.ToTable("TimeSlot");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Slot).HasColumnName("slot");

                entity.Property(e => e.Time)
                    .HasMaxLength(50)
                    .HasColumnName("time");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
