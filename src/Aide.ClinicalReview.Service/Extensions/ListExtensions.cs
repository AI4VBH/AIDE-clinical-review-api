namespace Aide.ClinicalReview.Service.Extensions
{
    public static class List
    {
        public static List<T> Empty<T>()
        {
            return new List<T>();
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || !list.Any();
        }
    }
}