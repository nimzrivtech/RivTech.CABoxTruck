import { EventEmitter } from '@angular/core';
import { Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { PathConstants } from '@app/shared/constants/path.constants';
import { SubmissionListData } from '../../../data/submission-list.data';
@Component({
  selector: 'app-submission-search-filter',
  templateUrl: './submission-search-filter.component.html',
  styleUrls: ['./submission-search-filter.component.scss']
})
export class SubmissionSearchFilterComponent implements OnInit {
  searchFilterForm: FormGroup;
  statusData: any = [
    {value: 1, label: 'Received'},
    {value: 2, label: 'In Research'},
    {value: 3, label: 'Incomplete'},
    {value: 4, label: 'Complete'},
    {value: 5, label: 'Withdrawn'},
    {value: 7, label: 'Quoted'},
    {value: 8, label: 'Canceled'},
    {value: 9, label: 'Declined'},
    {value: 10, label: 'Active'},
    {value: 11, label: 'Quote in Progress'},
    {value: 12, label: 'Not Accepted'},
    {value: 13, label: 'Expired'},
    {value: 14, label: 'Data Only'},
    {value: 15, label: 'Test Class'},
  ];
  submissionType: any = [
    {value: 'New', label: 'New'},
    {value: 'Renewal', label: 'Renewal'}
  ];
  hasMarginRight: boolean = true;

  @Output() submitSearch: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

  constructor(
    public submissionListData: SubmissionListData,
    private _router: Router
  ) { }

  ngOnInit(): void {
    this.submissionListData.initForms();
    this.searchFilterForm = this.submissionListData.constructFormGroup();
  }

  onSubmit() {
    this.submissionListData.currentPage = 1;
    this.submissionListData.sortField = "";
    this.submissionListData.order = "desc";
    this._router.navigate([PathConstants.Submission.Index], {
      queryParams: {
        'q': null,
        'status': null,
        'renewal': null,
        'dashboard': null
      },
      queryParamsHandling: 'merge'
    })
    this.submissionListData.searchFilterForm = this.searchFilterForm;
    this.submitSearch.emit(this.searchFilterForm);
  }
}
