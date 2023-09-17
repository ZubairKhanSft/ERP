import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

import { AppUtils } from '../../../helpers';
import { InvoiceDetailModel, ItemListItemModel } from '../../../models';
import { InvoiceService, ItemService, SalesTaxService } from '../../../services';
declare let jsPDF;
import autoTable from 'jspdf-autotable';

@Component({
    selector: 'app-invoice-detail',
    templateUrl: './invoice.detail.component.html'
})

export class InvoiceDetailComponent implements OnInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    multiSelectDropdownConfigs: IDropdownSettings;
    model: InvoiceDetailModel = new InvoiceDetailModel();
    items: Array<ItemListItemModel> = new Array<ItemListItemModel>();
    selectedItems: Array<ItemListItemModel> = new Array<ItemListItemModel>();
    salesTaxItems;
  
    itemId: Array<ItemListItemModel> = new Array<ItemListItemModel>();
    selectedTax;
    
    @ViewChild('htmlData', {static: false}) htmlData: ElementRef;
    constructor(private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private invoiceService: InvoiceService,
        private taxService:SalesTaxService,
        private itemService: ItemService) {
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });
    }

    ngOnInit() {
        this.loadItems();
        this.loadTaxes();
        this.loadInvoice();
    }
    public openPDF(): void {
        const DATA = this.htmlData.nativeElement;
        
        const doc = new jsPDF('p', 'pt', 'a4');
        
        doc.fromHTML(DATA.innerHTML, 15, 15);
        doc.output('dataurlnewwindow');

        
      }
    public downloadPDF(): void {
        const DATA = this.htmlData.nativeElement;
        const doc = new jsPDF('p', 'pt', 'a4');
        const handleElement = {
          '#editor': function(element, renderer) {
            return true;
          }
        };
        doc.fromHTML(DATA.innerHTML, 15, 15, {
          'width': 200,
          'elementHandlers': handleElement
        });
        doc.save('Invoice-Detail.pdf');
      }
    loadInvoice() {
        this.blockUI.start();
        this.invoiceService.getDetail(this.model.id).subscribe(
            (data: any) => {
                this.blockUI.stop();
                Object.assign(this.model, data);
                this.model.createdOn = this.appUtils.getFormattedDate(this.model.createdOn, null);
                this.model.invoiceDate = this.appUtils.getFormattedDate(this.model.invoiceDate, null);
                this.model.dueDate = this.appUtils.getFormattedDate(this.model.dueDate, null);

                this.updateSelectedItems();
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    loadItems() {
        this.itemService.getAllActiveOnly()
            .subscribe((data: any) => {
                if (!data || data.length === 0) {
                    return;
                }

                this.items = data;

                this.updateSelectedItems();
            },
                error => {
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    updateSelectedItems() {
     
        if (this.items.length === 0 || this.model.items.length === 0) {
            return;
        }
    
        const tempArray = new Array<ItemListItemModel>();
         const tempTax=[]
       // this.model.totalAmount = 0;
        this.model.items.map((invoiceItem) => {
            const item = this.items.find(x => x.id === invoiceItem.id);
            console.log("itemss",invoiceItem)
            if (item) {
                 item.rate = invoiceItem.rate;
                 item.qty= invoiceItem.quantity;
                 item.rate=invoiceItem.rate;
                 item.price=invoiceItem.price;
                 item.description=invoiceItem.description;
                 item.lineAmount=invoiceItem.lineAmount;
                 
                 tempArray.push(item);
    
                //this.model.totalAmount += invoiceItem.rate;
                //Get item taxes
                debugger;
                if(invoiceItem.taxId!=0){
                    const taxitem=this.salesTaxItems.find(x=> x.id===invoiceItem.taxId);
                    tempTax.push(taxitem);
 
                }else{
                    tempTax.push(null)
                }
            }
    
            
        });

        debugger;
    
        this.selectedItems = tempArray;
       // this.itemId=[];
        this.itemId=tempArray;
        this.selectedTax=tempTax;
        console.log("bindselecteditem",this.itemId);
    }

    loadTaxes(){
        this.taxService.getSelectListItems()
            .subscribe((data: any) => {
                if (!data || data.length === 0) {
                    return;
                }

                this.salesTaxItems = data;

                 this.updateSelectedItems();
            },
                error => {
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    delete(): void {
        if (!confirm('Are you sure you want to delete the selected invoice?')) {
            return;
        }
        this.blockUI.start();
        this.invoiceService.delete(this.model.id).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.router.navigate(['/invoice/manage']);
                }, 100);
                setTimeout(() => {
                    this.toastr.success('Invoice has been deleted successfully.');
                }, 500);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    // print() {
    //     window.open(location.origin + '/print/invoice/' + this.model.id);
    // }
    print() {

       

              
                        debugger
                        var rowData = this.selectedItems;

                        
                        var strSalesInvoiceNo = this.model.invoiceNumber;
                        var chassisNo = "";
                        var vehicle = "";
                        var sellingPrice = "";
                        var PrevPaid = "";
                        var adv = "";
                        var totalBal = "";
                        var custName= "";
                        var invDate = "";


                        var data = [];
                     
                        var balance = 0;
                        var paid = sellingPrice;
                        var totalPaid = parseFloat(PrevPaid)+parseFloat(adv);
                        data.push([invDate, 'SALE PRICE', '', '', sellingPrice]);
                        
                   
                       
                        data.push([, , ]);
                        data.push([, , ]);
                        data.push([, , ]);
                        data.push([, , ]);
                        data.push(['TOTAL', , totalPaid,'', totalBal]);
                        //data.push([, '', 'TOTAL']);
                        //data.push([drmInwords, '', ' TOTAL']);
                        //data.push([, , , , , 'Total', sumOfSellingPrice]);

                        var d = new Date();

                        var month = d.getMonth() + 1;
                        var day = d.getDate();

                        var ReportDate = (('' + day).length < 2 ? '0' : '') + day + '/' + (('' + month).length < 2 ? '0' : '') + month + '/' + d.getFullYear();

                        //first table

                        var columns2 = ['', '', '', ''];
                        var data2 = [];
                        data2.push(['INVOICE NO.', strSalesInvoiceNo, 'CHASIS NO.', chassisNo, ]);
                        data2.push(['DATE', invDate, 'CAR', vehicle]);
                        data2.push(['CUSTOMER', custName, '', '']);
                        //data2.push(['CUSTOMER ID', '203030', '', '']);

                        var columns = ['Date', 'Description', 'DEBIT', 'CREDIT', 'BALANCE'];


                        var doc = new jsPDF();

                        var totalPagesExp = "{total_pages_count_string}";

                        var pageContent = function (data) {
                            // HEADER

                            doc.setFontSize(25);
                            doc.setTextColor(111, 168, 210);
                            doc.setFontStyle('normal');






                            doc.text("INVOICE", data.settings.margin.left + 145, 35);


                            doc.setTextColor(40);
                            doc.setFontSize(10);
                            doc.text("Invoice Nuber:", data.settings.margin.left + 120, 45);
                            doc.text(strSalesInvoiceNo, data.settings.margin.left + 145, 45);

                            //doc.text("Invoice Date:", data.settings.margin.left + 120, 50);
                            //doc.text(invDate, data.settings.margin.left + 145, 50);

                            //doc.text("CONSIGNEE:", data.settings.margin.left + 0, 60);
                            //doc.text("MR."+consignee, data.settings.margin.left + 0,65);

                            //doc.text("From:", data.settings.margin.left + 130, 50);
                            //doc.text(From, data.settings.margin.left + 140, 50);

                            //doc.text("To:", data.settings.margin.left + 130, 60);
                            //doc.text(To, data.settings.margin.left + 140, 60);

                            doc.setFontSize(15);
                            doc.text("DAA MOTORS FZCO", data.settings.margin.left + 0, 40);
                            doc.setFontSize(10);
                            doc.text("Show room # 118, DUCAMZ,RAS AL KHOR, DUBAI, U.A.E.", data.settings.margin.left + 0, 45);
                            doc.text("Tel : +971 4 333 8986", data.settings.margin.left + 0, 50);
                            doc.text("E-Mail : automan1@eim.ae", data.settings.margin.left + 0, 55);
                            //doc.text("Tel : +971 4 333 8986", data.settings.margin.left + 0, 60);
                            //doc.text("Fax : +971 4 333 2256", data.settings.margin.left + 0, 60);
                            //doc.text("E-Mail : automan1@eim.ae", data.settings.margin.left + 0, 65);
                            //doc.setFontSize(20)
                            //doc.setTextColor(0, 0, 255)
                            //doc.text("TOTAL AMOUNT IN WORDS (DHM):", data.settings.margin.left + 0, 170);
                            //doc.setFontStyle('bold');
                            //doc.text(totalInWords+' DHMS Only', data.settings.margin.left+65, 170);
                            doc.setFontSize(10);
                            doc.text("Make all checks payable to DAA MOTORS FZCO", data.settings.margin.left + 50, 250);
                            doc.setFontStyle('bold');
                            doc.text("THANK YOU FOR YOUR BUSINESS!", data.settings.margin.left + 55, 255);

                            // FOOTER
                            var str = "Page " + data.pageCount;
                            // Total page number plugin only available in jspdf v1.0+
                            if (typeof doc.putTotalPages === 'function') {
                                str = str + " of " + totalPagesExp;
                            }
                            doc.setFontSize(10);
                            doc.text(str, data.settings.margin.left, doc.internal.pageSize.height - 10);

                            //doc.text('Note: This is computer generated ', data.settings.margin.left, 20);
                        };

                        window.open(doc.output('bloburl'), '_blank');

                        // doc.autoTable(columns2, data2, {
                        //     theme: 'gridWithNoHeader',

                        //     margin: { top: 60 },


                        // });
                        // doc.autoTable(columns, data, {
                        //     theme: 'grid',
                        //     headerStyles: {
                        //         //columnWidth: 'wrap',
                        //         cellPadding: 2,
                        //         lineWidth: 0,
                        //         valign: 'top',
                        //         fontStyle: 'bold',
                        //         halign: 'left',    //'center' or 'right'
                        //         fillColor: [111, 168, 210],
                        //         //textColor: [78, 53, 73], //Black
                        //         textColor: [255, 255, 255], //White
                        //         fontSize: 8,

                        //         rowHeight: 20
                        //     },
                        //     addPageContent: pageContent,
                        //     margin: { top: 100 },

                        //     drawCell: function (cell, data) {
                        //         var rows = data.table.rows;
                        //         if ((data.row.index == rows.length - 1) || (data.row.index == rows.length - 2)) {
                        //             // doc.setFillColor(200, 200, 255);
                        //             doc.setFillColor(111, 168, 210);
                        //             doc.setTextColor(255, 255, 255);
                        //             doc.setFontStyle('bold');
                        //             //doc.setFontSize(16);
                        //         }
                        //     },
                        // });
                        // //To add Image
                        var img = new Image;
                        img.onload = function () {

                           

                        };

                        img.crossOrigin = "";  // for demo as we are at different origin than image
                      //  img.src = "/Content/Images/invheader2.bmp";  // image

                       
                        var imgfooter = new Image;
                        imgfooter.onload = function () {

                          
                            doc.addImage(this, 13, 260);
                            window.open(doc.output('bloburl'), '_blank');
                            window.location.href = "/Sales/SalesPayment";
                            // doc.output("dataurlnewwindow");
                        };

                        imgfooter.crossOrigin = "";  // for demo as we are at different origin than image
                      //  imgfooter.src = "/Content/Images/invFooter.bmp";  // image


                        // Total page number plugin only available in jspdf v1.0+
                        if (typeof doc.putTotalPages === 'function') {
                            doc.putTotalPages(totalPagesExp);
                        }
                        //doc.output("dataurlnewwindow");

                    
                

    }

 
}
