import { Component, OnInit } from '@angular/core';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-physical-damage-section',
  templateUrl: './physical-damage-section.component.html',
  styleUrls: ['./physical-damage-section.component.scss']
})
export class PhysicalDamageSectionComponent implements OnInit {
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  hideMe: boolean = false;
  constructor(public policySummaryData: PolicySummaryData) { }

  ngOnInit() {
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };
  }
  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  isNaN(val): boolean {
    return Number(val).toFixed(2) === 'NaN';
  }

}
