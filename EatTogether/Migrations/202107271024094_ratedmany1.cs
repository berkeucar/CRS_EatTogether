namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratedmany1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RatedUserModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserRatedId = c.String(nullable: false, maxLength: 450),
                        UserRatesId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserRatedId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserRatesId, cascadeDelete: false)
                .Index(t => t.UserRatedId)
                .Index(t => t.UserRatesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RatedUserModels", "UserRatesId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RatedUserModels", "UserRatedId", "dbo.AspNetUsers");
            DropIndex("dbo.RatedUserModels", new[] { "UserRatesId" });
            DropIndex("dbo.RatedUserModels", new[] { "UserRatedId" });
            DropTable("dbo.RatedUserModels");
        }
    }
}
