namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boardupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Board", "SponsorBonus", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Board", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.Board", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Board", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Board", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Board", "SponsorBonus");
        }
    }
}
