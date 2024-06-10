import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SummaryData } from '@app/modules/submission/data/summary.data';
import { BsModalRef, BsModalService, PageChangedEvent } from 'ngx-bootstrap';
import { SubmissionListData } from '../../../data/submission-list.data';
import { TableData } from '@app/modules/submission/data/tables.data';
import { TableConstants } from '@app/shared/constants/table.constants';
import {ActivatedRoute, Router} from '@angular/router';
import { FormGroup, NgForm } from '@angular/forms';

@Component({
  selector: 'app-submission-table',
  templateUrl: './submission-table.component.html',
  styleUrls: ['./submission-table.component.scss']
})
export class SubmissionTableComponent implements OnInit {
  public showRowsNumber: number = 10;
  TableConstants = TableConstants;

  @Output() submitSearch: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();
  constructor(
    public submissionListData: SubmissionListData,
    private tableData: TableData,
    private route: ActivatedRoute,
    public summaryData: SummaryData,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.summaryData.populateAssignedToList()
  }

  setPage(page: number) {
    this.submissionListData.setPage(page);
  }

  getName(assignedToId) {
    if (assignedToId == null || assignedToId == '') { return; }
    this.summaryData.initialize();
    const assigned = this.summaryData.assignedToList.find(x => x.value == assignedToId);
    if (assigned != null) { return assigned.label; }
    return;
  }

  changeShowRowsNumber(value): void{

    if (this.submissionListData.itemPerPage === value) { return; }

    var totalPages = this.submissionListData.totalRowCount / value;

    this.submissionListData.isOverPage = false;

    if(this.submissionListData.currentPage > totalPages)
    {
      this.submissionListData.isOverPage = true;
    }

    this.showRowsNumber = value;
    this.submissionListData.itemPerPage = value;
    this.submissionListData.currentPage = 1;
    //this.submissionListData.sortField = "";
    //this.submissionListData.order = "desc";

    this.loadDataTable(this.submissionListData.currentPage, this.submissionListData.itemPerPage,this.submissionListData.keyword, this.submissionListData.sortField, this.submissionListData.order);
   
  }
  policyPageChanged(event: PageChangedEvent): void {
    if (this.submissionListData.currentPage === event.page || this.submissionListData.isOverPage == true) { 
      this.submissionListData.isOverPage = false;
      return; 
    }
      this.submissionListData.currentPage = event.page;
      this.loadDataTable(this.submissionListData.currentPage, this.submissionListData.itemPerPage,this.submissionListData.keyword, this.submissionListData.sortField, this.submissionListData.order);
  }

  loadDataTable(currentPage:number,itemPerPage:number,keyword: string,sortField:string, order: string)
  {
    const searchAnyKeywordValue = this.route.snapshot.queryParams['q'];
    if (searchAnyKeywordValue && searchAnyKeywordValue !== '') {
      this.submissionListData.searchAnyKeyword(searchAnyKeywordValue);
      return;
    }

    const paramStatusId = this.route.snapshot.queryParams['status'];
    const paramRenewal = this.route.snapshot.queryParams['renewal'];
    const paramDashboard = this.route.snapshot.queryParams['dashboard'];
    this.submissionListData.setDefaultStatus(paramStatusId);
    
    if (typeof this.submissionListData.searchFilterForm === "undefined" || typeof paramStatusId != "undefined") {
      this.submissionListData.searchFilterForm = this.submissionListData.constructFormGroup();
    }
    
    this.submissionListData.searchDataFromFormValue(this.submissionListData.searchFilterForm, paramRenewal, paramDashboard);
  }

  onSearchAnyKeyword(keyword: string): void {
    this.router.navigate(['/submissions/list'], {queryParams: { q: keyword} });
    this.submissionListData.sortField = "";
    this.submissionListData.order = "desc";
    this.submissionListData.currentPage = 1;
    this.submissionListData.searchAnyKeyword(keyword)
  }

  sort(targetElement) {
    const elem = targetElement;
    let order = elem.getAttribute('data-order');
    const property = elem.getAttribute('data-name');

    if (this.submissionListData.order == '' && property == 'submissionNumber') {
      order = 'asc';
    }

    if (order === 'desc') {
      elem.setAttribute('data-order', 'asc');
    } else {
      elem.setAttribute('data-order', 'desc');
    }

    this.submissionListData.sortField = property;
    this.submissionListData.order = order;
    this.loadDataTable(this.submissionListData.currentPage, this.submissionListData.itemPerPage,this.submissionListData.keyword, this.submissionListData.sortField, this.submissionListData.order);
  }
}
