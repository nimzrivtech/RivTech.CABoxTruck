import { Injectable } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { RiskHistoryDTO, RiskHistoryListDTO } from '../../../../shared/models/submission/coverages/riskHistoryDto';
import { take } from 'rxjs/operators';
import { CoverageHistoryService } from '../../services/coverage-history.service';
import { SubmissionData } from '../submission.data';
import Utils from '../../../../shared/utilities/utils';
import { ToastrService } from 'ngx-toastr';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { currentPriorInvalid, historicalFieldRequired, historicalFieldNoZeroInputRequired, lessThanStartInvalidYear, lessThan6MonthsStartInvalidYear, policyTermValidator, historicalRequired } from '../../helpers/historicalRequired.validator';
import { LimitsData } from '../limits/limits.data';
import { DatePipe } from '@angular/common';
import { RadiusOfOperationsData } from '../riskspecifics/radius-of-operations.data';
import { BrokerGenInfoData } from '../broker/general-information.data';
import { ApplicantData } from '../applicant/applicant.data';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';

export interface CoverageForms {
  historicalCoverageForm?: FormGroup;
}

@Injectable()
export class CoverageData {
  // All forms
  coverageForms: CoverageForms;

  dateOptions: IAngularMyDpOptions;
  isDriverGreaterThanTen: boolean = false;
  isVehicleGreaterThanTen: boolean = false;
  isTouched: boolean = false;

  year = (new Date()).getFullYear();

  constructor(
    private formBuilder: FormBuilder,
    private coverageHistoryService: CoverageHistoryService,
    public submissionData: SubmissionData,
    private limitsData: LimitsData,
    private toastr: ToastrService,
    private radiusOfOPerationsData: RadiusOfOperationsData,
    private brokerData: BrokerGenInfoData,
    private applicantData: ApplicantData
  ) { }

  initiateFormFields() {
    this.isTouched = false;
    this.coverageForms = {};
    this.coverageForms.historicalCoverageForm = this.formBuilder.group({
      policyTermStartDate: this.formBuilder.array([null, null, null, null, null]),
      policyTermEndDate: this.formBuilder.array([null, null, null, null, null]),
      insuranceCarrierOrBroker: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
      liabilityLimits: this.formBuilder.array([null, null, null, null, null]),
      liabilityDeductible: this.formBuilder.array([null, null, null, null, null]),
      collisionDeductible: this.formBuilder.array([null, null, null, null, null]),
      comprehensiveDeductible: this.formBuilder.array([null, null, null, null, null]),
      cargoLimits: this.formBuilder.array([null, null, null, null, null]),
      refrigeratedCargoLimits: this.formBuilder.array([null, null, null, null, null]),
      autoLiabilityPremiumPerVehicle: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
      physicalDamagePremiumPerVehicle: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
      totalPremiumPerVehicle: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
      numberOfVehicles: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
      numberOfPowerUnits: this.formBuilder.array([null, null, null, null, null]),
      numberOfPowerUnitsAL: this.formBuilder.array([null, null, null, null, null]),
      numberOfPowerUnitsAPD: this.formBuilder.array([null, null, null, null, null]),
      numberOfSpareVehicles: this.formBuilder.array([null, null, null, null, null]),
      numberOfDrivers: this.formBuilder.array([null, null, null, null, null]),
      lossRunDateForEachTerm: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
      grossRevenues: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
      totalFleetMileage: this.formBuilder.array([null, null, null, null, null]),
      numberOfOneWayTrips: this.formBuilder.array([null, null, null, null, null]), // historicalFieldRequired
    },
      {
        validators:
          [
            lessThanStartInvalidYear('policyTermStartDate', 'policyTermEndDate'),
            currentPriorInvalid(),
            // lessThan6MonthsStartInvalidYear('lossRunDateForEachTerm'),
            policyTermValidator('policyTermStartDate', this.brokerData),
            policyTermValidator('policyTermEndDate', this.brokerData),
            historicalRequired('liabilityDeductible')
          ]
      });
  }

  resetFormValidators() {
    if (this.limitsData.isCollision) {
      this.setAsRequired('collisionDeductible');
    } else {
      this.resetField('collisionDeductible');
    }
    if (this.limitsData.isComprehensive) {
      this.setAsRequired('comprehensiveDeductible');
    } else {
      this.resetField('comprehensiveDeductible');
    }
    if (this.limitsData.isCargo) {
      this.setAsRequired('cargoLimits');
    } else {
      this.resetField('cargoLimits');
    }
    if (this.limitsData.isRefrigeration) {
      this.setAsRequired('refrigeratedCargoLimits');
    } else {
      this.resetField('refrigeratedCargoLimits');
    }

    this.isDriverGreaterThanTen = false;
    this.isVehicleGreaterThanTen = false;
    if (this.submissionData.riskDetail.drivers?.length > 10) {
      this.isDriverGreaterThanTen = true;
      this.setAsRequired('numberOfDrivers');
    } else {
      this.resetField('numberOfDrivers');
    }
    if (this.submissionData.riskDetail.vehicles?.length > 10) {
      this.isVehicleGreaterThanTen = true;
      this.setAsRequired('numberOfSpareVehicles');
    } else {
      this.resetField('numberOfSpareVehicles');
    }
  }

  setHistoricalPageCheck() {
    this.isDriverGreaterThanTen = false;
    this.isVehicleGreaterThanTen = false;
    if (this.submissionData.riskDetail.drivers?.length > 10) {
      this.isDriverGreaterThanTen = true;
      this.setAsRequired('numberOfDrivers');
    }

    if (this.submissionData.riskDetail.vehicles?.length > 10) {
      this.isVehicleGreaterThanTen = true;
      this.setAsRequired('numberOfSpareVehicles');
    }

    if (this.submissionData.riskDetail.riskCoverage == null && this.submissionData.riskDetail.riskCoverages !== null && this.submissionData.riskDetail.riskCoverages.length > 0) {
      const option1 = this.submissionData.riskDetail.riskCoverages.find(x => x.optionNumber === 1);
      this.submissionData.riskDetail.riskCoverage = option1;
    }
    if (this.submissionData.riskDetail.riskCoverage) {
      if (this.submissionData.riskDetail.riskCoverage.compDeductibleId && this.submissionData.riskDetail.riskCoverage.fireDeductibleId == null && this.submissionData.riskDetail.riskCoverage.compDeductibleId !== 1) {
        this.limitsData.isComprehensive = true;
        this.setAsRequired('comprehensiveDeductible');
      }
      if (this.submissionData.riskDetail.riskCoverage.cargoLimitId && this.submissionData.riskDetail.riskCoverage.cargoLimitId !== 1) {
        this.limitsData.isCargo = true;
        this.setAsRequired('cargoLimits');
      }
      if (this.submissionData.riskDetail.riskCoverage.refCargoLimitId && this.submissionData.riskDetail.riskCoverage.refCargoLimitId !== 1) {
        this.limitsData.isRefrigeration = true;
        this.setAsRequired('refrigeratedCargoLimits');
      }
      if (this.submissionData.riskDetail.riskCoverage.collDeductibleId && this.submissionData.riskDetail.riskCoverage.collDeductibleId !== 1) {
        this.limitsData.isCollision = true;
        this.setAsRequired('collisionDeductible');
      }
    }
  }

  private resetField(fieldName: string) {
    // this.coverageForms.historicalCoverageForm.get(fieldName).reset();
    // this.coverageForms.historicalCoverageForm.get(fieldName).clearValidators();
    // this.coverageForms.historicalCoverageForm.get(fieldName).updateValueAndValidity();
  }

  private setAsRequired(fieldName: string) {
    // this.coverageForms.historicalCoverageForm.get(fieldName).setValidators(historicalFieldRequired);
    // this.coverageForms.historicalCoverageForm.get(fieldName).updateValueAndValidity();
  }

  populateHistoryFields() {
    this.setHistoricalPageCheck();
    if (this.submissionData.riskDetail.riskHistory.riskHistories.length > 0) {
      const datePipe = new DatePipe('en-US');
      const policyTermStartDate: any = [];
      const policyTermEndDate: any = [];
      const insuranceCarrierOrBroker: any = [];
      const liabilityLimits: any = [];
      const liabilityDeductible: any = [];
      const collisionDeductible: any = [];
      const comprehensiveDeductible: any = [];
      const cargoLimits: any = [];
      const refrigeratedCargoLimits: any = [];
      const autoLiabilityPremiumPerVehicle: any = [];
      const physicalDamagePremiumPerVehicle: any = [];
      const totalPremiumPerVehicle: any = [];
      const numberOfVehicles: any = [];
      const numberOfPowerUnits: any = [];
      const numberOfPowerUnitsAL: any = [];
      const numberOfPowerUnitsAPD: any = [];
      const numberOfSpareVehicles: any = [];
      const numberOfDrivers: any = [];
      const lossRunDateForEachTerm: any = [];
      const grossRevenues: any = [];
      const totalFleetMileage: any = [];
      const numberOfOneWayTrips: any = [];

      this.submissionData.riskDetail.riskHistory.riskHistories.forEach(property => {
        policyTermStartDate.splice(property.historyYear, 0, property.policyTermStartDate ? { isRange: false, singleDate: { jsDate: new Date(property.policyTermStartDate) } } : null);
        policyTermEndDate.splice(property.historyYear, 0, property.policyTermEndDate ? { isRange: false, singleDate: { jsDate: new Date(property.policyTermEndDate) } } : null);
        insuranceCarrierOrBroker.splice(property.historyYear, 0, property.insuranceCarrierOrBroker);
        liabilityLimits.splice(property.historyYear, 0, property.liabilityLimits);
        liabilityDeductible.splice(property.historyYear, 0, property.liabilityDeductible);
        collisionDeductible.splice(property.historyYear, 0, property.collisionDeductible);
        comprehensiveDeductible.splice(property.historyYear, 0, property.comprehensiveDeductible);
        cargoLimits.splice(property.historyYear, 0, property.cargoLimits);
        refrigeratedCargoLimits.splice(property.historyYear, 0, property.refrigeratedCargoLimits);
        autoLiabilityPremiumPerVehicle.splice(property.historyYear, 0, property.autoLiabilityPremiumPerVehicle);
        physicalDamagePremiumPerVehicle.splice(property.historyYear, 0, property.physicalDamagePremiumPerVehicle);
        totalPremiumPerVehicle.splice(property.historyYear, 0, property.totalPremiumPerVehicle);
        numberOfVehicles.splice(property.historyYear, 0, property.numberOfVehicles);
        numberOfPowerUnits.splice(property.historyYear, 0, property.numberOfPowerUnits);
        numberOfPowerUnitsAL.splice(property.historyYear, 0, property.numberOfPowerUnitsAL);
        numberOfPowerUnitsAPD.splice(property.historyYear, 0, property.numberOfPowerUnitsAPD);
        numberOfSpareVehicles.splice(property.historyYear, 0, property.numberOfSpareVehicles);
        numberOfDrivers.splice(property.historyYear, 0, property.numberOfDrivers);
        lossRunDateForEachTerm.splice(property.historyYear, 0, property.lossRunDateForEachTerm ? { isRange: false, singleDate: { jsDate: new Date(property.lossRunDateForEachTerm) } } : null);
        grossRevenues.splice(property.historyYear, 0, property.grossRevenues);
        totalFleetMileage.splice(property.historyYear, 0, property.totalFleetMileage);
        numberOfOneWayTrips.splice(property.historyYear, 0, property.numberOfOneWayTrips);
      });

      this.coverageForms.historicalCoverageForm.patchValue({
        policyTermStartDate: policyTermStartDate,
        policyTermEndDate: policyTermEndDate,
        insuranceCarrierOrBroker: insuranceCarrierOrBroker,
        liabilityLimits: liabilityLimits,
        liabilityDeductible: liabilityDeductible,
        collisionDeductible: collisionDeductible,
        comprehensiveDeductible: comprehensiveDeductible,
        cargoLimits: cargoLimits,
        refrigeratedCargoLimits: refrigeratedCargoLimits,
        autoLiabilityPremiumPerVehicle: autoLiabilityPremiumPerVehicle,
        physicalDamagePremiumPerVehicle: physicalDamagePremiumPerVehicle,
        totalPremiumPerVehicle: totalPremiumPerVehicle,
        numberOfVehicles: numberOfVehicles,
        numberOfPowerUnits: numberOfPowerUnits,
        numberOfPowerUnitsAL: numberOfPowerUnitsAL,
        numberOfPowerUnitsAPD: numberOfPowerUnitsAPD,
        numberOfSpareVehicles: numberOfSpareVehicles,
        numberOfDrivers: numberOfDrivers,
        lossRunDateForEachTerm: lossRunDateForEachTerm,
        grossRevenues: grossRevenues,
        totalFleetMileage: totalFleetMileage,
        numberOfOneWayTrips: numberOfOneWayTrips,
      });
    }
  }

  retrieveDropDownValues() {
  }

  // Services
  saveRiskHistory(callbackFn?: Function) {
    const histories: RiskHistoryDTO[] = [];
    for (let i = 0; i < this.coverageForms.historicalCoverageForm.value.policyTermStartDate.length; i++) {
      histories.push(new RiskHistoryDTO({
        historyYear: i,
        policyTermStartDate: this.coverageForms.historicalCoverageForm.value.policyTermStartDate[i]?.singleDate?.jsDate?.toLocaleDateString() || null,
        policyTermEndDate: this.coverageForms.historicalCoverageForm.value.policyTermEndDate[i]?.singleDate?.jsDate?.toLocaleDateString() || null,
        insuranceCarrierOrBroker: this.coverageForms.historicalCoverageForm.value.insuranceCarrierOrBroker[i],
        liabilityLimits: this.coverageForms.historicalCoverageForm.value.liabilityLimits[i],
        liabilityDeductible: this.coverageForms.historicalCoverageForm.value.liabilityDeductible[i],
        collisionDeductible: this.limitsData.isCollision ? this.coverageForms.historicalCoverageForm.value.collisionDeductible[i] : null,
        comprehensiveDeductible: this.limitsData.isComprehensive ? this.coverageForms.historicalCoverageForm.value.comprehensiveDeductible[i] : null,
        cargoLimits: this.limitsData.isCargo ? this.coverageForms.historicalCoverageForm.value.cargoLimits[i] : null,
        refrigeratedCargoLimits: this.limitsData.isRefrigeration ? this.coverageForms.historicalCoverageForm.value.refrigeratedCargoLimits[i] : null,
        autoLiabilityPremiumPerVehicle: this.coverageForms.historicalCoverageForm.value.autoLiabilityPremiumPerVehicle[i],
        physicalDamagePremiumPerVehicle: this.coverageForms.historicalCoverageForm.value.physicalDamagePremiumPerVehicle[i],
        totalPremiumPerVehicle: this.coverageForms.historicalCoverageForm.value.totalPremiumPerVehicle[i],
        numberOfVehicles: parseInt(this.coverageForms.historicalCoverageForm.value.numberOfVehicles[i]),
        numberOfPowerUnits: parseInt(this.coverageForms.historicalCoverageForm.value.numberOfPowerUnits[i]),
        numberOfSpareVehicles: this.isVehicleGreaterThanTen && this.coverageForms.historicalCoverageForm.value.numberOfSpareVehicles[i] ? parseInt(this.coverageForms.historicalCoverageForm.value.numberOfSpareVehicles[i]) : null,
        numberOfDrivers: this.isDriverGreaterThanTen && this.coverageForms.historicalCoverageForm.value.numberOfDrivers[i] ? parseInt(this.coverageForms.historicalCoverageForm.value.numberOfDrivers[i]) : null,
        lossRunDateForEachTerm: this.coverageForms.historicalCoverageForm.value.lossRunDateForEachTerm[i]?.singleDate?.jsDate?.toLocaleDateString() || null,
        grossRevenues: this.coverageForms.historicalCoverageForm.value.grossRevenues[i],
        totalFleetMileage: parseInt(this.coverageForms.historicalCoverageForm.value.totalFleetMileage[i]),
        numberOfOneWayTrips: this.coverageForms.historicalCoverageForm.value.numberOfOneWayTrips[i],
      }));
    }
    Utils.blockUI();
    this.coverageHistoryService.saveCoverageHistory(new RiskHistoryListDTO({ riskDetailId: this.submissionData.riskDetail.id, riskHistories: histories })).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.submissionData.riskDetail.riskHistory = data;
      if (!this.submissionData.isIssued) {
        this.radiusOfOPerationsData.setAverageMilesPerVehicle();
      }
      if (callbackFn) { callbackFn(); }
      console.log('History Saved!');
    }, (error) => {
      Utils.unblockUI();
      if (callbackFn) { callbackFn(); }
      console.log(error);
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
  }

  initDateOptions() {
    this.dateOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
    };
  }

  get required() {
    const yearsInBusiness = this.applicantData.applicantForms.businessDetails.controls['yearBusinessInput']?.value;
    return yearsInBusiness > 0;
  }

  setRequiredValidator() {
    const requiredFields = ['policyTermStartDate', 'policyTermEndDate', 'liabilityLimits',
      'numberOfPowerUnits', 'totalFleetMileage'];

    this.setValidator(requiredFields, this.coverageForms.historicalCoverageForm);
  }

  setValidator(requiredFields: string[], form: FormGroup) {
    if (this.required) {
      this.addRequiredValidator(requiredFields, form);
    } else {
      this.removeRequiredValidator(requiredFields, form);
    }
  }

  private removeRequiredValidator(fieldNames: string[], form: FormGroup) {
    fieldNames.forEach(f => {
      form.get(f).clearValidators();
      form.get(f).updateValueAndValidity();
    });
  }

  private addRequiredValidator(fieldNames: string[], form: FormGroup) {
    fieldNames.forEach(f => {
      if(f === 'liabilityDeductible'){
        form.get(f).setValidators(historicalFieldNoZeroInputRequired);
      }
      else{
        form.get(f).setValidators(historicalFieldRequired);
      }
      form.get(f).updateValueAndValidity();
    });
  }

  //Bug: Empty field still has zero value
  //Workaround: Reset value to "" upon clicking backspace
  public onKeyUpEraseAtInputField(event: any, coverageForm: any){
    if (event.key === "Backspace" || event.key === "Delete")
    {
      const inputValue = coverageForm.controls[0]?.value;
      if (inputValue === '0' || inputValue=== 0){  
        coverageForm.controls[0]?.setValue("");
      }
    }
  }
}
