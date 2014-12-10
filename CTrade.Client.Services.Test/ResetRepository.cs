using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test
{
    [TestClass]
    public class ResetRepository
    {
        [TestMethod]
        public void Reset()
        {
            TestRuntime.ResetRepository(Constants.MasterConnectionKey);
            TestRuntime.CreateIndex(Constants.MasterConnectionKey, DesignDocumentBuilder.CreateIndexContent());
        }

        [TestMethod]
        public void CreateIndices()
        {
            TestRuntime.CreateIndex(Constants.MasterConnectionKey, DesignDocumentBuilder.CreateIndexContent());
        }

        [TestMethod]
        public void Clear()
        {
            TestRuntime.ResetRepository(Constants.MasterConnectionKey);
        }
    }
}
