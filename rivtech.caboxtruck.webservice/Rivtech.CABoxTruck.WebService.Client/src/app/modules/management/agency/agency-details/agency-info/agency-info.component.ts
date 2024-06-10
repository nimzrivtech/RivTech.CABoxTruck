import { Component, Input, OnInit } from '@angular/core';
import { ErrorMessageConstant } from '../../../../../shared/constants/error-message.constants';
import { AgencyLabelConstants } from '../../../../../shared/constants/agency.label.constants';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import FormUtils from '../../../../../shared/utilities/form.utils';
import { AgencyData } from '../../../../../modules/management/data/agency.data';
import { SubAgencyData } from '../../../../../modules/management/data/sub-agency.data';
import * as moment from 'moment';
import { SubAgencyLabelConstants } from '../../../../../shared/constants/sub-agency.label.constants';
import { GeneralValidationService } from '../../../../../core/services/submission/validations/general-validation.service';
import { takeUntil } from 'rxjs/operators';
import { BaseClass } from '../../../../../shared/base-class';
import { Validators } from '@angular/forms';
import { CustomValidators } from '../../../../../shared/validators/custom.validator';

const Flat = 'F';
@Component({
  selector: 'app-agency-info',
  templateUrl: './agency-info.component.html',
  styleUrls: ['./agency-info.component.scss']
})
export class AgencyInfoComponent extends BaseClass implements OnInit {
  @Input() formGroup: any;
  public FormUtils = FormUtils;
  AgencyLabelConstants = AgencyLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;
  GenericLabel = GenericLabel;
  SubAgencyLabelConstants = SubAgencyLabelConstants;

  constructor(public agencyData: AgencyData,
              public subAgencyData: SubAgencyData,
              private generalValidationService: GeneralValidationService
              ) {
                super();
              }

  get fc() { return this.formGroup.controls; }

  get isFlat(): boolean {
    return this.formGroup.get('commissionType').value === Flat;
  }

  get isValidExpirationDate(): boolean {
    return moment(this.formGroup.get('licenseEffectiveDate').value?.singleDate?.jsDate).isSameOrBefore(this.formGroup.get('licenseExpirationDate').value?.singleDate?.jsDate);
  }

  ngOnInit() {
    // this.formGroup.get('commissionType').valueChanges.subscribe(value => {
    //   this.agencyData.commissionTypeLogic(this.formGroup);
    // });
    // this.agencyData.commissionTypeLogic(this.formGroup);
    this.isEftAvailableChildValidation();
  }

  onEftChanged(): void {
    this.agencyData.eftAvailableLogic(this.formGroup);
  }

  onCommissionTypeChanged(): void {
    // this.agencyData.commissionTypeLogic(this.formGroup);
  }

  isEftAvailableChildValidation() {
    this.agencyData.agencyFormGroup.get('isEftAvailable').valueChanges.pipe(takeUntil(this.stop$)).subscribe(value => {
      if (!value) {
        this.generalValidationService.clearValidatorFormControl(this.agencyData.agencyFormGroup, 'eftAccountNumber');
        this.generalValidationService.clearValidatorFormControl(this.agencyData.agencyFormGroup, 'eftAccountType');
        this.generalValidationService.clearValidatorFormControl(this.agencyData.agencyFormGroup, 'eftRoutingNumber');
        this.generalValidationService.clearValidatorFormControl(this.agencyData.agencyFormGroup, 'eftCommissionEmail');
      } else {
        this.generalValidationService.resetValidatorFormControl(this.agencyData.agencyFormGroup, 'eftAccountNumber', [Validators.required]);
        this.generalValidationService.resetValidatorFormControl(this.agencyData.agencyFormGroup, 'eftAccountType', [Validators.required]);
        this.generalValidationService.resetValidatorFormControl(this.agencyData.agencyFormGroup, 'eftRoutingNumber', [Validators.required]);
        this.generalValidationService.resetValidatorFormControl(this.agencyData.agencyFormGroup, 'eftCommissionEmail', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$'), CustomValidators.spaceValidator]);
      }
    });

  }
}
