import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { PageSectionsValidations } from '../../../../../../../shared/constants/page-sections.contants';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { ApplicantBusinessValidationService } from '../../../../../../../core/services/validations/applicant-business-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';

@Component({
  selector: 'app-business-details',
  templateUrl: './business-details.component.html',
  styleUrls: ['./business-details.component.scss']
})
export class BusinessDetailsComponent implements OnInit {

  constructor(public applicantData: ApplicantData,
    private router: Router,
    public location: Location,
    public submissionData: SubmissionData,
    public applicantBusinessValidation: ApplicantBusinessValidationService,
    public formValidation: SubmissionNavValidateService,
    private menuOverrideService: MenuOverrideService) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  ngOnInit() {
  }

  public onClick(clickType?) {
    this.nextBack.location = this.location;
    const prevUrl = this.nextBack.getPrev();
    const nextUrl = this.nextBack.getNext();
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([prevUrl]);
        break;
      case ClickTypes.Next:
        this.applicantData.applicantForms.businessDetails.markAllAsTouched();
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([nextUrl]);
        break;
    }
  }
}
