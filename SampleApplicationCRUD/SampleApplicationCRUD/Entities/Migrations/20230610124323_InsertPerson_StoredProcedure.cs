using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertPerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
               CREATE PROCEDURE [dbo].[InsertPerson]
                (@PersonID uniqueidentifier, @PersonName nvarchar(40), @PersonEmail nvarchar(60),
                    @DateOfBirth datetime2(7), @PersonGender nvarchar(10), @CountryID uniqueidentifier,
                    @PersonAddress nvarchar(100), @ReceiveNewsLetters bit)
                AS BEGIN
                    INSERT INTO [dbo].[People](PersonID, PersonName, PersonEmail, DateOfBirth,
                        PersonGender, CountryID, PersonAddress, ReceiveNewsLetters)
                    Values (@PersonID, @PersonName, @PersonEmail, @DateOfBirth, @PersonGender,
                        @CountryID, @PersonAddress, @ReceiveNewsLetters)
                END
            ";

            migrationBuilder.Sql(sp_InsertPerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                DROP PROCEDURE [dbo].[InsertPerson]
            ";

            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
