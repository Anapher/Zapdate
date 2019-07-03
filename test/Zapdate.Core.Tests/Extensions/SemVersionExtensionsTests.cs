﻿using System.Linq;
using Xunit;
using Zapdate.Core.Domain;
using Zapdate.Core.Extensions;

namespace Zapdate.Core.Tests.Extensions
{
    public class SemVersionExtensionsTests
    {
        [Fact]
        public void TestToBinary()
        {
            var versions = new [] { new SemVersion(1, 10), new SemVersion(5, 0), new SemVersion(1, 2) };
            var result = versions.OrderBy(x => x.ToBinaryVersion());
            Assert.Equal(new[] { new SemVersion(1, 2), new SemVersion(1, 10), new SemVersion(5, 0) }, result);
        }
    }
}
