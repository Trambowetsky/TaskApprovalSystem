using Microsoft.EntityFrameworkCore;

namespace TaskApprovalSystem.Models;

public class AppDbContext : DbContext
{
    public DbSet<Request> Requests { get; set; }
    
    public DbSet<AuditLog> AuditLogs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasMaxLength(2000);

            entity.Property(x => x.Status)
                .HasConversion<string>();

            entity.Property(x => x.Type)
                .HasConversion<string>();
            
            entity.HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.CreatedById);
        });
        
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OldStatus)
                .HasConversion<string>();

            entity.Property(x => x.NewStatus)
                .HasConversion<string>();

            entity.Property(x => x.ChangedBy)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}