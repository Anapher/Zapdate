using System;
using Xunit;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Core.Tests.Domain.Entities
{
    public class UpdatePackageDistributionTests
    {
        [Fact]
        public void TestPublishNow()
        {
            var dist = new UpdatePackageDistribution("");
            dist.Publish(null);

            Assert.True(dist.IsDistributing);
            Assert.True(dist.IsPublished);
        }

        [Fact]
        public void TestPublishAtSpecificDateInFuture()
        {
            var date = DateTimeOffset.UtcNow.AddDays(1);

            var dist = new UpdatePackageDistribution("");
            dist.Publish(date);

            Assert.False(dist.IsDistributing);
            Assert.True(dist.IsPublished);

            Assert.Equal(date, dist.PublishDate);
        }

        [Fact]
        public void TestPublishAtSpecificDateInPast()
        {
            var date = DateTimeOffset.UtcNow.AddDays(-1);

            var dist = new UpdatePackageDistribution("");
            dist.Publish(date);

            Assert.True(dist.IsDistributing);
            Assert.True(dist.IsPublished);

            Assert.Equal(date, dist.PublishDate);
        }

        [Fact]
        public void TestRollback()
        {
            var dist = new UpdatePackageDistribution("");
            dist.Publish();

            Assert.True(dist.IsDistributing);
            Assert.True(dist.IsPublished);

            dist.Rollback();

            Assert.True(dist.IsRolledBack);
            Assert.False(dist.IsDistributing);
        }

        [Fact]
        public void TestUnpublish()
        {
            var dist = new UpdatePackageDistribution("");
            dist.Publish();

            Assert.True(dist.IsDistributing);
            Assert.True(dist.IsPublished);

            dist.Unpublish();

            Assert.False(dist.IsDistributing);
            Assert.False(dist.IsPublished);
        }

        [Fact]
        public void TestRollbackAndPublishAgain()
        {
            var dist = new UpdatePackageDistribution("");
            dist.Publish();

            Assert.True(dist.IsDistributing);
            Assert.True(dist.IsPublished);

            dist.Rollback();

            Assert.True(dist.IsRolledBack);
            Assert.False(dist.IsDistributing);

            dist.Publish();

            Assert.True(dist.IsDistributing);
            Assert.True(dist.IsPublished);
        }
    }
}
