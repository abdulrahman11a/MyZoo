namespace Clinic.Infrastructure.Configuration
{
    internal class locationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            #region Properties
            builder.Property(l => l.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(l => l.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(12);

            builder.Property(l => l.Address)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(sh => sh.Capacity)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(l => l.Description)
                   .HasMaxLength(1000);

            builder.Property(l => l.IsActive)
                   .HasDefaultValue(true);
            #endregion

            #region Relationships
            // Relationships (Appointments, ScheduleTimes) are inferred by EF Core
            // and configured in appointmentConfig.cs and scheduleTimeConfig.cs
            #endregion

            #region Indexing
            // Index on NAME for faster lookups
            builder.HasIndex(l => l.Name)
                   .IsUnique();

            // Index on ISActive for filtering active/inactive locations
            builder.HasIndex(l => l.IsActive);
            #endregion
        }
    }
}