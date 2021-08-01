namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedidentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "scoredTimes", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "score");
            DropColumn("dbo.AspNetUsers", "scoredTimes");
        }
    }
}
