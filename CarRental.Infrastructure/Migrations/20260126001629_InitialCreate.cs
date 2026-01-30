using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberPlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DailyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "CarName", "CarType", "DailyPrice", "Description", "IsActive", "ModelYear", "NumberPlate" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000000"), "Toyota Corolla", "Sedan", 30m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001000" },
                    { new Guid("20000000-0000-0000-0000-000000000001"), "Honda Civic", "SUV", 31m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001001" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "Ford Focus", "Van", 32m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001002" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "BMW 3 Series", "Sedan", 33m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001003" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "Audi A4", "SUV", 34m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001004" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "Chevrolet Malibu", "Van", 35m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001005" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "Nissan Altima", "Sedan", 36m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001006" },
                    { new Guid("20000000-0000-0000-0000-000000000007"), "Hyundai Elantra", "SUV", 37m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001007" },
                    { new Guid("20000000-0000-0000-0000-000000000008"), "Kia Rio", "Van", 38m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001008" },
                    { new Guid("20000000-0000-0000-0000-000000000009"), "Mercedes C Class", "Sedan", 39m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001009" },
                    { new Guid("20000000-0000-0000-0000-000000000010"), "Toyota Corolla", "SUV", 40m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001010" },
                    { new Guid("20000000-0000-0000-0000-000000000011"), "Honda Civic", "Van", 41m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001011" },
                    { new Guid("20000000-0000-0000-0000-000000000012"), "Ford Focus", "Sedan", 42m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001012" },
                    { new Guid("20000000-0000-0000-0000-000000000013"), "BMW 3 Series", "SUV", 43m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001013" },
                    { new Guid("20000000-0000-0000-0000-000000000014"), "Audi A4", "Van", 44m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001014" },
                    { new Guid("20000000-0000-0000-0000-000000000015"), "Chevrolet Malibu", "Sedan", 45m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001015" },
                    { new Guid("20000000-0000-0000-0000-000000000016"), "Nissan Altima", "SUV", 46m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001016" },
                    { new Guid("20000000-0000-0000-0000-000000000017"), "Hyundai Elantra", "Van", 47m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001017" },
                    { new Guid("20000000-0000-0000-0000-000000000018"), "Kia Rio", "Sedan", 48m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001018" },
                    { new Guid("20000000-0000-0000-0000-000000000019"), "Mercedes C Class", "SUV", 49m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001019" },
                    { new Guid("20000000-0000-0000-0000-000000000020"), "Toyota Corolla", "Van", 50m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001020" },
                    { new Guid("20000000-0000-0000-0000-000000000021"), "Honda Civic", "Sedan", 51m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001021" },
                    { new Guid("20000000-0000-0000-0000-000000000022"), "Ford Focus", "SUV", 52m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001022" },
                    { new Guid("20000000-0000-0000-0000-000000000023"), "BMW 3 Series", "Van", 53m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001023" },
                    { new Guid("20000000-0000-0000-0000-000000000024"), "Audi A4", "Sedan", 54m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001024" },
                    { new Guid("20000000-0000-0000-0000-000000000025"), "Chevrolet Malibu", "SUV", 55m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001025" },
                    { new Guid("20000000-0000-0000-0000-000000000026"), "Nissan Altima", "Van", 56m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001026" },
                    { new Guid("20000000-0000-0000-0000-000000000027"), "Hyundai Elantra", "Sedan", 57m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001027" },
                    { new Guid("20000000-0000-0000-0000-000000000028"), "Kia Rio", "SUV", 58m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001028" },
                    { new Guid("20000000-0000-0000-0000-000000000029"), "Mercedes C Class", "Van", 59m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001029" },
                    { new Guid("20000000-0000-0000-0000-000000000030"), "Toyota Corolla", "Sedan", 60m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001030" },
                    { new Guid("20000000-0000-0000-0000-000000000031"), "Honda Civic", "SUV", 61m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001031" },
                    { new Guid("20000000-0000-0000-0000-000000000032"), "Ford Focus", "Van", 62m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001032" },
                    { new Guid("20000000-0000-0000-0000-000000000033"), "BMW 3 Series", "Sedan", 63m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001033" },
                    { new Guid("20000000-0000-0000-0000-000000000034"), "Audi A4", "SUV", 64m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001034" },
                    { new Guid("20000000-0000-0000-0000-000000000035"), "Chevrolet Malibu", "Van", 65m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001035" },
                    { new Guid("20000000-0000-0000-0000-000000000036"), "Nissan Altima", "Sedan", 66m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001036" },
                    { new Guid("20000000-0000-0000-0000-000000000037"), "Hyundai Elantra", "SUV", 67m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001037" },
                    { new Guid("20000000-0000-0000-0000-000000000038"), "Kia Rio", "Van", 68m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001038" },
                    { new Guid("20000000-0000-0000-0000-000000000039"), "Mercedes C Class", "Sedan", 69m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001039" },
                    { new Guid("20000000-0000-0000-0000-000000000040"), "Toyota Corolla", "SUV", 70m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001040" },
                    { new Guid("20000000-0000-0000-0000-000000000041"), "Honda Civic", "Van", 71m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001041" },
                    { new Guid("20000000-0000-0000-0000-000000000042"), "Ford Focus", "Sedan", 72m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001042" },
                    { new Guid("20000000-0000-0000-0000-000000000043"), "BMW 3 Series", "SUV", 73m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001043" },
                    { new Guid("20000000-0000-0000-0000-000000000044"), "Audi A4", "Van", 74m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001044" },
                    { new Guid("20000000-0000-0000-0000-000000000045"), "Chevrolet Malibu", "Sedan", 75m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001045" },
                    { new Guid("20000000-0000-0000-0000-000000000046"), "Nissan Altima", "SUV", 76m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001046" },
                    { new Guid("20000000-0000-0000-0000-000000000047"), "Hyundai Elantra", "Van", 77m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001047" },
                    { new Guid("20000000-0000-0000-0000-000000000048"), "Kia Rio", "Sedan", 78m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001048" },
                    { new Guid("20000000-0000-0000-0000-000000000049"), "Mercedes C Class", "SUV", 79m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001049" },
                    { new Guid("20000000-0000-0000-0000-000000000050"), "Toyota Corolla", "Van", 80m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001050" },
                    { new Guid("20000000-0000-0000-0000-000000000051"), "Honda Civic", "Sedan", 81m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001051" },
                    { new Guid("20000000-0000-0000-0000-000000000052"), "Ford Focus", "SUV", 82m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001052" },
                    { new Guid("20000000-0000-0000-0000-000000000053"), "BMW 3 Series", "Van", 83m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001053" },
                    { new Guid("20000000-0000-0000-0000-000000000054"), "Audi A4", "Sedan", 84m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001054" },
                    { new Guid("20000000-0000-0000-0000-000000000055"), "Chevrolet Malibu", "SUV", 85m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001055" },
                    { new Guid("20000000-0000-0000-0000-000000000056"), "Nissan Altima", "Van", 86m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001056" },
                    { new Guid("20000000-0000-0000-0000-000000000057"), "Hyundai Elantra", "Sedan", 87m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001057" },
                    { new Guid("20000000-0000-0000-0000-000000000058"), "Kia Rio", "SUV", 88m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001058" },
                    { new Guid("20000000-0000-0000-0000-000000000059"), "Mercedes C Class", "Van", 89m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001059" },
                    { new Guid("20000000-0000-0000-0000-000000000060"), "Toyota Corolla", "Sedan", 90m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001060" },
                    { new Guid("20000000-0000-0000-0000-000000000061"), "Honda Civic", "SUV", 91m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001061" },
                    { new Guid("20000000-0000-0000-0000-000000000062"), "Ford Focus", "Van", 92m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001062" },
                    { new Guid("20000000-0000-0000-0000-000000000063"), "BMW 3 Series", "Sedan", 93m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001063" },
                    { new Guid("20000000-0000-0000-0000-000000000064"), "Audi A4", "SUV", 94m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001064" },
                    { new Guid("20000000-0000-0000-0000-000000000065"), "Chevrolet Malibu", "Van", 95m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001065" },
                    { new Guid("20000000-0000-0000-0000-000000000066"), "Nissan Altima", "Sedan", 96m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001066" },
                    { new Guid("20000000-0000-0000-0000-000000000067"), "Hyundai Elantra", "SUV", 97m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001067" },
                    { new Guid("20000000-0000-0000-0000-000000000068"), "Kia Rio", "Van", 98m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001068" },
                    { new Guid("20000000-0000-0000-0000-000000000069"), "Mercedes C Class", "Sedan", 99m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001069" },
                    { new Guid("20000000-0000-0000-0000-000000000070"), "Toyota Corolla", "SUV", 30m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001070" },
                    { new Guid("20000000-0000-0000-0000-000000000071"), "Honda Civic", "Van", 31m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001071" },
                    { new Guid("20000000-0000-0000-0000-000000000072"), "Ford Focus", "Sedan", 32m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001072" },
                    { new Guid("20000000-0000-0000-0000-000000000073"), "BMW 3 Series", "SUV", 33m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001073" },
                    { new Guid("20000000-0000-0000-0000-000000000074"), "Audi A4", "Van", 34m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001074" },
                    { new Guid("20000000-0000-0000-0000-000000000075"), "Chevrolet Malibu", "Sedan", 35m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001075" },
                    { new Guid("20000000-0000-0000-0000-000000000076"), "Nissan Altima", "SUV", 36m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001076" },
                    { new Guid("20000000-0000-0000-0000-000000000077"), "Hyundai Elantra", "Van", 37m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001077" },
                    { new Guid("20000000-0000-0000-0000-000000000078"), "Kia Rio", "Sedan", 38m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001078" },
                    { new Guid("20000000-0000-0000-0000-000000000079"), "Mercedes C Class", "SUV", 39m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001079" },
                    { new Guid("20000000-0000-0000-0000-000000000080"), "Toyota Corolla", "Van", 40m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001080" },
                    { new Guid("20000000-0000-0000-0000-000000000081"), "Honda Civic", "Sedan", 41m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001081" },
                    { new Guid("20000000-0000-0000-0000-000000000082"), "Ford Focus", "SUV", 42m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001082" },
                    { new Guid("20000000-0000-0000-0000-000000000083"), "BMW 3 Series", "Van", 43m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001083" },
                    { new Guid("20000000-0000-0000-0000-000000000084"), "Audi A4", "Sedan", 44m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001084" },
                    { new Guid("20000000-0000-0000-0000-000000000085"), "Chevrolet Malibu", "SUV", 45m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001085" },
                    { new Guid("20000000-0000-0000-0000-000000000086"), "Nissan Altima", "Van", 46m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001086" },
                    { new Guid("20000000-0000-0000-0000-000000000087"), "Hyundai Elantra", "Sedan", 47m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001087" },
                    { new Guid("20000000-0000-0000-0000-000000000088"), "Kia Rio", "SUV", 48m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001088" },
                    { new Guid("20000000-0000-0000-0000-000000000089"), "Mercedes C Class", "Van", 49m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001089" },
                    { new Guid("20000000-0000-0000-0000-000000000090"), "Toyota Corolla", "Sedan", 50m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001090" },
                    { new Guid("20000000-0000-0000-0000-000000000091"), "Honda Civic", "SUV", 51m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001091" },
                    { new Guid("20000000-0000-0000-0000-000000000092"), "Ford Focus", "Van", 52m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001092" },
                    { new Guid("20000000-0000-0000-0000-000000000093"), "BMW 3 Series", "Sedan", 53m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001093" },
                    { new Guid("20000000-0000-0000-0000-000000000094"), "Audi A4", "SUV", 54m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001094" },
                    { new Guid("20000000-0000-0000-0000-000000000095"), "Chevrolet Malibu", "Van", 55m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001095" },
                    { new Guid("20000000-0000-0000-0000-000000000096"), "Nissan Altima", "Sedan", 56m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001096" },
                    { new Guid("20000000-0000-0000-0000-000000000097"), "Hyundai Elantra", "SUV", 57m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001097" },
                    { new Guid("20000000-0000-0000-0000-000000000098"), "Kia Rio", "Van", 58m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001098" },
                    { new Guid("20000000-0000-0000-0000-000000000099"), "Mercedes C Class", "Sedan", 59m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001099" },
                    { new Guid("20000000-0000-0000-0000-000000000100"), "Toyota Corolla", "SUV", 60m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001100" },
                    { new Guid("20000000-0000-0000-0000-000000000101"), "Honda Civic", "Van", 61m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001101" },
                    { new Guid("20000000-0000-0000-0000-000000000102"), "Ford Focus", "Sedan", 62m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001102" },
                    { new Guid("20000000-0000-0000-0000-000000000103"), "BMW 3 Series", "SUV", 63m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001103" },
                    { new Guid("20000000-0000-0000-0000-000000000104"), "Audi A4", "Van", 64m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001104" },
                    { new Guid("20000000-0000-0000-0000-000000000105"), "Chevrolet Malibu", "Sedan", 65m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001105" },
                    { new Guid("20000000-0000-0000-0000-000000000106"), "Nissan Altima", "SUV", 66m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001106" },
                    { new Guid("20000000-0000-0000-0000-000000000107"), "Hyundai Elantra", "Van", 67m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001107" },
                    { new Guid("20000000-0000-0000-0000-000000000108"), "Kia Rio", "Sedan", 68m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001108" },
                    { new Guid("20000000-0000-0000-0000-000000000109"), "Mercedes C Class", "SUV", 69m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001109" },
                    { new Guid("20000000-0000-0000-0000-000000000110"), "Toyota Corolla", "Van", 70m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001110" },
                    { new Guid("20000000-0000-0000-0000-000000000111"), "Honda Civic", "Sedan", 71m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001111" },
                    { new Guid("20000000-0000-0000-0000-000000000112"), "Ford Focus", "SUV", 72m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001112" },
                    { new Guid("20000000-0000-0000-0000-000000000113"), "BMW 3 Series", "Van", 73m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001113" },
                    { new Guid("20000000-0000-0000-0000-000000000114"), "Audi A4", "Sedan", 74m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001114" },
                    { new Guid("20000000-0000-0000-0000-000000000115"), "Chevrolet Malibu", "SUV", 75m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001115" },
                    { new Guid("20000000-0000-0000-0000-000000000116"), "Nissan Altima", "Van", 76m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001116" },
                    { new Guid("20000000-0000-0000-0000-000000000117"), "Hyundai Elantra", "Sedan", 77m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001117" },
                    { new Guid("20000000-0000-0000-0000-000000000118"), "Kia Rio", "SUV", 78m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001118" },
                    { new Guid("20000000-0000-0000-0000-000000000119"), "Mercedes C Class", "Van", 79m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001119" },
                    { new Guid("20000000-0000-0000-0000-000000000120"), "Toyota Corolla", "Sedan", 80m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001120" },
                    { new Guid("20000000-0000-0000-0000-000000000121"), "Honda Civic", "SUV", 81m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001121" },
                    { new Guid("20000000-0000-0000-0000-000000000122"), "Ford Focus", "Van", 82m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001122" },
                    { new Guid("20000000-0000-0000-0000-000000000123"), "BMW 3 Series", "Sedan", 83m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001123" },
                    { new Guid("20000000-0000-0000-0000-000000000124"), "Audi A4", "SUV", 84m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001124" },
                    { new Guid("20000000-0000-0000-0000-000000000125"), "Chevrolet Malibu", "Van", 85m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001125" },
                    { new Guid("20000000-0000-0000-0000-000000000126"), "Nissan Altima", "Sedan", 86m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001126" },
                    { new Guid("20000000-0000-0000-0000-000000000127"), "Hyundai Elantra", "SUV", 87m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001127" },
                    { new Guid("20000000-0000-0000-0000-000000000128"), "Kia Rio", "Van", 88m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001128" },
                    { new Guid("20000000-0000-0000-0000-000000000129"), "Mercedes C Class", "Sedan", 89m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001129" },
                    { new Guid("20000000-0000-0000-0000-000000000130"), "Toyota Corolla", "SUV", 90m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001130" },
                    { new Guid("20000000-0000-0000-0000-000000000131"), "Honda Civic", "Van", 91m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001131" },
                    { new Guid("20000000-0000-0000-0000-000000000132"), "Ford Focus", "Sedan", 92m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001132" },
                    { new Guid("20000000-0000-0000-0000-000000000133"), "BMW 3 Series", "SUV", 93m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001133" },
                    { new Guid("20000000-0000-0000-0000-000000000134"), "Audi A4", "Van", 94m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001134" },
                    { new Guid("20000000-0000-0000-0000-000000000135"), "Chevrolet Malibu", "Sedan", 95m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001135" },
                    { new Guid("20000000-0000-0000-0000-000000000136"), "Nissan Altima", "SUV", 96m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001136" },
                    { new Guid("20000000-0000-0000-0000-000000000137"), "Hyundai Elantra", "Van", 97m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001137" },
                    { new Guid("20000000-0000-0000-0000-000000000138"), "Kia Rio", "Sedan", 98m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001138" },
                    { new Guid("20000000-0000-0000-0000-000000000139"), "Mercedes C Class", "SUV", 99m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001139" },
                    { new Guid("20000000-0000-0000-0000-000000000140"), "Toyota Corolla", "Van", 30m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001140" },
                    { new Guid("20000000-0000-0000-0000-000000000141"), "Honda Civic", "Sedan", 31m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001141" },
                    { new Guid("20000000-0000-0000-0000-000000000142"), "Ford Focus", "SUV", 32m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001142" },
                    { new Guid("20000000-0000-0000-0000-000000000143"), "BMW 3 Series", "Van", 33m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001143" },
                    { new Guid("20000000-0000-0000-0000-000000000144"), "Audi A4", "Sedan", 34m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001144" },
                    { new Guid("20000000-0000-0000-0000-000000000145"), "Chevrolet Malibu", "SUV", 35m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001145" },
                    { new Guid("20000000-0000-0000-0000-000000000146"), "Nissan Altima", "Van", 36m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001146" },
                    { new Guid("20000000-0000-0000-0000-000000000147"), "Hyundai Elantra", "Sedan", 37m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001147" },
                    { new Guid("20000000-0000-0000-0000-000000000148"), "Kia Rio", "SUV", 38m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001148" },
                    { new Guid("20000000-0000-0000-0000-000000000149"), "Mercedes C Class", "Van", 39m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001149" },
                    { new Guid("20000000-0000-0000-0000-000000000150"), "Toyota Corolla", "Sedan", 40m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001150" },
                    { new Guid("20000000-0000-0000-0000-000000000151"), "Honda Civic", "SUV", 41m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001151" },
                    { new Guid("20000000-0000-0000-0000-000000000152"), "Ford Focus", "Van", 42m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001152" },
                    { new Guid("20000000-0000-0000-0000-000000000153"), "BMW 3 Series", "Sedan", 43m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001153" },
                    { new Guid("20000000-0000-0000-0000-000000000154"), "Audi A4", "SUV", 44m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001154" },
                    { new Guid("20000000-0000-0000-0000-000000000155"), "Chevrolet Malibu", "Van", 45m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001155" },
                    { new Guid("20000000-0000-0000-0000-000000000156"), "Nissan Altima", "Sedan", 46m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001156" },
                    { new Guid("20000000-0000-0000-0000-000000000157"), "Hyundai Elantra", "SUV", 47m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001157" },
                    { new Guid("20000000-0000-0000-0000-000000000158"), "Kia Rio", "Van", 48m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001158" },
                    { new Guid("20000000-0000-0000-0000-000000000159"), "Mercedes C Class", "Sedan", 49m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001159" },
                    { new Guid("20000000-0000-0000-0000-000000000160"), "Toyota Corolla", "SUV", 50m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001160" },
                    { new Guid("20000000-0000-0000-0000-000000000161"), "Honda Civic", "Van", 51m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001161" },
                    { new Guid("20000000-0000-0000-0000-000000000162"), "Ford Focus", "Sedan", 52m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001162" },
                    { new Guid("20000000-0000-0000-0000-000000000163"), "BMW 3 Series", "SUV", 53m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001163" },
                    { new Guid("20000000-0000-0000-0000-000000000164"), "Audi A4", "Van", 54m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001164" },
                    { new Guid("20000000-0000-0000-0000-000000000165"), "Chevrolet Malibu", "Sedan", 55m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001165" },
                    { new Guid("20000000-0000-0000-0000-000000000166"), "Nissan Altima", "SUV", 56m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001166" },
                    { new Guid("20000000-0000-0000-0000-000000000167"), "Hyundai Elantra", "Van", 57m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001167" },
                    { new Guid("20000000-0000-0000-0000-000000000168"), "Kia Rio", "Sedan", 58m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001168" },
                    { new Guid("20000000-0000-0000-0000-000000000169"), "Mercedes C Class", "SUV", 59m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001169" },
                    { new Guid("20000000-0000-0000-0000-000000000170"), "Toyota Corolla", "Van", 60m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001170" },
                    { new Guid("20000000-0000-0000-0000-000000000171"), "Honda Civic", "Sedan", 61m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001171" },
                    { new Guid("20000000-0000-0000-0000-000000000172"), "Ford Focus", "SUV", 62m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001172" },
                    { new Guid("20000000-0000-0000-0000-000000000173"), "BMW 3 Series", "Van", 63m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001173" },
                    { new Guid("20000000-0000-0000-0000-000000000174"), "Audi A4", "Sedan", 64m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001174" },
                    { new Guid("20000000-0000-0000-0000-000000000175"), "Chevrolet Malibu", "SUV", 65m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001175" },
                    { new Guid("20000000-0000-0000-0000-000000000176"), "Nissan Altima", "Van", 66m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001176" },
                    { new Guid("20000000-0000-0000-0000-000000000177"), "Hyundai Elantra", "Sedan", 67m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001177" },
                    { new Guid("20000000-0000-0000-0000-000000000178"), "Kia Rio", "SUV", 68m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001178" },
                    { new Guid("20000000-0000-0000-0000-000000000179"), "Mercedes C Class", "Van", 69m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001179" },
                    { new Guid("20000000-0000-0000-0000-000000000180"), "Toyota Corolla", "Sedan", 70m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001180" },
                    { new Guid("20000000-0000-0000-0000-000000000181"), "Honda Civic", "SUV", 71m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001181" },
                    { new Guid("20000000-0000-0000-0000-000000000182"), "Ford Focus", "Van", 72m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001182" },
                    { new Guid("20000000-0000-0000-0000-000000000183"), "BMW 3 Series", "Sedan", 73m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001183" },
                    { new Guid("20000000-0000-0000-0000-000000000184"), "Audi A4", "SUV", 74m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001184" },
                    { new Guid("20000000-0000-0000-0000-000000000185"), "Chevrolet Malibu", "Van", 75m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001185" },
                    { new Guid("20000000-0000-0000-0000-000000000186"), "Nissan Altima", "Sedan", 76m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001186" },
                    { new Guid("20000000-0000-0000-0000-000000000187"), "Hyundai Elantra", "SUV", 77m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001187" },
                    { new Guid("20000000-0000-0000-0000-000000000188"), "Kia Rio", "Van", 78m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001188" },
                    { new Guid("20000000-0000-0000-0000-000000000189"), "Mercedes C Class", "Sedan", 79m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001189" },
                    { new Guid("20000000-0000-0000-0000-000000000190"), "Toyota Corolla", "SUV", 80m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001190" },
                    { new Guid("20000000-0000-0000-0000-000000000191"), "Honda Civic", "Van", 81m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001191" },
                    { new Guid("20000000-0000-0000-0000-000000000192"), "Ford Focus", "Sedan", 82m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001192" },
                    { new Guid("20000000-0000-0000-0000-000000000193"), "BMW 3 Series", "SUV", 83m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001193" },
                    { new Guid("20000000-0000-0000-0000-000000000194"), "Audi A4", "Van", 84m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001194" },
                    { new Guid("20000000-0000-0000-0000-000000000195"), "Chevrolet Malibu", "Sedan", 85m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001195" },
                    { new Guid("20000000-0000-0000-0000-000000000196"), "Nissan Altima", "SUV", 86m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001196" },
                    { new Guid("20000000-0000-0000-0000-000000000197"), "Hyundai Elantra", "Van", 87m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001197" },
                    { new Guid("20000000-0000-0000-0000-000000000198"), "Kia Rio", "Sedan", 88m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001198" },
                    { new Guid("20000000-0000-0000-0000-000000000199"), "Mercedes C Class", "SUV", 89m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001199" },
                    { new Guid("20000000-0000-0000-0000-000000000200"), "Toyota Corolla", "Van", 90m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001200" },
                    { new Guid("20000000-0000-0000-0000-000000000201"), "Honda Civic", "Sedan", 91m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001201" },
                    { new Guid("20000000-0000-0000-0000-000000000202"), "Ford Focus", "SUV", 92m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001202" },
                    { new Guid("20000000-0000-0000-0000-000000000203"), "BMW 3 Series", "Van", 93m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001203" },
                    { new Guid("20000000-0000-0000-0000-000000000204"), "Audi A4", "Sedan", 94m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001204" },
                    { new Guid("20000000-0000-0000-0000-000000000205"), "Chevrolet Malibu", "SUV", 95m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001205" },
                    { new Guid("20000000-0000-0000-0000-000000000206"), "Nissan Altima", "Van", 96m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001206" },
                    { new Guid("20000000-0000-0000-0000-000000000207"), "Hyundai Elantra", "Sedan", 97m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001207" },
                    { new Guid("20000000-0000-0000-0000-000000000208"), "Kia Rio", "SUV", 98m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001208" },
                    { new Guid("20000000-0000-0000-0000-000000000209"), "Mercedes C Class", "Van", 99m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001209" },
                    { new Guid("20000000-0000-0000-0000-000000000210"), "Toyota Corolla", "Sedan", 30m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001210" },
                    { new Guid("20000000-0000-0000-0000-000000000211"), "Honda Civic", "SUV", 31m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001211" },
                    { new Guid("20000000-0000-0000-0000-000000000212"), "Ford Focus", "Van", 32m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001212" },
                    { new Guid("20000000-0000-0000-0000-000000000213"), "BMW 3 Series", "Sedan", 33m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001213" },
                    { new Guid("20000000-0000-0000-0000-000000000214"), "Audi A4", "SUV", 34m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001214" },
                    { new Guid("20000000-0000-0000-0000-000000000215"), "Chevrolet Malibu", "Van", 35m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001215" },
                    { new Guid("20000000-0000-0000-0000-000000000216"), "Nissan Altima", "Sedan", 36m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001216" },
                    { new Guid("20000000-0000-0000-0000-000000000217"), "Hyundai Elantra", "SUV", 37m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001217" },
                    { new Guid("20000000-0000-0000-0000-000000000218"), "Kia Rio", "Van", 38m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001218" },
                    { new Guid("20000000-0000-0000-0000-000000000219"), "Mercedes C Class", "Sedan", 39m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001219" },
                    { new Guid("20000000-0000-0000-0000-000000000220"), "Toyota Corolla", "SUV", 40m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001220" },
                    { new Guid("20000000-0000-0000-0000-000000000221"), "Honda Civic", "Van", 41m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001221" },
                    { new Guid("20000000-0000-0000-0000-000000000222"), "Ford Focus", "Sedan", 42m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001222" },
                    { new Guid("20000000-0000-0000-0000-000000000223"), "BMW 3 Series", "SUV", 43m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001223" },
                    { new Guid("20000000-0000-0000-0000-000000000224"), "Audi A4", "Van", 44m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001224" },
                    { new Guid("20000000-0000-0000-0000-000000000225"), "Chevrolet Malibu", "Sedan", 45m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001225" },
                    { new Guid("20000000-0000-0000-0000-000000000226"), "Nissan Altima", "SUV", 46m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001226" },
                    { new Guid("20000000-0000-0000-0000-000000000227"), "Hyundai Elantra", "Van", 47m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001227" },
                    { new Guid("20000000-0000-0000-0000-000000000228"), "Kia Rio", "Sedan", 48m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001228" },
                    { new Guid("20000000-0000-0000-0000-000000000229"), "Mercedes C Class", "SUV", 49m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001229" },
                    { new Guid("20000000-0000-0000-0000-000000000230"), "Toyota Corolla", "Van", 50m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001230" },
                    { new Guid("20000000-0000-0000-0000-000000000231"), "Honda Civic", "Sedan", 51m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001231" },
                    { new Guid("20000000-0000-0000-0000-000000000232"), "Ford Focus", "SUV", 52m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001232" },
                    { new Guid("20000000-0000-0000-0000-000000000233"), "BMW 3 Series", "Van", 53m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001233" },
                    { new Guid("20000000-0000-0000-0000-000000000234"), "Audi A4", "Sedan", 54m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001234" },
                    { new Guid("20000000-0000-0000-0000-000000000235"), "Chevrolet Malibu", "SUV", 55m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001235" },
                    { new Guid("20000000-0000-0000-0000-000000000236"), "Nissan Altima", "Van", 56m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001236" },
                    { new Guid("20000000-0000-0000-0000-000000000237"), "Hyundai Elantra", "Sedan", 57m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001237" },
                    { new Guid("20000000-0000-0000-0000-000000000238"), "Kia Rio", "SUV", 58m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001238" },
                    { new Guid("20000000-0000-0000-0000-000000000239"), "Mercedes C Class", "Van", 59m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001239" },
                    { new Guid("20000000-0000-0000-0000-000000000240"), "Toyota Corolla", "Sedan", 60m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001240" },
                    { new Guid("20000000-0000-0000-0000-000000000241"), "Honda Civic", "SUV", 61m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001241" },
                    { new Guid("20000000-0000-0000-0000-000000000242"), "Ford Focus", "Van", 62m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001242" },
                    { new Guid("20000000-0000-0000-0000-000000000243"), "BMW 3 Series", "Sedan", 63m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001243" },
                    { new Guid("20000000-0000-0000-0000-000000000244"), "Audi A4", "SUV", 64m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001244" },
                    { new Guid("20000000-0000-0000-0000-000000000245"), "Chevrolet Malibu", "Van", 65m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001245" },
                    { new Guid("20000000-0000-0000-0000-000000000246"), "Nissan Altima", "Sedan", 66m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001246" },
                    { new Guid("20000000-0000-0000-0000-000000000247"), "Hyundai Elantra", "SUV", 67m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001247" },
                    { new Guid("20000000-0000-0000-0000-000000000248"), "Kia Rio", "Van", 68m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001248" },
                    { new Guid("20000000-0000-0000-0000-000000000249"), "Mercedes C Class", "Sedan", 69m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001249" },
                    { new Guid("20000000-0000-0000-0000-000000000250"), "Toyota Corolla", "SUV", 70m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001250" },
                    { new Guid("20000000-0000-0000-0000-000000000251"), "Honda Civic", "Van", 71m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001251" },
                    { new Guid("20000000-0000-0000-0000-000000000252"), "Ford Focus", "Sedan", 72m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001252" },
                    { new Guid("20000000-0000-0000-0000-000000000253"), "BMW 3 Series", "SUV", 73m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001253" },
                    { new Guid("20000000-0000-0000-0000-000000000254"), "Audi A4", "Van", 74m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001254" },
                    { new Guid("20000000-0000-0000-0000-000000000255"), "Chevrolet Malibu", "Sedan", 75m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001255" },
                    { new Guid("20000000-0000-0000-0000-000000000256"), "Nissan Altima", "SUV", 76m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001256" },
                    { new Guid("20000000-0000-0000-0000-000000000257"), "Hyundai Elantra", "Van", 77m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001257" },
                    { new Guid("20000000-0000-0000-0000-000000000258"), "Kia Rio", "Sedan", 78m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001258" },
                    { new Guid("20000000-0000-0000-0000-000000000259"), "Mercedes C Class", "SUV", 79m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001259" },
                    { new Guid("20000000-0000-0000-0000-000000000260"), "Toyota Corolla", "Van", 80m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001260" },
                    { new Guid("20000000-0000-0000-0000-000000000261"), "Honda Civic", "Sedan", 81m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001261" },
                    { new Guid("20000000-0000-0000-0000-000000000262"), "Ford Focus", "SUV", 82m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001262" },
                    { new Guid("20000000-0000-0000-0000-000000000263"), "BMW 3 Series", "Van", 83m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001263" },
                    { new Guid("20000000-0000-0000-0000-000000000264"), "Audi A4", "Sedan", 84m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001264" },
                    { new Guid("20000000-0000-0000-0000-000000000265"), "Chevrolet Malibu", "SUV", 85m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001265" },
                    { new Guid("20000000-0000-0000-0000-000000000266"), "Nissan Altima", "Van", 86m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001266" },
                    { new Guid("20000000-0000-0000-0000-000000000267"), "Hyundai Elantra", "Sedan", 87m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001267" },
                    { new Guid("20000000-0000-0000-0000-000000000268"), "Kia Rio", "SUV", 88m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001268" },
                    { new Guid("20000000-0000-0000-0000-000000000269"), "Mercedes C Class", "Van", 89m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001269" },
                    { new Guid("20000000-0000-0000-0000-000000000270"), "Toyota Corolla", "Sedan", 90m, "Sample Toyota Corolla 2010 for testing.", true, "2010", "REG-001270" },
                    { new Guid("20000000-0000-0000-0000-000000000271"), "Honda Civic", "SUV", 91m, "Sample Honda Civic 2011 for testing.", true, "2011", "REG-001271" },
                    { new Guid("20000000-0000-0000-0000-000000000272"), "Ford Focus", "Van", 92m, "Sample Ford Focus 2012 for testing.", true, "2012", "REG-001272" },
                    { new Guid("20000000-0000-0000-0000-000000000273"), "BMW 3 Series", "Sedan", 93m, "Sample BMW 3 Series 2013 for testing.", true, "2013", "REG-001273" },
                    { new Guid("20000000-0000-0000-0000-000000000274"), "Audi A4", "SUV", 94m, "Sample Audi A4 2014 for testing.", true, "2014", "REG-001274" },
                    { new Guid("20000000-0000-0000-0000-000000000275"), "Chevrolet Malibu", "Van", 95m, "Sample Chevrolet Malibu 2015 for testing.", true, "2015", "REG-001275" },
                    { new Guid("20000000-0000-0000-0000-000000000276"), "Nissan Altima", "Sedan", 96m, "Sample Nissan Altima 2016 for testing.", true, "2016", "REG-001276" },
                    { new Guid("20000000-0000-0000-0000-000000000277"), "Hyundai Elantra", "SUV", 97m, "Sample Hyundai Elantra 2017 for testing.", true, "2017", "REG-001277" },
                    { new Guid("20000000-0000-0000-0000-000000000278"), "Kia Rio", "Van", 98m, "Sample Kia Rio 2018 for testing.", true, "2018", "REG-001278" },
                    { new Guid("20000000-0000-0000-0000-000000000279"), "Mercedes C Class", "Sedan", 99m, "Sample Mercedes C Class 2019 for testing.", true, "2019", "REG-001279" },
                    { new Guid("20000000-0000-0000-0000-000000000280"), "Toyota Corolla", "SUV", 30m, "Sample Toyota Corolla 2020 for testing.", true, "2020", "REG-001280" },
                    { new Guid("20000000-0000-0000-0000-000000000281"), "Honda Civic", "Van", 31m, "Sample Honda Civic 2021 for testing.", true, "2021", "REG-001281" },
                    { new Guid("20000000-0000-0000-0000-000000000282"), "Ford Focus", "Sedan", 32m, "Sample Ford Focus 2022 for testing.", true, "2022", "REG-001282" },
                    { new Guid("20000000-0000-0000-0000-000000000283"), "BMW 3 Series", "SUV", 33m, "Sample BMW 3 Series 2023 for testing.", true, "2023", "REG-001283" },
                    { new Guid("20000000-0000-0000-0000-000000000284"), "Audi A4", "Van", 34m, "Sample Audi A4 2024 for testing.", true, "2024", "REG-001284" },
                    { new Guid("20000000-0000-0000-0000-000000000285"), "Chevrolet Malibu", "Sedan", 35m, "Sample Chevrolet Malibu 2010 for testing.", true, "2010", "REG-001285" },
                    { new Guid("20000000-0000-0000-0000-000000000286"), "Nissan Altima", "SUV", 36m, "Sample Nissan Altima 2011 for testing.", true, "2011", "REG-001286" },
                    { new Guid("20000000-0000-0000-0000-000000000287"), "Hyundai Elantra", "Van", 37m, "Sample Hyundai Elantra 2012 for testing.", true, "2012", "REG-001287" },
                    { new Guid("20000000-0000-0000-0000-000000000288"), "Kia Rio", "Sedan", 38m, "Sample Kia Rio 2013 for testing.", true, "2013", "REG-001288" },
                    { new Guid("20000000-0000-0000-0000-000000000289"), "Mercedes C Class", "SUV", 39m, "Sample Mercedes C Class 2014 for testing.", true, "2014", "REG-001289" },
                    { new Guid("20000000-0000-0000-0000-000000000290"), "Toyota Corolla", "Van", 40m, "Sample Toyota Corolla 2015 for testing.", true, "2015", "REG-001290" },
                    { new Guid("20000000-0000-0000-0000-000000000291"), "Honda Civic", "Sedan", 41m, "Sample Honda Civic 2016 for testing.", true, "2016", "REG-001291" },
                    { new Guid("20000000-0000-0000-0000-000000000292"), "Ford Focus", "SUV", 42m, "Sample Ford Focus 2017 for testing.", true, "2017", "REG-001292" },
                    { new Guid("20000000-0000-0000-0000-000000000293"), "BMW 3 Series", "Van", 43m, "Sample BMW 3 Series 2018 for testing.", true, "2018", "REG-001293" },
                    { new Guid("20000000-0000-0000-0000-000000000294"), "Audi A4", "Sedan", 44m, "Sample Audi A4 2019 for testing.", true, "2019", "REG-001294" },
                    { new Guid("20000000-0000-0000-0000-000000000295"), "Chevrolet Malibu", "SUV", 45m, "Sample Chevrolet Malibu 2020 for testing.", true, "2020", "REG-001295" },
                    { new Guid("20000000-0000-0000-0000-000000000296"), "Nissan Altima", "Van", 46m, "Sample Nissan Altima 2021 for testing.", true, "2021", "REG-001296" },
                    { new Guid("20000000-0000-0000-0000-000000000297"), "Hyundai Elantra", "Sedan", 47m, "Sample Hyundai Elantra 2022 for testing.", true, "2022", "REG-001297" },
                    { new Guid("20000000-0000-0000-0000-000000000298"), "Kia Rio", "SUV", 48m, "Sample Kia Rio 2023 for testing.", true, "2023", "REG-001298" },
                    { new Guid("20000000-0000-0000-0000-000000000299"), "Mercedes C Class", "Van", 49m, "Sample Mercedes C Class 2024 for testing.", true, "2024", "REG-001299" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "user1@example.com", "bCW7U4jwJs71FiGK9DZK8Q==.WoPsW82LMwGdtRMwsBdcX9+QkpCkZd/ZzOSgGgo7/cc=" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "user2@example.com", "bCW7U4jwJs71FiGK9DZK8Q==.WoPsW82LMwGdtRMwsBdcX9+QkpCkZd/ZzOSgGgo7/cc=" }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CarId", "EndDateTime", "StartDateTime", "UserId" },
                values: new object[,]
                { 
                    { new Guid("30000000-0000-0000-0000-000000000000"), new Guid("20000000-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2025, 1, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002"), new DateTimeOffset(new DateTime(2025, 1, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000003"), new DateTimeOffset(new DateTime(2025, 1, 6, 14, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 4, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000004"), new DateTimeOffset(new DateTime(2025, 1, 7, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 1, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("10000000-0000-0000-0000-000000000001") }                          
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CarId",
                table: "Reservations",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
