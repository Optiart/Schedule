namespace Schedule.DataAccess
{
    using System.Data.Entity;

    public partial class ScheduleDbContext : DbContext
    {
        public ScheduleDbContext()
            : base("name=ScheduleDbContext")
        {
        }

        public virtual DbSet<ResultDto> Results { get; set; }
        public virtual DbSet<TabDto> Tabs { get; set; }
        public virtual DbSet<AlgorithmSummaryDto> AlgorithmSummaries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResultDto>()
                .Property(e => e.Result)
                .IsUnicode(false);

            modelBuilder.Entity<TabDto>()
                .Property(e => e.Productivity)
                .IsUnicode(false);

            modelBuilder.Entity<TabDto>()
                .Property(e => e.WorkPerPallete)
                .IsUnicode(false);

            modelBuilder.Entity<TabDto>()
                .HasMany(e => e.Results)
                .WithRequired(e => e.Tabs)
                .HasForeignKey(e => e.TabId);

            modelBuilder.Entity<AlgorithmSummaryDto>()
                .Property(e => e.Cstar)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AlgorithmSummaryDto>()
                .Property(e => e.Cmax)
                .HasPrecision(18, 0);
        }
    }
}
