namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repurchasewallet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CusomerPlan", "RepurchaseWallet", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CusomerPlan", "RepurchaseWallet");
        }
    }
}
