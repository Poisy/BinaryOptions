using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddedAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Balance", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Nationality", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", 0, 100000.0, "ea301405-2086-4b73-b6d8-87f18121d9e9", "admin@mail.com", false, false, null, "BG", "admin@mail.com", "ADMIN", "AQAAAAEAACcQAAAAEIsJToFbMhERLP+uVSR4dNLAdyp/YjPRTJgZPZ9BsF7NykQpzbHiPUNvYPmzoB6s5Q==", null, false, "c14b7d4c-2a9a-49c4-85d9-45d13d7354e2", false, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000000");
        }
    }
}
