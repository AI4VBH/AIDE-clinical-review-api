namespace Aide.ClinicalReview.Service.Services
{
    public interface IDicomService
    {
        Task<Stream?> GetDicomFileAsync(string key);
    }
}