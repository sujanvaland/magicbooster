namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2m : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerPayment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayToCustomerId = c.Int(nullable: false),
                        PayByCustomerId = c.Int(nullable: false),
                        BitcoinAddress = c.String(),
                        PaymentProcessor1 = c.String(),
                        PaymentProcessor2 = c.String(),
                        PaymentProcessor3 = c.String(),
                        PaymentProcessor4 = c.String(),
                        PaymentProcessor5 = c.String(),
                        Paymentdate = c.DateTime(),
                        Status = c.String(),
                        PaymentProff = c.String(),
                        Remarks = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BoardId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.PayToCustomerId, cascadeDelete: true)
                .Index(t => t.PayToCustomerId);
            
            AddColumn("dbo.CustomerPosition", "ReferredById", c => c.Int(nullable: false));
            AlterColumn("dbo.Board", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.Board", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerPayment", "PayToCustomerId", "dbo.Customer");
            DropIndex("dbo.CustomerPayment", new[] { "PayToCustomerId" });
            AlterColumn("dbo.Board", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Board", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.CustomerPosition", "ReferredById");
            DropTable("dbo.CustomerPayment");
        }
    }
}
