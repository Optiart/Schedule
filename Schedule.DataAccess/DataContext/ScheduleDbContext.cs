namespace Schedule.DataAccess
{
    using System.Data.Entity;

    public partial class ScheduleDbContext : DbContext
    {
        public ScheduleDbContext()
            : base("name=ScheduleDbContext")
        {
        }

        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<Tabs> Tabs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Results>()
                .Property(e => e.result)
                .IsUnicode(false);

            modelBuilder.Entity<Tabs>()
                .Property(e => e.productivity)
                .IsUnicode(false);

            modelBuilder.Entity<Tabs>()
                .Property(e => e.work_per_pallete)
                .IsUnicode(false);

            modelBuilder.Entity<Tabs>()
                .HasMany(e => e.Results)
                .WithRequired(e => e.Tabs)
                .HasForeignKey(e => e.tab_id);
        }
    }
}
