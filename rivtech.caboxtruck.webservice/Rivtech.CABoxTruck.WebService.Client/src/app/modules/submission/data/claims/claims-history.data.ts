import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClaimsHistoryDto } from '../../../../shared/models/submission/claimsHistory/claimsHistoryDto';
import { historicalFieldRequired, lessThan6MonthsStartInvalidYear } from '../../helpers/historicalRequired.validator';


import { ClaimsHistoryService } from '../../services/claims-history.service';
import { take } from 'rxjs/operators';

import { ToastrService } from 'ngx-toastr';
import { SubmissionData } from '../submission.data';
import Utils from '../../../../shared/utilities/utils';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { CoverageData } from '../coverages/coverages.data';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';

@Injectable()
export class ClaimsHistoryoData {
  formInfo: FormGroup;

  brokerInfoDto: ClaimsHistoryDto = new ClaimsHistoryDto();
  datePickerOptions: IAngularMyDpOptions;
  isTouched: boolean = false;

  constructor(private formBuilder: FormBuilder,
    private claimsHistoryService: ClaimsHistoryService,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    private coverageData: CoverageData
  ) { }

  initiateFormFields() {
    this.datePickerOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
      disableSince: { year: 0, month: 0, day: 0 },
      disableUntil: { year: 0, month: 0, day: 0 }
    };
    this.isTouched = false;
    this.formInfo = this.formBuilder.group({
      id: this.formBuilder.array([null, null, null, null, null]),
      lossRunDate: this.formBuilder.array([null, null, null, null, null]),
      groundUpNet: this.formBuilder.array([null, null, null, null, null]),
      biPiPmpIncLossTotal: this.formBuilder.array([null, null, null, null, null]),
      biPiPmpIncLoss100KCap: this.formBuilder.array([null, null, null, null, null]),
      propertyDamageIncLoss: this.formBuilder.array([null, null, null, null, null]),
      claimCountBiPip: this.formBuilder.array([null, null, null, null, null]),
      claimCountPropertyDamage: this.formBuilder.array([null, null, null, null, null]),
      openClaimsUnder500: this.formBuilder.array([null, null, null, null, null]),
      physicalDamageNetLoss: this.formBuilder.array([null, null, null, null, null]),
      physicalDamageClaimCount: this.formBuilder.array([null, null, null, null, null]),
      cargoNetLoss: this.formBuilder.array([null, null, null, null, null]),
      cargoClaimCount: this.formBuilder.array([null, null, null, null, null]),
      generalLiabilityNetLoss: this.formBuilder.array([null, null, null, null, null]),
      generalLiabilityClaimCount: this.formBuilder.array([null, null, null, null, null]),
    }, { validators: [lessThan6MonthsStartInvalidYear('lossRunDate', this.coverageData.coverageForms)] });
  }

  populateHistoryFields() {
    if (this.submissionData.riskDetail?.claimsHistory?.length > 0) {

      const id: any = [];
      const lossRunDate: any = [];
      const groundUpNet: any = [];
      const biPiPmpIncLossTotal: any = [];
      const biPiPmpIncLoss100KCap: any = [];
      const propertyDamageIncLoss: any = [];
      const claimCountBiPip: any = [];
      const claimCountPropertyDamage: any = [];
      const openClaimsUnder500: any = [];
      const physicalDamageNetLoss: any = [];
      const physicalDamageClaimCount: any = [];
      const cargoNetLoss: any = [];
      const cargoClaimCount: any = [];
      const generalLiabilityNetLoss: any = [];
      const generalLiabilityClaimCount: any = [];

      this.submissionData.riskDetail?.claimsHistory?.forEach(property => {
        id.splice(property.order, 0, property.id);
        lossRunDate.splice(property.order, 0, property.lossRunDate ? { isRange: false, singleDate: { jsDate: new Date(property.lossRunDate) } } : null);
        groundUpNet.splice(property.order, 0, property.groundUpNet);
        biPiPmpIncLossTotal.splice(property.order, 0, property.biPiPmpIncLossTotal);
        biPiPmpIncLoss100KCap.splice(property.order, 0, property.biPiPmpIncLoss100KCap);
        propertyDamageIncLoss.splice(property.order, 0, property.propertyDamageIncLoss);
        claimCountBiPip.splice(property.order, 0, property.claimCountBiPip);
        claimCountPropertyDamage.splice(property.order, 0, property.claimCountPropertyDamage);
        openClaimsUnder500.splice(property.order, 0, property.openClaimsUnder500);
        physicalDamageNetLoss.splice(property.order, 0, property.physicalDamageNetLoss);
        physicalDamageClaimCount.splice(property.order, 0, property.physicalDamageClaimCount);
        cargoNetLoss.splice(property.order, 0, property.cargoNetLoss);
        cargoClaimCount.splice(property.order, 0, property.cargoClaimCount);
        generalLiabilityNetLoss.splice(property.order, 0, property.generalLiabilityNetLoss);
        generalLiabilityClaimCount.splice(property.order, 0, property.generalLiabilityClaimCount);
      });

      this.formInfo.patchValue({
        id: id,
        lossRunDate: lossRunDate,
        groundUpNet: groundUpNet,
        biPiPmpIncLossTotal: biPiPmpIncLossTotal,
        biPiPmpIncLoss100KCap: biPiPmpIncLoss100KCap,
        propertyDamageIncLoss: propertyDamageIncLoss,
        claimCountBiPip: claimCountBiPip,
        claimCountPropertyDamage: claimCountPropertyDamage,
        openClaimsUnder500: openClaimsUnder500,
        physicalDamageNetLoss: physicalDamageNetLoss,
        physicalDamageClaimCount: physicalDamageClaimCount,
        cargoNetLoss: cargoNetLoss,
        cargoClaimCount: cargoClaimCount,
        generalLiabilityNetLoss: generalLiabilityNetLoss,
        generalLiabilityClaimCount: generalLiabilityClaimCount,
      });
    }

  }

  public initiateFormValues(obj: ClaimsHistoryDto) {
    this.formInfo.patchValue(obj);
  }

  public retrieveDropDownValues() {
  }

  save(callBackFn?: Function) {
    const claimsHistories: ClaimsHistoryDto[] = [];
    const formValue = this.formInfo.value;
    for (let i = 0; i <= 4; i++) {
      claimsHistories.push(new ClaimsHistoryDto({
        riskDetailId: this.submissionData.riskDetail?.id,
        order: i,
        id: this.formInfo.value.id[i],
        lossRunDate: this.formInfo.value.lossRunDate[i]?.singleDate?.jsDate?.toLocaleDateString() || null,
        groundUpNet: formValue.groundUpNet[i] === '' ? null : formValue.groundUpNet[i],
        biPiPmpIncLossTotal: this.formInfo.value.biPiPmpIncLossTotal[i],
        biPiPmpIncLoss100KCap: this.formInfo.value.biPiPmpIncLoss100KCap[i],
        propertyDamageIncLoss: this.formInfo.value.propertyDamageIncLoss[i],
        claimCountBiPip: this.formInfo.value.claimCountBiPip[i],
        claimCountPropertyDamage: this.formInfo.value.claimCountPropertyDamage[i],
        openClaimsUnder500: this.formInfo.value.openClaimsUnder500[i],
        physicalDamageNetLoss: this.formInfo.value.physicalDamageNetLoss[i],
        physicalDamageClaimCount: this.formInfo.value.physicalDamageClaimCount[i],
        cargoNetLoss: this.formInfo.value.cargoNetLoss[i],
        cargoClaimCount: this.formInfo.value.cargoClaimCount[i],
        generalLiabilityNetLoss: this.formInfo.value.generalLiabilityNetLoss[i],
        generalLiabilityClaimCount: this.formInfo.value.generalLiabilityClaimCount[i],
      }));
    }

    Utils.blockUI();
    this.claimsHistoryService.post(claimsHistories).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.submissionData.riskDetail.claimsHistory = data;
      if (callBackFn) { callBackFn(); }
    }, (error) => {
      Utils.unblockUI();
      console.log(error);
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });

  }

  get fc() { return this.fg.controls; }
  get fg() { return this.formInfo; }

  get required() {
    return this.coverageData.required;
  }

  setRequiredValidator() {
    const requiredFields = ['lossRunDate'];
    this.coverageData.setValidator(['lossRunDate'], this.formInfo);
  }

}
