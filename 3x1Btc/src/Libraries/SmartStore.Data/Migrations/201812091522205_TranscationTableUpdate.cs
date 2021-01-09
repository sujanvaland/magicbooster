namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranscationTableUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transaction", "Status");
            DropColumn("dbo.Transaction", "TranscationType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaction", "TranscationType", c => c.Int(nullable: false));
            AddColumn("dbo.Transaction", "Status", c => c.Int(nullable: false));
        }
    }
}
