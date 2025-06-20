namespace Clinic.Infrastructure.Configuration
{
    internal class notificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
         
            #region Properties
            builder.Property(n => n.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(n => n.CreatedAT)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.Property(n => n.NotificationType)
                   .IsRequired()
                   .HasConversion(
                       v => v.ToString(),
                       v => (NotificationType)Enum.Parse(typeof(NotificationType), v, true)
                   );

            builder.Property(n => n.STAT)
                   .IsRequired()
                   .HasDefaultValue(NotificationStatus.Pending)
                   .HasConversion(
                       v => v.ToString(),
                       v => (NotificationStatus)Enum.Parse(typeof(NotificationStatus), v, true)
                   );

            builder.Property(n => n.VetId).IsRequired(false);
            builder.Property(n => n.PatientId).IsRequired(false);
            #endregion

            #region Relationships


            builder.HasOne(n => n.Vet)
                .WithMany(v => v.Notifications)
                .HasForeignKey(n => n.VetId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasOne(n => n.Patient)
                   .WithMany(p => p.Notifications)
                   .HasForeignKey(n => n.PatientId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .IsRequired(false);


            #endregion

            #region Indexing
            builder.HasIndex(n => n.VetId);
            builder.HasIndex(n => n.PatientId);
            builder.HasIndex(n => n.CreatedAT);
            builder.HasIndex(n => n.STAT);
            #endregion
        }

    }
}