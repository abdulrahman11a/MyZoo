namespace Clinic.Core.Vet_Spec
{
    public class VetWithAppointmentsAndScheduleTimesAndPrescriptionNotificationSpecification:BaseSpecifications<Vet,int>
    {
        public VetWithAppointmentsAndScheduleTimesAndPrescriptionNotificationSpecification():base(null)
        {
            AddInclude(v => v.Department);
        }

        public  VetWithAppointmentsAndScheduleTimesAndPrescriptionNotificationSpecification(int id) : base(v=>v.Id==id)
        {
            AddInclude(v => v.Department);
        }


    }





}
