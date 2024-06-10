import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { LimitsData } from '../../../../../../../modules/submission/data/limits/limits.data';
import { LimitsService } from '../../../../../../../modules/submission/services/coverage-limits.service';
import { LimitsValidationService } from '../../../../../../../core/services/validations/limits-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';

@Component({
  selector: 'app-limits-page',
  templateUrl: './limits-page.component.html',
  styleUrls: ['./limits-page.component.scss']
})
export class LimitsPageComponent implements OnInit {
  formBuilder: any;

  constructor(private router: Router,
    public location: Location,
    public submissionData: SubmissionData,
    public limitsData: LimitsData,
    public limitsValidation: LimitsValidationService,
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
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([nextUrl]);
        break;
    }
  }
}
