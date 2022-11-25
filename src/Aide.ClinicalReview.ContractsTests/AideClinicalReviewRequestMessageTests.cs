// 
// Copyright 2022 Crown Copyright
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

using Aide.ClinicalReview.Contracts.Messages;
using File = Aide.ClinicalReview.Contracts.Messages.File;

namespace Aide.ClinicalReview.ContractsTests
{
    public sealed class AideClinicalReviewRequestMessageTests
    {
        [Fact]
        public void AideClinicalReviewRequestMessage_IsNotNull()
        {
            var message = new AideClinicalReviewRequestMessage();

            Assert.NotNull(message);
            Assert.IsType<AideClinicalReviewRequestMessage>(message);
        }

        [Fact]
        public void Credentials_IsNotNull()
        {
            var message = new Credentials();

            Assert.NotNull(message);
            Assert.IsType<Credentials>(message);
        }

        [Fact]
        public void File_IsNotNull()
        {
            var message = new File();

            Assert.NotNull(message);
            Assert.IsType<File>(message);
        }

        [Fact]
        public void PatientMetadata_IsNotNull()
        {
            var message = new PatientMetadata();

            Assert.NotNull(message);
            Assert.IsType<PatientMetadata>(message);
        }
    }
}