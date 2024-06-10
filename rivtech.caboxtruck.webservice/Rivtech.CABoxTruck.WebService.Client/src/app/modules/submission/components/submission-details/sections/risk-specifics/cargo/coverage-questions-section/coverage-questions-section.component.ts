import { Component, OnInit, ViewChild } from '@angular/core';
import { RiskSpecificsData } from '../../../../../../data/risk-specifics.data';
import {DynamicFormComponent} from '../../../../../../../../shared/components/dynamic/dynamic-form/dynamic-form.component';
import {MenuOverrideService} from '../../../../../../../../core/services/layout/menu-override.service'

@Component({
  selector: 'app-coverage-questions-section',
  templateUrl: './coverage-questions-section.component.html',
  styleUrls: ['./coverage-questions-section.component.scss']
})
export class CoverageQuestionsSectionComponent implements OnInit {
  @ViewChild(DynamicFormComponent) riskSpecificDynamicForm: DynamicFormComponent;

  constructor(public riskSpecificsData: RiskSpecificsData, private menuOverrideService: MenuOverrideService) { }

  ngOnInit() {
  }

  onChildInit(form) {
    // this.riskSpecificsData.initiateFormValues(form);
  }
}
