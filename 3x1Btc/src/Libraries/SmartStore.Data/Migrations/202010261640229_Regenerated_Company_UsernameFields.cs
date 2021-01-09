namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Regenerated_Company_UsernameFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "SponsorsName", c => c.String());
            AddColumn("dbo.Customer", "PlacementUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "PlacementUserName");
            DropColumn("dbo.Customer", "SponsorsName");
        }
    }
}
