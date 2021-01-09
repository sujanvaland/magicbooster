namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class boardplan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerPositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        BoardId = c.Int(nullable: false),
                        PlacedUnderPositionId = c.Int(nullable: false),
                        PlacedUnderMemberId = c.Int(nullable: false),
                        IsCycled = c.Boolean(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        CycledDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Board", t => t.BoardId, cascadeDelete: true)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.Board",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Payout = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayOnComplete = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AutoPurchaseSetting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CycledBoardId = c.Int(nullable: false),
                        PurchaseInBoardId = c.Int(nullable: false),
                        NoOfPosition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DirectBonus = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Active = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerPositions", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.CustomerPositions", "BoardId", "dbo.Board");
            DropIndex("dbo.CustomerPositions", new[] { "BoardId" });
            DropIndex("dbo.CustomerPositions", new[] { "CustomerId" });
            DropTable("dbo.Package");
            DropTable("dbo.AutoPurchaseSetting");
            DropTable("dbo.Board");
            DropTable("dbo.CustomerPositions");
        }
    }
}
