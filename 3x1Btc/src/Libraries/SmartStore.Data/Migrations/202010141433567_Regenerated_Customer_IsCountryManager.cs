namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Regenerated_Customer_IsCountryManager : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "IsCountryManager", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "IsCountryManager");
        }
    }
}
