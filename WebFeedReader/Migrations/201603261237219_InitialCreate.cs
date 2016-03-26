namespace WebFeedReader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeedItems",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Feed_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Feeds", t => t.Feed_ID)
                .Index(t => t.Feed_ID);
            
            CreateTable(
                "dbo.Feeds",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeedItems", "Feed_ID", "dbo.Feeds");
            DropIndex("dbo.FeedItems", new[] { "Feed_ID" });
            DropTable("dbo.Feeds");
            DropTable("dbo.FeedItems");
        }
    }
}
