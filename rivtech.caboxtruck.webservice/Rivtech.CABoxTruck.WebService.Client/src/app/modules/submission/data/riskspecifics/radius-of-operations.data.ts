import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RadiusOfOperationsService } from '../../../../core/services/submission/risk-specifics/radius-of-operations.service';
import Utils from '../../../../shared/utilities/utils';
import { IRadiusOfOperationsDto, RadiusOfOperationsDto } from '../../../../shared/models/submission/riskspecifics/radius-of-operations.dto';
import { SubmissionData } from '../submission.data';
import { ToastrService } from 'ngx-toastr';
import { SelectItem } from '../../../../shared/models/dynamic/select-item';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';

@Injectable()
export class RadiusOfOperationsData {
  destinationList: any[];
  mileageList: SelectItem[] = [];
  radiusOfOperationsForm: FormGroup;
  radiusOfOperationNotEqualToHundred: boolean = false;

  ratingTypeList: SelectItem[] = [
    { label: 'Garaging Terr Only', value: 1 },
    { label: 'Primary Terr Only', value: 2 },
    { label: 'Weighted Average', value: 3 },
    { label: 'Worst Area', value: 4 },
    { label: 'Other', value: 5 },
  ];

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    private radiusOfOperationsService: RadiusOfOperationsService
    ) { }

  initiateFormGroup(obj: IRadiusOfOperationsDto): FormGroup {
    this.destinationList = [];
    this.radiusOfOperationsForm = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      zeroToFiftyMilesPercentage: [this.setInitialValue(obj.zeroToFiftyMilesPercentage, 0), []], // Validators.required
      fiftyOneToTwoHundredMilesPercentage: [this.setInitialValue(obj.fiftyOneToTwoHundredMilesPercentage, 0), []], // Validators.required
      twoHundredPlusMilesPercentage: [this.setInitialValue(obj.twoHundredPlusMilesPercentage, 0), []], // Validators.required
      ownerOperatorPercentage: [this.setInitialValue(obj.ownerOperatorPercentage, 0), [Validators.required]],
      ratingTypeId: [this.setInitialValue(obj.ratingTypeId, 3), [Validators.required]],
      anyDestinationToMexicoPlanned: [this.setInitialValue(obj.anyDestinationToMexicoPlanned, false)],
      anyNyc5BoroughsExposure: [this.setInitialValue(obj.anyNyc5BoroughsExposure, false)],
      exposureDescription: [this.setInitialValue(obj.exposureDescription, '')],
      averageMilesPerVehicle: [this.setInitialValue(obj.averageMilesPerVehicle, 'Unavailable')],
    }, { validators: this.validatePercentageOfTravel });

    this.radiusOfOperationsForm.controls.zeroToFiftyMilesPercentage.valueChanges.subscribe(() => this.checkIfRadiusOfOperationEqualToHundred());
    this.radiusOfOperationsForm.controls.fiftyOneToTwoHundredMilesPercentage.valueChanges.subscribe(() => this.checkIfRadiusOfOperationEqualToHundred());
    this.radiusOfOperationsForm.controls.twoHundredPlusMilesPercentage.valueChanges.subscribe(() => this.checkIfRadiusOfOperationEqualToHundred());

    this.generateMileageList();

    return this.radiusOfOperationsForm;
  }

  populateRadiusOperationField() {
    if (this.submissionData.riskDetail?.radiusOfOperations) {
      this.initiateFormGroup(this.submissionData.riskDetail?.radiusOfOperations);
    }

  }


  setAverageMilesPerVehicle() {
    const currentHistoricalCoverage = this.submissionData.riskDetail?.riskHistory?.riskHistories.find(rh => rh.historyYear === 0);
    const fleetMileage: number = currentHistoricalCoverage?.totalFleetMileage ? currentHistoricalCoverage?.totalFleetMileage : 0;
    const vehicleCount: number = currentHistoricalCoverage?.numberOfVehicles ? currentHistoricalCoverage?.numberOfVehicles : 0;

    let averageMilesPerVehicle = 'Unavailable';
    if (fleetMileage !== 0 && vehicleCount !== 0) {
      let averageMileage = Math.round((fleetMileage / vehicleCount) / 1000) * 1000;
      if (averageMileage === 0) {
        averageMileage = 1000; // Min data sets value
      }
      if (averageMileage > 150000) {
        averageMileage = 150000; // Max data sets value
      }
      averageMilesPerVehicle = String(averageMileage);
    }
    this.radiusOfOperationsForm?.patchValue({averageMilesPerVehicle: averageMilesPerVehicle});
    this.saveDestinationRadiusOperation();
  }

  private checkIfRadiusOfOperationEqualToHundred() {
    this.radiusOfOperationNotEqualToHundred =
      (this.radiusOfOperationsForm.controls.zeroToFiftyMilesPercentage.value +
        this.radiusOfOperationsForm.controls.fiftyOneToTwoHundredMilesPercentage.value +
        this.radiusOfOperationsForm.controls.twoHundredPlusMilesPercentage.value
      ) !== 100;
  }

  validatePercentageOfTravel(form: FormGroup) {
    const fifty = form.controls['zeroToFiftyMilesPercentage'].value;
    const fiftyOne = form.controls['fiftyOneToTwoHundredMilesPercentage'].value;
    const twoHundred = form.controls['twoHundredPlusMilesPercentage'].value;
    let total: number;
    total = Number(fifty) + Number(fiftyOne) + Number(twoHundred);
    if (+total === 0) { return { 'requiredPercentageOfTravel': true }; }
    return +total.toFixed(1) !== 100.0 ? { 'invalidPercentageOfTravel': true } : null;
  }

  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length === 0) ? val : obj;
  }

  initiateFormValues(fb: FormGroup) {
  }

  saveDestinationRadiusOperation() {
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail.id;
    const updatedRadiusOfOperations: IRadiusOfOperationsDto = new RadiusOfOperationsDto(this.radiusOfOperationsForm.value);
    updatedRadiusOfOperations.riskDetailId = riskDetailId;
    if (`${updatedRadiusOfOperations.zeroToFiftyMilesPercentage}` === '') { updatedRadiusOfOperations.zeroToFiftyMilesPercentage = 0; }
    if (`${updatedRadiusOfOperations.fiftyOneToTwoHundredMilesPercentage}` === '') { updatedRadiusOfOperations.fiftyOneToTwoHundredMilesPercentage = 0; }
    if (`${updatedRadiusOfOperations.twoHundredPlusMilesPercentage}` === '') { updatedRadiusOfOperations.twoHundredPlusMilesPercentage = 0; }
    if (!updatedRadiusOfOperations.anyNyc5BoroughsExposure || updatedRadiusOfOperations.exposureDescription == '') {
      updatedRadiusOfOperations.exposureDescription = null;
    }

    this.radiusOfOperationsService.updateRadiusOfOperations(updatedRadiusOfOperations).subscribe((updatedRadiusOfOperationsId: string) => {
      Utils.unblockUI();
      updatedRadiusOfOperations.id = updatedRadiusOfOperationsId;
      this.submissionData.riskDetail.radiusOfOperations = updatedRadiusOfOperations;
      console.log('Destination Radius of Operation Saved!');
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
    return;
  }

  private generateMileageList() {
    const mileageOptions = [
      { value: 'Unavailable', label: 'Unavailable' },
      { value: `Didn't Ask`, label: `Didn't Ask` }
    ];
    for (let i = 1; i <= 150; i++) {
      const mileageOption = (i * 1000).toString();
      mileageOptions.push({ value: mileageOption, label: mileageOption })
    }
    this.mileageList = mileageOptions;
  }
}
