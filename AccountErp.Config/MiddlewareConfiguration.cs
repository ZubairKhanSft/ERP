using AccountErp.DataLayer;
using AccountErp.DataLayer.Repositories;
using AccountErp.Infrastructure;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Infrastructure.Services;
using AccountErp.Managers;
using AccountErp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AccountErp.Config
{
    public class MiddlewareConfiguration
    {
        public static void ConfigureEf(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

            //services.AddEntityFrameworkNpgsql()
            //    .AddDbContext<DataContext>(
            //        options => options.UseNpgsql(connectionString));
        }
        public static void ConfigureUow(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void ConfigureManager(IServiceCollection services)
        {
            services.AddScoped<ISeedManager, SeedManager>();
            services.AddScoped<IBankAccountManager, BackAccountManager>();
            services.AddScoped<ICreditCardManager, CreditCardManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IBillManager, BillManager>();
            services.AddScoped<IInvoiceManager, InvoiceManager>();
            services.AddScoped<IMasterDataManager, MasterDataManager>();
            services.AddScoped<IItemManager, ItemManager>();
            services.AddScoped<IVendorManager, VendorManager>();
            services.AddScoped<ISalesTaxManager, SalesTaxManager>();
            services.AddScoped<IBillPaymentManager, BillPaymentManager>();
            services.AddScoped<IInvoicePaymentManager, InvoicePaymentManager>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IQuotationManager, QuotationManager>();
            services.AddScoped<IRecurringInvoiceManager, RecurringInvoiceManager>();
            services.AddScoped<IRecurringJobManager, RecurringJobManager>();
            services.AddScoped<IReportManager, ReportManager>();
            services.AddScoped<IChartofAccountManager, ChartofAccountManager>();
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped<IDashboardManager, DashboardManager>();
            services.AddScoped<IReconciliationManager, ReconciliationManager>();
            services.AddScoped<IEndingStatementBalanceManager, EndingStatementBalanceManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IProductCategoryManager, ProductCategoryManager>();
            services.AddScoped<IWareHouseManager, WareHouseManager>();
            services.AddScoped<IUserAccessMAnager, UserAccessManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserRoleManager, UserRoleManager>();
            services.AddScoped<ICreditMemoManager, CreditMemoManager>();
            services.AddScoped<IProjectManager, ProjectManager>();
           
            services.AddScoped<IScreenManager, ScreenManager>();
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<IRolePermissionManager, RolePermissionManager>();
            services.AddScoped<IUserScreenPermissionAccessManager, UserScreenPermissionAccessManager>();
            services.AddScoped<IScreenAccessMAnager, ScreenAccessManager>();
            services.AddScoped<ICurrencyCrManager, CurrencyCrManager>();



        }
        public static void ConfigureRepository(IServiceCollection services)
        {
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<ISalesTaxRepository, SalesTaxRepository>();
            services.AddScoped<IBillPaymentRepository, BillPaymentRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IInvoicePaymentRepository, InvoicePaymentRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IRecurringInvoiceRepository, RecurringInvoiceRepository>();
            services.AddScoped<IRecurringJobRepository, RecuringJobRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IReconciliationRepository, ReconciliationRepository>();
            services.AddScoped<IEndingStatementBalanceRepository, EndingStatementBalanceRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IWareHouseRepository, WareHouseRepository>();

            services.AddScoped<IUserAccessRepository, UserAccessRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<ICreditMemoRepository, CreditMemoRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
          
            services.AddScoped<IScreenRepository, ScreenRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IUserScreenPermissionAccessRepository, UserScreenPermissionAccessRepository>();
            services.AddScoped<IScreenAccessRepository, ScreenAccessRepository>();
            services.AddScoped<ICurrencyCrRepository, CurrencyCrRepository>();



        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
        }
    }
}
