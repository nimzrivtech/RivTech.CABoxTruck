import { Component, OnInit, ViewChild } from '@angular/core';
import { RiskSpecificsData } from '../../../../../data/risk-specifics.data';
@Component({
  selector: 'app-risk-specifics-questions',
  templateUrl: './risk-specifics-questions.component.html',
  styleUrls: ['./risk-specifics-questions.component.scss']
})
export class RiskSpecificsQuestionsComponent implements OnInit {
  constructor(public riskSpecificsData: RiskSpecificsData) { }

  ngOnInit() {
  }

  onChildInit(form) {
    // this.riskSpecificsData.initiateFormValues(form);
  }
}
