namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManytoMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "SessionModel_Id", "dbo.SessionModels");
            DropIndex("dbo.AspNetUsers", new[] { "SessionModel_Id" });

            CreateTable(
                "dbo.Session_ApplicationUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 450),
                        SessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SessionModels", t => t.SessionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.SessionId);
            
            DropColumn("dbo.AspNetUsers", "SessionModel_Id");


        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "SessionModel_Id", c => c.Int());
            DropForeignKey("dbo.Session_ApplicationUser", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Session_ApplicationUser", "SessionId", "dbo.SessionModels");
            DropIndex("dbo.Session_ApplicationUser", new[] { "SessionId" });
            DropIndex("dbo.Session_ApplicationUser", new[] { "ApplicationUserId" });
            DropTable("dbo.Session_ApplicationUser");
            CreateIndex("dbo.AspNetUsers", "SessionModel_Id");
            AddForeignKey("dbo.AspNetUsers", "SessionModel_Id", "dbo.SessionModels", "Id");
        }
    }
}
