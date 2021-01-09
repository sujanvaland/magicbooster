namespace SmartStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countrymanager : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CountryManager",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateOfJoining = c.DateTime(nullable: false),
                        CountryId = c.Int(nullable: false),
                        CountryCode = c.String(),
                        CountryName = c.String(),
                        TotalTeam = c.Int(nullable: false),
                        WhatsAppLink = c.String(),
                        TelegramLink = c.String(),
                        FacebookLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CountryManager");
        }
    }
}
