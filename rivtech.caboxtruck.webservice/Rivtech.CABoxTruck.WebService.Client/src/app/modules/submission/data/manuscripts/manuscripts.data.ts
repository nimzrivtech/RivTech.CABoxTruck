import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PagerService } from '../../../../core/services/pager.service';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { SubmissionData } from '../submission.data';
import { TableData } from '../tables.data';
import { ManuscriptDTO ,ManuscriptListRequestDTO} from '@app/shared/models/submission/manuscript/ManuscriptDto';
import { ManuscriptService } from '../../services/manuscript.service';
import { BrokerInfoDto } from '@app/shared/models/submission/brokerinfo/brokerInfoDto';
import { QuoteLimitsData } from '../quote/quote-limits.data';
import { SummaryData } from '../summary.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import Utils from '@app/shared/utilities/utils';

@Injectable(
  { providedIn: 'root'}
)
export class ManuscriptsData {
  manuscripts: ManuscriptDTO[];
  manuscriptForm: FormGroup;
  dateOptions: IAngularMyDpOptions;
  pager: any = {};
  currentPage: number = 1;
  pagedItems: any[];
  loading: boolean;
  currentServerDateTime: Date;
  itemPerPage: number = 10;
  totalRowCount: number = 9;
  sortField : string = 'CreatedDate';
  order : string = 'ASC';
  constructor(
    private fb: FormBuilder,
    private service: ManuscriptService,
    private submissionData: SubmissionData,
    private tableData: TableData,
    private pagerService: PagerService,
    private quoteLimitsData: QuoteLimitsData,
    private summaryData: SummaryData,
    private policySummaryData: PolicySummaryData
  ) { }

  initiateFormFields() {
    this.submissionData.getCurrentServerDateTime2().subscribe((date) => {
      this.currentServerDateTime = date;
    });
    this.manuscripts = [];
    this.pagedItems = [];
    this.manuscriptForm = this.fb.group({
      id: [],
      effectiveDate: [],
      expirationDate: [],
      isProrate: [],
      isSelected: [],
      title: [null, Validators.required],
      description: [null, Validators.required],
      premium: [null, [Validators.required]],
      premiumType: [],
    });
    this.initDateOptions();
  }

  populateFields(isPolicy: boolean) {
    if (this.submissionData.riskDetail?.riskManuscripts) {
      this.currentPage = 1;
      this.itemPerPage = 10;
      
      this.manuscripts = this.submissionData.riskDetail.riskManuscripts;
      this.getAllManuscriptList(this.getMSListPayload(this.currentPage, this.itemPerPage, isPolicy));
    }
  }

  updateById(id: string, value: boolean, fromEndorsement: boolean = false) {
    const manuscript = this.manuscripts.find(m => m.id === id);
    manuscript.isSelected = value;
    if (manuscript) {
      this.save(manuscript, fromEndorsement);
    }
  }

  save(manuscript: ManuscriptDTO, fromEndorsement: boolean = false) {
    manuscript.riskDetailId = this.submissionData.riskDetail?.id;
    Utils.blockUI();
    if (!manuscript.id) {
      this.service.post(manuscript).subscribe(data => {
        this.manuscripts.push(data);
        this.getAllManuscriptList(this.getMSListPayload(this.currentPage, this.itemPerPage,fromEndorsement), true);
        this.refreshQuoteOptionsAndSummaryHeader();
        if (fromEndorsement) { this.policySummaryData.updateSubtatusForPendingEndorsement(); }
        Utils.unblockUI();
      });
    } else {
      this.service.put(manuscript).subscribe(data => {
        const updatingManuscript = this.manuscripts.find(m => m.id === data.id);
        Object.assign(updatingManuscript, data);
        this.getAllManuscriptList(this.getMSListPayload(this.currentPage, this.itemPerPage,fromEndorsement));
        this.refreshQuoteOptionsAndSummaryHeader();
        if (fromEndorsement) { this.policySummaryData.updateSubtatusForPendingEndorsement(); }
        Utils.unblockUI();
      });
    }
    this.submissionData.riskDetail.riskManuscripts = this.manuscripts;
  }

  
  getAllManuscriptList(payload: ManuscriptListRequestDTO = {}, isNew: boolean = false) {
    Utils.blockUI();
    this.service.getManuscriptWithPaging(payload).subscribe((data) => {
     this.totalRowCount = data.paginationDetails.totalCount;
      this.currentPage =isNew? data.paginationDetails.totalPages : data.paginationDetails.currentPage;
      this.pagedItems = data.riskManuscript;
      Utils.unblockUI();
    });
  }

  getAllManuscript() {
    Utils.blockUI();
    this.service.get(this.submissionData.riskDetail.id).subscribe((data) => {
      Utils.unblockUI();
      this.submissionData.riskDetail.riskManuscripts = data;
      this.manuscripts = data;
    });
  }

  getSortManuscript(payload: ManuscriptListRequestDTO = {}) {
    //requested to removed block UI
    this.service.getManuscriptWithPaging(payload).subscribe((data) => {
     this.totalRowCount = data.paginationDetails.totalCount;
      this.currentPage = data.paginationDetails.currentPage;
      this.pagedItems = data.riskManuscript;
    });
  }
 

  getMSListPayload(pageNumber: number = 1, pageSize: number = this.itemPerPage, isPolicy:boolean): ManuscriptListRequestDTO {
    const payload: ManuscriptListRequestDTO = {
      pageNumber: pageNumber,
      pageSize: pageSize,
      riskDetailId : this.submissionData.riskDetail?.id,
      isPolicy: isPolicy,
      sortField: this.sortField,
      order: this.order
    };
    return payload;
  }

  updateDates(broker: BrokerInfoDto) {
    if (this.manuscripts?.length > 0) {
      const manuscript = this.manuscripts[0];
      manuscript.effectiveDate = broker?.effectiveDate;
      manuscript.expirationDate = broker?.expirationDate;
      this.service.putDates(manuscript).subscribe();
    }
  }

  delete(id: string) {
    this.service.delete(id).subscribe(() => {
      this.manuscripts = this.manuscripts.filter(ai => ai.id !== id);
      this.submissionData.riskDetail.riskManuscripts = this.manuscripts;
      this.getAllManuscriptList(this.getMSListPayload(this.currentPage, this.itemPerPage, false));
      this.refreshQuoteOptionsAndSummaryHeader();
    });
  }

  deleteEndrosement(id: string, expirationDate: Date) {
    this.service.delete(id, true, expirationDate).subscribe((data) => {
      // if (data) {
      //   this.manuscripts = this.manuscripts.filter(ai => ai.id !== id);
      // } else {
      //   const manu = this.manuscripts.find(ai => ai.id === id);
      //   if (manu.isProrate) { manu.expirationDate = expirationDate; }
      //   manu.isSelected = false;
      //   manu.isActive = false;
      // }
      if (data) {
          this.manuscripts = this.manuscripts.filter(ai => ai.id !== id);
      } 
      this.submissionData.riskDetail.riskManuscripts = this.manuscripts;
      this.getAllManuscriptList(this.getMSListPayload(this.currentPage, this.itemPerPage, true));
      this.refreshQuoteOptionsAndSummaryHeader();
      this.policySummaryData.updateSubtatusForPendingEndorsement();
    });
  }

  initDateOptions() {
    this.dateOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
    };
  }

  sort(targetElement) {
    const sortData = this.tableData.sortPagedData(targetElement, this.manuscripts, this.currentPage);
    this.manuscripts = sortData.sortedData;
    this.pagedItems = sortData.pagedItems;
  }

  setPage(page: number, data: any[], isNew: boolean = false) {
    if (page < 1) {
      return;
    }

    this.pager = this.pagerService.getPager(this.manuscripts.length, page);

    if (isNew) {
      this.currentPage = this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }

      this.currentPage = page;
    }

    this.pagedItems = data.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  moveToLastPage() {
    this.pager = this.pagerService.getPager(this.manuscripts.length);
    this.setPage(this.pager.endPage, this.manuscripts);
  }

  public refreshQuoteOptionsAndSummaryHeader(): void {
    this.quoteLimitsData.loadQuoteOptions().subscribe(data => {
      if (this.submissionData.isIssued) {
        this.policySummaryData.getBillingSummary();
      } else {
        const bindOptionId = 1;
        const riskCoverage = data?.riskCoverages.find(coverage => coverage.optionNumber == bindOptionId);
        this.summaryData.estimatedPremium = riskCoverage?.riskCoveragePremium?.premium;
      }
    });
  }
}
