namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noofposition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "NoOfPosition", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "NoOfPosition");
        }
    }
}
