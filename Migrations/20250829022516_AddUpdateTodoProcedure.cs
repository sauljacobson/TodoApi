using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateTodoProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE tododb.UpdateTodo(
                    in tid INT, 
                    in newTitle VARCHAR(255),
                    in done BOOLEAN, 
                    in uid INT
                )
                BEGIN
                    UPDATE TodoItems 
                    SET 
                        Title = newTitle, 
                        IsDone = done
                    WHERE 
                        Id = tid AND UserId = uid;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS UpdateTodo");
        }
    }
}
