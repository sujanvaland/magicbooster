namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateplancommission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlanCommission", "LevelId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlanCommission", "LevelId");
        }
    }
}
