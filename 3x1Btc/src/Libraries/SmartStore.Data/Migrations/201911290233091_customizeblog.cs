namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customizeblog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPost", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.BlogPost", "Customer_Id", c => c.Int());
            CreateIndex("dbo.BlogPost", "CustomerId");
            CreateIndex("dbo.BlogPost", "Customer_Id");
            AddForeignKey("dbo.BlogPost", "CustomerId", "dbo.Customer", "Id");
            AddForeignKey("dbo.BlogPost", "Customer_Id", "dbo.Customer", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPost", "Customer_Id", "dbo.Customer");
            DropForeignKey("dbo.BlogPost", "CustomerId", "dbo.Customer");
            DropIndex("dbo.BlogPost", new[] { "Customer_Id" });
            DropIndex("dbo.BlogPost", new[] { "CustomerId" });
            DropColumn("dbo.BlogPost", "Customer_Id");
            DropColumn("dbo.BlogPost", "CustomerId");
        }
    }
}
