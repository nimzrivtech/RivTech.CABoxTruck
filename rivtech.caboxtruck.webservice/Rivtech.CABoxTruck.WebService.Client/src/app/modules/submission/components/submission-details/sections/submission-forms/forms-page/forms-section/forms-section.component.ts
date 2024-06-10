import { Component, OnInit } from '@angular/core';
import { FormsData } from '../../../../../../../../modules/submission/data/forms/forms.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormUploadComponent } from './form-upload/form-upload.component';
import { FormDTO } from '@app/shared/models/submission/forms/FormDto';
import { FormHiredPhysdamComponent } from './form-popup/form-hired-physdam/form-hired-physdam.component';
import { FormDriverRequirementComponent } from './form-popup/form-driver-requirement/form-driver-requirement.component';
import { FormTerritoryExclusionComponent } from './form-popup/form-territory-exclusion/form-territory-exclusion.component';
import { FormExclusionRadiusComponent } from './form-popup/form-exclusion-radius/form-exclusion-radius.component';
import { FormTrailerInterchangeComponent } from './form-popup/form-trailer-interchange/form-trailer-interchange.component';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { ManuscriptsData } from '@app/modules/submission/data/manuscripts/manuscripts.data';
import { FormGeneralLiabilityComponent } from './form-popup/form-general-liability/form-general-liability.component';
import { FormMcs90Component } from './form-popup/form-mcs90/form-mcs90.component';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../shared/utilities/utils';
import { ErrorMessageConstant } from '../../../../../../../../shared/constants/error-message.constants';
import { RaterApiData } from '../../../../../../../../modules/submission/data/rater-api.data';
import { ReportData } from '@app/modules/submission/data/report/report.data';
import { AuthService } from '@app/core/services/auth.service';
import { FormLossPayeeComponent } from './form-popup/form-loss-payee/form-loss-payee.component';
import { FormPAaddcombFPBComponent } from './form-popup/form-pa-addcomb-fpb/form-pa-addcomb-fpb.component';
import { FormCoveredAutoDesignationComponent } from './form-popup/form-covered-auto-designation/form-covered-auto-designation.component';

@Component({
  selector: 'app-forms-section',
  templateUrl: './forms-section.component.html',
  styleUrls: ['./forms-section.component.scss']
})
export class FormsSectionComponent implements OnInit {
  modalRef: BsModalRef;
  hideMe: boolean = false;
  TableConstants = TableConstants;

  constructor(
    private modalService: BsModalService,
    public formsData: FormsData,
    private manuscriptData: ManuscriptsData,
    private raterApiData: RaterApiData,
    public submissionData: SubmissionData,
    public reportData: ReportData,
    private authService: AuthService) { }

  ngOnInit() {
    this.formsData.initiateFormFields();
    this.formsData.retrieveDropdownValues();
    Utils.blockUI();

    setTimeout(() => {
      this.formsData.populateFields$().subscribe(() => Utils.unblockUI());
    }, 1000);
  }

  openAddFormDialog() {
    this.modalRef = this.modalService.show(FormUploadComponent, {
      initialState: {
        isEdit: false
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md modal-dialog-centered'
    });
  }

  openEditOtherDialog(data: any, component: any) {
    this.modalRef = this.modalService.show(component, {
      initialState: {
        formId: data.formId,
        other: data.other
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md modal-dialog-centered'
    });
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  get formList() {
    return this.formsData.forms.filter(f => f.include);
  }

  includeChanged(control: FormDTO) {
    if (this.submissionData?.isIssued) { return; }
    this.openDialog(control);

    if (control.form?.id === 'ManuscriptEndorsement') {
      this.manuscriptData.updateById(control.id, control.isSelected);
    } else {
      // if (!control.isSelected) {
      //   this.formsData.updateRiskForm(control);
      // }
      if (!control.isSelected && this.formsData.isSubPhysicalDamageForm(control?.form?.id)) {
        this.formsData.updateRiskForm(control);
      }

      if (!this.formsData.isSubPhysicalDamageForm(control?.form?.id)) {
        this.formsData.updateRiskForm(control);
      }
    }
  }

  private openDialog(control: FormDTO) {

    const data = {
      formId: control.form?.id,
      other: control.other
    };

    if (!control.isSelected) { return; }

    switch (control.form?.id) {
      case 'ExclusionRadiusEndorsement': {
        this.openEditOtherDialog(data, FormExclusionRadiusComponent);
        break;
      }
      case 'HiredPhysicalDamage': {
        this.openEditOtherDialog(data, FormHiredPhysdamComponent);
        break;
      }
      case 'DriverRequirements': {
        this.openEditOtherDialog(data, FormDriverRequirementComponent);
        break;
      }
      case 'TerritoryExclusion': {
        this.openEditOtherDialog(data, FormTerritoryExclusionComponent);
        break;
      }
      case 'TrailerInterchangeCoverage': {
        this.openEditOtherDialog(data, FormTrailerInterchangeComponent);
        break;
      }
      case 'GeneralLiabilityEndorsement': {
        this.openEditOtherDialog(data, FormGeneralLiabilityComponent);
        break;
      }
      case 'MCS90': {
        this.openEditOtherDialog(data, FormMcs90Component);
        break;
      }
      case 'LossPayeeEndorsement': {
        this.openEditOtherDialog(data, FormLossPayeeComponent);
        break;
      }
      case 'PennsylvaniaAddCombFirstPartyBenefitsEndorsement': {
        this.openEditOtherDialog(data, FormPAaddcombFPBComponent);
        break;
      }
      case 'CoveredAutoDesignationSymbol': {
        this.openEditOtherDialog(data, FormCoveredAutoDesignationComponent);
        break;
      }
    }
  }

  viewForm(form: FormDTO) {
    this.formsData.viewForm(form);
  }

  viewFormDebugMode(form: FormDTO) {
    if (this.isDebugMode) { this.formsData.viewForm(form); }
  }

  viewForms() {
    Utils.blockUI();
    const bindOptionId = Number(this.submissionData?.riskDetail?.binding?.bindOptionId) ?? 1;
    // recalculate premium before issuing policy
    this.raterApiData.calculateForQuote(bindOptionId)
      .then(() => {
        this.formsData.refreshQuoteOptionsAndSummaryHeader();
        const estimatedPremium = this.submissionData?.riskDetail?.riskCoverages[bindOptionId - 1]?.riskCoveragePremium?.premium;
        // should prevent issuance if premium is 0;
        if (estimatedPremium < 1) { throw new Error('Estimated Premium is Invalid.'); }
        // generate packet
        this.formsData.viewForms(this.formList);
      }).catch((error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.packetGenerationErrorMsg);
      }).finally(() => {
        this.raterApiData.disableCalculatePremium = false;
      });
  }

  deleteUploadedForm(riskForm: FormDTO) {
    if (!this.actionsEnabled(riskForm)) { return; }
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${riskForm.form?.name}'?`, () => {
      this.formsData.deleteRiskForm(riskForm.id);
    });
  }

  actionsEnabled(form: FormDTO) {
    return form.form.isSupplementalDocument && form.createdBy == this.authService.getCurrentUserId();
  }

  get isDebugMode() {
    return this.submissionData.riskDetail?.nameAndAddress?.businessPrincipal === 'RIVTECHQA';
  }

}
