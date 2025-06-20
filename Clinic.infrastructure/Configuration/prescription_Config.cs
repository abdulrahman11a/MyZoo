namespace Clinic.Infrastructure.Configuration
{
    internal class prescriptionConfig : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
          

            #region Properties
            builder.Property(p => p.MedicationName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Dosage)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.Frequency)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.Instructions)
                   .HasMaxLength(500);

            builder.Property(p => p.StartDate)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.Property(p => p.EndDate)
                   .IsRequired()
                   .HasColumnType("datetime");

            // Foreign keys to Patient and Vet
            builder.Property(p => p.PatientId)
                   .IsRequired();

            builder.Property(p => p.VetId)
                   .IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(p => p.Patient)
                   .WithMany(p=>p.Prescriptions)
                   .HasForeignKey(p => p.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Vet)
                   .WithMany(v=>v.Prescriptions)
                   .HasForeignKey(p => p.VetId)
                   .OnDelete(DeleteBehavior.Cascade);


            #endregion

            #region Indexing
            // Index on PatientId for faster joins
            builder.HasIndex(p => p.PatientId);

            // Index on VetId for faster joins
            builder.HasIndex(p => p.VetId);

            // Index on StartDate and EndDate for range queries
            builder.HasIndex(p => new { p.StartDate, p.EndDate });
            #endregion
        }
    }
}