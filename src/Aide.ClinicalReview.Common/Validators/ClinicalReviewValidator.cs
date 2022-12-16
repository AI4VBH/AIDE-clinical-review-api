// 
// Copyright 2022 Guy’s and St Thomas’ NHS Foundation Trust
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Aide.ClinicalReview.Contracts.Models;

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

            if(string.IsNullOrWhiteSpace(acknowledge.UserId))
            {
                errors.Add("userId is a required field.");
            }

            if (acknowledge.Roles is null || acknowledge.Roles.Length < 1)
            {
                errors.Add("Roles are required.");
            }

            return errors;
        }
    }
}
