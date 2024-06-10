import { Component, OnInit } from '@angular/core';
import { RiskDTO } from '../../../../../../shared/models/risk/riskDto';
import { RiskSpecificsData } from '../../../../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../../data/submission.data';

@Component({
  selector: 'app-risk-specifics',
  templateUrl: './risk-specifics.component.html',
  styleUrls: ['./risk-specifics.component.scss']
})
export class RiskSpecificsComponent implements OnInit {

  constructor(public riskSpecificData: RiskSpecificsData,
    private submissionData: SubmissionData) { }

  ngOnInit() {
    this.riskSpecificData.retrieveDropDownValues();
    this.riskSpecificData.setBusinessRules();
    if (this.submissionData?.riskDetail) { this.riskSpecificData.initiateFormFields(this.submissionData.riskDetail); }
    else { this.riskSpecificData.initiateFormFields(new RiskDTO()); }
  }

}
