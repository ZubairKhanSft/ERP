import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { AppUtils, AppSettings } from '../../../helpers';
import { DataTableResponseModel } from '../../../models';
import { SalesTaxService } from '../../../services';

@Component({
    selector: 'app-sales-tax-manage',
    templateUrl: './sales-tax.manage.component.html'
})

export class SalesTaxManageComponent implements OnInit, AfterViewInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    @ViewChild(DataTableDirective, { static: false })
    datatableElement: DataTableDirective;
    dtOptions: DataTables.Settings = {};
    dtInstance: DataTables.Api;
    rowIndex = 0;
    pageLength = 10;
    search: any = null;

    constructor(private http: HttpClient,
        private router: Router,
        private toastr: ToastrService,
        private salesTaxService: SalesTaxService,
        private appUtils: AppUtils,
        private appSettings: AppSettings) { }

    ngOnInit(): void {
        const self = this;
        this.dtOptions = {
            serverSide: true,
            processing: true,
            language: {
                loadingRecords: '&nbsp;',
                processing: '<div class="block-ui-spinner"><div class="loader"></div></div>',
                searchPlaceholder: 'Filter sales tax',
                paginate: {
                    first: '<i class="fa fa-angle-double-left">',
                    last: '<i class="fa fa-angle-double-right">',
                    previous: '<i class="fa fa-angle-left">',
                    next: '<i class="fa fa-angle-right">'
                }
            },
            search: { search: self.search },
            displayStart: self.rowIndex,
            paging: true,
            pagingType: 'full_numbers',
            pageLength: self.pageLength,
            lengthMenu: [10, 15, 25, 50, 100],
            ajax: (dataTablesParameters: any, callback) => {
                self.http
                    .post<DataTableResponseModel>(this.appSettings.ApiBaseUrl + 'salesTax/paged-result', dataTablesParameters, {})
                    .subscribe(resp => {
                        callback({
                            recordsTotal: resp.recordsTotal,
                            recordsFiltered: resp.recordsFiltered,
                            data: resp.data
                        });
                    });
            },
            columns: [
                {
                    data: 'code',
                    title: 'Sales Tax Code',
                    width: '20%'
                },
                {
                    data: 'taxPercentage',
                    title: 'Tax',
                    width: '10%',
                    render: function (data, type, row) {
                        return self.appUtils.getWithPercentageSign(data);
                    }
                },
                {
                    data: 'description',
                    title: 'Description',
                    width: '20%'
                },
                {
                    data: 'status',
                    title: 'Status',
                    width: '10%',
                    render: function (data) {
                        return data === 1
                            ? '<span class="badge badge-success">Active</span>'
                            : '<span class="badge badge-danger">Inactive</span>';
                    }
                },
                {
                    data: null,
                    title: 'Action',
                    width: '10%',
                    orderable: false,
                    className: 'text-center',
         /*            render: function (data, type, row) {
                        const statusHtml =
                            row.status === 1
                                ? `<em class='fa fa-ban cursor-pointer m-r-5' title='Deactivate' action-type='toggle-status'></em>`
                                : row.status === 2
                                    ? `<em class='fa fa-check cursor-pointer m-r-5' title='Activate' action-type='toggle-status'></em>`
                                    : '';

                        const htmlString = statusHtml
                            + `<em class='fa fa-edit cursor-pointer m-r-3' title='Edit' action-type='edit'></em>`
                            + `<em class='fa fa-trash cursor-pointer' title='Delete' action-type='delete'></em>`;
                        return htmlString;
                    } */
                    render: function (data, type, row) {
                        const htmlString = 
                        ` <button type="button" class="btn btn-outline-success btn-sm dropdown-toggle"
                        data-toggle="dropdown">
                        Action
                    </button>
                    <div class="dropdown-menu dropdown-menu-fit dropdown-menu-right">
                        <ul class="kt-nav">`
                            const htmlStatus=
                            row.status === 1
                            ?`<li class="kt-nav__item">
                            <a class="kt-nav__link">
                                <em class="kt-nav__link-icon la la-ban"></em>
                                <span class="kt-nav__link-text" action-type = 'toggle-status'>Deactivate</span>
                            </a>
                        </li>`
                        :`<li class="kt-nav__item">
                                <a  class="kt-nav__link">
                                    <em class="kt-nav__link-icon la la-check"></em>
                                    <span class="kt-nav__link-text" action-type = 'toggle-status'>Activate</span>
                                </a>
                            </li>`

                            const htmlString2=htmlString +htmlStatus
                            
                            +`<li class="kt-nav__item">
                                <a class="kt-nav__link">
                                    <em class="kt-nav__link-icon la la-edit"></em>
                                    <span class="kt-nav__link-text" action-type='edit'> Edit</span>
                                </a>
                            </li>
                            
                            <li class="kt-nav__item">
                            <a class="kt-nav__link">
                                <em class="kt-nav__link-icon la la-trash"></em>
                                <span class="kt-nav__link-text" action-type='delete'>
                                    Delete
                                </span>
                            </a>
                        </li>
                        </ul>
                    </div>`
                        // const statusHtml =
                        //     row.status === 1
                        //         ? `<em class='fa fa-ban cursor-pointer m-r-5' title='Deactivate' action-type='toggle-status'></em>`
                        //         : row.status === 2
                        //             ? `<em class='fa fa-check cursor-pointer m-r-5' title='Activate' action-type='toggle-status'></em>`
                        //             : '';

                        // const htmlString = statusHtml
                        //     + `<em class='fa fa-edit cursor-pointer m-r-3' title='Edit' action-type='edit'></em>`
                        //     + `<em class='fa fa-trash cursor-pointer' title='Delete' action-type='delete'></em>`;

                        return htmlString2;
                    }
                }
            ],
            rowCallback: function (row, data: any) {
                const toggleStatusElem = $(row).find('[action-type = "toggle-status"]');
                $(toggleStatusElem).unbind('click');
                $(toggleStatusElem).on('click', function () {
                    self.toggleStatus(data.id, data.status);
                });

                const editElem = $(row).find('[action-type = "edit"]');
                $(editElem).unbind('click');
                $(editElem).on('click', function () {
                    self.router.navigate(['/sales-tax/edit', data.id]);
                });

                const delElem = $(row).find('[action-type = delete]');
                $(delElem).unbind('click');
                $(delElem).on('click', function () {
                    self.delete(data.id);
                });
            },
            drawCallback: function () {
                if ($('.pagination li').length <= 5) {
                    $('.pagination').hide();
                }
            },
        };
    }

    ngAfterViewInit(): void {
        this.datatableElement.dtInstance
            .then((dtInstance: DataTables.Api) => this.dtInstance = dtInstance);
    }

    toggleStatus(id: string, status: number): void {
        this.blockUI.start();
        this.salesTaxService.toggleStatus(id).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.dtInstance.ajax.reload();
                });
                this.toastr.success(`Sales tax has been ${(status === 1 ? 'deactivated' : 'activated')} successfully.`);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    delete(id: number): void {
        if (!confirm('Are you sure you want to delete the selected sales tax?')) {
            return;
        }
        this.blockUI.start();
        this.salesTaxService.delete(id).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.dtInstance.ajax.reload();
                });
                this.toastr.success('Sales tax has been deleted successfully.');
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }
}

