namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gcttoken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerToken",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        NoOfToken = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        EarningSource = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.CustomerBlogPost",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogUrl = c.String(),
                        CustomerId = c.Int(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerBlogPost", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.CustomerToken", "CustomerId", "dbo.Customer");
            DropIndex("dbo.CustomerBlogPost", new[] { "CustomerId" });
            DropIndex("dbo.CustomerToken", new[] { "CustomerId" });
            DropTable("dbo.CustomerBlogPost");
            DropTable("dbo.CustomerToken");
        }
    }
}
