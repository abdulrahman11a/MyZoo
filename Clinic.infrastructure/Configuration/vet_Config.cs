namespace Clinic.Infrastructure.Configuration
{
    internal class vetConfig : IEntityTypeConfiguration<Vet>
    {
        public void Configure(EntityTypeBuilder<Vet> builder)
        {

            #region Properties
            builder.Property(v => v.Name)
                   .IsRequired()
                   .HasMaxLength(100); 

       
            builder.Property(v => v.Age)
                   .IsRequired();

            
            builder.Property(v => v.email)
                   .IsRequired() // Matches the null! initializer
                   .HasMaxLength(255); // Standard length for email addresses

          
            builder.Property(v => v.DateOfGraduation)
                   .IsRequired()
                   .HasColumnType("datetime");

        
            builder.Property(v => v.DepartmentId)
                   .IsRequired();

               builder.Property(p => p.ISActive)
            .HasDefaultValue(true);
            #endregion

            #region Relationships
            // this two relation Ef understand in relation in class  not need handle By Fluent 
            // public ICollection<prescription> Prescriptions { get; set; } = new List<prescription>();
            //public ICollection<notification> Notifications { get; set; } = new List<notification>();

            // and  this hndle in  IEntityTypeConfiguration<scheduleTime,appointment>
            //public ICollection<scheduleTime> ScheduleTimes { get; set; } = new List<scheduleTime>();
            //public ICollection<appointment> Appointments { get; set; } = new List<appointment>();

       

            // Relationship: Vet → Department (Many-to-One)
            builder.HasOne(v => v.Department)
                   .WithMany(d => d.Vets)
                   .HasForeignKey(v => v.DepartmentId)
                   .OnDelete(DeleteBehavior.Cascade); // or .Cascade/.SetNull based on business rule

            #endregion

            #region Indexing
            // Index on email for faster lookups (e.g., login or search)
            builder.HasIndex(v => v.email)
                   .IsUnique(); // Assuming emails are unique for Vets

            // Index on DepartmentId for faster joins with Department
            builder.HasIndex(v => v.DepartmentId);
            #endregion
        }
    }
}