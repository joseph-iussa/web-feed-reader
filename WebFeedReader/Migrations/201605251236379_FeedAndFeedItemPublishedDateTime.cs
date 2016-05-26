namespace WebFeedReader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedAndFeedItemPublishedDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeedItems", "PublishedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Feeds", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feeds", "LastUpdated");
            DropColumn("dbo.FeedItems", "PublishedOn");
        }
    }
}
