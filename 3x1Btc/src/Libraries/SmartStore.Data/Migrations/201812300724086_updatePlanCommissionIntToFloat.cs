namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePlanCommissionIntToFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlanCommission", "CommissionPercentage", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlanCommission", "CommissionPercentage", c => c.Int(nullable: false));
        }
    }
}
