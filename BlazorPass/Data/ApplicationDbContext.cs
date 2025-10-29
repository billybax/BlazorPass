using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
 public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
 : base(options)
 {
 }

 public DbSet<LocPass> LocPasses { get; set; } = default!;

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
 modelBuilder.Entity<LocPass>(entity =>
 {
	 entity.ToTable("LocPass");
	 entity.HasKey(e => e.Id);
	 entity.Property(e => e.ResName);
	 entity.Property(e => e.Login);
	 entity.Property(e => e.Pass);
	 entity.Property(e => e.Comment);
 });

 base.OnModelCreating(modelBuilder);
 }
}
