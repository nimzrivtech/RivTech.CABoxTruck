import { Component, OnInit, ViewChild } from '@angular/core';
import { ClaimsHistoryoData } from '../../../../data/claims/claims-history.data';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { Router } from '@angular/router';
import { NextBackButtonComponent } from '../../submission-details-shared/components/next-back-button/next-back-button.component';
import { Location } from '@angular/common';
import { ClaimsHistoryDto } from '../../../../../../shared/models/submission/claimsHistory/claimsHistoryDto';
import { ClaimsHistoryValidationService } from '../../../../../../core/services/validations/claims-history-validation.service';
import { SubmissionNavValidateService } from '../../../../../../core/services/navigation/submission-nav-validate.service';


import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';
@Component({
  selector: 'app-claims-history',
  templateUrl: './claims-history.component.html',
  styleUrls: ['./claims-history.component.scss']
})
export class ClaimsHistoryComponent implements OnInit {
  constructor(public Data: ClaimsHistoryoData,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    private location: Location,
    public claimsValidation: ClaimsHistoryValidationService,
    public submissionData: SubmissionData,
    public formValidation: SubmissionNavValidateService,
  ) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  ngOnInit() {
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.Data.formInfo; }

  public onClick(clickType?) {
    this.nextBack.location = this.location;
    let url = '';
    switch (clickType) {
      case ClickTypes.Back:
        this.menuOverrideService.overrideSideBarBehavior();
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

  get isScroll() {
    // return this.Data.formInfo.controls['isBrokeredAccount'].value || this.Data.formInfo.controls['isIncumbentAgency'].value || this.Data.formInfo.controls['isMidtermMove'].value;
    return true;
  }
}
