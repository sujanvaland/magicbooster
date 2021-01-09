namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YouTubeVideos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YoutubeVideos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoLink = c.String(),
                        CustomerId = c.Int(nullable: false),
                        NoOfViews = c.Int(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.FacebookPost",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoLink = c.String(),
                        CustomerId = c.Int(nullable: false),
                        NoOfLikes = c.Int(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.SupportRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Message = c.String(),
                        CustomerId = c.Int(nullable: false),
                        LastReplied = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.FAQ",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Anaswer = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupportRequest", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.FacebookPost", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.YoutubeVideos", "CustomerId", "dbo.Customer");
            DropIndex("dbo.SupportRequest", new[] { "CustomerId" });
            DropIndex("dbo.FacebookPost", new[] { "CustomerId" });
            DropIndex("dbo.YoutubeVideos", new[] { "CustomerId" });
            DropTable("dbo.FAQ");
            DropTable("dbo.SupportRequest");
            DropTable("dbo.FacebookPost");
            DropTable("dbo.YoutubeVideos");
        }
    }
}
