import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { AppUtils, AppSettings } from '../../../helpers';
import { DataTableResponseModel } from '../../../models';
import { VendorService } from '../../../services';

@Component({
    selector: 'app-vendor-manage',
    templateUrl: './vendor.manage.component.html'
})

export class VendorManageComponent implements OnInit, AfterViewInit {
    @BlockUI('container-blockui-main') blockUI: NgBlockUI;
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
        private vendorService: VendorService,
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
                searchPlaceholder: 'Filter vendor',
                paginate: {
                    first: '<i class="la la-angle-double-left kt-font-bolder">',
                    last: '<i class="la la-angle-double-right kt-font-bolder">',
                    previous: '<i class="la la-angle-left kt-font-bolder">',
                    next: '<i class="la la-angle-right kt-font-bolder">'
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
                    .post<DataTableResponseModel>(this.appSettings.ApiBaseUrl + 'Vendor/paged-result', dataTablesParameters, {})
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
                    data: 'name',
                    title: 'Name',
                    width: '25%',
                    render: function (data, type, row) {
                        return `<a href='javascript:;' action-type='view-detail'>${row.name} </a>`;
                    }
                },
                {
                    data: 'hstNumber',
                    title: 'HST#',
                    width: '15%'
                },
                {
                    data: 'email',
                    title: 'Email',
                    width: '25%'
                },
                {
                    data: 'phone',
                    title: 'Phone',
                    width: '15%',
                    render: function (data) {
                        return self.appUtils.applyPhoneMask(data);
                    }
                },
                {
                    data: 'status',
                    title: 'Status',
                    width: '10%',
                    render: function (data) {
                        return data === 1
                            ? '<span class="kt-badge kt-badge--success kt-badge--inline">Active</span>'
                            : '<span class="kt-badge kt-badge--danger kt-badge--inline">Inactive</span>';
                    }
                },
                {
                    data: null,
                    title: 'Action',
                    width: '10%',
                    orderable: false,
                    className: 'text-center',
                    render: function (data, type, row) {
                        const htmlString = 
                        ` <button type="button" class="btn btn-outline-success btn-sm dropdown-toggle"
                        data-toggle="dropdown">
                        Action
                    </button>
                    <div class="dropdown-menu dropdown-menu-fit dropdown-menu-right">
                        <ul class="kt-nav">
                            <li class="kt-nav__item">
                                <a class="kt-nav__link">
                                    <em class="kt-nav__link-icon la la-credit-card"></em>
                                    <span class="kt-nav__link-text" action-type = 'make-bill'>Create Bill</span>
                                </a>
                            </li>`
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
                                    <em class="kt-nav__link-icon la la-file"></em>
                                    <span class="kt-nav__link-text" action-type='view-detail'>
                                        View Detail
                                    </span>
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
                    self.router.navigate(['/vendor/edit', data.id]);
                });

                const delElem = $(row).find('[action-type = delete]');
                $(delElem).unbind('click');
                $(delElem).on('click', function () {
                    self.delete(data.id);
                });

                const detailElem = $(row).find('[action-type = view-detail]');
                $(detailElem).unbind('click');
                $(detailElem).on('click', function () {
                    self.router.navigate(['/vendor/detail', data.id]);
                });

                const billElem = $(row).find('[action-type = make-bill]');
                $(billElem).unbind('click');
                $(billElem).on('click', function () {
                    self.router.navigate(['/bill/add', data.id]);
                });
            },
            drawCallback: function () {
                if ($('.pagination li').length <= 5) {
                    $('.pagination').hide();
                }
            }
        };
    }

    ngAfterViewInit(): void {
        this.datatableElement.dtInstance
            .then((dtInstance: DataTables.Api) => this.dtInstance = dtInstance);
    }

    toggleStatus(id: number, status: number): void {
        this.blockUI.start();
        this.vendorService.toggleStatus(id).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.dtInstance.ajax.reload();
                });
                this.toastr.success(`Vendor has been ${(status === 1 ? 'deactivated' : 'activated')} successfully.`);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    delete(id: number): void {
        if (!confirm('Are you sure you want to delete the selected vendor?')) {
            return;
        }
        this.blockUI.start();
        this.vendorService.delete(id).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.dtInstance.ajax.reload();
                });
                this.toastr.success('Vendor has been deleted successfully.');
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

}

