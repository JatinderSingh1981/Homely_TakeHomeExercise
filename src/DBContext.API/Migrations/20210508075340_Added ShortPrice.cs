using Microsoft.EntityFrameworkCore.Migrations;

namespace DBContext.API.Migrations
{
    public partial class AddedShortPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Listings",
            //    columns: table => new
            //    {
            //        ListingId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        StreetNumber = table.Column<string>(type: "varchar(100)", nullable: true),
            //        Street = table.Column<string>(type: "varchar(100)", nullable: true),
            //        Suburb = table.Column<string>(type: "varchar(100)", nullable: false),
            //        State = table.Column<string>(type: "varchar(100)", nullable: false),
            //        PostCode = table.Column<int>(type: "int", nullable: false),
            //        CategoryType = table.Column<int>(type: "int", nullable: false),
            //        StatusType = table.Column<int>(type: "int", nullable: false),
            //        DisplayPrice = table.Column<string>(type: "varchar(100)", nullable: true),
            //        ShortPrice = table.Column<string>(type: "varchar(100)", nullable: true),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Listings", x => x.ListingId);
            //    });

            migrationBuilder.AddColumn<string>(
               name: "ShortPrice",
               table: "Listings",
               type: "varchar(100)",
               nullable: true,
               defaultValue: "");

            migrationBuilder.Sql("Update dbo.Listings set ShortPrice=dbo.ufnGetShortPrice(ListingId)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Listings");
        }
    }
}
