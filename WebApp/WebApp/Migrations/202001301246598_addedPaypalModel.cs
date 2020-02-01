namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPaypalModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayPalPaymentDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Intent = c.String(),
                        State = c.String(),
                        Cart = c.String(),
                        CreateTime = c.String(),
                        PayerPaymentMethod = c.String(),
                        PayerStatus = c.String(),
                        PayerEmail = c.String(),
                        PayerFirstName = c.String(),
                        PayerMiddleName = c.String(),
                        PayerLastName = c.String(),
                        PayerId = c.String(),
                        PayerCountryCode = c.String(),
                        ShippingAddressRecipientName = c.String(),
                        ShippingAddressStreet = c.String(),
                        ShippingAddressCity = c.String(),
                        ShippingAddressState = c.String(),
                        ShippingAddressPostalCode = c.Int(nullable: false),
                        ShippingAddressCountryCode = c.String(),
                        TransactionsAmountTotal = c.Double(nullable: false),
                        TransactionsAmountCurrency = c.String(),
                        TransactionsDetailsSubtotal = c.Double(nullable: false),
                        TransactionsDetailsShipping = c.Double(nullable: false),
                        TransactionsDetailsHandlingFee = c.Double(nullable: false),
                        TransactionsDetailsInsurance = c.Double(nullable: false),
                        TransactionsShippingDiscount = c.Double(nullable: false),
                        TransactionsItemListItemsName = c.String(),
                        TransactionsItemListItemsPrice = c.Double(nullable: false),
                        TransactionsItemListItemsCurrencty = c.String(),
                        TransactionsItemListItemsQuantity = c.Int(nullable: false),
                        TransactionsItemListItemsTax = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tickets", "PayPalPaymentDetailsID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tickets", "PayPalPaymentDetailsID");
            AddForeignKey("dbo.Tickets", "PayPalPaymentDetailsID", "dbo.PayPalPaymentDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "PayPalPaymentDetailsID", "dbo.PayPalPaymentDetails");
            DropIndex("dbo.Tickets", new[] { "PayPalPaymentDetailsID" });
            DropColumn("dbo.Tickets", "PayPalPaymentDetailsID");
            DropTable("dbo.PayPalPaymentDetails");
        }
    }
}
