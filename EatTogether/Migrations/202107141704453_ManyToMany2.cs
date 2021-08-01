namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToMany2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SessionModels", "PlaceName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SessionModels", "PlaceName");
        }
    }
}
