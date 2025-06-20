namespace Clinic.infrastructure.Configuration
{
    public class ScheduleTime_Config : IEntityTypeConfiguration<ScheduleTime>
    {
        public void Configure(EntityTypeBuilder<ScheduleTime> builder)
        {
            #region Properties
            builder.Property(sh => sh.StartTime)
                  .IsRequired()
                  .HasColumnType("datetime");

            builder.Property(sh => sh.EndTime)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.Property(sh => sh.Capacity)
                   .IsRequired()
                   .HasDefaultValue(1);
            builder.Property(sh => sh.ScheduleGroup)
      .HasConversion(
          sg => GetEnumMemberValue(sg),
          val => GetEnumFromMemberValue<ScheduleGroup>(val));

            #endregion

            #region Relationships

            // Relationship with Vet (one-to-many)
            builder.HasOne(sh => sh.Vet)
                   .WithMany(v => v.ScheduleTimes)
                   .HasForeignKey(sh => sh.VetId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Relationship with Vet (one-to-many)
            builder.HasOne(sh => sh.Location)
                   .WithMany(l => l.ScheduleTimes)
                   .HasForeignKey(sh => sh.LocationId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Indxing
            // Unique index to prevent overlapping schedules for a vet at a location
            builder.HasIndex(sh => new { sh.VetId, sh.LocationId, sh.StartTime, sh.EndTime })
                 .IsUnique();
            #endregion

        }

/*
        Database:
You need to manually map the enum to the EnumMember values, because ToString() doesn't respect EnumMember.
*/
    public static string GetEnumMemberValue<T>(T enumValue) where T : Enum
        {
            var member = typeof(T).GetMember(enumValue.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                                   .FirstOrDefault() as EnumMemberAttribute;
            return attribute?.Value ?? enumValue.ToString();
        }

        public static T GetEnumFromMemberValue<T>(string value) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(EnumMemberAttribute)) as EnumMemberAttribute;

                if ((attribute != null && attribute.Value == value) || field.Name == value)
                    return (T)field.GetValue(null);
            }

            throw new ArgumentException($"Unknown value '{value}' for enum '{typeof(T)}'");
        }


        public class EnumMemberConverter<T> : JsonConverter<T> where T : struct, Enum
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value = reader.GetString();
                return ScheduleTime_Config.GetEnumFromMemberValue<T>(value);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(ScheduleTime_Config.GetEnumMemberValue(value));
            }
        }

    }
}
