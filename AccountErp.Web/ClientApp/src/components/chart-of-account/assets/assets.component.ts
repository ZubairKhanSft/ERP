import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { ChartOfAccountsService } from 'src/services/chart-of-accounts.service';
import { AppUtils } from '../../../helpers';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-assets',
  templateUrl: './assets.component.html',
  styleUrls: ['./assets.component.css']
})
export class AssetsComponent implements OnInit {
  @Output() openAddAccountModal = new EventEmitter();
  @Output() closeAddAccountModal = new EventEmitter();
  @BlockUI('container-blockui') blockUI: NgBlockUI;
  accType=1;
  accountList;
  constructor(
    private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private appUtils: AppUtils,
    private chartofaccService:ChartOfAccountsService) { }

  ngOnInit() {
   
    this.getAssetAccounts();
}

getAssetAccounts(){
  debugger;

    this.chartofaccService.getAccountsByMasterType(this.accType).subscribe(
        (data: any) => {
            this.accountList=[];
            Object.assign(this.accountList, data);
            console.log("assets",this.accountList)
           
        },
        error => {
            this.blockUI.stop();
            this.appUtils.ProcessErrorResponse(this.toastr, error);
        });
}

}
