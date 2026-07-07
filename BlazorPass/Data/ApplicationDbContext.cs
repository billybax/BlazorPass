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
	 entity.Property(e => e.UserId).HasColumnName("user_id");
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
	 entity.Property(e => e.ClientUpdatedAt).HasColumnName("client_updated_at").HasColumnType("timestamp with time zone");
	 entity.Property(e => e.ServerUpdatedAt).HasColumnName("server_updated_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("now()");
	 entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
	 entity.HasIndex(e => e.UserId).HasDatabaseName("idx_training_history_user_id");
	 entity.HasIndex(e => e.ServerUpdatedAt).HasDatabaseName("idx_training_history_server_updated_at");
	 entity.HasIndex(e => new { e.UserId, e.ClientId }).HasDatabaseName("idx_training_history_user_client_id").IsUnique();
 });

 base.OnModelCreating(modelBuilder);
 }
}
