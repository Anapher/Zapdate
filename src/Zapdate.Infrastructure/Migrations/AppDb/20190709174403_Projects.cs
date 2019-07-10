using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zapdate.Infrastructure.Migrations.AppDb
{
    public partial class Projects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AsymmetricKey_PublicKey = table.Column<string>(nullable: true),
                    AsymmetricKey_PrivateKey = table.Column<string>(nullable: true),
                    AsymmetricKey_IsPrivateKeyEncrypted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                columns: table => new
                {
                    FileHash = table.Column<string>(nullable: false),
                    Length = table.Column<long>(nullable: false),
                    CompressedLength = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.FileHash);
                });

            migrationBuilder.CreateTable(
                name: "UpdatePackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CustomFields = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    VersionPrerelease = table.Column<string>(nullable: true),
                    VersionBuild = table.Column<string>(nullable: true),
                    VersionBinary = table.Column<long>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdatePackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdateChangelog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatePackageId = table.Column<int>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateChangelog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdateChangelog_UpdatePackages_UpdatePackageId",
                        column: x => x.UpdatePackageId,
                        principalTable: "UpdatePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UpdateFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatePackageId = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    Signature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdateFile_UpdatePackages_UpdatePackageId",
                        column: x => x.UpdatePackageId,
                        principalTable: "UpdatePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UpdatePackageDistribution",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatePackageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTimeOffset>(nullable: true),
                    IsRolledBack = table.Column<bool>(nullable: false),
                    IsEnforced = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdatePackageDistribution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdatePackageDistribution_UpdatePackages_UpdatePackageId",
                        column: x => x.UpdatePackageId,
                        principalTable: "UpdatePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpdateChangelog_UpdatePackageId_Language",
                table: "UpdateChangelog",
                columns: new[] { "UpdatePackageId", "Language" },
                unique: true,
                filter: "[Language] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UpdateFile_UpdatePackageId",
                table: "UpdateFile",
                column: "UpdatePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UpdatePackageDistribution_UpdatePackageId_Name",
                table: "UpdatePackageDistribution",
                columns: new[] { "UpdatePackageId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UpdatePackages_OrderNumber",
                table: "UpdatePackages",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_UpdatePackages_Version",
                table: "UpdatePackages",
                column: "Version",
                unique: true,
                filter: "[Version] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "StoredFiles");

            migrationBuilder.DropTable(
                name: "UpdateChangelog");

            migrationBuilder.DropTable(
                name: "UpdateFile");

            migrationBuilder.DropTable(
                name: "UpdatePackageDistribution");

            migrationBuilder.DropTable(
                name: "UpdatePackages");
        }
    }
}
