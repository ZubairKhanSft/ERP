import { Component, OnInit } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { InvoicePaymentModel, SelectListItemModel, CustomerPaymentInfoModel, InvoiceSummaryModel } from 'src/models';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppUtils } from 'src/helpers';
import { InvoiceService, BankAccountService, CustomerService, InvoicePaymentService } from 'src/services';
import { ChartOfAccountsService } from 'src/services/chart-of-accounts.service';
import { AccountTransactionsService } from 'src/services/account-transactions.service';

@Component({
  selector: 'app-add-customer-payment',
  templateUrl: './add-customer-payment.component.html',
  styleUrls: ['./add-customer-payment.component.css']
})
export class AddCustomerPaymentComponent implements OnInit {
  @BlockUI('container-blockui') blockUI: NgBlockUI;
    model: InvoicePaymentModel = new InvoicePaymentModel();
    invoicePaymentModes: Array<SelectListItemModel> = new Array<SelectListItemModel>();
    bankAccounts: Array<SelectListItemModel> = new Array<SelectListItemModel>();
    depositFromAccounts: Array<SelectListItemModel> = new Array<SelectListItemModel>();
    customerPaymentInfoModel: CustomerPaymentInfoModel = new CustomerPaymentInfoModel();
    invoiceSummaryModel: InvoiceSummaryModel = new InvoiceSummaryModel();

    customers : any=[];
    config = {displayKey:"value",search:true,limitTo:10,height: 'auto',placeholder:'Select Customer',customComparator: ()=>{},moreText: 'more',noResultsFound: 'No results found!',searchPlaceholder:'Search',searchOnKey: 'value',clearOnSelection: false,inputDirection: 'ltr',}
    selectedCustomer;
    customer:any={};
    invoices:any=[];
    allAccounts;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private invoiceService: InvoiceService,
        private bankAccountService: BankAccountService,
        private customerService: CustomerService,
        private invoicePaymentService: InvoicePaymentService,
        private accountTransactionService:AccountTransactionsService,
        private chartofaccService:ChartOfAccountsService) {
        this.route.params.subscribe((params) => {
            debugger;
            this.model.invoiceId = params['id'];
            this.model.paymentType=1
            this.loadInvoiceSummary();
            this.loadAccounts();
        });
    }

    ngOnInit() {
        this.invoicePaymentModes = this.appUtils.getInvoicePaymentModesSelectList();
        this.loadBankAccounts();
        this.loadInvoiceSummary();
        this.loadCustomers();
        this.loadUnpaidInvoices();
        this.model.paymentDate = this.appUtils.getDateForNgDatePicker(null);
    }

    loadBankAccounts() {
        this.bankAccountService.getSelectItems()
            .subscribe(
                data => {
                    Object.assign(this.bankAccounts, data);
                },
                error => {
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    loadUnpaidInvoices(){
        
        this.invoicePaymentService.getUnpaidInvoice()
        .subscribe(
            data => {
                Object.assign(this.invoices, data);
                console.log("invoices",this.invoices)
            },
            error => {
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    loadAccounts(){
        this.blockUI.start();
        this.accountTransactionService.getAllaccounts()
            .subscribe((data) => {
                debugger;
                this.blockUI.stop();
               
               
                this.allAccounts=[];
                var master=[];
      
                Object.assign(this.allAccounts, data);
                console.log("all accounts",this.allAccounts)
               
            },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
        
      }

    loadInvoiceSummary() {
        this.blockUI.start();
        this.invoiceService.getSummary(this.model.invoiceId)
            .subscribe(
                (data) => {
                    this.blockUI.stop();
                    Object.assign(this.invoiceSummaryModel, data);
                    this.model.amount=this.invoiceSummaryModel.totalAmount;
                    setTimeout(() => {
                        this.loadCustomerPaymentInfo();
                    }, 100);
                },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    loadCustomerPaymentInfo() {
        this.blockUI.start();
        this.customerService.getPaymentInfo(this.invoiceSummaryModel.customerId)
            .subscribe(
                data => {
                    this.blockUI.stop();
                    Object.assign(this.customerPaymentInfoModel, data);

                    if (this.customerPaymentInfoModel.accountNumber != null) {
                        const selectListItem = new SelectListItemModel();
                        selectListItem.keyString = this.customerPaymentInfoModel.accountNumber;
                        selectListItem.value = this.customerPaymentInfoModel.accountNumber;
                        this.depositFromAccounts.push(selectListItem);
                    }
                },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    chengePaymentMode() {
      debugger;
      var ledgerType;
        if (this.model.paymentMode !== '2') {
            this.model.chequeNumber = '';
        }
        // if (this.model.paymentMode !== '0') {
        //     ledgerType=1;
        // }

        

        if (this.model.paymentMode == '0') {
            ledgerType=1;
            this.chartofaccService.getaccbyledgertype(ledgerType)
            .subscribe(
                data => {
                    this.bankAccounts=[];
                    Object.assign(this.bankAccounts, data);
                    console.log("cashacc",this.bankAccounts)
                },
                error => {
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
        //this.bankAccounts=[{keyInt:1,keyString:"",value:"Cash on hand"},{keyInt:2,keyString:"",value:"Petty cash"}]

         }else{
             ledgerType=2;
             this.chartofaccService.getaccbyledgertype(ledgerType)
             .subscribe(
                 data => {
                     this.bankAccounts=[];
                     Object.assign(this.bankAccounts, data);
                     console.log("cashacc",this.bankAccounts)
                 },
                 error => {
                     this.appUtils.ProcessErrorResponse(this.toastr, error);
                 });
         }
        
    }

    loadCustomers() {
        this.blockUI.start();
        this.customerService.getSelectItems()
            .subscribe((data) => {
                debugger;
                this.blockUI.stop();
                console.log("customers",this.customers)
               
                this.customers=[];
                Object.assign(this.customers, data);
                // if(this.customers.length>0){
                //     this.customrlist=[];
                //     this.customers.forEach(element => {
                //         this.customrlist.push({"id":element.id,"value":element.value})
                //     });
                // }
               
            },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    getCustomerDetail() {
        debugger;
        if(this.selectedCustomer!=undefined){
            this.model.customerId=this.selectedCustomer.keyInt;
            this.invoiceSummaryModel.customerId=this.selectedCustomer.keyInt;
        
        if (this.model.customerId === null
            || this.model.customerId === '') {
          
            return;
        }

        this.customerService.getDetail(Number(this.model.customerId))
            .subscribe(
                (data) => {
                    Object.assign(this.customer, data);
                    this.loadCustomerPaymentInfo();
                  
                });
            }
    }

    chengeInvoice(){
        
        debugger;
        if(this.model.invoiceId!=undefined){
          
        
        if (this.model.invoiceId === null
            || this.model.invoiceId === 0) {
          
            return;
        }
        this.loadInvoiceSummary();
        this.blockUI.start();
        this.invoiceService.getDetail(this.model.invoiceId).subscribe(
            (data: any) => {
                this.blockUI.stop();
                Object.assign(this.invoiceSummaryModel, data);
              
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
        }
    }

    submit() {
        debugger;
        this.blockUI.start();
        if (this.model.paymentDate) {
            this.model.paymentDate = this.appUtils.getFormattedDate(this.model.paymentDate, null);
        }
        
        this.invoicePaymentService.add(this.model)
            .subscribe(
                data => {
                    this.blockUI.stop();
                    setTimeout(() => {
                        this.router.navigate(['/invoice/payments']);
                    }, 100);
                    setTimeout(() => {
                        this.toastr.success('Payment has been done successfully');
                    }, 500);
                },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }
}
