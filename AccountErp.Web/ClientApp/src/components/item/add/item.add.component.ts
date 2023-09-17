import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { AppUtils } from '../../../helpers';
import { ItemAddModel, SelectListItemModel } from '../../../models'
import { ItemService, MasterDataService, SalesTaxService } from '../../../services';
import { ChartOfAccountsService } from 'src/services/chart-of-accounts.service';

@Component({
    selector: 'app-item-add',
    templateUrl: './item.add.component.html'
})

export class ItemAddComponent implements OnInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    model: ItemAddModel = new ItemAddModel();

    itemType: Array<SelectListItemModel> = new Array<SelectListItemModel>();
    salesTaxes: Array<SelectListItemModel> = new Array<SelectListItemModel>();

    incomeAccount;
    expenseAccount;

    incomeaccId=3;
    expenseaccId=4;

    constructor(private router: Router,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private itemService: ItemService,
        private salesTaxSerivce : SalesTaxService,
        private chartofaccService:ChartOfAccountsService,
        private masterDataService: MasterDataService) {
    }

    ngOnInit() {
        this.loadItemType();
        this.loadSalesTax();
        this.loadIncomeAccounts();
        this.loadExpenseAccounts();
    }

    loadItemType() {
        this.blockUI.start();
        this.masterDataService.getItemType()
            .subscribe((data) => {
                this.blockUI.stop();
                
                Object.assign(this.itemType, data);
            },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    loadSalesTax() {
        this.blockUI.start();
        this.salesTaxSerivce.getSelectListItems()
            .subscribe((data) => {
                this.blockUI.stop();
                Object.assign(this.salesTaxes, data);
                console.log("taxlist",this.salesTaxes)
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
        this.itemService.add(this.model).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.router.navigate(['item/manage']);
                }, 100);
                setTimeout(() => {
                    this.toastr.success('Item & service has been added successfully');
                }, 500);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }
}