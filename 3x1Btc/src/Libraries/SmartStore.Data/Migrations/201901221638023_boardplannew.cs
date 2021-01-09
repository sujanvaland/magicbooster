namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boardplannew : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CustomerPositions", newName: "CustomerPosition");
            AddColumn("dbo.CustomerPosition", "PlacedUnderCustomerId", c => c.Int(nullable: false));
            DropColumn("dbo.CustomerPosition", "PlacedUnderMemberId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerPosition", "PlacedUnderMemberId", c => c.Int(nullable: false));
            DropColumn("dbo.CustomerPosition", "PlacedUnderCustomerId");
            RenameTable(name: "dbo.CustomerPosition", newName: "CustomerPositions");
        }
    }
}
