import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { AppUtils } from '../../../helpers';
import { ItemDetailModel } from '../../../models';
import { ItemService } from '../../../services';

@Component({
    selector: 'app-item-detail',
    templateUrl: './item.detail.component.html'
})

export class ItemDetailComponent implements OnInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    isModelLoaded = false;
    model: ItemDetailModel = new ItemDetailModel();

    constructor(private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private itemService: ItemService) {
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });
    }

    ngOnInit() {
        this.loadItem();
    }

    loadItem() {
        this.blockUI.start();
        this.itemService.getDetail(this.model.id).subscribe(
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
        if (!confirm('Are you sure you want to delete the selected item/service?')) {
            return;
        }
        this.blockUI.start();
        this.itemService.delete(this.model.id)
            .subscribe(
                () => {
                    this.blockUI.stop();
                    setTimeout(() => {
                        this.router.navigate(['/item/manage']);
                    }, 100);
                    setTimeout(() => {
                        this.toastr.success('Item/service has been deleted successfully.');
                    }, 500);
                },
                error => {
                    this.blockUI.stop();
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }
}
