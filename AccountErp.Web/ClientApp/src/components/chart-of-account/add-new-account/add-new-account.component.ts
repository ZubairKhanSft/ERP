import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { SalesTaxAddModel } from 'src/models';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppUtils } from 'src/helpers';
import { SalesTaxService, BankAccountService } from 'src/services';
import { chartOfAccountsList } from '../chartOfAccountsList';
import { ChartOfAccountsService } from 'src/services/chart-of-accounts.service';
import { customerAccountModel } from 'src/models/chartofaccount/customeraccount';

@Component({
  selector: 'app-add-new-account',
  templateUrl: './add-new-account.component.html',
  styleUrls: ['./add-new-account.component.css']
})
export class AddNewAccountComponent implements OnInit {
  @BlockUI('container-blockui') blockUI: NgBlockUI;
    model: customerAccountModel = new customerAccountModel();
    @Output() closeAddAccountModal = new EventEmitter();
    @Output() reloadAccounts = new EventEmitter();
    
    @Input() accType:number;
    @Input() accId:number;
    acctypeList;
    constructor(private router: Router,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private accountList:chartOfAccountsList,
        private bankAccountService: BankAccountService,
        private chartofaccService: ChartOfAccountsService) {
          
    }

    ngOnInit() {
      debugger;
     
      if(this.accId!== undefined){
        this.model.id=this.accId;
        this.getForEdit();
      }
     if(this.accType!== undefined ){
     this.getAccountTypes();
     }
    }

    getAccountTypes(){
      debugger;
    
        this.chartofaccService.getAccountsByMasterType(this.accType).subscribe(
            (data: any) => {
                this.acctypeList=[];
                Object.assign(this.acctypeList, data);
                console.log("assets",this.acctypeList)
               
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    submit() {
      debugger;
        this.blockUI.start();
        if(this.model.id===undefined){
          this.chartofaccService.add(this.model).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                  this.closeAddAccountModal.emit();
                }, 100);
                setTimeout(() => {
                    this.toastr.success('Account  has been added successfully');
                    
                }, 500);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
        }else{
          this.chartofaccService.edit(this.model).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                  this.closeAddAccountModal.emit();
                }, 100);
                setTimeout(() => {
                  debugger;
                  this.reloadAccounts.emit();
                    this.toastr.success('Account  has been updated successfully');
                   
                }, 500);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
                
            });
        }
       
    }

    getForEdit() {
     
     
      this.chartofaccService.getForEdit(this.model.id).subscribe(
          (data: any) => {
            debugger;
              Object.assign(this.model, data);
              console.log("accmodel",this.model)
          },
          error => {
              this.blockUI.stop();
              this.appUtils.ProcessErrorResponse(this.toastr, error);
          });
    }

    
}
