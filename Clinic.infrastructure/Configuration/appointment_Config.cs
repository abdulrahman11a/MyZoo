namespace Clinic.infrastructure.Configuration
{
    internal class appointment_Config : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            #region Properties
          
            builder.Property(a => a.AppointmentDate)
                   .IsRequired()
                   .HasColumnType("datetime");

            // Purpose of the appointment
            builder.Property(a => a.Purpose).IsRequired()
                   .HasMaxLength(500);

            // Status as an enum, stored as a string
            // Approach 1: Using .HasConversion<string>()
            /*builder.Property(a => a.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasDefaultValue(AppointmentStatus.Scheduled);*/

            // Approach 2: Using explicit HasConversion with custom logic
            builder.Property(a => a.Status)
                   .IsRequired()
                   .HasConversion(
                       storeStatus => storeStatus.ToString(),
                       useStatus => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), useStatus)
                   )
                   .HasDefaultValue(AppointmentStatus.Pending);
            #endregion

            #region Relationships
            // Relationship with Patient (one-to-many)
            builder.HasOne(a => a.Patient)
                   .WithMany(p => p.Appointments)
                   .HasForeignKey(a => a.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Vet (one-to-many)
            builder.HasOne(a => a.Vet)
                   .WithMany(v => v.Appointments)
                   .HasForeignKey(a => a.VetId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Location (one-to-many)
            builder.HasOne(a => a.Location)
                   .WithMany(l => l.Appointments)
                   .HasForeignKey(a => a.LocationId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Indexing
            // Index on AppointmentDate for faster queries
            builder.HasIndex(a => a.AppointmentDate);

            // Index on Status for filtering by appointment status
            builder.HasIndex(a => a.Status);

            // Composite index on VetId, LocationId, and AppointmentDate to optimize queries
            builder.HasIndex(a => new { a.VetId, a.LocationId, a.AppointmentDate });
            #endregion
        }
    }
}
