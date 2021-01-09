namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campaignupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdCampaign", "PictureId", "dbo.Picture");
            DropIndex("dbo.AdCampaign", new[] { "PictureId" });
            AlterColumn("dbo.AdCampaign", "ExpiryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdCampaign", "ExpiryDate", c => c.Int());
            CreateIndex("dbo.AdCampaign", "PictureId");
            AddForeignKey("dbo.AdCampaign", "PictureId", "dbo.Picture", "Id");
        }
    }
}
