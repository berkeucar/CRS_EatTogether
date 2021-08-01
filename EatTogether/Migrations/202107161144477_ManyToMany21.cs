namespace EatTogether.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToMany21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlaceModels", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlaceModels", "Type");
        }
    }
}
