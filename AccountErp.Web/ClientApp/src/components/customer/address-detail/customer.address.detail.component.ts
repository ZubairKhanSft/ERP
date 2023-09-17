import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { SelectListItemModel, CustomerUpsertModel } from '../../../models';
import { AppUtils } from '../../../helpers';
import { MasterDataService } from '../../../services';

@Component({
    selector: 'app-customer-address-detail',
    templateUrl: './customer.address.detail.component.html'
})

export class CustomerAddressDetailComponent implements OnInit {
    @Input() model: CustomerUpsertModel;
    @Output() moveForward = new EventEmitter();
    @Output() moveBackward = new EventEmitter();
    @BlockUI('conatiner-blockui') blockUI: NgBlockUI;
    wizardStep = 2;
    countries: Array<SelectListItemModel> = new Array<SelectListItemModel>();

    constructor(private toastr: ToastrService,
        private masterDataService: MasterDataService,
        private appUtils: AppUtils) { }

    next() {
        this.moveForward.emit();
    }

    prev() {
        this.moveBackward.emit();
    }

    loadCountries() {
        this.blockUI.start();
        this.masterDataService.getCountries().subscribe(
            (data: any) => {
                this.blockUI.stop();
                Object.assign(this.countries, data);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    ngOnInit() {
        this.loadCountries();
    }
}
