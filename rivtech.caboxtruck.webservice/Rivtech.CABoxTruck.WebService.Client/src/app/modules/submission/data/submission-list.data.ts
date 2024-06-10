import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BaseClass } from 'app/shared/base-class';
import { takeUntil } from 'rxjs/operators';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { NgxSpinnerService } from 'ngx-spinner';
import Utils from 'app/shared/utilities/utils';
import { IRiskSummaryDto } from '../../../shared/models/submission/risk-summary.dto';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { PagerService } from '../../../core/services/pager.service';
import { FilterSubmissionListDTO } from '../../../shared/models/submission/filter-submission-list.dto';
import { Sort } from '../../../shared/utilities/sort';
import { SubmissionService } from '../../../core/services/submission/submission.service';
import { DriverService } from '../services/driver.service';
import { LvMainUse } from '../../../shared/constants/business-details.options.constants';
import { formatDate } from '@angular/common';

@Injectable()

export class SubmissionListData extends BaseClass {
  searchFilterForm: FormGroup;
  //new implementation of paging declarion
  itemPerPage: number = 10;
  totalRowCount: number = 9;
  isOverPage: boolean = false;
  OnFirstLoad: boolean = true;
  docListPager: any = {};
  policydocsOnFirstLoad: boolean = true;
  keyword : string;
  sortField : string;
  order : string;

  loading: boolean;
  pager: any = {};
  currentPage: number = 1;
  pagedItems: any[];
  
  submissionsData: IRiskSummaryDto[];
  datepickerOptions: IAngularMyDpOptions;
  states: any = [];
  useClassList = LvMainUse;

  constructor(
    private formBuilder: FormBuilder,
    private pagerService: PagerService,
    private submissionService: SubmissionService,
    private spinner: NgxSpinnerService,
    private driverService: DriverService,
  ) {
    super();
  }

  private defaultStatus;

  setDefaultStatus(status: string) {
    this.defaultStatus = status;
  }

  getDefaultStatus() {
    const temp = this.defaultStatus;
    // this.defaultStatus = '';
    return temp;
  }

  initForms() {
    this.submissionsData = new Array();
    this.getAllStates();
    this.setPage(1);

    this.datepickerOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
      disableSince: { year: 0, month: 0, day: 0 },
      disableUntil: { year: 0, month: 0, day: 0 }
    };
  }

  constructFormGroup() {
    const formBuilder = this.formBuilder.group({
      submissionNumber: new FormControl(''),
      insuredName: new FormControl(''),
      insuredDBA: new FormControl(''),
      broker: new FormControl(''),
      underwriter: new FormControl(''),
      au_rep: new FormControl(''),
      au_psr: new FormControl(''),
      owner: new FormControl(''),
      inceptionDateFrom: new FormControl(''),
      inceptionDateTo: new FormControl(''),
      status: new FormControl(''),
      state: new FormControl(''),
      submissionType: new FormControl('')
    });
    formBuilder.get('status').patchValue(this.getDefaultStatus());
    this.setDefaultStatus('');
    return formBuilder;
  }


  async searchDataFromFormValue(searchFilterForm: FormGroup, renewal: boolean = false, dashboard: boolean = false) {
    const request = new FilterSubmissionListDTO();
    const dateFrom = searchFilterForm.value.inceptionDateFrom?.singleDate?.formatted || '';
    const dateTo = searchFilterForm.value.inceptionDateTo?.singleDate?.formatted || '';
    request.pageNumber = this.currentPage;
    request.pageSize = this.itemPerPage;
    request.sortField = this.sortField;
    request.order = this.order;
    request.submissionNumber = searchFilterForm.value.submissionNumber;
    request.insuredName = searchFilterForm.value.insuredName;
    request.insuredDBA = searchFilterForm.value.insuredDBA;
    request.broker = searchFilterForm.value.broker;
    request.inceptionDateFrom = dateFrom;
    request.inceptionDateTo = dateTo;
    request.status = searchFilterForm.value.status;
    request.underwriter = searchFilterForm.value.underwriter;
    request.au_rep = searchFilterForm.value.au_rep;
    request.au_psr = searchFilterForm.value.au_psr;
    request.owner = searchFilterForm.value.owner;
    request.state = searchFilterForm.value.state;
    request.isRenewal = renewal;
    request.isDashboard = dashboard;
    if (searchFilterForm.value.submissionType != '') {
      request.isRenewal = searchFilterForm.value.submissionType == 'Renewal' ? true : false;
      request.isDashboard = searchFilterForm.value.submissionType == 'Renewal' ? false : true;
    }

    this.submissionsData = new Array();
    Utils.blockUI();
    this.submissionService.getSubmissionListAsync(request).pipe(takeUntil(this.stop$)).subscribe(result => {
      // update use class value from id to label
      this.totalRowCount = result.paginationDetails.totalCount;
      result.riskSummaryDtos.forEach((data) => {
        const tempUseClass = this.useClassList.find(cls => cls.value == data?.useClass)?.label;
        data.useClass = tempUseClass;
      });
      this.pagedItems = result.riskSummaryDtos;
      Utils.unblockUI();
      // this.setPage(1);
    }, error => {
      Utils.unblockUI();
      if (error.status == 429) {
          NotifUtils.showError(ErrorMessageConstant.tooManyRequestErrorMEssage);
      } else {
        NotifUtils.showError(ErrorMessageConstant.contactAdminErrorMessage);
      }
      // this.setPage(1);
    });
  }

  async searchDataFromTextValue(searchText: string) {
    const request = new FilterSubmissionListDTO();
    request.searchText = searchText;
    request.pageNumber = this.currentPage;
    request.pageSize = this.itemPerPage;
    request.sortField = this.sortField;
    request.order = this.order;

    this.submissionsData = new Array();
    Utils.blockUI();
    this.submissionService.getSubmissionListAsync(request).pipe(takeUntil(this.stop$)).subscribe(result => {
      
      this.totalRowCount = result.paginationDetails.totalCount;
      result.riskSummaryDtos.forEach((data) => {
        const tempUseClass = this.useClassList.find(cls => cls.value == data?.useClass)?.label;
        data.useClass = tempUseClass;
      });
      this.pagedItems = result.riskSummaryDtos;
      Utils.unblockUI();
      // this.setPage(1);
    }, error => {
      Utils.unblockUI();
      if (error.status == 429) {
          NotifUtils.showError(ErrorMessageConstant.tooManyRequestErrorMEssage);
      } else {
        NotifUtils.showError(ErrorMessageConstant.contactAdminErrorMessage);
      }
      // this.setPage(1);
    });
  }

  setPage(page: number) {
    if (page < 1) {
      return;
    }

    this.pager = this.pagerService.getPager(this.submissionsData.length, page);

    this.pagedItems = this.submissionsData.slice(this.pager.startIndex, this.pager.endIndex + 1);
    this.currentPage = page;
  }

  sort(targetElement) {
    const sort = new Sort();
    const elem = targetElement;
    const order = elem.getAttribute('data-order');
    const type = elem.getAttribute('data-type');
    const property = elem.getAttribute('data-name');

    if (order === 'desc') {
      this.pagedItems.sort(sort.startSort(property, order, type));
      elem.setAttribute('data-order', 'asc');
    } else {
      this.pagedItems.sort(sort.startSort(property, order, type));
      elem.setAttribute('data-order', 'desc');
    }

    this.pager = this.pagerService.getPager(this.pagedItems.length, this.currentPage);
    this.pagedItems = this.pagedItems.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  getAllStates() {
    this.driverService.getStates().pipe(takeUntil(this.stop$)).subscribe(data => {
      this.states = data.map(s => ({
        value: s.stateCode?.toString(),
        label: s.fullStateName?.toString()?.replace('[E]', '')
      }));
    });
  }

  searchAnyKeyword(keyword: string) {
    this.searchDataFromTextValue(keyword);
  }

  // searchAnyKeyword(keyword: string) {
  //   const searchData = this.submissionsData.filter(data => (data.policyNumber +
  //                                                           data.submissionNumber +
  //                                                           data.insuredName +
  //                                                           data.broker +
  //                                                           formatDate(data.inceptionDate, 'mediumDate', 'en') +
  //                                                           data.status +
  //                                                           data.numberOfUnits +
  //                                                           data.state +
  //                                                           data.submissionType +
  //                                                           data.policyLimit +
  //                                                           data.newVenture +
  //                                                           data.neededBy +
  //                                                           data.brokerContact +
  //                                                           data.qAC +
  //                                                           data.midTerm +
  //                                                           data.lastNoteAdded +
  //                                                           data.auSpecialist +
  //                                                           data.auRep +
  //                                                           data.aupsr +
  //                                                           data.brokerZip +
  //                                                           data.brokerState +
  //                                                           data.brokerCity +
  //                                                           formatDate(data.dateSubmitted, 'mediumDate', 'en') +
  //                                                           data.underwriter +
  //                                                           data.expiringPolicy +
  //                                                           data.useClass +
  //                                                           data.lossRatio).toLowerCase().search(keyword.toLowerCase()) > -1);
  //   this.pager = this.pagerService.getPager(searchData.length, 1);
  //   this.pagedItems = searchData.slice(this.pager.startIndex, this.pager.endIndex + 1);
  //   this.currentPage = 1;
  // }
}
