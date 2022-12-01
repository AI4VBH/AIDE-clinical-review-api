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

namespace Aide.ClinicalReview.ContractsTests
{
    public sealed class ClinicalReviewStudyTests
    {
        [Fact]
        public void ClinicalReviewRecord_IsOfType()
        {
            var record = new ClinicalReviewStudy();

            Assert.IsType<ClinicalReviewStudy>(record);
        }

        [Fact]
        public void Series_IsOfType()
        {
            var record = new Series();

            Assert.IsType<Series>(record);
        }
    }
}