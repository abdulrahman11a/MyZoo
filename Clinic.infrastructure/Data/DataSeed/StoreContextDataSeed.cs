namespace Clinic.Infrastructure.Data.DataSeed
{
    public static class StoreContextDataSeed
    {
        public static async Task DataSeedAsync(StoreDbContext dbContext, ILogger logger = null)
        {
            try
            {
                // Define the base path for JSON files relative to the project directory
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Clinic.Infrastructure", "Data", "DataSeed", "JsonFiles");

                // Seed Departments
                if (!dbContext.Departments.Any())
                {
                    var departmentFilePath = Path.Combine(basePath, "departments.json");
                    if (!File.Exists(departmentFilePath))
                    {
                        logger?.LogError($"Departments JSON file not found at: {departmentFilePath}");
                        throw new FileNotFoundException($"Departments JSON file not found at: {departmentFilePath}");
                    }

                    logger?.LogInformation("Seeding Departments...");
                    var departmentData = await File.ReadAllTextAsync(departmentFilePath);
                    var departments = JsonSerializer.Deserialize<List<Department>>(departmentData);
                    if (departments?.Count > 0)
                    {
                        await dbContext.Departments.AddRangeAsync(departments);
                        await dbContext.SaveChangesAsync();
                        logger?.LogInformation("Departments seeded successfully.");
                    }
                    else
                    {
                        logger?.LogWarning("No Departments data found to seed.");
                    }
                }
                else
                {
                    logger?.LogInformation("Departments already seeded, skipping...");
                }

                // Seed Locations
                if (!dbContext.Locations.Any())
                {
                    var locationFilePath = Path.Combine(basePath, "locations.json");
                    if (!File.Exists(locationFilePath))
                    {
                        logger?.LogError($"Locations JSON file not found at: {locationFilePath}");
                        throw new FileNotFoundException($"Locations JSON file not found at: {locationFilePath}");
                    }

                    logger?.LogInformation("Seeding Locations...");
                    var locationData = await File.ReadAllTextAsync(locationFilePath);
                    var locations = JsonSerializer.Deserialize<List<Location>>(locationData);
                    if (locations?.Count > 0)
                    {
                        await dbContext.Locations.AddRangeAsync(locations);
                        await dbContext.SaveChangesAsync();
                        logger?.LogInformation("Locations seeded successfully.");
                    }
                    else
                    {
                        logger?.LogWarning("No Locations data found to seed.");
                    }
                }
                else
                {
                    logger?.LogInformation("Locations already seeded, skipping...");
                }

                // Seed Vets
                if (!dbContext.Vets.Any())
                {
                    var vetFilePath = Path.Combine(basePath, "Vets.json");
                    if (!File.Exists(vetFilePath))
                    {
                        logger?.LogError($"Vets JSON file not found at: {vetFilePath}");
                        throw new FileNotFoundException($"Vets JSON file not found at: {vetFilePath}");
                    }

                    logger?.LogInformation("Seeding Vets...");
                    var vetData = await File.ReadAllTextAsync(vetFilePath);
                    var vets = JsonSerializer.Deserialize<List<Vet>>(vetData);
                    if (vets?.Count > 0)
                    {
                        await dbContext.Vets.AddRangeAsync(vets);
                        await dbContext.SaveChangesAsync();
                        logger?.LogInformation("Vets seeded successfully.");
                    }
                    else
                    {
                        logger?.LogWarning("No Vets data found to seed.");
                    }
                }
                else
                {
                    logger?.LogInformation("Vets already seeded, skipping...");
                }

                // Seed Patients
                if (!dbContext.Patients.Any())
                {
                    var patientFilePath = Path.Combine(basePath, "patients.json");
                    if (!File.Exists(patientFilePath))
                    {
                        logger?.LogError($"Patients JSON file not found at: {patientFilePath}");
                        throw new FileNotFoundException($"Patients JSON file not found at: {patientFilePath}");
                    }

                    logger?.LogInformation("Seeding Patients...");
                    var patientData = await File.ReadAllTextAsync(patientFilePath);
                    var patients = JsonSerializer.Deserialize<List<Patient>>(patientData);
                    if (patients?.Count > 0)
                    {
                        await dbContext.Patients.AddRangeAsync(patients);
                        await dbContext.SaveChangesAsync();
                        logger?.LogInformation("Patients seeded successfully.");
                    }
                    else
                    {
                        logger?.LogWarning("No Patients data found to seed.");
                    }
                }
                else
                {
                    logger?.LogInformation("Patients already seeded, skipping...");
                }

                // Seed ScheduleTimes
                if (!dbContext.ScheduleTimes.Any())
                {
                    var scheduleTimeFilePath = Path.Combine(basePath, "scheduleTimes.json");
                    if (!File.Exists(scheduleTimeFilePath))
                    {
                        logger?.LogError($"ScheduleTimes JSON file not found at: {scheduleTimeFilePath}");
                        throw new FileNotFoundException($"ScheduleTimes JSON file not found at: {scheduleTimeFilePath}");
                    }

                    logger?.LogInformation("Seeding ScheduleTimes...");
                    var scheduleTimeData = await File.ReadAllTextAsync(scheduleTimeFilePath);
                    var scheduleTimes = JsonSerializer.Deserialize<List<ScheduleTime>>(scheduleTimeData);
                    if (scheduleTimes?.Count > 0)
                    {
                        await dbContext.ScheduleTimes.AddRangeAsync(scheduleTimes);
                        await dbContext.SaveChangesAsync();
                        logger?.LogInformation("ScheduleTimes seeded successfully.");
                    }
                    else
                    {
                        logger?.LogWarning("No ScheduleTimes data found to seed.");
                    }
                }
                else
                {
                    logger?.LogInformation("ScheduleTimes already seeded, skipping...");
                }

                // Seed Appointments
                //if (!dbContext.Appointments.Any())
                //{
                //    var appointmentFilePath = Path.Combine(basePath, "appointments.json");
                //    if (!File.Exists(appointmentFilePath))
                //    {
                //        logger?.LogError($"Appointments JSON file not found at: {appointmentFilePath}");
                //        throw new FileNotFoundException($"Appointments JSON file not found at: {appointmentFilePath}");
                //    }

                //    logger?.LogInformation("Seeding Appointments...");

                //    var appointmentData = await File.ReadAllTextAsync(appointmentFilePath);

                //    var options = new JsonSerializerOptions
                //    {
                //        PropertyNameCaseInsensitive = true,
                //        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                //    };

                //    var appointments = JsonSerializer.Deserialize<List<Appointment>>(appointmentData, options);
                //    if (appointments?.Count > 0)
                //    {
                //        await dbContext.Appointments.AddRangeAsync(appointments);
                //        await dbContext.SaveChangesAsync();
                //        logger?.LogInformation("Appointments seeded successfully.");
                //    }
                //    else
                //    {
                //        logger?.LogWarning("No Appointments data found to seed.");
                //    }
                //}
                //else
                //{
                //    logger?.LogInformation("Appointments already seeded, skipping...");
                //}


                // Seed Prescriptions
                if (!dbContext.Prescriptions.Any())
                {
                    var prescriptionFilePath = Path.Combine(basePath, "prescriptions.json");
                    if (!File.Exists(prescriptionFilePath))
                    {
                        logger?.LogError($"Prescriptions JSON file not found at: {prescriptionFilePath}");
                        throw new FileNotFoundException($"Prescriptions JSON file not found at: {prescriptionFilePath}");
                    }

                    logger?.LogInformation("Seeding Prescriptions...");
                    var prescriptionData = await File.ReadAllTextAsync(prescriptionFilePath);
                    var prescriptions = JsonSerializer.Deserialize<List<Prescription>>(prescriptionData);
                    if (prescriptions?.Count > 0)
                    { 
                        await dbContext.Prescriptions.AddRangeAsync(prescriptions);
                        await dbContext.SaveChangesAsync();
                        logger?.LogInformation("Prescriptions seeded successfully.");
                    }
                    else
                    {
                        logger?.LogWarning("No Prescriptions data found to seed.");
                    }
                }
                else
                {
                    logger?.LogInformation("Prescriptions already seeded, skipping...");
                }

               
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred during data seeding.");
                throw; // Re-throw to ensure the application stops if seeding fails
            }
        }
    }
}