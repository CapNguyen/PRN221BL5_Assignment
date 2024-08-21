using Microsoft.EntityFrameworkCore;

namespace Assignment3.Models;

public partial class PRN221_BL5_Assignment3Context : DbContext
{
    public PRN221_BL5_Assignment3Context()
    {
    }

    public PRN221_BL5_Assignment3Context(DbContextOptions<PRN221_BL5_Assignment3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendee> Attendees { get; set; } = null!;
    public virtual DbSet<Event> Events { get; set; } = null!;
    public virtual DbSet<EventCategory> EventCategories { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendee>(entity =>
        {
            entity.Property(e => e.AttendeeId).HasColumnName("AttendeeID");

            entity.Property(e => e.Email).HasMaxLength(255);

            entity.Property(e => e.EventId).HasColumnName("EventID");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.Property(e => e.RegistrationTime).HasColumnType("datetime");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Event)
                .WithMany(p => p.Attendees)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_Attendees_Events");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Attendees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Attendees_Users");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.EventId).HasColumnName("EventID");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            entity.Property(e => e.Description).HasColumnType("text");

            entity.Property(e => e.EndTime).HasColumnType("datetime");

            entity.Property(e => e.Location).HasMaxLength(255);

            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Events_EventCategories");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.Property(e => e.Email).HasMaxLength(255);

            entity.Property(e => e.FullName).HasMaxLength(255);

            entity.Property(e => e.Password).HasMaxLength(255);

            entity.Property(e => e.Role).HasMaxLength(50);

            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}