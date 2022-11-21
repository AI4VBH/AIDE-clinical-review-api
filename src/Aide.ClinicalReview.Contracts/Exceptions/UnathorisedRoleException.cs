using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Contracts.Exceptions
{
    public class UnathorisedRoleException : Exception
    {
        public UnathorisedRoleException()
        {
        }

        public UnathorisedRoleException(string message)
            : base(message)
        {
        }

        public UnathorisedRoleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}