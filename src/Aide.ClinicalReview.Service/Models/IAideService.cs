namespace Aide.ClinicalReview.Service.Models
{
    public interface IAideService
    {
        ServiceStatus Status { get; set; }
        static string? ServiceName { get; }
    }
}