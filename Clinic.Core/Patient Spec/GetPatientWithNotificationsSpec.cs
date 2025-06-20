namespace Clinic.Core.Patient_Spec
{
    public class GetPatientWithNotificationsSpec : BaseSpecifications<Patient, int>
    {
        public GetPatientWithNotificationsSpec(int id, int days) : base(p => p.Id == id)
        {
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            AddInclude(p => p.Notifications
                .Where(n => n.CreatedAT >= now.AddDays(-days))
                .OrderByDescending(n => n.CreatedAT));
            AddInclude(p => p.Notifications);
            AddNavigationInclude("Notifications.Vet");
        }

        public GetPatientWithNotificationsSpec() : base(null)
        {
            AddInclude(p => p.Notifications);
            AddNavigationInclude("Notifications.Vet");
        }


        public GetPatientWithNotificationsSpec(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.Notifications);
            AddNavigationInclude("Notifications.Vet");
        }
    }
}