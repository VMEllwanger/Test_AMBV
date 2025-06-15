using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBranchIdCustomerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Sales",
                newName: "Customer");

            migrationBuilder.RenameColumn(
                name: "BranchName",
                table: "Sales",
                newName: "Branch");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "SaleItems",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sales",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Sales",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "SaleItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SaleItems");

            migrationBuilder.RenameColumn(
                name: "Customer",
                table: "Sales",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "Branch",
                table: "Sales",
                newName: "BranchName");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "SaleItems",
                newName: "Total");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Sales",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Sales",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
