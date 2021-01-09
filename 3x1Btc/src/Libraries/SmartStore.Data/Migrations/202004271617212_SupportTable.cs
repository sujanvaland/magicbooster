namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupportTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SupportRequest", "Email", c => c.String());
            AddColumn("dbo.SupportRequest", "Name", c => c.String());
            AddColumn("dbo.SupportRequest", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SupportRequest", "Status");
            DropColumn("dbo.SupportRequest", "Name");
            DropColumn("dbo.SupportRequest", "Email");
        }
    }
}
