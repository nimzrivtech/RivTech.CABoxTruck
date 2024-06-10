import { Component, OnInit } from '@angular/core';
import { CoverageData } from '../../../../../../../../../modules/submission/data/coverages/coverages.data';

@Component({
  selector: 'app-projection-figures-chart',
  templateUrl: './projection-figures-chart.component.html',
  styleUrls: ['./projection-figures-chart.component.scss']
})
export class ProjectionFiguresChartComponent implements OnInit {

  get f() { return this.coverageData.coverageForms.historicalCoverageForm.controls; }
  constructor(public coverageData: CoverageData) { }

  ngOnInit() {
  }

}
