import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import FormUtils from 'app/shared/utilities/form.utils';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { GenericLabelConstants } from 'app/shared/constants/generic.labels.constants';
import { PolicyBillingLabelsConstants } from 'app/shared/constants/policy-billing.labels.constants';
import { PolicyBillingData } from 'app/modules/policy/data/policy-billing.data';
import { LvNameSuffix } from 'app/shared/constants/applicant.options.constants';

@Component({
  selector: 'app-entity-name',
  templateUrl: './entity-name.component.html',
  styleUrls: ['./entity-name.component.scss']
})
export class EntityNameComponent implements OnInit {

  @Input() checkPaymentForm: FormGroup;

  public FormUtils = FormUtils;

  ErrorMessageConstant = ErrorMessageConstant;
  CheckPaymentConstants = PolicyBillingLabelsConstants.checkPayment;
  GenericLabel = GenericLabelConstants;

  public nameSuffixList = LvNameSuffix;

  constructor(public billingData: PolicyBillingData) { }


  ngOnInit() {
  }

  onChangeIndividual(value: boolean): void {
    this.billingData.clearAndSetValidators();
  }
}
