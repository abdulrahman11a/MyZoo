namespace Clinic.APIS.Helper
{
    public class CapacityHelper
    {
        private readonly StoreDbContext _dbContext;

        public CapacityHelper(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DecrementCapacityAsync(int locationId, int scheduleTimeId)
        {
            var location = await _dbContext.Locations.FindAsync(locationId);
            var scheduleTime = await _dbContext.ScheduleTimes.FindAsync(scheduleTimeId);

            if (location != null && location.Capacity > 0)
            {
                location.Capacity -= 1;
                _dbContext.Locations.Update(location);
            }

            if (scheduleTime != null && scheduleTime.Capacity > 0)
            {
                scheduleTime.Capacity -= 1;
                _dbContext.ScheduleTimes.Update(scheduleTime);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task IncrementCapacityAsync(int locationId, int scheduleTimeId)
        {
            var location = await _dbContext.Locations.FindAsync(locationId);
            var scheduleTime = await _dbContext.ScheduleTimes.FindAsync(scheduleTimeId);

            if (location != null)
            {
                location.Capacity += 1;
                _dbContext.Locations.Update(location);
            }

            if (scheduleTime != null)
            {
                scheduleTime.Capacity += 1;
                _dbContext.ScheduleTimes.Update(scheduleTime);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}