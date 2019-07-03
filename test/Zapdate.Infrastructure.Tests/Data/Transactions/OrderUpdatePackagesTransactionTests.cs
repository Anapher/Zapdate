using MockQueryable.Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;
using Zapdate.Infrastructure.Data.Transactions;

namespace Zapdate.Infrastructure.Tests.Data.Transactions
{
    public class OrderUpdatePackagesTransactionTests
    {
        [Fact]
        public async Task HasNoPackages_NewPackage()
        {
            var packages = new[] { CreateUpdatePackage("1.0.0") };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.0");

            Assert.Equal(0, packages[0].OrderNumber);
        }

        [Fact]
        public async Task HasOnePackage_ChangeToPreviousVersion()
        {
            var packages = new[] { CreateUpdatePackage("1.0.0", 0) };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.0", "1.2.0");

            Assert.Equal(0, packages[0].OrderNumber);
        }

        [Fact]
        public async Task HasOnePackage_ChangeToGreaterVersion()
        {
            var packages = new[] { CreateUpdatePackage("1.2.0", 0) };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.2.0", "1.0.0");

            Assert.Equal(0, packages[0].OrderNumber);
        }

        [Fact]
        public async Task HasOnePackage_NewGreaterPackage_MajorMinor()
        {
            var packages = new[] { CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("1.1.0") };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.1.0");

            Assert.Equal(0, packages[0].OrderNumber);
            Assert.Equal(1, packages[1].OrderNumber);
        }

        [Fact]
        public async Task HasOnePackage_NewGreaterPackage_Build()
        {
            var packages = new[] { CreateUpdatePackage("1.0.1"), CreateUpdatePackage("1.0.0", 0) };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.1");

            Assert.Equal(0, packages[1].OrderNumber);
            Assert.Equal(1, packages[0].OrderNumber);
        }

        [Fact]
        public async Task HasOnePackage_NewOlderPackage_Build()
        {
            var packages = new[] { CreateUpdatePackage("1.0.1", 0), CreateUpdatePackage("1.0.0") };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.0");

            Assert.Equal(1, packages[0].OrderNumber);
            Assert.Equal(0, packages[1].OrderNumber);
        }

        [Fact]
        public async Task HasOnePackage_NewOlderPackage_MajorMinor()
        {
            var packages = new[] { CreateUpdatePackage("1.2.0", 0), CreateUpdatePackage("1.0.0") };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.0");

            Assert.Equal(1, packages[0].OrderNumber);
            Assert.Equal(0, packages[1].OrderNumber);
        }

        [Fact]
        public async Task HasManyPackages_NewPackageInBetween_MajorMinor()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.1", 3),
                CreateUpdatePackage("1.1.0", 1), CreateUpdatePackage("2.0.0", 2),
                CreateUpdatePackage("1.2.0")
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.2.0");

            AssertCorrectOrderNumbers(packages);
        }

        [Fact]
        public async Task HasManyPackages_NewPackageInBetween_Build()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.1", 3),
                CreateUpdatePackage("1.1.0", 1), CreateUpdatePackage("2.0.0", 2),
                CreateUpdatePackage("1.0.1")
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.1");

            AssertCorrectOrderNumbers(packages);
        }

        [Fact]
        public async Task HasManyPackages_NewPackageOnTop_Build()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.1", 3),
                CreateUpdatePackage("1.1.0", 1), CreateUpdatePackage("2.0.0", 2),
                CreateUpdatePackage("2.0.2")
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "2.0.2");

            AssertCorrectOrderNumbers(packages);
        }

        [Fact]
        public async Task HasManyPackages_NewPackageOnTop_MajorMinor()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.1", 3),
                CreateUpdatePackage("1.1.0", 1), CreateUpdatePackage("2.0.0", 2),
                CreateUpdatePackage("2.2.0")
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "2.2.0");

            AssertCorrectOrderNumbers(packages);
        }

        [Fact]
        public async Task HasManyPackages_ChangeVersionToPrevious()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.1", 4),
                CreateUpdatePackage("1.1.0", 1), CreateUpdatePackage("2.0.0", 3),
                CreateUpdatePackage("1.0.1", 2)
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.0", "1.5.0");

            AssertCorrectOrderNumbers(packages);
        }

        [Fact]
        public async Task HasManyPackages_ChangeVersionToNewer()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.2", 4),
                CreateUpdatePackage("1.1.0", 2), CreateUpdatePackage("2.0.0", 3),
                CreateUpdatePackage("2.0.1", 1)
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "2.0.1", "1.0.1");

            AssertCorrectOrderNumbers(packages);
        }

        [Fact]
        public async Task HasManyPackages_ChangeVersionFromMiddleToTop()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.2", 4),
                CreateUpdatePackage("1.1.0", 2), CreateUpdatePackage("2.0.0", 3),
                CreateUpdatePackage("3.0.0", 1)
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "3.0.0", "1.0.1");

            AssertCorrectOrderNumbers(packages);
        }

        [Fact]
        public async Task HasManyPackages_ChangeVersionFromTopToMiddle()
        {
            var packages = new[] {
                CreateUpdatePackage("1.0.0", 0), CreateUpdatePackage("2.0.2", 3),
                CreateUpdatePackage("1.1.0", 1), CreateUpdatePackage("2.0.0", 2),
                CreateUpdatePackage("1.0.1", 4)
            };
            var mock = packages.AsQueryable().BuildMock();

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(mock.Object, "1.0.1", "3.0.0");

            AssertCorrectOrderNumbers(packages);
        }

        private static void AssertCorrectOrderNumbers(IReadOnlyList<UpdatePackage> packages)
        {
            Assert.Equal(packages.OrderBy(x => x.VersionInfo.SemVersion).Select((x, i) => (i, x.VersionInfo.SemVersion)),
                packages.OrderBy(x => x.OrderNumber).Select(x => (x.OrderNumber, x.VersionInfo.SemVersion)));   
        }

        private static UpdatePackage CreateUpdatePackage(SemVersion version, int? orderNumber = null)
        {
            var package = new UpdatePackage(version);
            if (orderNumber != null)
                package.OrderNumber = orderNumber.Value;

            return package;
        }
    }
}
