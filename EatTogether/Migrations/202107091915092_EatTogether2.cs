namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EatTogether2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SessionModels", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.SessionModels", "City", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SessionModels", "City", c => c.String());
            AlterColumn("dbo.SessionModels", "Description", c => c.String());
        }
    }
}
