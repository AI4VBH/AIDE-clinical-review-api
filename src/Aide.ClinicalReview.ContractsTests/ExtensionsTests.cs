using Aide.ClinicalReview.Contracts.Extensions;
using System.Collections.Generic;

namespace Aide.ClinicalReview.ContractsTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void EmptyList_IsEmpty()
        {
            var list = List.Empty<string>();

            Assert.Empty(list);
        }
    }
}