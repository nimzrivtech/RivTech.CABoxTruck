import { Injectable } from '@angular/core';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { VehicleData } from '@app/modules/submission/data/vehicle/vehicle.data';
import FormUtils from '../../../shared/utilities/form.utils';
import { LimitsData } from '@app/modules/submission/data/limits/limits.data';

@Injectable()
export class VehicleValidationService {
  vehicleSection: boolean;
  
  get VehicleHeaderForm() {
    return this.vehicleData.vehicleHeaderForm;
  }

  constructor(public submissionData: SubmissionData,
    public vehicleData: VehicleData,
    public limitsData: LimitsData,) { }

  checkVehiclePage() {
    this.vehicleSection = this.checkVehicleSection();
  }

  checkVehicleHeader() {
    this.vehicleSection = this.checkVehicleHeaderDetails();
  }

  checkVehicleSection() {
    // put validation  requirements to display badge.
    this.VehicleHeaderForm.markAllAsTouched();
    var isValid = (!this.limitsData.isTrailerType || (this.limitsData.isTrailerType && FormUtils.validateFormControl(this.VehicleHeaderForm.controls['trailerTypeLimitId']) && this.VehicleHeaderForm.controls['trailerTypeLimitId'].value != 0))
      && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['mainUseDropdown']) && this.VehicleHeaderForm.controls['mainUseDropdown'].value != 0)
      && ((this.submissionData.riskDetail.raterVersionId < 7 || 
          (typeof this.submissionData.riskDetail.raterVersionId == 'undefined' || (this.submissionData.riskDetail.raterVersionId > 6 && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['secondaryBusinessGroupDropdown'])))))
          && this.VehicleHeaderForm.controls['secondaryBusinessGroupDropdown'].value != 0)
      && FormUtils.validateFormControl(this.VehicleHeaderForm.controls['accountCategoryDropdown']) 
      && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['accountCategoryUWDropdown']) && this.VehicleHeaderForm.controls['accountCategoryUWDropdown'].value != 0)
      && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['mainGVW']) && this.VehicleHeaderForm.controls['mainGVW'].value != 0);

      if(isValid == false)
      {
        return false;
      }

    if (this.submissionData.riskDetail?.vehicles?.filter(x => x.deletedDate == null).length > 0 && !this.submissionData.riskDetail?.vehicles?.find(x => !x.isValidated)) {
      const vehicles = this.submissionData.riskDetail?.vehicles.filter(x => x.options !== null && x.options !== undefined);
      if (vehicles.length === 0) {
        console.log('No selected options for vehicles');
        return false;
      }

      const bindOption = this.submissionData.riskDetail?.binding?.bindOptionId;
      if (bindOption !== null && bindOption !== undefined) {
        const vehiclesBind = this.submissionData.riskDetail?.vehicles.filter(x => x.options !== null && x.options.indexOf(bindOption) !== -1);
        if (vehiclesBind.length === 0) {
          console.log('No vehicle selected for bind option', bindOption);
          return false;
        }
      }

      return true;
    }
    return false;
  }

  checkVehicleHeaderDetails() {
    // put validation  requirements to display badge.
    this.VehicleHeaderForm.markAllAsTouched();
    var isValid = (!this.limitsData.isTrailerType || (this.limitsData.isTrailerType && FormUtils.validateFormControl(this.VehicleHeaderForm.controls['trailerTypeLimitId']) && this.VehicleHeaderForm.controls['trailerTypeLimitId'].value != 0))
      && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['mainUseDropdown']) && this.VehicleHeaderForm.controls['mainUseDropdown'].value != 0)
      && ((this.submissionData.riskDetail.raterVersionId < 7 || 
          (typeof this.submissionData.riskDetail.raterVersionId == 'undefined' || (this.submissionData.riskDetail.raterVersionId > 6 && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['secondaryBusinessGroupDropdown'])))))
          && this.VehicleHeaderForm.controls['secondaryBusinessGroupDropdown'].value != 0)
      && FormUtils.validateFormControl(this.VehicleHeaderForm.controls['accountCategoryDropdown']) 
      && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['accountCategoryUWDropdown']) && this.VehicleHeaderForm.controls['accountCategoryUWDropdown'].value != 0)
      && (FormUtils.validateFormControl(this.VehicleHeaderForm.controls['mainGVW']) && this.VehicleHeaderForm.controls['mainGVW'].value != 0);
      
      return isValid;
  }
}
