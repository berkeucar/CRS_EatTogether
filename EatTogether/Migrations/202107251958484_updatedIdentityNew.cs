namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedIdentityNew : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "score", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "score", c => c.Int(nullable: false));
        }
    }
}
