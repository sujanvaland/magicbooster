namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailnotificationcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerPosition", "EmailSentOnCycle", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerPosition", "EmailSentOnCycle");
        }
    }
}
