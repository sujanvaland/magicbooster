namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hyip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CusomerPlan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        PlanId = c.Int(nullable: false),
                        AmountInvested = c.Single(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        ROIToPay = c.Single(nullable: false),
                        ROIPaid = c.Single(nullable: false),
                        NoOfPayout = c.Int(nullable: false),
                        NoOfPayoutPaid = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsExpired = c.Boolean(nullable: false),
                        ExpiredDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.Deleted);
            
            CreateTable(
                "dbo.ROIPaid",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                        PaidDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.Deleted);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                        FinalAmount = c.Single(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        ProcessorId = c.Int(nullable: false),
                        RefId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        TranscationType = c.Int(nullable: false),
                        TranscationNote = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.Deleted);
            
            CreateTable(
                "dbo.PlanCommission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanId = c.Int(nullable: false),
                        CommissionPercentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plan", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.PlanId);
            
            CreateTable(
                "dbo.Plan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        PlanDetails = c.String(),
                        NoOfPayouts = c.Int(nullable: false),
                        ROIPercentage = c.Single(nullable: false),
                        MinimumInvestment = c.Single(nullable: false),
                        MaximumInvestment = c.Single(nullable: false),
                        PayEveryXDays = c.Int(nullable: false),
                        SubjectToAcl = c.Boolean(nullable: false),
                        LimitedToStores = c.Boolean(nullable: false),
                        Published = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Deleted);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlanCommission", "PlanId", "dbo.Plan");
            DropForeignKey("dbo.Transaction", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.ROIPaid", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.CusomerPlan", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Plan", new[] { "Deleted" });
            DropIndex("dbo.PlanCommission", new[] { "PlanId" });
            DropIndex("dbo.Transaction", new[] { "Deleted" });
            DropIndex("dbo.Transaction", new[] { "CustomerId" });
            DropIndex("dbo.ROIPaid", new[] { "Deleted" });
            DropIndex("dbo.ROIPaid", new[] { "CustomerId" });
            DropIndex("dbo.CusomerPlan", new[] { "Deleted" });
            DropIndex("dbo.CusomerPlan", new[] { "CustomerId" });
            DropTable("dbo.Plan");
            DropTable("dbo.PlanCommission");
            DropTable("dbo.Transaction");
            DropTable("dbo.ROIPaid");
            DropTable("dbo.CusomerPlan");
        }
    }
}
