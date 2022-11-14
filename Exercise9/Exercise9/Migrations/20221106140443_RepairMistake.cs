using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exercise9.Migrations
{
    public partial class RepairMistake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPatient",
                table: "Prescription_Medicaments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPatient",
                table: "Prescription_Medicaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
