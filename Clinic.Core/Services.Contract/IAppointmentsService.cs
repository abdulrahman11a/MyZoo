namespace Clinic.Core.Services.Contract
{
    public interface IAppointmentsService
    {
        Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(int appointmentId);
        Task<List<Appointment>> GetAllAppointmentsForVetAsync(int vetId);
    }
}
