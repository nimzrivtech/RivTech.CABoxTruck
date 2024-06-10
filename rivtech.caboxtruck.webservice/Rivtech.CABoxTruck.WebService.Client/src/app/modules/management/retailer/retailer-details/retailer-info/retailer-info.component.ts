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
import { BaseClass } from '../../../../../shared/base-class';
import { RetailerData } from '@app/modules/management/data/retailer.data';

const Flat = 'F';
@Component({
  selector: 'app-retailer-info',
  templateUrl: './retailer-info.component.html',
  styleUrls: ['./retailer-info.component.scss']
})
export class RetailerInfoComponent extends BaseClass implements OnInit {
  @Input() formGroup: any;
  public FormUtils = FormUtils;
  AgencyLabelConstants = AgencyLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;
  GenericLabel = GenericLabel;
  SubAgencyLabelConstants = SubAgencyLabelConstants;

  constructor(public agencyData: AgencyData,
              public retailerData: RetailerData,
              public subAgencyData: SubAgencyData) {
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
  }

  onEftChanged(): void {
    this.agencyData.eftAvailableLogic(this.formGroup);
  }

  onCommissionTypeChanged(): void {
    // this.agencyData.commissionTypeLogic(this.formGroup);
  }

}
