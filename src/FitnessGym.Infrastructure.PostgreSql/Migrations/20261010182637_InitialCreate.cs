using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessGym.Infrastructure.PostgreSql.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "persons",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                PassportNumber = table.Column<string>(type: "text", nullable: false),
                FullName = table.Column<string>(type: "text", nullable: false),
                Gender = table.Column<int>(type: "integer", nullable: false),
                BirthDate = table.Column<DateOnly>(type: "date", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_persons", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "specializations",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_specializations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                Phone = table.Column<string>(type: "text", nullable: false),
                SubscriptionStart = table.Column<DateOnly>(type: "date", nullable: false),
                SubscriptionEnd = table.Column<DateOnly>(type: "date", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_clients", x => x.Id);
                table.ForeignKey(
                    name: "FK_clients_persons_Id",
                    column: x => x.Id,
                    principalTable: "persons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "trainers",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                SpecializationId = table.Column<int>(type: "integer", nullable: false),
                ExperienceYears = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_trainers", x => x.Id);
                table.ForeignKey(
                    name: "FK_trainers_persons_Id",
                    column: x => x.Id,
                    principalTable: "persons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_trainers_specializations_SpecializationId",
                    column: x => x.SpecializationId,
                    principalTable: "specializations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "sessions",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                ClientId = table.Column<int>(type: "integer", nullable: false),
                TrainerId = table.Column<int>(type: "integer", nullable: false),
                StartsAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                EndsAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                HallName = table.Column<string>(type: "text", nullable: false),
                IsTrial = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_sessions", x => x.Id);
                table.ForeignKey(
                    name: "FK_sessions_clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_sessions_trainers_TrainerId",
                    column: x => x.TrainerId,
                    principalTable: "trainers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.InsertData(
            table: "specializations",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { 0, "Йога" },
                { 1, "Пилатес" },
                { 2, "Кроссфит" },
                { 3, "Плавание" },
                { 4, "Бокс" },
                { 5, "Тяжёлая атлетика" },
                { 6, "Стретчинг" },
                { 7, "Функциональный тренинг" },
                { 8, "Лёгкая атлетика" },
                { 9, "Реабилитация" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_sessions_ClientId",
            table: "sessions",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_sessions_TrainerId",
            table: "sessions",
            column: "TrainerId");

        migrationBuilder.CreateIndex(
            name: "IX_trainers_SpecializationId",
            table: "trainers",
            column: "SpecializationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "sessions");

        migrationBuilder.DropTable(
            name: "clients");

        migrationBuilder.DropTable(
            name: "trainers");

        migrationBuilder.DropTable(
            name: "persons");

        migrationBuilder.DropTable(
            name: "specializations");
    }
}
