using AccountErp.DataLayer.EntityConfigurations;
using AccountErp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountErp.DataLayer
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillAttachment> BillAttachments { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceAttachment> InvoiceAttachments { get; set; }
        public DbSet<InvoiceService> InvoiceServices { get; set; }
        public DbSet<InvoicePayment> InvoicePayments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<SalesTax> SalesTaxes { get; set; }
        public DbSet<BillPayment> BillPayments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationAttachment> QuotationAttachments { get; set; }
        public DbSet<QuotationService> QuotationServices { get; set; }

        public DbSet<RecurringInvoice> RecurringInvoice { get; set; }
        public DbSet<RecurringInvoiceAttachment> RecurringInvoiceAttachment { get; set; }
        public DbSet<RecurringInvoiceService> RecurringInvoiceService { get; set; }
        public DbSet<COA_Account> COA_Account { get; set; }
        public DbSet<COA_AccountType> COA_AccountType { get; set; }
        public DbSet<COA_AccountMaster> COA_AccountMaster { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Reconciliation> Reconciliation { get; set; }
        public DbSet<EndingStatementBalance> EndingStatementBalance { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<WareHouse> WareHouse { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<LoginModule> LoginModule { get; set; }
        public DbSet<UserScreenAccess> UserScreenAccess { get; set; }
        public DbSet<ScreenDetail> ScreenDetail { get; set; }
        public DbSet<Screen> Screen { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }

        public DbSet<CreditMemo> CreditMemo { get; set; }

        public DbSet<CreditMemoService> CreditMemoService { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectTransaction> ProjectTransactions { get; set; }
        public DbSet<ShippingAddress> ShippingAddress { get; set; }
       
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserScreenPermissionAccess> UserScreenPermissionAccess { get; set; }
        public DbSet<ScreenAccess> ScreenAccesses { get; set; }
        public DbSet<CurrencyCr> CurrencyCr { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new CreditCardConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new BillAttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new BillItemConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceAttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceServiceConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new BillPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new InvoicePaymentConfiguration());
            modelBuilder.ApplyConfiguration(new QuotationAttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new QuotationConfiguration());
            modelBuilder.ApplyConfiguration(new QuotationServicesConfiguration());
            modelBuilder.ApplyConfiguration(new RecurringInvoiceAttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new RecurringInvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new RecurringInvoiceServiceConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new ReconciliationConfigurations());
            modelBuilder.ApplyConfiguration(new EndingStatementBalanceConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new WareHouseConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new LoginModuleConfiguration());
            modelBuilder.ApplyConfiguration(new UserScreenAccessConfiguration());
            modelBuilder.ApplyConfiguration(new ScreenDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ScreenConfiguration());
            modelBuilder.ApplyConfiguration(new CreditMemoConfiguration());
            modelBuilder.ApplyConfiguration(new CreditMemoServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTransactionConfiguration());
            
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserScreenPermissionAccessConfiguration());
            modelBuilder.ApplyConfiguration(new ScreenAccessConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyCrConfiguration());




        }
    }
}
