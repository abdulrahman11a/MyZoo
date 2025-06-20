namespace Clinic.Infrastructure.Configuration
{
    internal class patientConfig : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            #region Properties
            // Single HasKey call to define the primary key
            builder.HasKey(p => p.Id);

            // Disabling auto-increment/IDENTITY on the Id column
            builder.Property(p => p.Id)
                   .ValueGeneratedNever(); // Prevents auto-generation of Id values

            // Configuring FullName
            builder.Property(p => p.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            // Configuring Email
            builder.Property(p => p.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            // Configuring Address
            builder.Property(p => p.Address)
                   .IsRequired()
                   .HasMaxLength(500);

            // Configuring PhoneNumber
            builder.Property(p => p.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

           
            builder.Property(p => p.ISActive) 
                   .HasDefaultValue(true);
            #endregion

            #region Indexing
            // Index on Email for faster lookups (e.g., login or search)
            builder.HasIndex(p => p.Email)
                   .IsUnique();

            // Index on IsActive for filtering active/inactive patients
            builder.HasIndex(p => p.ISActive);
            #endregion
        }
    }
}
