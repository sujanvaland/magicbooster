namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCustomerPlan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureId = c.Int(nullable: false),
                        Name = c.String(),
                        Size = c.String(),
                        Published = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        UpdatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Deleted);
            
            AddColumn("dbo.CusomerPlan", "LastPaidDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Banner", new[] { "Deleted" });
            DropColumn("dbo.CusomerPlan", "LastPaidDate");
            DropTable("dbo.Banner");
        }
    }
}
