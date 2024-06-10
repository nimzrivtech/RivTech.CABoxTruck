import { Component, OnInit } from '@angular/core';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
  selector: 'app-insured-section',
  templateUrl: './insured-section.component.html',
  styleUrls: ['./insured-section.component.scss']
})
export class InsuredSectionComponent implements OnInit {

  hideMe: boolean = false;
  constructor(public summaryData: PolicySummaryData) { }

  ngOnInit() {
    this.summaryData.insuredForm = this.summaryData.insuredFormSection();
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

}
