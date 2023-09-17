import { Component, OnInit } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { SelectListItemModel, BillPaymentModel, VendorPaymentInfoModel, ExpenseSummaryModel } from 'src/models';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppUtils } from 'src/helpers';
import { BankAccountService,BillService, VendorService, BillPaymentService, CreditCardService } from 'src/services';
import { ChartOfAccountsService } from 'src/services/chart-of-accounts.service';
import { AccountTransactionsService } from 'src/services/account-transactions.service';

@Component({
  selector: 'app-add-vendor-payment',
  templateUrl: './add-vendor-payment.component.html',
  styleUrls: ['./add-vendor-payment.component.css']
})
export class AddVendorPaymentComponent implements OnInit {
  @BlockUI('container-blockui') blockUI: NgBlockUI;
  model: BillPaymentModel = new BillPaymentModel();
  paymentModes: Array<SelectListItemModel> = new Array<SelectListItemModel>();
  bankAccounts: Array<SelectListItemModel> = new Array<SelectListItemModel>();
  creditCards: Array<SelectListItemModel> = new Array<SelectListItemModel>();
  depositToAccounts: Array<SelectListItemModel> = new Array<SelectListItemModel>();
  vendorPaymentInfoModel: VendorPaymentInfoModel = new VendorPaymentInfoModel();
  expenseSummaryModel: ExpenseSummaryModel = new ExpenseSummaryModel();
  vendors=[];
  bills=[];
  selectedVendor;
  allAccounts;
  vendor:any={};
  config = {displayKey:"value",search:true,limitTo:10,height: 'auto',placeholder:'Select Vendor',customComparator: ()=>{},moreText: 'more',noResultsFound: 'No results found!',searchPlaceholder:'Search',searchOnKey: 'value',clearOnSelection: false,inputDirection: 'ltr',}

  constructor(private router: Router,
      private route: ActivatedRoute,
      private toastr: ToastrService,
      private appUtils: AppUtils,
      private billService: BillService,
      private bankAccountService: BankAccountService,
      private vendorService: VendorService,
      private billPaymentService: BillPaymentService,
      private chartofaccService:ChartOfAccountsService,
      private accountTransactionService:AccountTransactionsService,
      private creditCardService: CreditCardService) {
      this.route.params.subscribe((params) => {
        
          debugger;
          this.model.billId = params['id'];
          this.model.paymentType=2
          this.loadExpenseSummary();
          this.loadAccounts();

      });
  }

  ngOnInit() {
      this.paymentModes = this.appUtils.getPaymentModesSelectList();
      this.loadBankAccounts();
      this.loadCreditCards();
      this.loadExpenseSummary();
      this.model.paymentDate = this.appUtils.getDateForNgDatePicker(null);
      this.loadVendors();
      this.loadUnpaidBills();
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

  getVendorDetail(){
    debugger;
    if(this.selectedVendor!=undefined){
        this.model.vendorId=this.selectedVendor.keyInt;
        this.expenseSummaryModel.vendorId=this.selectedVendor.keyInt;
    
    if (this.model.vendorId === null
        || this.model.vendorId === '') {
      
        return;
    }

    this.vendorService.getDetail(Number(this.model.vendorId))
        .subscribe(
            (data) => {
                Object.assign(this.vendor, data);
                this.loadVendorPaymentInfo();
                console.log("vendor",this.vendor)
               
            });
        }
  }

  loadUnpaidBills(){
        
    this.billPaymentService.getUnpaidBills()
    .subscribe(
        data => {
            Object.assign(this.bills, data);
            console.log("bill",this.bills)
        },
        error => {
            this.appUtils.ProcessErrorResponse(this.toastr, error);
        });
}



  chengeBill(){
    debugger;
    if(this.model.billId!=undefined){
      
    
    if (this.model.billId === null
        || this.model.billId === 0) {
      
        return;
    }
    this.loadExpenseSummary();
    this.blockUI.start();
    this.billService.getDetail(this.model.billId).subscribe(
        (data: any) => {
            this.blockUI.stop();
            Object.assign(this.expenseSummaryModel, data);
            this.model.amount=this.expenseSummaryModel.totalAmount;
          
        },
        error => {
            this.blockUI.stop();
            this.appUtils.ProcessErrorResponse(this.toastr, error);
        });
    }
  }

  loadCreditCards() {
      this.creditCardService.getSelectItems()
          .subscribe(
              data => {
                  Object.assign(this.creditCards, data);
              },
              error => {
                  this.appUtils.ProcessErrorResponse(this.toastr, error);
              });
  }

  loadExpenseSummary() {
      this.blockUI.start();
      this.billService.getSummary(this.model.billId)
          .subscribe(
              (data) => {
                  this.blockUI.stop();
                  Object.assign(this.expenseSummaryModel, data);
                  setTimeout(() => {
                      this.loadVendorPaymentInfo();
                  }, 100);
              },
              error => {
                  this.blockUI.stop();
                  this.appUtils.ProcessErrorResponse(this.toastr, error);
              });
  }

  loadVendorPaymentInfo() {
     
      this.blockUI.start();
      this.vendorService.getPaymentInfo(this.expenseSummaryModel.vendorId)
          .subscribe(
              data => {
                  this.blockUI.stop();
                  debugger;
                  Object.assign(this.vendorPaymentInfoModel, data);

                  if (this.vendorPaymentInfoModel.accountNumber != null) {
                      const selectListItem = new SelectListItemModel();
                      selectListItem.keyString = this.vendorPaymentInfoModel.accountNumber;
                      selectListItem.value = this.vendorPaymentInfoModel.accountNumber;
                      this.depositToAccounts.push(selectListItem);
                  }

              },
              error => {
                  this.blockUI.stop();
                  this.appUtils.ProcessErrorResponse(this.toastr, error);
              });
  }

 

  loadVendors() {
    this.blockUI.start();
    this.vendorService.getSelectItems()
        .subscribe((data) => {
            this.blockUI.stop();
            this.vendors=[];
            Object.assign(this.vendors, data);
            console.log("vendr",this.vendors);
        },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
}


  submit() {
      debugger;
      this.blockUI.start();
      if (this.model.paymentDate) {
          this.model.paymentDate = this.appUtils.getFormattedDate(this.model.paymentDate, null);
      }
      this.billPaymentService.add(this.model)
          .subscribe(
              data => {
                  this.blockUI.stop();
                  setTimeout(() => {
                      this.router.navigate(['/bill/payments']);
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
