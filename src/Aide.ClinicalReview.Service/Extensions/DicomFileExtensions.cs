using FellowOakDicom;

namespace Aide.ClinicalReview.Service.Extensions
{
    public static class DicomFileExtensions
    {
        public static T? GetValueOrDefualt<T>(this DicomFile file, DicomTag tag)
        {
            var receivedValue = file.Dataset.TryGetSingleValue<T>(tag, out T value);
            if (receivedValue)
            {
                return value;
            }

            return default(T);
        }
    }
}
