namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePlanTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plan", "StartROIAfterHours", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plan", "StartROIAfterHours");
        }
    }
}
