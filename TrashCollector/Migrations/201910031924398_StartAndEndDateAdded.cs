namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartAndEndDateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "StartHoldDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "EndHoldDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "EndHoldDate");
            DropColumn("dbo.Customers", "StartHoldDate");
        }
    }
}
