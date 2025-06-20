public class VetWithAppointmentsAndPatientsSpecAndLocatio : BaseSpecifications<Vet, int>
{
    public VetWithAppointmentsAndPatientsSpecAndLocatio() : base(null)
    {
        AddInclude(v => v.Department);
        AddInclude(v => v.Appointments);
        AddNavigationInclude("Appointments.Patient");
        AddNavigationInclude("Appointments.Location");
    }

    public VetWithAppointmentsAndPatientsSpecAndLocatio(int id) : base(v => v.Id == id)
    {
        AddInclude(v => v.Department);
        AddInclude(v => v.Appointments);
        AddNavigationInclude("Appointments.Patient");
        AddNavigationInclude("Appointments.Location");
    }
}
