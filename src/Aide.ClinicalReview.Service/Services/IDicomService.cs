using Monai.Deploy.Storage.API;

namespace Aide.ClinicalReview.Service.Services
{
    public interface IDicomService
    {
        Task<Stream?> GetDicomFileAsync(string key, string? bucket = null);

        Task<IList<VirtualFileInfo>> GetAllDicomFileInfoInPath(string path, string? bucket = null);
    }
}