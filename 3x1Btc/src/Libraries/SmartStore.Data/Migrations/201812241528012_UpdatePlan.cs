namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePlan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plan", "IsPrincipalBack", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plan", "IsPrincipalBack");
        }
    }
}
