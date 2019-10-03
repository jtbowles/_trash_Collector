namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeExtraPickUpAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "ExtraPickUp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "ExtraPickUp");
        }
    }
}
