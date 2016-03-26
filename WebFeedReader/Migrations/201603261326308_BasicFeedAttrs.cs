namespace WebFeedReader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasicFeedAttrs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FeedItems", "Feed_ID", "dbo.Feeds");
            DropIndex("dbo.FeedItems", new[] { "Feed_ID" });
            AlterColumn("dbo.FeedItems", "Title", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.FeedItems", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.FeedItems", "Feed_ID", c => c.Long(nullable: false));
            AlterColumn("dbo.Feeds", "Title", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Feeds", "Url", c => c.String(nullable: false, maxLength: 4000));
            CreateIndex("dbo.FeedItems", "Feed_ID");
            AddForeignKey("dbo.FeedItems", "Feed_ID", "dbo.Feeds", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeedItems", "Feed_ID", "dbo.Feeds");
            DropIndex("dbo.FeedItems", new[] { "Feed_ID" });
            AlterColumn("dbo.Feeds", "Url", c => c.String());
            AlterColumn("dbo.Feeds", "Title", c => c.String());
            AlterColumn("dbo.FeedItems", "Feed_ID", c => c.Long());
            AlterColumn("dbo.FeedItems", "Content", c => c.String());
            AlterColumn("dbo.FeedItems", "Title", c => c.String());
            CreateIndex("dbo.FeedItems", "Feed_ID");
            AddForeignKey("dbo.FeedItems", "Feed_ID", "dbo.Feeds", "ID");
        }
    }
}
