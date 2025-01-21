using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BusApiProyect.Data.Migrations
{
    /// <inheritdoc />
    public partial class models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BusNumber = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bus", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Origin = table.Column<string>(type: "longtext", nullable: false),
                    Origin_Latitude = table.Column<double>(type: "double", nullable: false),
                    Origin_Longitude = table.Column<double>(type: "double", nullable: false),
                    Destination = table.Column<string>(type: "longtext", nullable: false),
                    Destination_Latitude = table.Column<double>(type: "double", nullable: false),
                    Destination_Longitude = table.Column<double>(type: "double", nullable: false),
                    Distance = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BusSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BusForScheduleId = table.Column<int>(type: "int", nullable: false),
                    RouteScheduledId = table.Column<int>(type: "int", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Arrival_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusSchedule_Bus_BusForScheduleId",
                        column: x => x.BusForScheduleId,
                        principalTable: "Bus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusSchedule_Route_RouteScheduledId",
                        column: x => x.RouteScheduledId,
                        principalTable: "Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserForBookingId = table.Column<int>(type: "int", nullable: false),
                    ScheduleForBookingId = table.Column<int>(type: "int", nullable: false),
                    SeatsBooked = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_BusSchedule_ScheduleForBookingId",
                        column: x => x.ScheduleForBookingId,
                        principalTable: "BusSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_User_UserForBookingId",
                        column: x => x.UserForBookingId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ScheduleForBookingId",
                table: "Booking",
                column: "ScheduleForBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserForBookingId",
                table: "Booking",
                column: "UserForBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedule_BusForScheduleId",
                table: "BusSchedule",
                column: "BusForScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedule_Id_DepartureTime_Arrival_Time",
                table: "BusSchedule",
                columns: new[] { "Id", "DepartureTime", "Arrival_Time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedule_RouteScheduledId",
                table: "BusSchedule",
                column: "RouteScheduledId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "BusSchedule");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Bus");

            migrationBuilder.DropTable(
                name: "Route");
        }
    }
}
