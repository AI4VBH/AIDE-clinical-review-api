using Aide.ClinicalReview.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aide.ClinicalReview.Common.Validators
{
    public static class ClinicalReviewValidator
    {
        public static IList<string> ValidateAcknowledgeClinicalReview(AcknowledgeClinicalReview acknowledge)
        {
            var errors = new List<string>();

            if(acknowledge is null)
            {
                errors.Add("AcknowledgeClinicalReview body is required.");

                return errors;
            }

            if(acknowledge.userId is null)
            {
                errors.Add("userId is a required field.");
            }

            if (acknowledge.Roles is null || acknowledge.Roles.Length < 1)
            {
                errors.Add("Roles are required.");
            }

            if (acknowledge.Acceptance is false && acknowledge.Reason is null)
            {
                errors.Add("Reason cannot be null when a clinical review is rejected.");
            }

            return errors;
        }
    }
}
