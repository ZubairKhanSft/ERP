import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AppUtils, AppSettings } from 'src/helpers';
import { NgBlockUI, BlockUI } from 'ng-block-ui';
import * as jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { NgForm } from '@angular/forms';
import { TrialBalanceService } from 'src/services/trial.balance.service';
import { TrialBalanceDetail } from 'src/models/trialBalance/trial.balance.model';

@Component({
  selector: 'app-trial-balance',
  templateUrl: './trial-balance.component.html',
  styleUrls: ['./trial-balance.component.css']
})
export class TrialBalanceComponent implements OnInit {

  @ViewChild ('terms', {static: false}) terms: ElementRef ;
  @ViewChild('htmlData', {static: false}) htmlData: ElementRef;
  @BlockUI('container-blockui') blockUI: NgBlockUI;

  trialBalanceData={totalAmount:0,totalUnpaidAmount:0,totalNotYetOverDue:0,totalCountNotYetOverDue:0,
    totalLessThan30:0,totalCountLessThan30 :0,totalCountThirtyFirstToSixty:0,
    totalThirtyFirstToSixty:0,totalSixtyOneToNinety:0,totalCountSixtyOneToNinety:0,
    totalMoreThanNinety:0,totalCountMoreThanNinety:0,agedPayablesReportDtoList:[{}]};
  model: TrialBalanceDetail = new TrialBalanceDetail();
  selectedstType = 0;
  startDate;
  endDate;
  fromDate;
  toDate;
  asOfDate;
  statement;
  today = new Date();
  allStartingBalance;
  allCreditBalance;
  allDebitBalance;
  allNetMovementBalance;
  allEndingBalance;

  allStartingBalanceLiabilites;
  allDebitLiabilites;
  allCreditLiabilites;
  allNetMovementLiabilites;
  allEndingLiabilites;

  allStartingBalanceEquity;
  allDebitEquity;
  allCreditEquity;
  allNetMovementEquity;
  allEndingEquity;

  allDebitIncome;
  allCreditIncome;
  allNetMovementIncome;

   allDebitExpense;
   allCreditExpense;
   allNetMovementExpense;
   allDebitExpTotal;
   allCreditExpTotal;

  temp;

  assetAccDetails;
  liabilitiesAccDetails;

  constructor(private appSettings: AppSettings,
    private trialBalanceService: TrialBalanceService,
    private toastr: ToastrService,
        private appUtils: AppUtils) { }

  ngOnInit() {

    this.setDefaultDate();
    this.showTrialBalance();

    this.assetAccDetails=[{"id":1,"accName":"Account Receivable","debit":100,"credit":50},{"id":2,"accName":"Cash on Hand","debit":10,"credit":20}]
    this.liabilitiesAccDetails=[{"id":1,"accName":"GST","debit":0.00,"credit":50}];
  }

  showTrialBalance() {
    // this.purchaseVendortData = {vendorReportsList={}};
    console.log("from",this.fromDate);
    console.log("from",this.toDate);
    // if (this.selectedVendor !== undefined) {
     debugger;
     var body={ 
     
    
     "asOfDate": this.model.asOfDate,
      "reportType": this.selectedstType};
     
 
     this.trialBalanceService.getDetail(body)
     .subscribe(
         (data) => {
           debugger
           this.statement=[];
           Object.assign(this.statement,data);
           console.log(this.statement);
    
          //  this.CalculateTotalPurchase();
         });
      //  }
   }
  public openPDF(): void {
    const doc = new jsPDF('p', 'pt', 'a4');
    doc.setFontSize(15);
    doc.text('Statement of Account', 400, 40);
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
  debugger;
    const doc = new jsPDF('p', 'pt', 'a4');
    doc.setFontSize(15);
    doc.text('Statement of Account', 400, 40);
    doc.text('Outstanding Invoices', 400, 70);
   autoTable(doc, {
    html: '#my-table',
    styles: {
 },
 tableLineWidth: 0.5,
 startY: 550,
 tableLineColor: [4, 6, 7], // choose RGB
   });
    autoTable(doc, {
      html: '#my-table',
      styles: {
   },
   tableLineWidth: 0.5,
   startY: 300,
   tableLineColor: [4, 6, 7], // choose RGB
     });
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
   changeAsOfdate(){
    debugger;
    console.log('quotatindate', this.asOfDate);
    const jsDate = new Date(this.asOfDate.year, this.asOfDate.month - 1, this.asOfDate.day);
    // this.asOfDate = jsDate.toISOString();
    this.model.asOfDate=jsDate.toISOString();

   }

   setDefaultDate(){
        
    var qdt=new Date()
    this.asOfDate={ day: qdt.getDate(), month: qdt.getMonth()+1, year: qdt.getFullYear()};
    const jsbillDate = new Date(this.asOfDate.year, this.asOfDate.month - 1, this.asOfDate.day);
    this.model.asOfDate=jsbillDate.toISOString();
  }
}
