using Xunit;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces;

namespace Zapdate.Core.Tests
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
