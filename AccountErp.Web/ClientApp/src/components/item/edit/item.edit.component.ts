import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { AppUtils } from '../../../helpers';
import { ItemEditModel, SelectListItemModel } from '../../../models';
import { ItemService, MasterDataService, SalesTaxService } from '../../../services';
import { ChartOfAccountsService } from 'src/services/chart-of-accounts.service';

@Component({
    selector: 'app-item-edit',
    templateUrl: './item.edit.component.html'
})

export class ItemEditComponent implements OnInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    model: ItemEditModel = new ItemEditModel();
    itemType: Array<SelectListItemModel> = new Array<SelectListItemModel>();
    salesTaxes: Array<SelectListItemModel> = new Array<SelectListItemModel>();

    incomeAccount;
    expenseAccount;

    incomeaccId=3;
    expenseaccId=4;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private itemService: ItemService,
        private salesTaxSerivce: SalesTaxService,
        private chartofaccService:ChartOfAccountsService,
        private masterDataService: MasterDataService) {
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });
    }

    ngOnInit() {
        this.loadIncomeAccounts();
        this.loadExpenseAccounts();
        this.loadItemType();
        this.loadSalesTax();
      
        this.loadItem();
    }

    loadItemType() {
        this.masterDataService.getItemType()
            .subscribe((data) => {
                Object.assign(this.itemType, data);
            },
                error => {
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    loadSalesTax() {
        this.blockUI.start();
        this.salesTaxSerivce.getSelectListItems()
            .subscribe((data) => {
                this.blockUI.stop();
                Object.assign(this.salesTaxes, data);
            },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    loadItem() {
        this.blockUI.start();
        this.itemService.getForEdit(this.model.id).subscribe(
            (data: any) => {
                this.blockUI.stop();
                Object.assign(this.model, data);
                console.log("itemedit",this.model)
                if (!this.model.salesTaxId) {
                    this.model.salesTaxId = '';
                }
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    loadIncomeAccounts(){
      
        debugger;
      
          this.chartofaccService.getAccountsByMasterType(this.incomeaccId).subscribe(
              (data: any) => {
                  this.incomeAccount=[];
                  var tempacc=[];
                  var incacclist=[];
                  Object.assign(tempacc, data);
                 
                  tempacc.map(function (item) {
                      if(item.bankAccount.length!==0){
                        item.bankAccount.map(function (acc) {
                              incacclist.push(acc)
                         
                        });
                        
                      }
                    
                   
                  });
                  this.incomeAccount=incacclist;
                  console.log("icmacc",this.incomeAccount)
              },
              error => {
                  this.blockUI.stop();
                  this.appUtils.ProcessErrorResponse(this.toastr, error);
              });
      
}

loadExpenseAccounts(){
   
        debugger;
      
          this.chartofaccService.getAccountsByMasterType(this.expenseaccId).subscribe(
              (data: any) => {
                  this.expenseAccount=[];
                  var tempacc=[];
                  var incacclist=[];
                  Object.assign(tempacc, data);
                 
                  tempacc.map(function (item) {
                      if(item.bankAccount.length!==0){
                        item.bankAccount.map(function (acc) {
                              incacclist.push(acc)
                         
                        });
                        
                      }
                    
                   
                  });
                  this.expenseAccount=incacclist;
                  console.log("icmacc",this.expenseAccount)
                 
              },
              error => {
                  this.blockUI.stop();
                  this.appUtils.ProcessErrorResponse(this.toastr, error);
              });
      
}

    submit() {
        debugger;
        this.blockUI.start();
        if(this.model.rate==undefined || this.model.rate==Number("")){
            this.model.rate=0;
        }
        this.itemService.edit(this.model).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.router.navigate(['/item/manage']);
                }, 100);
                setTimeout(() => {
                    this.toastr.success('Item/service has been updated successfully');
                }, 500);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }
}

