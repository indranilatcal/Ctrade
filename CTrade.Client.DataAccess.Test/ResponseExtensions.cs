using CTrade.Client.DataAccess.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.DataAccess.Test
{
    static class ResponseExtensions
    {
        internal static void HasError(this IDbResponse response, string expectedError = null)
        {
            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            if(!string.IsNullOrWhiteSpace(expectedError))
                Assert.AreEqual(expectedError, response.HttpHeaderInfo.Error);
        }

        internal static void HasNoError(this IDbResponse response)
        {
            Assert.IsFalse(response.HttpHeaderInfo.HasError);
        }
    }
}
