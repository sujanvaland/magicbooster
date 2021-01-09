namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "StatusId", c => c.Int(nullable: false));
            AddColumn("dbo.Transaction", "TranscationTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "TranscationTypeId");
            DropColumn("dbo.Transaction", "StatusId");
        }
    }
}
