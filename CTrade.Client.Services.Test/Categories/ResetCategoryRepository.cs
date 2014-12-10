using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.Categories
{
    [TestClass]
    public class ResetCategoryRepository
    {
        [TestMethod]
        public void Reset()
        {
            new CategoriesFixture().Init();
        }
    }
}
