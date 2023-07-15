using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPeople_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPeopleRight = @"
               CREATE PROCEDURE [dbo].[GetAllPeopleRight]
                AS BEGIN
                    SELECT PersonID, PersonName, PersonEmail, DateOfBirth, PersonGender, CountryID, PersonAddress, ReceiveNewsLetters 
                        FROM [dbo].[People]
                END
            ";

            migrationBuilder.Sql(sp_GetAllPeopleRight);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPeopleRight = @"
                DROP PROCEDURE [dbo].[GetAllPeople]
            ";

            migrationBuilder.Sql(sp_GetAllPeopleRight);
        }
    }
}
