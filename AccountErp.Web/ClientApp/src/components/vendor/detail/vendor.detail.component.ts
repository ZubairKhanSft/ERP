import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { AppUtils } from '../../../helpers';
import { VendorDetailModel } from '../../../models';
import { VendorService } from '../../../services';

@Component({
    selector: 'app-vendor-detail',
    templateUrl: './vendor.detail.component.html'
})

export class VendorDetailComponent implements OnInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    model: VendorDetailModel = new VendorDetailModel();
    isModelLoaded = false;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private vendorService: VendorService) {
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });
    }

    ngOnInit() {
        this.blockUI.start();
        this.vendorService.getDetail(this.model.id).subscribe(
            (data: any) => {
                this.blockUI.stop();
                Object.assign(this.model, data);
                this.isModelLoaded = true;
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    delete(): void {
        if (!confirm('Are you sure you want to delete the selected vendor?')) {
            return;
        }
        this.blockUI.start();
        this.vendorService.delete(this.model.id).subscribe(
            () => {
                this.blockUI.stop();
                this.router.navigate(['/vendor/manage']);
                setTimeout(() => {
                    this.toastr.success('Vendor has been deleted successfully.');
                }, 100);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }
}

