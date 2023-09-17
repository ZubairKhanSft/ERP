import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { AppUtils, AppSettings } from '../../../helpers';
import { DataTableResponseModel, SelectListItemModel, ItemFilterModel } from '../../../models';
import { ItemService, MasterDataService } from '../../../services';

@Component({
    selector: 'app-item-manage',
    templateUrl: './item.manage.component.html'
})

export class ItemManageComponent implements OnInit, AfterViewInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    @ViewChild(DataTableDirective, { static: false })
    datatableElement: DataTableDirective;
    dtOptions: DataTables.Settings = {};
    dtInstance: DataTables.Api;
    rowIndex = 0;
    pageLength = 10;
    search: any = null;
    itemTypes: Array<SelectListItemModel> = new Array<SelectListItemModel>();
    filterModel: ItemFilterModel = new ItemFilterModel();

    constructor(private http: HttpClient,
        private router: Router,
        private toastr: ToastrService,
        private itemService: ItemService,
        private appUtils: AppUtils,
        private appSettings: AppSettings,
        private masterDataService: MasterDataService) { }

    ngOnInit(): void {
        this.loadItemTypes();
        const self = this;
        this.dtOptions = {
            dom: '<"top">rt<"bottom"lip><"clear">',
            serverSide: true,
            processing: true,
            language: {
                loadingRecords: '&nbsp;',
                processing: '<div class="block-ui-spinner"><div class="loader"></div></div>',
                searchPlaceholder: 'Filter invoice...',
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
            order: [[0, 'asc']],
            stateSave: true,
            ajax: (dataTablesParameters: any, callback) => {

                dataTablesParameters.itemTypeId = self.filterModel.itemTypeId;
                dataTablesParameters.filterKey = self.filterModel.filterKey;

                self.http
                    .post<DataTableResponseModel>(this.appSettings.ApiBaseUrl + 'Item/paged-result', dataTablesParameters, {})
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
                    width: '15%',
                    render: function (data, type, row) {
                        return `<a href='javascript:;' action-type='view-detail'>${row.name}</a>`;
                    }
                },
               
                {
                    className: 'text-right',
                    data: 'rate',
                    title: 'Rate',
                    width: '15%',
                    render: function (data, type, row) {
                        const sign = self.appUtils.getCurrencySign();
                        return `${sign}${data}`;
                    }
                }, {
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
            /*         render: function (data, type, row) {
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
                    self.router.navigate(['/item/edit', data.id]);
                });

                const delElem = $(row).find('[action-type = delete]');
                $(delElem).unbind('click');
                $(delElem).on('click', function () {
                    self.delete(data.id);
                });

                const detailElem = $(row).find('[action-type = view-detail]');
                $(detailElem).unbind('click');
                $(detailElem).on('click', function () {
                    self.router.navigate(['/item/detail', data.id]);
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
        this.itemService.toggleStatus(id).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.dtInstance.ajax.reload();
                });
                this.toastr.success(`Item & service has been ${(status === 1 ? 'deactivated' : 'activated')} successfully.`);
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

    loadItemTypes() {
        this.masterDataService.getItemType()
            .subscribe(
                data => {
                    Object.assign(this.itemTypes, data);
                },
                error => {
                    this.appUtils.ProcessErrorResponse(this.toastr, error);
                });
    }

    delete(id: number): void {
        if (!confirm('Are you sure you want to delete the selected item/service?')) {
            return;
        }
        this.blockUI.start();
        this.itemService.delete(id).subscribe(
            () => {
                this.blockUI.stop();
                setTimeout(() => {
                    this.dtInstance.ajax.reload();
                });
                this.toastr.success('Item/service has been deleted successfully.');
            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }
    doFilter() {
        this.dtInstance.ajax.reload();
    }

    resetFilter() {
        this.filterModel.itemTypeId = '';
        this.filterModel.filterKey = '';
        this.doFilter();
    }
}

