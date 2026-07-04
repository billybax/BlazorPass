using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
 public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
 : base(options)
 {
 }

 public DbSet<LocPass> LocPasses { get; set; } = default!;
 public DbSet<TrainingHistory> TrainingHistories { get; set; } = default!;

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

 modelBuilder.Entity<TrainingHistory>(entity =>
 {
	 entity.ToTable("training_history");
	 entity.HasKey(e => e.Id);
	 entity.Property(e => e.Id).HasColumnName("id");
	 entity.Property(e => e.TrainDate).HasColumnName("trainDate").HasColumnType("date");
	 entity.Property(e => e.TrainTime).HasColumnName("trainTime").HasColumnType("time");
	 entity.Property(e => e.Minutes).HasColumnName("minutes");
	 entity.Property(e => e.Train).HasColumnName("train").HasColumnType("text");
	 entity.Property(e => e.KpBefore).HasColumnName("kpBefore");
	 entity.Property(e => e.KpAfter).HasColumnName("kpAfter");
	 entity.Property(e => e.RFaktor).HasColumnName("rfaktor").HasColumnType("text");
	 entity.Property(e => e.Practise).HasColumnName("practise").HasColumnType("text");
	 entity.Property(e => e.Sleep).HasColumnName("sleep").HasColumnType("text");
	 entity.Property(e => e.Health).HasColumnName("health").HasColumnType("text");
	 entity.Property(e => e.ClientId).HasColumnName("client_id").HasColumnType("text");
	 entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("now()");
	 entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
	 entity.HasIndex(e => e.UpdatedAt).HasDatabaseName("idx_training_history_updated_at");
 });

 base.OnModelCreating(modelBuilder);
 }
}
