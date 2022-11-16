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