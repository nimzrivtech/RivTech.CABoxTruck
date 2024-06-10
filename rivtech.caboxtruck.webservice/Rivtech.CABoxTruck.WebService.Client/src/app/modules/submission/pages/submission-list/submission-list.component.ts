import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { LayoutService } from '../../../../core/services/layout/layout.service';
import { SubmissionListData } from '../../data/submission-list.data';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-submission-list',
  templateUrl: './submission-list.component.html',
  styleUrls: ['./submission-list.component.css']
})
export class SubmissionListComponent implements OnInit {

  constructor(
    private layoutService: LayoutService,
    private submissionListData: SubmissionListData,
    private route: ActivatedRoute,
  ) { }

  async ngOnInit() {
    this.layoutService.clearMenu();
    this.submissionListData.setDefaultStatus('');
    this.submissionListData.currentPage = 1;
    this.submissionListData.itemPerPage = 10;
    this.submissionListData.sortField = '';
    this.submissionListData.order = '';
    this.submissionListData.searchFilterForm = this.submissionListData.constructFormGroup();

    const searchAnyKeywordValue = this.route.snapshot.queryParams['q'];
    if (searchAnyKeywordValue && searchAnyKeywordValue !== '') {
      this.submissionListData.searchAnyKeyword(searchAnyKeywordValue);
      return;
    }

    const paramStatusId = this.route.snapshot.queryParams['status'];
    const paramRenewal = this.route.snapshot.queryParams['renewal'];
    const paramDashboard = this.route.snapshot.queryParams['dashboard'];
    this.submissionListData.setDefaultStatus(paramStatusId);
    await this.submissionListData.searchDataFromFormValue(this.submissionListData.constructFormGroup(), paramRenewal, paramDashboard);
  }

  async setSearchFormValue(formGroup: FormGroup) {
    await this.submissionListData.searchDataFromFormValue(formGroup);
  }

}
