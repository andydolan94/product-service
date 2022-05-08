namespace ProductService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProductOptions", "ProductId");
            AddForeignKey("dbo.ProductOptions", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOptions", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductOptions", new[] { "ProductId" });
        }
    }
}
