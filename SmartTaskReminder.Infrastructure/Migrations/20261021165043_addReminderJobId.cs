using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartTaskReminder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReminderJobId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReminderJobId",
                table: "TaskItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderJobId",
                table: "TaskItems");
        }
    }
}
