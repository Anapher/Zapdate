using Xunit;
using Zapdate.Server.Core.Errors;
using Zapdate.Server.Core.Interfaces;

namespace Zapdate.Server.Core.Tests
{
    public static class ErrorUtils
    {
        public static void AssertError(IBusinessErrors errors, ErrorType? errorType = null, ErrorCode? code = null)
        {
            Assert.True(errors.HasError);

            if (errorType != null)
                Assert.Equal(errorType.ToString(), errors.Error.Type);

            if (code != null)
                Assert.Equal((int) code, errors.Error.Code);
        }
    }
}
