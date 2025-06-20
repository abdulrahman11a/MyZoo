namespace Clinic.Infrastructure.Data
{
    public class AppIdentityDBContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure RefreshTokens as an owned entity
            modelBuilder.Entity<AppUser>()
                .OwnsMany(u => u.RefreshTokens, rt =>
                {
                    rt.ToTable("AppUser_RefreshTokens");
                    rt.WithOwner().HasForeignKey("AppUserId");
                    rt.Property(rt => rt.Token).IsRequired();
                    rt.Property(rt => rt.CreatedAt).IsRequired();
                    rt.Property(rt => rt.ExpireAt).IsRequired();
                });

        }
        public  DbSet<UserAddress> UserAddress { get; set; }    
    }
}
