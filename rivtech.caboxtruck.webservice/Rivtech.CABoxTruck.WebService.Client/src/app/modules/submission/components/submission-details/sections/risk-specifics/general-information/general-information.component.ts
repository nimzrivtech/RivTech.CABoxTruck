import { Component, OnInit, ViewChild } from '@angular/core';
import { RiskSpecificsData } from '../../../../../data/risk-specifics.data';
import { BaseComponent } from '../../../../../../../shared/base-component';
import { Router } from '@angular/router';
import { SelectItem } from '../../../../../../../shared/models/dynamic/select-item';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';

@Component({
  selector: 'app-general-information',
  templateUrl: './general-information-static.component.html',
  styleUrls: ['./general-information.component.scss']
})
export class GeneralInformationComponent extends BaseComponent implements OnInit {
  daysOfServiceList: SelectItem[] = [
    {label: 'Weekdays only', value: 'Weekdays only'},
    {label: 'Weekends only', value: 'Weekends only'},
    {label: 'Weekdays and Weekends', value: 'Weekdays and Weekends'}
  ];
  hideMe: boolean = false;
  
  constructor(    
    private menuOverrideService: MenuOverrideService,
    public riskSpecificsData: RiskSpecificsData,
    public router: Router)
  {
    super();
  }

  get formControl() { return this.riskSpecificsData.riskSpecificsForms.generalInformationForm.controls; }

  ngOnInit() {  
  }

  onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate(['/submissions/new/coverages/coverage-questions']);
        break;
      case ClickTypes.Next:
        // if (this.form.invalid){
        //   this.form.markAllAsTouched(); //mark form as touched to display validation error
        //   window.scrollTo(0, 0);
        //   return;
        // }
        this.menuOverrideService.overrideSideBarBehavior();        
        this.router.navigate(['/submissions/new/risk-specifics/driver-info']);
        break;
    }
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
