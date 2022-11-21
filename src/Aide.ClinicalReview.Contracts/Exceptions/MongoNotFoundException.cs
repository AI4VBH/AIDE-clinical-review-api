using System.Globalization;
using System.Runtime.Serialization;

namespace Aide.ClinicalReview.Contracts.Exceptions
{
    [Serializable]
    public class MongoNotFoundException : Exception
    {
        private static readonly string MessageFormat = "Object cannot be found in Mongo '{0}'.";

        public MongoNotFoundException(string serviceName)
            : base(string.Format(CultureInfo.InvariantCulture, MessageFormat, serviceName))
        {
        }

        public MongoNotFoundException(string serviceName, Exception innerException)
            : base(string.Format(CultureInfo.InvariantCulture, MessageFormat, serviceName), innerException)
        {
        }

        private MongoNotFoundException()
        {
        }

        protected MongoNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}