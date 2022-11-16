using Aide.ClinicalReview.Contracts.Extensions;
using Aide.ClinicalReview.Contracts.Messages;
using Aide.ClinicalReview.Contracts.Models;

namespace Aide.ClinicalReview.ContractsTests
{
    public sealed class ClinicalReviewRecordTests
    {
        [Fact]
        public void ClinicalReviewRecord_IsOfTypeWithMessage()
        {
            var record = new ClinicalReviewRecord();
            record.ClinicalReviewMessage = new AideClinicalReviewRequestMessage();

            Assert.IsType<ClinicalReviewRecord>(record);
            Assert.IsType<AideClinicalReviewRequestMessage>(record.ClinicalReviewMessage);
        }
    }
}