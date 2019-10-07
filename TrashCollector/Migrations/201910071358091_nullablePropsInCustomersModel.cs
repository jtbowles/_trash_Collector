namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullablePropsInCustomersModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Balance", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "ExtraPickUp", c => c.DateTime());
            AlterColumn("dbo.Customers", "StartHoldDate", c => c.DateTime());
            AlterColumn("dbo.Customers", "EndHoldDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "EndHoldDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "StartHoldDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "ExtraPickUp", c => c.DateTime(nullable: false));
            DropColumn("dbo.Customers", "Balance");
        }
    }
}
