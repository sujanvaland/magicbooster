namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adCampaing : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.CustomerPayment", "PayToCustomerId", "dbo.Customer");
            //DropIndex("dbo.CustomerPayment", new[] { "PayToCustomerId" });
            CreateTable(
                "dbo.AdCampaign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WebsiteUrl = c.String(maxLength: 200),
                        Banner125 = c.String(maxLength: 200),
                        Banner486 = c.String(maxLength: 200),
                        Banner728 = c.String(maxLength: 200),
                        AssignedCredit = c.Int(nullable: false),
                        UsedCredit = c.Int(nullable: false),
                        AvailableCredit = c.Int(nullable: false),
                        CreditType = c.String(maxLength: 12),
                        AdType = c.String(maxLength: 12),
                        NoOfDays = c.Int(),
                        ExpiryDate = c.Int(),
                        CustomerId = c.Int(nullable: false),
                        PictureId = c.Int(),
                        Enabled = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .ForeignKey("dbo.Picture", t => t.PictureId)
                .Index(t => t.CustomerId)
                .Index(t => t.PictureId);
            
            AlterColumn("dbo.Board", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Board", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //DropColumn("dbo.CustomerPosition", "ReferredById");
            //DropTable("dbo.CustomerPayment");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CustomerPosition", "ReferredById", c => c.Int(nullable: false));
            DropForeignKey("dbo.AdCampaign", "PictureId", "dbo.Picture");
            DropForeignKey("dbo.AdCampaign", "CustomerId", "dbo.Customer");
            DropIndex("dbo.AdCampaign", new[] { "PictureId" });
            DropIndex("dbo.AdCampaign", new[] { "CustomerId" });
            AlterColumn("dbo.Board", "Payout", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.Board", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            DropTable("dbo.AdCampaign");
            CreateIndex("dbo.CustomerPayment", "PayToCustomerId");
            AddForeignKey("dbo.CustomerPayment", "PayToCustomerId", "dbo.Customer", "Id", cascadeDelete: true);
        }
    }
}
