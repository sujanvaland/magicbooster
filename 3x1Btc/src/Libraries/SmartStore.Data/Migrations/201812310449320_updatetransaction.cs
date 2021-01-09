namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "WithdrawalAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "WithdrawalAddress");
        }
    }
}
