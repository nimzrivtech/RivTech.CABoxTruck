import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DriverInfoPageService } from '../../../../core/services/submission/risk-specifics/driver-info-page.service';
import { DriverHiringCriteriaDto, IDriverHiringCriteriaDto } from '../../../../shared/models/submission/riskspecifics/driver-hiring-criteria.dto';
import { IDriverInfoPageDto } from '../../../../shared/models/submission/riskspecifics/driver-info-page.dto';
import Utils from '../../../../shared/utilities/utils';
import { DriverInfoDto, IDriverInfoDto } from '../../../../shared/models/submission/riskspecifics/driver-info.dto';
import { SubmissionData } from '../submission.data';
import { DriverHiringCriteriaData } from './driver-hiring-criteria.data';
import { ToastrService } from 'ngx-toastr';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';

@Injectable()
export class DriverInfoData {
  driverInfoForm: FormGroup;
  shouldShowDriverHiringCriteria: boolean = false;
  isPolicyUI: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    private driverHiringCriteriaData: DriverHiringCriteriaData,
    private driverInfoPageService: DriverInfoPageService
  ) { }

  initiateFormGroup(obj: IDriverInfoDto): FormGroup {
    const driversCount = this.driversCount;
    this.driverInfoForm = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      totalNumberOfDrivers: [this.setInitialValue(driversCount, 0)], // Validators.required
      driversHired: [this.setInitialValue(obj.driversHired, 0)], // Validators.required
      driversTerminated: [this.setInitialValue(obj.driversTerminated, 0)], // Validators.required
      mvrPullingFrequency: [this.setInitialValue(obj.mvrPullingFrequency, null)], // Validators.required
      isBusinessPrincipalADriverInPolicy: [this.setInitialValue(obj.isBusinessPrincipalADriverInPolicy, false)],
      areVolunteersUsedInBusiness: [this.setInitialValue(obj.areVolunteersUsedInBusiness, false)],
      percentageOfStaff: [this.setInitialValue(obj.percentageOfStaff, 0)],
      isHiringFromOthers: [this.setInitialValue(obj.isHiringFromOthers, false)],
      annualCostOfHire: [this.setInitialValue(obj.annualCostOfHire, 0)],
      isHiringFromOthersWithoutDriver: [this.setInitialValue(obj.isHiringFromOthersWithoutDriver, false)],
      annualCostOfHireWithoutDriver: [this.setInitialValue(obj.annualCostOfHireWithoutDriver, 0)],
      isLeasingToOthers: [this.setInitialValue(obj.isLeasingToOthers, false)],
      annualIncomeDerivedFromLease: [this.setInitialValue(obj.annualIncomeDerivedFromLease, 0)],
      isLeasingToOthersWithoutDriver: [this.setInitialValue(obj.isLeasingToOthersWithoutDriver, false)],
      annualIncomeDerivedFromLeaseWithoutDriver: [this.setInitialValue(obj.annualIncomeDerivedFromLeaseWithoutDriver, 0)],
      isThereAssumedLiabilityByContract: [this.setInitialValue(obj.isThereAssumedLiabilityByContract, false)],
      doDriversTakeVehiclesHome: [this.setInitialValue(obj.doDriversTakeVehiclesHome, false)],
      areVehiclesSolelyOwnedByApplicant: [this.setInitialValue(obj.areVehiclesSolelyOwnedByApplicant, false)],
      willBeAddingDeletingVehiclesDuringTerm: [this.setInitialValue(obj.willBeAddingDeletingVehiclesDuringTerm, false)],
      addingDeletingVehiclesDuringTermDescription: [this.setInitialValue(obj.addingDeletingVehiclesDuringTermDescription, '')],
    }, {
    });

    this.shouldShowDriverHiringCriteria = driversCount > 2;
    this.driverInfoForm.controls.totalNumberOfDrivers.valueChanges.subscribe(() => this.checkIfDriverHiringCriteriaShouldBeShown());

    return this.driverInfoForm;
  }

  populateDrivingInfoField() {
    if (this.submissionData.riskDetail?.driverInfo) {
      this.initiateFormGroup(this.submissionData.riskDetail?.driverInfo);
    }
  }

  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length === 0) ? val : obj;
  }

  initiateFormValues(fb: FormGroup) {
  }

  private checkIfDriverHiringCriteriaShouldBeShown() {
    this.shouldShowDriverHiringCriteria = this.driverInfoForm.controls.totalNumberOfDrivers.value > 2;
  }

  saveDrivingInfo() {
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail.id;
    const updatedDriverInfo: IDriverInfoDto = new DriverInfoDto(this.driverInfoForm.value);
    updatedDriverInfo.riskDetailId = riskDetailId;
    if (!updatedDriverInfo.areVolunteersUsedInBusiness) {
      updatedDriverInfo.percentageOfStaff = 0;
    }
    if (!updatedDriverInfo.isHiringFromOthers) {
      updatedDriverInfo.annualCostOfHire = 0;
      updatedDriverInfo.isHiringFromOthersWithoutDriver = false;
    }
    if (!updatedDriverInfo.isHiringFromOthersWithoutDriver) {
      updatedDriverInfo.annualCostOfHireWithoutDriver = 0;
    }
    if (!updatedDriverInfo.isLeasingToOthers) {
      updatedDriverInfo.annualIncomeDerivedFromLease = 0;
      updatedDriverInfo.isLeasingToOthersWithoutDriver = false;
    }
    if (!updatedDriverInfo.isLeasingToOthersWithoutDriver) {
      updatedDriverInfo.annualIncomeDerivedFromLeaseWithoutDriver = 0;
    }
    if (!updatedDriverInfo.willBeAddingDeletingVehiclesDuringTerm ||
      updatedDriverInfo.addingDeletingVehiclesDuringTermDescription === '') {
      updatedDriverInfo.addingDeletingVehiclesDuringTermDescription = null;
    }


    let updatedDriverHiringCriteria: IDriverHiringCriteriaDto;

    if (this.driverHiringCriteriaData.driverHiringCriteriaForm) {
      updatedDriverHiringCriteria = this.shouldShowDriverHiringCriteria
        ? new DriverHiringCriteriaDto(this.driverHiringCriteriaData.driverHiringCriteriaForm.value)
        : new DriverHiringCriteriaDto();
      updatedDriverHiringCriteria.riskDetailId = riskDetailId;
    }

    const updatedDriverInfoPage: IDriverInfoPageDto = { driverInfo: updatedDriverInfo, driverHiringCriteria: updatedDriverHiringCriteria };

    this.driverInfoPageService.updateDriverInfoPage(riskDetailId, updatedDriverInfoPage).subscribe(() => {
      Utils.unblockUI();
      this.submissionData.riskDetail.driverInfo = updatedDriverInfo;
      this.submissionData.riskDetail.driverHiringCriteria = updatedDriverHiringCriteria;
      console.log('Driver Info Hiring Saved!');
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
    return;
  }

  get driversCount() {
    const riskDetail = this.submissionData.riskDetail;
    const drivers = riskDetail?.drivers;
    if (this.isPolicyUI) {
      const bindOptionId = riskDetail?.binding?.bindOptionId;
      return drivers?.filter(driver => driver.isValidated && driver.options?.split(',')?.includes(`${bindOptionId}`))?.length;
    } else {
      return drivers?.filter(driver => driver.isValidated)?.length;
    }
  }
}
