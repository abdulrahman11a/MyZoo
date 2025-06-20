namespace Clinic.APIS.Jobs
{
    public class CapacityCleanupJob
    {
        private readonly StoreDbContext _dbContext;
        private readonly CapacityHelper _capacityHelper;
        private readonly IUnitOfWork _unitOfWork;

        public CapacityCleanupJob(StoreDbContext dbContext, CapacityHelper capacityHelper, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _capacityHelper = capacityHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task ReAddCapacityForExpiredAppointments()
        {
            var expiredAppointments = await _dbContext.Appointments
                .Where(a => a.Status == AppointmentStatus.Cancelled ||
                           (a.AppointmentDate != null && a.AppointmentDate <= DateTime.UtcNow))
                .ToListAsync();

            foreach (var appointment in expiredAppointments)
            {
                // Dynamically fetch the ScheduleTime for this appointment
                var scheduleSpec = new ScheduleTimeByVetLocationAndTimeSpecification(
                    appointment.VetId,
                    appointment.LocationId,
                    appointment.AppointmentDate,
                    appointment.AppointmentDate.AddHours(1)
                );
                var scheduleTime = await _unitOfWork.Repository<ScheduleTime, int>().GetEntityWithSpecAsync(scheduleSpec);

                if (scheduleTime != null)
                {
                    // Re-increment capacity
                    await _capacityHelper.IncrementCapacityAsync(appointment.LocationId, scheduleTime.Id);
                }

                // Mark appointment as processed by updating status
                appointment.Status = AppointmentStatus.Cancelled;
                _dbContext.Appointments.Update(appointment);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}