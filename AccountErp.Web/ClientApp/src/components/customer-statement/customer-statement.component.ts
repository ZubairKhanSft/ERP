import { BillService } from './../../services/bill.service';
import { Component, OnInit, ViewChild, Input, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomerService, BankAccountService, VendorService, BillPaymentService, CreditCardService } from 'src/services';
import { AppUtils, AppSettings } from 'src/helpers';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DataTableDirective } from 'angular-datatables';
import { quotationAddModel } from 'src/models/quotation/quotation.add.model';
import { CustomerDetailModel, SelectListItemModel, InvoiceFilterModel, AttachmentEditModel, InvoiceEditModel
       , InvoiceDetailModel, 
       ItemListItemModel} from 'src/models';
import { CustomerStatementDetail } from 'src/models/customerStatement/customer.statement.detail.model';
import { CustomerStatementService } from 'src/services/customer-statement.service';
import * as jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-customer-statement',
  templateUrl: './customer-statement.component.html',
  styleUrls: ['./customer-statement.component.css']
})
export class CustomerStatementComponent implements OnInit {
  [x: string]: any;
  @BlockUI('container-blockui-main') blockUI: NgBlockUI;
  @ViewChild(DataTableDirective, { static: false })
  @Input() selectedItems: Array<ItemListItemModel> = new Array<ItemListItemModel>();
  @ViewChild('htmlData', {static: false}) htmlData: ElementRef;
  @ViewChild ('terms', {static: false}) terms: ElementRef ;
  datatableElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtInstance: DataTables.Api;
  rowIndex = 0;
  pageLength = 10;
  search: any = null;
  selectedCustomer;
  selectedStatement;
  firstName;
  // model: quotationAddModel = new quotationAddModel();
  // model: InvoiceEditModel = new InvoiceEditModel();
   model1: InvoiceDetailModel = new InvoiceDetailModel();
   model: CustomerStatementDetail = new CustomerStatementDetail();
  customer: CustomerDetailModel = new CustomerDetailModel();
  config = {displayKey: 'value', search: true, height: 'auto', placeholder:'Select Item',
  customComparator: ()=>{},moreText: 'more',noResultsFound: 'No results found!', searchPlaceholder:'Search',
  searchOnKey: 'value',clearOnSelection: false,inputDirection: 'ltr', }
  filterModel: InvoiceFilterModel = new InvoiceFilterModel();
  customers: any;
  isShow = true;
  optionValue;
  selectedstType = 'outstanding';
  startDate;
  endDate;
  fromDate;
  toDate;
  statementData;
  today = new Date();
  overDueAmount = 0;
  totalDueAmount = 0;
  OutstandingAmont = 0;
  outStandingBalance = 0;
  totalAcc: number;
  modalReference: any;
  showMyContainer : boolean =  true;
  loggedOut;
  totalAccBalance;
  tempBalance=0;
  temp;
  statementTypes: any = ['Outstanding Invoices', 'Account Activity'];
//   statementTypes: any = [
//     {id: 1, name:'Outstanding Invoiceserman'},
//     {id: 2, name:'Account Activity'},
// ];

  public text1 = 'Create Statement';
  billPaymentService: any;
  invoiceService: any;
  toggleDisplay() {
    this.isShow = !this.isShow;
  }
  openPop() {
    // this.showMyContainer = !this.showMyContainer;
    this.showMyContainer = true;

  }
  exit() {
   window.location.reload();
  //  this.ngOnInit();
  }
  public changeText(): void {
    if (this.text1 === 'Create Statement') {
      this.text1 = 'refresh';
    } else {
      this.text1 = 'Create Statement';
    }
  }
  constructor(private http: HttpClient,
    private router: Router,
    private toastr: ToastrService,
    private customerService: CustomerService,
    private appUtils: AppUtils,
    private appSettings: AppSettings,
    private billService: BillService,
    private bankAccountService: BankAccountService,
    private vendorService: VendorService,
    private customerStatementService: CustomerStatementService,
    private creditCardService: CreditCardService,
    private modalService: NgbModal,
    ) { }

  ngOnInit() {
    this.loadCustomers();
  }
  loadCustomers() {
    this.blockUI.start();
    this.customerService.getSelectItems()
        .subscribe((data) => {
            this.blockUI.stop();
            this.customers=[];
            Object.assign(this.customers, data);
        },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
}
showcustomerStatement() {
//   debugger;
//   this.statementData={
//     "id": 0,
//     "customerId": 0,
//     "startDate": "2020-06-19T08:58:27.560Z",
//     "endDate": "2020-06-19T08:58:27.560Z",
//     "status": 0,
//     "address": {
//       "id": 0,
//       "countryId": 0,
//       "countryName": "string",
//       "streetNumber": "string",
//       "streetName": "string",
//       "city": "string",
//       "state": "string",
//       "postalCode": "string"
//     },
//     "customer": {
//       "id": 0,
//       "firstName": "string",
//       "middleName": "string",
//       "lastName": "string",
//       "email": "string",
//       "phone": "string",
//       "accountNumber": "string",
//       "bankName": "string",
//       "bankBranch": "string",
//       "ifsc": "string",
//       "discount": 0,
//       "address": {
//         "id": 0,
//         "countryId": 0,
//         "countryName": "string",
//         "streetNumber": "string",
//         "streetName": "string",
//         "city": "string",
//         "state": "string",
//         "postalCode": "string"
//       },
//       "addressId": 0,
//       "shippingAddress": {
//         "id": 0,
//         "countryId": 0,
//         "countryName": "string",
//         "addressLine1": "string",
//         "addressLine2": "string",
//         "city": "string",
//         "state": "string",
//         "postalCode": "string",
//         "shipTo": "string",
//         "deliveryInstruction": "string"
//       },
//       "shippingAddressId": 0
//     },
//     "shippingAddress": {
//       "id": 0,
//       "countryId": 0,
//       "countryName": "string",
//       "addressLine1": "string",
//       "addressLine2": "string",
//       "city": "string",
//       "state": "string",
//       "postalCode": "string",
//       "shipTo": "string",
//       "deliveryInstruction": "string"
//     },
//     "invoiceList": [
//       {
//         "id": 1,
//         "customerId": 1,
//         "customerName": "string",
//         "invoiceNumber": "INV-20-001",
//         "description": "string",
//         "amount": 1000,
//         "discount": 0,
//         "tax": 0,
//         "invoiceDate": "2020-06-19T08:58:27.560Z",
//         "dueDate": "2020-06-19T08:58:27.560Z",
//         "poSoNumber": 0,
//         "strInvoiceDate": "string",
//         "strDueDate": "string",
//         "totalAmount": 1000,
//         "paid":0,
//         "balance":1000,
//         "createdOn": "2020-06-19T08:58:27.560Z",
//         "status": 3
//       },
//       {
//         "id": 2,
//         "customerId": 1,
//         "customerName": "string",
//         "invoiceNumber": "INV-2--002",
//         "description": "string",
//         "amount": 200,
//         "discount": 0,
//         "tax": 0,
//         "invoiceDate": "2020-06-19T08:58:27.560Z",
//         "dueDate": "2020-07-19T08:58:27.560Z",
//         "poSoNumber": 0,
//         "strInvoiceDate": "string",
//         "strDueDate": "string",
//         "totalAmount": 200,
//         "paid":100,
//         "balance":100,
//         "createdOn": "2020-06-19T08:58:27.560Z",
//         "status": 1

//       }
//     ],
//     "createdOn": "2020-06-19T08:58:27.560Z"
//   }

//  this.showcustomerStatement();
  debugger;
  this.statementData={};
  this.temp={};
  var body={};
  if (this.selectedCustomer !== undefined) {
   if(this.selectedstType=='outstanding'){
    body={
  "id": 0,
  "customerId": this.selectedCustomer.keyInt,
  "status": 0,
 }
   }
   else{
    body={
      "id": 0,
  "customerId": this.selectedCustomer.keyInt,
  "startDate": this.fromDate,
  "endDate": this.toDate,
  "status": 1,
 }
   }
   
    this.customerStatementService.getCustomerStatement(body)
    .subscribe(
        (data) => {
          console.log("statement",data)
        
          Object.assign(this.temp, data);
             
             //Object.assign(this.statementData, data);
             this.temp.invoiceList.map((item) => {
              debugger;
              item.paidAmount=item.totalAmount;
              if(item.status==1){
               item.balanceAccAmount=0.00
              }else{
                this.totalAccBalance=0;
                this.tempBalance+=Number(item.totalAmount);
                var balAmnt=Number(this.temp.openingBalance)+this.tempBalance;
                this.totalAccBalance=balAmnt.toFixed(2);
                item.balanceAccAmount=balAmnt.toFixed(2);
               
              }
           });
         
           this.statementData=this.temp;
             this.CalculateAmount();
            // this.model.phone = this.customer.phone;
            // this.model.email = this.customer.email;

            // if (!this.customer.discount) {
            //     this.customer.discount = 0;
            // }

           // this.updateTotalAmount();
        });
    }
  
}
openItemesModal(content: any) {
  this.modalReference = this.modalService.open(content,
      {
          backdrop: 'static',
          keyboard: false,
          size: 'lg'
      });
}

getPaidAmount(item){
 
  if(item.status==1){
    return item.totalAmount.toFixed(2);
  }else{
    return 0.00;
   
  }
}

getBalanceAmount(item){
  debugger;
  if(item.status==1){
    return 0.00;
  }else{
    return Number(item.totalAmount).toFixed(2);
   
  }
}

getBalanceAccAmount(item){
 // debugger;
  if(item.status==1){
    return 0.00;
  }else{
    this.totalAccBalance=0;
    this.tempBalance+=Number(item.totalAmount);
    var balAmnt=Number(this.statementData.openingBalance)+this.tempBalance;
    this.totalAccBalance=balAmnt.toFixed(2);
    return balAmnt.toFixed(2);
   
  }
}

getClosingBalanceAmount(){
  return this.totalAccBalance;
}

closeItemesModal() {
  this.updateTotalAmount();
  this.modalReference.close();
}
   public openPDF(): void {
        const doc = new jsPDF('p', 'pt', 'a4');
        doc.setFontSize(15);
        doc.text('Statement of Account', 400, 40);
        // doc.text('Outstanding Invoices', 400, 70);
        autoTable(doc, {
           html: '#my-table',
           styles: {
            // cellPadding: 0.5,
           // fontSize: 12,
        },
        tableLineWidth: 0.5,
        startY: 400, /* if start position is fixed from top */
        tableLineColor: [4, 6, 7], // choose RGB
          });
          const DATA = this.htmlData.nativeElement;
        doc.fromHTML(DATA.innerHTML, 30, 15);
        doc.output('dataurlnewwindow');
      }


    public downloadPDF(): void {
        const doc = new jsPDF('p', 'pt', 'a4');
        doc.setFontSize(15);
        doc.text('Statement of Account', 400, 40);
        doc.text('Outstanding Invoices', 400, 70);
       // autoTable(doc, { html: '#my-table' });
       autoTable(doc, {
        html: '#my-table',
        styles: {
     },
     tableLineWidth: 0.5,
     startY: 550,
     tableLineColor: [4, 6, 7], // choose RGB
       });
        autoTable(doc, {
          html: '#my-table1',
          styles: {
       },
       tableLineWidth: 0.5,
       startY: 300,
       tableLineColor: [4, 6, 7], // choose RGB
         });
        // const handleElement = {
        //   '#editor': function(element, renderer) {
        //     return true;
        //   }
        // };
        // doc.fromHTML(DATA.innerHTML, 15, 15, {
        //   'width': 200,
        //   'elementHandlers': handleElement
        // });
        const DATA = this.htmlData.nativeElement;
        doc.save('Customer-statement.pdf');
      }

      onSubmit(form: NgForm) {
        // console.log(this.terms);
        //  console.log(this.terms.nativeElement.checked);
    }

changeStartDate(){
  debugger;

  console.log('quotatindate', this.startDate);
  const jsDate = new Date(this.startDate.year, this.startDate.month - 1, this.startDate.day);
  this.fromDate = jsDate.toISOString();
 }

 changeEnddate(){
  debugger;
  console.log('quotatindate', this.endDate);
  const jsDate = new Date(this.endDate.year, this.endDate.month - 1, this.endDate.day);
  this.toDate = jsDate.toISOString();
 }
  getCustomerDetail() {
   // debugger;
  // alert(this.selectedCustomer.keyInt);
    if (this.selectedCustomer !== undefined) {
        this.model.customerId = this.selectedCustomer.keyInt;
    if (this.model.customerId === null
        || this.model.customerId === '') {
        this.model.phone = '';
        this.model.email = '';
        // this.model.quotationNumber = '';
        this.model.discount = 0;
        return;
    }
    this.customerService.getDetail(Number(this.model.customerId))
        .subscribe(
            (data) => {
                Object.assign(this.customer, data);
                this.model.phone = this.customer.phone;
                this.model.email = this.customer.email;

                if (!this.customer.discount) {
                    this.customer.discount = 0;
                }

               // this.updateTotalAmount();
            });
        }
}

CalculateAmount() {
  debugger;
this.totalDueAmount = 0;
this.overDueAmount = 0;
this.outStandingBalance = 0;
  this.statementData.invoiceList.map((item) => {
   if (item.status === 3) {
     this.overDueAmount += item.totalAmount;
     this.outStandingBalance += item.totalAmount;
   }
   if (item.status === 0) {
     this.totalDueAmount += item.totalAmount;
     this.outStandingBalance += item.totalAmount;
   }
});
}
// loadInvoice() {
//   this.blockUI.start();
//   this.invoiceService.getForEdit(this.model.id).subscribe(
//       (data: any) => {
//           this.blockUI.stop();
//           Object.assign(this.model, data);

//           const qdt = new Date(this.model.invoiceDate)
//           this.invDate = { day: qdt.getDate(), month: qdt.getMonth()+1, year: qdt.getFullYear()};

//           const expdt = new Date(this.model.dueDate);
//           this.dueDate={ day: expdt.getDate(), month: expdt.getMonth()+1, year: expdt.getFullYear()};
          

//           if (!this.model.attachments || this.model.attachments.length === 0) {
//               const attachmentFile = new AttachmentEditModel();
//               this.model.attachments.push(attachmentFile);
//           }

//           this.getCustomerDetail();
//           this.updateSelectedItems();
//           this.updateTotalAmount();
//       },
//       error => {
//           this.blockUI.stop();
//           this.appUtils.ProcessErrorResponse(this.toastr, error);
//       });
// }
  updateSelectedItems() {
    throw new Error("Method not implemented.");
  }
// submit() {
//   this.blockUI.start();
//   if (this.model.paymentDate) {
//       this.model.paymentDate = this.appUtils.getFormattedDate(this.model.paymentDate, null);
//   }
//   this.billPaymentService.add(this.model)
//       .subscribe(
//           data => {
//               this.blockUI.stop();
//               setTimeout(() => {
//                   this.router.navigate(['/bill/payments']);
//               }, 100);
//               setTimeout(() => {
//                   this.toastr.success('Payment has been done successfully');
//               }, 500);
//           },
//           error => {
//               this.blockUI.stop();
//               this.appUtils.ProcessErrorResponse(this.toastr, error);
//           });
// }
}
