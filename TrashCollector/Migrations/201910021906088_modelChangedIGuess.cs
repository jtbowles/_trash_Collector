namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelChangedIGuess : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "Balance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Balance", c => c.Int(nullable: false));
        }
    }
}
