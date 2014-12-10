using CTrade.Client.Services.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test
{
    static class ResponseExtensions
    {
        internal static void HasError(this IServiceResponse response, string expectedError = null)
        {
            Assert.IsTrue(response.HasError);
            if(!string.IsNullOrWhiteSpace(expectedError))
                Assert.AreEqual(expectedError, response.Error);
        }

        internal static void HasNoError(this IServiceResponse response)
        {
            Assert.IsFalse(response.HasError);
        }
    }
}
