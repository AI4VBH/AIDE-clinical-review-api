using MongoDB.Driver;
using System.Linq.Expressions;

namespace Aide.ClinicalReview.Database.Repository
{
    public abstract class RepositoryBase
    {
        public static async Task<long> CountAsync<T>(IMongoCollection<T> collection, FilterDefinition<T> filter)
            => await collection.CountDocumentsAsync(filter ?? Builders<T>.Filter.Empty);

        /// <summary>
        /// Get All T that match filters provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Collection to run against.</param>
        /// <param name="filterFunction">Filter function you can filter on properties of T.</param>
        /// <param name="sortFunction">Function used to sort data.</param>
        /// <param name="skip">Items to skip.</param>
        /// <param name="limit">Items to limit results by.</param>
        /// <returns></returns>
        public static async Task<IList<T>> GetAllAsync<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> filterFunction, SortDefinition<T> sortFunction, int? skip = null, int? limit = null)
        {
            return await collection
                .Find(filterFunction ?? Builders<T>.Filter.Empty)
                .Skip(skip)
                .Limit(limit)
                .Sort(sortFunction)
                .ToListAsync();
        }

        public static async Task<IList<T>> GetAllAsync<T>(IMongoCollection<T> collection, FilterDefinition<T> filterFunction, SortDefinition<T> sortFunction, int? skip = null, int? limit = null)
        {
            return await collection
                .Find(filterFunction)
                .Skip(skip)
                .Limit(limit)
                .Sort(sortFunction)
                .ToListAsync();
        }
    }
}
