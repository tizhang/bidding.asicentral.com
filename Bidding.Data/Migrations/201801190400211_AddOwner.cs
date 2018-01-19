namespace Bidding.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BiddingActions",
                c => new
                    {
                        BiddingActionId = c.Long(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        Comment = c.String(),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(),
                        BiddingItem_BiddingItemId = c.Long(),
                    })
                .PrimaryKey(t => t.BiddingActionId)
                .ForeignKey("dbo.BiddingItems", t => t.BiddingItem_BiddingItemId)
                .Index(t => t.BiddingItem_BiddingItemId);
            
            CreateTable(
                "dbo.BiddingConfigs",
                c => new
                    {
                        BiddingConfigId = c.Long(nullable: false, identity: true),
                        MinIncrement = c.Double(nullable: false),
                        Blind = c.Boolean(nullable: false),
                        StartPrice = c.Double(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BiddingConfigId);
            
            CreateTable(
                "dbo.BiddingItems",
                c => new
                    {
                        BiddingItemId = c.Long(nullable: false, identity: true),
                        GroupName = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        OwnerId = c.Int(nullable: false),
                        Config_BiddingConfigId = c.Long(),
                    })
                .PrimaryKey(t => t.BiddingItemId)
                .ForeignKey("dbo.BiddingConfigs", t => t.Config_BiddingConfigId)
                .Index(t => t.Config_BiddingConfigId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BiddingItems", "Config_BiddingConfigId", "dbo.BiddingConfigs");
            DropForeignKey("dbo.BiddingActions", "BiddingItem_BiddingItemId", "dbo.BiddingItems");
            DropIndex("dbo.BiddingItems", new[] { "Config_BiddingConfigId" });
            DropIndex("dbo.BiddingActions", new[] { "BiddingItem_BiddingItemId" });
            DropTable("dbo.BiddingItems");
            DropTable("dbo.BiddingConfigs");
            DropTable("dbo.BiddingActions");
        }
    }
}
