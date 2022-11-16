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
