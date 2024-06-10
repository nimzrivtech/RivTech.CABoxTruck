import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import Utils from '../../../../shared/utilities/utils';
import { DotInfoDto, IDotInfoDto } from '../../../../shared/models/submission/riskspecifics/dot-info.dto';
import { SubmissionData } from '../submission.data';
import { DotInfoService } from '../../../../core/services/submission/risk-specifics/dot-info.service';
import { ToastrService } from 'ngx-toastr';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';

@Injectable()
export class DotInfoData {
  dotInfoForm: FormGroup;
  datepickerOptions: IAngularMyDpOptions;
  currentServerDateTime: Date;
  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    private dotInfoPageService: DotInfoService
  ) { }

  initiateFormGroup(obj: IDotInfoDto): FormGroup {
    this.submissionData.getCurrentServerDateTime2().subscribe((date) => {
      this.currentServerDateTime = date;
    });
    this.dotInfoForm = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      doesApplicantRequireDotNumber: [this.setInitialValue(obj.doesApplicantRequireDotNumber, false)],
      doesApplicantPlanToStayInterstate: [this.setInitialValue(obj.doesApplicantPlanToStayInterstate, true)],
      doesApplicantHaveInsterstateAuthority: [this.setInitialValue(obj.doesApplicantHaveInsterstateAuthority, true)],
      dotRegistrationDate: [this.formatDateForPicker(this.setInitialValue((obj.dotRegistrationDate ?? this.submissionData.currentServerDateTime ?? this.currentServerDateTime), new Date(this.currentServerDateTime))), [Validators.required]],
      isDotCurrentlyActive: [this.setInitialValue(obj.isDotCurrentlyActive, true)],
      chameleonIssues: [this.setInitialValue(obj.chameleonIssues, null), [Validators.required]],
      currentSaferRating: [this.setInitialValue(obj.currentSaferRating, null), [Validators.required]],
      issCabRating: [this.setInitialValue(obj.issCabRating, null), [Validators.required]],
      isThereAnyPolicyLevelAccidents: [this.setInitialValue(obj.isThereAnyPolicyLevelAccidents, false)],
      numberOfPolicyLevelAccidents: [this.setInitialValue(obj.numberOfPolicyLevelAccidents, 0), [Validators.required]],
      isUnsafeDrivingChecked: [this.setInitialValue(obj.isUnsafeDrivingChecked, false)],
      isHoursOfServiceChecked: [this.setInitialValue(obj.isHoursOfServiceChecked, false)],
      isDriverFitnessChecked: [this.setInitialValue(obj.isDriverFitnessChecked, false)],
      isControlledSubstanceChecked: [this.setInitialValue(obj.isControlledSubstanceChecked, false)],
      isVehicleMaintenanceChecked: [this.setInitialValue(obj.isVehicleMaintenanceChecked, false)],
      isCrashChecked: [this.setInitialValue(obj.isCrashChecked, false)]
    }, {
    });

    this.datepickerOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
      disableSince: { year: 0, month: 0, day: 0 },
      disableUntil: { year: 0, month: 0, day: 0 }
    };

    return this.dotInfoForm;
  }

  populateDotInfoFields() {
    if (this.submissionData.riskDetail?.dotInfo) {
      this.initiateFormGroup(this.submissionData.riskDetail?.dotInfo);
    }
  }

  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length === 0) ? val : obj;
  }

  initiateFormValues(fb: FormGroup) {
  }

  saveDotInfo() {
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail.id;
    const formValue = this.dotInfoForm.value;
    const updatedDotInfo: IDotInfoDto = new DotInfoDto(formValue);
    updatedDotInfo.dotRegistrationDate = formValue.dotRegistrationDate?.singleDate?.jsDate?.toLocaleDateString();
    if (updatedDotInfo.doesApplicantPlanToStayInterstate) {
      updatedDotInfo.doesApplicantHaveInsterstateAuthority = null;
    }
    if (!updatedDotInfo.isThereAnyPolicyLevelAccidents || !updatedDotInfo.numberOfPolicyLevelAccidents) {
      updatedDotInfo.numberOfPolicyLevelAccidents = 0;
    }

    this.dotInfoPageService.updateDotInfo(riskDetailId, updatedDotInfo).subscribe(() => {
      Utils.unblockUI();
      this.submissionData.riskDetail.dotInfo = updatedDotInfo;
      console.log('DotInfo Saved!');
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
    return;
  }

  private formatDateForPicker(dateValue?: any) {
    const dateSet = dateValue ? new Date(dateValue) : new Date(this.currentServerDateTime);
    return { isRange: false, singleDate: { jsDate: dateSet } };
  }

}
