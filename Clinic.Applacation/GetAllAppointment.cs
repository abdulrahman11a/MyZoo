namespace Clinic.Applacation
{
    public class GetAllAppointment : BaseSpecifications<Appointment, int>
    {
        public GetAllAppointment()
            : base(a =>
                a.AppointmentDate > DateTime.Now &&
                a.AppointmentDate <= DateTime.Now.AddHours(24))
        {
            AddInclude(a => a.Patient);
            AddInclude(a => a.Location);
            AddInclude(a => a.Vet);


        }
    }

}
