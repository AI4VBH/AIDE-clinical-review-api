namespace Aide.ClinicalReview.Contracts.Extensions
{
    public static class List
    {
        public static List<T> Empty<T>()
        {
            return new List<T>();
        }
    }
}