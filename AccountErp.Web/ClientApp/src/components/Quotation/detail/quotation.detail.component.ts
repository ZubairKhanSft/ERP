import { Component, OnInit, Output, ViewChild, ElementRef } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { InvoiceDetailModel, ItemListItemModel } from 'src/models';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppUtils } from 'src/helpers';
import { ItemService, SalesTaxService } from 'src/services';
import { QuotationService } from 'src/services/quotation.service.service';
import { QuotationDetailModel } from 'src/models/quotation/QuotationDetailModel';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as jsPDF from 'jspdf';

@Component({
  selector: 'app-quotation.detail',
  templateUrl: './quotation.detail.component.html',
  styleUrls: ['./quotation.detail.component.css']
})
export class QuotationDetailComponent implements OnInit {
  @BlockUI('container-blockui') blockUI: NgBlockUI;
  modalReference: any;
    multiSelectDropdownConfigs: IDropdownSettings;
    model: QuotationDetailModel = new QuotationDetailModel();
    items: Array<ItemListItemModel> = new Array<ItemListItemModel>();
    salesTaxItems;
    selectedItems: Array<ItemListItemModel> = new Array<ItemListItemModel>();
    itemId: Array<ItemListItemModel> = new Array<ItemListItemModel>();
    selectedTax: any=[];
    quotationDate;
    expiryDate;
    @ViewChild('htmlData', {static: false}) htmlData: ElementRef;
    constructor(private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private modalService: NgbModal,
        private appUtils: AppUtils,
        private QuotationService: QuotationService,
        private taxService:SalesTaxService,
        private itemService: ItemService) {
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });
    }

    ngOnInit() {
      
        this.loadTaxes();
        this.loadItems();
        this.loadInvoice();
       
    }

    loadInvoice() {
        this.blockUI.start();
        this.QuotationService.getDetail(this.model.id).subscribe(
            (data: any) => {
                this.blockUI.stop();
                Object.assign(this.model, data);
                this.model.createdOn = this.appUtils.getFormattedDate(this.model.createdOn, null);
                this.model.quotationDate=this.appUtils.getFormattedDate(this.model.quotationDate, null);
                this.model.expiryDate=this.appUtils.getFormattedDate(this.model.expiryDate, null);


                this.updateSelectedItems();
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

  

    confirmDelete(content: any){
        this.modalReference = this.modalService.open(content,
          {
              backdrop: 'static',
              keyboard: false,
              size: 'lg',
              
          });
      }
    
      closeConfirmatioModal() {
        //this.updateTotalAmount();
        this.modalReference.close();
    }

    loadItems() {
        this.itemService.getAllActiveOnly()
            .subscribe((data: any) => {
                if (!data || data.length === 0) {
                    return;
                }

                this.items = data;
                this.loadTaxes();
                this.updateSelectedItems();
            },
                error => {
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
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

    updateSelectedItems() {
     
        if (this.items.length === 0 || this.model.items.length === 0) {
            return;
        }

        const tempArray = new Array<ItemListItemModel>();
         const tempTax=[]
        this.model.amount = 0;
        this.model.items.map((invoiceItem) => {
            const item = this.items.find(x => x.id === invoiceItem.id);
            console.log("itemss",invoiceItem)
            if (item) {
                 item.rate = invoiceItem.rate;
                 item.qty= invoiceItem.quantity;
                 item.rate=invoiceItem.rate;
                 item.price=invoiceItem.price;
                 item.description=invoiceItem.description;
                 
                 tempArray.push(item);

                this.model.amount += invoiceItem.rate;
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

        this.selectedItems = tempArray;
       // this.itemId=[];
        this.itemId=tempArray;
        this.selectedTax=[];
        this.selectedTax=tempTax;


        console.log("taxngmodel",this.selectedTax);
    }


    delete(): void {
        if (!confirm('Are you sure you want to delete the selected invoice?')) {
            return;
        }
        this.blockUI.start();
        this.QuotationService.delete(this.model.id).subscribe(
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

    print() {
       // window.open(location.origin + '/print/invoice/' + this.model.id);
       const DATA = this.htmlData.nativeElement;
       const doc = new jsPDF('p', 'pt', 'a4');
       doc.fromHTML(DATA.innerHTML, 15, 15);
       doc.output('dataurlnewwindow');
    }
}
