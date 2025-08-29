using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteTodoProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE DeleteTodo(
                    IN tid INT, 
                    IN uid INT 
                )
                BEGIN 
                    DELETE FROM TodoItems 
                    WHERE 
                        Id = tid AND UserId = uid; 
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS DeleteTodo");
        }
    }
}
