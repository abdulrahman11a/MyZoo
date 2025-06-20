namespace Clinic.infrastructure.Configuration
{
    internal class department_Config : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            #region Properties
            builder.Property(d => d.DeptName).IsRequired().HasMaxLength(200);    
            builder.Property(d=>d.Description).IsRequired().HasMaxLength(200);  

            #endregion

            #region Relationships
              //Handel in class vet_confi
            #endregion
        }
    } 
}
