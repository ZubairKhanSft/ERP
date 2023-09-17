import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { NgBlockUI, BlockUI } from 'ng-block-ui';
import { DataTableDirective } from 'angular-datatables';
import { SelectListItemModel, InvoiceFilterModel, DataTableResponseModel } from 'src/models';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { InvoiceService, CustomerService } from 'src/services';
import { AppUtils, AppSettings } from 'src/helpers';
import { AUTO_STYLE } from '@angular/animations';

declare var appConfig: any;

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html'
})

export class DashboardComponent implements OnInit {
  @BlockUI('container-blockui') blockUI: NgBlockUI;
  @ViewChild(DataTableDirective, { static: false })
  datatableElement: DataTableDirective;
  dtOptionsInvoice: DataTables.Settings = {};
  dtOptionsBills: DataTables.Settings = {};
  
  dtInstance: DataTables.Api;
  rowIndex = 0;
  pageLength = 10;
  search: any = null;
  customers: Array<SelectListItemModel> = new Array<SelectListItemModel>();
  filterModel: InvoiceFilterModel = new InvoiceFilterModel();
 
  constructor(private http: HttpClient,
    private router: Router,
    private toastr: ToastrService,
    private invoiceService: InvoiceService,
    private appUtils: AppUtils,
    private appSettings: AppSettings,
    private customerService: CustomerService) { }


    ngOnInit() {
        setTimeout(() => {
            appConfig.initKTDefaults();
        }, 500);
this.getTopfiveInoices();
this.getTopfiveBills();
      
    }

    getTopfiveInoices(){
      const self = this;
      this.dtOptionsInvoice = {
          dom: '<"top">rt<"clear">',
          serverSide: true,
          processing: true,
          language: {
              loadingRecords: '&nbsp;',
              processing: '<div class="block-ui-spinner"><div class="loader"></div></div>',
              searchPlaceholder: 'Filter invoice...',
            
          },
          search: { search: self.search },
          stateSave: true,
          ajax: (dataTablesParameters: any, callback) => {

              dataTablesParameters.customerId = self.filterModel.customerId;
              dataTablesParameters.filterKey = self.filterModel.filterKey;
              
            

              self.http
                  .post<DataTableResponseModel>(this.appSettings.ApiBaseUrl + 'Invoice/getTopFiveInvoice', dataTablesParameters, {})
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
                  data: 'invoiceNumber',
                  title: 'Invoice#',
                  width: '20%',
                  render: function (data, type, row) {
                      return  `<a href='javascript:;' action-type='view-detail'>${data}
                      </a>`;
                  }
              },
              {
                  data: 'customerName',
                  title: 'Customer',
                  width: '25%'
              },
              {
                  className: 'text-right',
                  data: 'totalAmount',
                  title: 'Amount',
                  width: '15%',
                  render: function (data, type, row) {
                      return `<span class='m-r-15'>${self.appUtils.toMoney(data)}</span>`;
                  }
              },
              {
                  data: 'status',
                  title: 'Status',
                  width: '15%',
                  render: function (data, type, row) {
                      return data === 0
                          ? `<span class='kt-badge kt-badge--dark kt-badge--inline'>Pending</span>`
                          : data === 1 ?
                              `<span class='kt-badge kt-badge--success kt-badge--inline'>Paid</span>`
                          : data === 2 ?
                              `<span class='kt-badge kt-badge--danger kt-badge--inline'>Deleted</span>`
                              : `<span class='kt-badge kt-badge--warning kt-badge--inline'>Overdue</span>`;
                  }
              },
              {
                  data: 'createdOn',
                  title: 'Date',
                  width: '15%',
                  render: function (data) {
                      return self.appUtils.getFormattedDate(data, null);
                  }
              },
             
          ],
        
        
      };
    }


    getTopfiveBills(){
      const self = this;
      this.dtOptionsBills = {
          dom: '<"top">rt<"clear">',
          serverSide: true,
          processing: true,
          language: {
              loadingRecords: '&nbsp;',
              processing: '<div class="block-ui-spinner"><div class="loader"></div></div>',
              searchPlaceholder: 'Filter invoice...',
            
          },
          search: { search: self.search },
          stateSave: true,
          ajax: (dataTablesParameters: any, callback) => {

              dataTablesParameters.customerId = self.filterModel.customerId;
              dataTablesParameters.filterKey = self.filterModel.filterKey;
              
            

              self.http
                  .post<DataTableResponseModel>(this.appSettings.ApiBaseUrl + 'Bill/getTopFiveBills', dataTablesParameters, {})
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
            data: 'billNumber',
                  title: 'Bill Number',
                  width: '20%',
                  render: function (data, type, row) {
                      return  `<a href='javascript:;' action-type='view-detail'>${data}
                      </a>`;
                  }
            },
            {
                data: 'vendorName',
                title: 'Vendor',
                width: '25%'
            },
            {
                className: 'text-right',
                data: 'totalAmount',
                title: 'Amount',
                width: '15%',
                render: function (data, type, row) {
                    return `<span class="m-r-15">${self.appUtils.toMoney(data)}</span>`;
                }
            },
            {
                data: 'status',
                title: 'Status',
                width: '15%',
                render: function (data, type, row) {
                    return data === 0
                        ? `<span class='kt-badge kt-badge--dark kt-badge--inline'>Pending</span>`
                        : data === 1 ?
                            `<span class='kt-badge kt-badge--success kt-badge--inline'>Paid</span>`
                        : data === 2 ?
                            `<span class='kt-badge kt-badge--danger kt-badge--inline'>Deleted</span>`
                            : `<span class='kt-badge kt-badge--warning kt-badge--inline'>Overdue</span>`;
                }
            },
            {
                data: 'createdOn',
                title: 'Created On',
                width: '15%',
                render: function (data) {
                    return self.appUtils.getFormattedDate(data, null);
                }
            },
          
        ],
        
        
      };
    }
    // Line chart
    lineChartData: ChartDataSets[] = [
        { data: [85, 72, 78, 75, 77, 75], label: 'Yearly profit' },
      
      ];
    
      lineChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June'];
    
      lineChartOptions = {
        responsive: true,
      };
    
      lineChartColors: Color[] = [
        {
          borderColor: 'rgb(2, 105, 2)',
          backgroundColor: '#0abb87',
        },
      ];
    
      lineChartLegend = true;
      lineChartPlugins = [];
      lineChartType = 'line';

    // Bar chart 

    barChartData: ChartDataSets[] = [
        { data: [85, 72, 78, 75, 77, 75], label: 'Sales' },
        { data: [75, 77, 75, 78, 72, 85], label: 'Expense' },
      ];
    
      barChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June'];
    
      barChartOptions = {
        responsive: true,
      };
    
      barChartColors: Color[] = [
        {
          borderColor: 'rgb(2, 105, 2)',
          backgroundColor: '#0abb87',
        },
      ];
    
      barChartLegend = true;
      barChartPlugins = [];
      barChartType = 'bar';

    //   pie chart
    pieChartData: ChartDataSets[] = [
        { data: [185, 72, 78, 75, 77, 75], label: 'Sales' },
       
      ];
    
      pieChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June'];
    
      pieChartOptions = {
        responsive: true,
      };
    
    //   pieChartColors: Color[] = [
    //     {
    //       borderColor: 'rgb(2, 105, 2)',
    //       backgroundColor: '#0abb87',
    //     },
    //   ];
    
      pieChartLegend = true;
      pieChartPlugins = [];
      pieChartType = 'pie';


      
}

