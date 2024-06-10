import { Component, OnInit, ViewChild } from '@angular/core';

import { DriverData } from '../../../../data/driver/driver.data';

import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { Router } from '@angular/router';
import { NextBackButtonComponent } from '../../submission-details-shared/components/next-back-button/next-back-button.component';
import { Location } from '@angular/common';
import { DriverDto, DriverHeaderDto } from '../../../../../../shared/models/submission/Driver/DriverDto';
import { DriverIncidentDto } from '../../../../../../shared/models/submission/Driver/DriverIncidentDto';
import { SubmissionNavValidateService } from '../../../../../../core/services/navigation/submission-nav-validate.service';
import { DriverValidationService } from '../../../../../../core/services/validations/driver-validation.service';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';

@Component({
  selector: 'app-driver',
  templateUrl: './driver.component.html',
  styleUrls: ['./driver.component.scss']
})
export class DriverComponent implements OnInit {

  disableIssue: Boolean = true;
  constructor(
    public submissionData: SubmissionData,
    public driverData: DriverData,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    private location: Location,
    public formValidation: SubmissionNavValidateService,
    public driverValidation: DriverValidationService,
  ) { }
  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  ngOnInit() {
    this.driverData.initiateFormGroup(new DriverDto());
    this.driverData.initiateHeaderFormGroup(new DriverHeaderDto());
    this.driverData.initiateFormIncident(new DriverIncidentDto());
  }

  get isPageValid() {
    return this.submissionData.riskDetail?.drivers?.length > 0
    && !this.submissionData.riskDetail?.drivers?.find(x => !x.isValidated);
  }

  public onClick(clickType?) {
    //this handle new or with ID //need more enhancement
    this.nextBack.location = this.location;
    let url = "";
    switch (clickType) {
      case ClickTypes.Back:
        url = this.nextBack.getPrev();
        this.router.navigate([url]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        url = this.nextBack.getNext();
        this.router.navigate([url]);
        break;
    }
  }
}
