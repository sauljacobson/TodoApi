using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTodoProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE AddNewTodo(
                    in title VARCHAR(255),   
                    in id INT  
                )
                BEGIN    
                    INSERT INTO TodoItems(Title, IsDone, UserId)      
                        VALUES (title, false, id);  
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS AddNewTodo");
        }
    }
}
