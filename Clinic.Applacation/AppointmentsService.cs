namespace Clinic.Applacation
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IDatabase _database;
        private const string AppointmentKeyPrefix = "appointment:";
        public AppointmentsService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            string key = $"{AppointmentKeyPrefix}{appointmentId}";
            bool exists = await _database.KeyExistsAsync(key);

            if (!exists)
            {
                return false;
            }

            // Delete the appointment from Redis
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            string key = $"{AppointmentKeyPrefix}{appointmentId}";
            string cachedAppointment = await _database.StringGetAsync(key);

            if (string.IsNullOrEmpty(cachedAppointment))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Appointment>(cachedAppointment);
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            if (appointment == null )
            {
                return null; 
            }

            string key = await CreateIdAppointmentAsync(appointment);

            TimeSpan expiration;
            DateTime nowLocal = DateTime.Now; 

            if (appointment.AppointmentDate > nowLocal)
            {
                expiration = appointment.AppointmentDate - nowLocal;

                if (expiration.TotalMinutes < 60)
                {
                    expiration = TimeSpan.FromDays(1);
                }
            }
            else
            {
                expiration = TimeSpan.FromDays(1);
            }
            
            bool success = await _database.StringSetAsync(key, JsonSerializer.Serialize(appointment), expiration);

            return success ? appointment : null;
        }
        public async Task<string> CreateIdAppointmentAsync(Appointment appointment)
        {
            string key = $"{AppointmentKeyPrefix}temp:{Guid.NewGuid()}";
            return key;        
        }


        public async Task<List<Appointment>> GetAllAppointmentsForVetAsync(int vetId)
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: "appointment:*").ToArray();

            var appointments = new List<Appointment>();

            foreach (var key in keys)
            {
                string json = await _database.StringGetAsync(key);
                if (!string.IsNullOrEmpty(json))
                {
                    var appointment = JsonSerializer.Deserialize<Appointment>(json);
                    if (appointment != null && appointment.VetId == vetId)
                    {
                        appointments.Add(appointment);
                    }
                }
            }

            return appointments;
        }



    }


}