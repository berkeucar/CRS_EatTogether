namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EatTogether1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserCount = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        City = c.String(nullable:false),
                        SpaceRemaining = c.Int(nullable: false),
                        Place_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlaceModels", t => t.Place_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Place_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.PlaceModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Point = c.Int(nullable: false),
                        Info = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "SessionModel_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "SessionModel_Id");
            AddForeignKey("dbo.AspNetUsers", "SessionModel_Id", "dbo.SessionModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SessionModels", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SessionModels", "Place_Id", "dbo.PlaceModels");
            DropForeignKey("dbo.AspNetUsers", "SessionModel_Id", "dbo.SessionModels");
            DropIndex("dbo.AspNetUsers", new[] { "SessionModel_Id" });
            DropIndex("dbo.SessionModels", new[] { "User_Id" });
            DropIndex("dbo.SessionModels", new[] { "Place_Id" });
            DropColumn("dbo.AspNetUsers", "SessionModel_Id");
            DropTable("dbo.PlaceModels");
            DropTable("dbo.SessionModels");
        }
    }
}
