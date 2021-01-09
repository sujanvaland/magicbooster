namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Regenerated_Customer_PlacementId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "PlacementId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "PlacementId");
        }
    }
}
