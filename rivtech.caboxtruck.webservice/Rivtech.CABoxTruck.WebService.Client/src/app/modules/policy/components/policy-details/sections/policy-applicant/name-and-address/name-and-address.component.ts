import { Component, OnInit } from '@angular/core';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';
import { PathConstants } from '@app/shared/constants/path.constants';
import { Observable, of, throwError } from 'rxjs';
import { catchError, finalize, map, switchMap, tap } from 'rxjs/operators';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { createPolicyDetailsMenuItems } from '../../../policy-details-navitems';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import FormUtils from '@app/shared/utilities/form.utils';
import Utils from '@app/shared/utilities/utils';
import NotifUtils from '@app/shared/utilities/notif-utils';

@Component({
  selector: 'app-name-and-address',
  templateUrl: './name-and-address.component.html',
  styleUrls: ['./name-and-address.component.scss']
})
export class NameAndAddressComponent implements OnInit, CanComponentDeactivate {
  tempriskHeaderId: any;

  constructor(private navigationService: NavigationService,
    private layoutService: LayoutService,
    private applicantData: ApplicantData,
    private submissionData: SubmissionData,
    private riskSpecificsData: RiskSpecificsData,
    private policySummaryData: PolicySummaryData) { }

  ngOnInit() {
  }

  canDeactivate(): Observable<boolean> | boolean {
    Utils.blockUI();
    return this.savingUponNavigate()
      .pipe(
        map(val => {
          Utils.unblockUI();
          return true;
        }),
        catchError(error => {
          console.log('Error saving upon navigate.');
          NotifUtils.showError("Error while saving Name and Address.");
          return of(false); //return from(['A','B','C'])
        })
      );
  }

  savingUponNavigate(): Observable<any> {
    console.log('Saving upon navigate from Name and Address....');
    this.submissionData.validationList.applicantNameAndAddress = this.checkNameAndAddressSection;
    this.submissionData.validationList.applicantNameAndAddressBinding = this.checkNameAndAddressBindingSection;
    this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));

    if (this.policySummaryData.canEditPolicy) {
      return this.applicantData.updateNameAndAddressMidterm$().pipe(
        switchMap(() => this.policySummaryData.updateSubtatusForPendingEndorsement$()),
        catchError((err) => throwError(err)),
        finalize(() => {
          this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
          console.log('Saving upon navigate from Name and Address finished!');

        })
      )
    }
    return of({});
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Billing);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.BusinessDetails}`);
        break;
    }
  }

  get checkNameAndAddressSection() {
    this.applicantData.applicantForms.nameAndAddressForm.markAllAsTouched();
    return FormUtils.validateForm(this.applicantData.applicantForms.nameAndAddressForm);
  }

  get checkNameAndAddressBindingSection() {
    this.applicantData.applicantForms.billingInfoForm.markAllAsTouched();
    return FormUtils.validateForm(this.applicantData.applicantForms.billingInfoForm);
  }
}
