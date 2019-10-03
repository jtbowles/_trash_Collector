namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyUpdatedCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "DayId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "DayId");
            AddForeignKey("dbo.Customers", "DayId", "dbo.Days", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "DayId", "dbo.Days");
            DropIndex("dbo.Customers", new[] { "DayId" });
            DropColumn("dbo.Customers", "DayId");
        }
    }
}
