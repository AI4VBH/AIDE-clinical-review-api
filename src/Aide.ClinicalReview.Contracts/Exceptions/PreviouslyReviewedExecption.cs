using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Contracts.Exceptions
{
    public class PreviouslyReviewedException : Exception
    {
        public PreviouslyReviewedException()
        {
        }

        public PreviouslyReviewedException(string message)
            : base(message)
        {
        }

        public PreviouslyReviewedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
