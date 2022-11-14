using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exercise9.Migrations
{
    public partial class SeedDataPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "IdDoctor", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "jan@xxx", "Jan", "Kowalski" },
                    { 2, "anna@xxx", "Anna", "Nowak" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "IdPatient", "Birthdate", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zdzisław", "Fajny" },
                    { 2, new DateTime(1985, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Halina", "XYZ" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "IdDoctor",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "IdDoctor",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "IdPatient",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "IdPatient",
                keyValue: 2);
        }
    }
}
