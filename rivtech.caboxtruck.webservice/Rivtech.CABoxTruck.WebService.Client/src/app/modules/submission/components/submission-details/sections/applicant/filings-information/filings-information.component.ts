import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { FilingsInformationSectionComponent } from './filings-information-section/filings-information-section.component';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';
import { ApplicantFilingsValidationService } from '../../../../../../../core/services/validations/filings-information-validation.service';

@Component({
  selector: 'app-filings-information',
  templateUrl: './filings-information.component.html',
  styleUrls: ['./filings-information.component.scss']
})

export class FilingsInformationComponent implements OnInit {

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  @ViewChild(FilingsInformationSectionComponent) filingsInformation: FilingsInformationSectionComponent;

  constructor(
    public applicantData: ApplicantData,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    public filingsValidation: ApplicantFilingsValidationService,
    public formValidation: SubmissionNavValidateService,
    private location: Location
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
        this.applicantData.applicantForms.filingsInformationForm.markAllAsTouched();
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([this.nextBack.getNext()]);
        break;
    }
  }
}
