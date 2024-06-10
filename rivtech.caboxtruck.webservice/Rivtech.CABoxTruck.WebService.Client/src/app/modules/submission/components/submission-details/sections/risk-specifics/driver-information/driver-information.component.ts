import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { DriverHiringCriteriaData } from '../../../../../data/riskspecifics/driver-hiring-criteria.data';
import { DriverInfoData } from '../../../../../data/riskspecifics/driver-info.data';
import { Location } from '@angular/common';
import { DriverInfoValidationService } from '../../../../../../../core/services/validations/driver-information-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';

@Component({
  selector: 'app-driver-information',
  templateUrl: './driver-information.component.html',
  styleUrls: ['./driver-information.component.scss']
})
export class DriverInformationComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  constructor(private router: Router,
    private menuOverrideService: MenuOverrideService,
    private location: Location,
    public driverInfoData: DriverInfoData,
    public driverHiringCriteriaData: DriverHiringCriteriaData,
    public driverInfoValidation: DriverInfoValidationService,
    public formValidation: SubmissionNavValidateService,
  ) { }

  ngOnInit() {
  }

  public onClick(clickType?: any) {
    this.nextBack.location = this.location;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([this.nextBack.getPrev()]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        this.driverInfoData.driverInfoForm.markAllAsTouched();
        this.router.navigate([this.nextBack.getNext()]);
        break;
    }
  }
}
