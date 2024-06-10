import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { PolicyBillingData } from 'app/modules/policy/data/policy-billing.data';
import { PolicyBillingLabelsConstants } from 'app/shared/constants/policy-billing.labels.constants';
import { GenericLabelConstants } from 'app/shared/constants/generic.labels.constants';
import { ZipCodeData } from 'app/modules/submission/data/zipcode.data';
import Utils from 'app/shared/utilities/utils';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { PageSections } from 'app/shared/enum/page-sections.enum';
import FormUtils from 'app/shared/utilities/form.utils';
import { ApplicantData } from 'app/modules/submission/data/applicant/applicant.data';
import { SubmissionData } from 'app/modules/submission/data/submission.data';
import { ApplicantNameAddressService } from 'app/modules/submission/services/applicant-name-address.service';
import { take } from 'rxjs/operators';
import { AddressType } from 'app/shared/constants/name-and-address.options.constants';

@Component({
  selector: 'app-post-payment-check',
  templateUrl: './post-payment-check.component.html',
  styleUrls: ['./post-payment-check.component.scss']
})
export class PostPaymentCheckComponent implements OnInit {

  @Input() checkPaymentForm: FormGroup;
  @Input() showIsIndividualToggle: boolean = true;

  public FormUtils = FormUtils;

  public errorMessageConstant = ErrorMessageConstant;
  public CheckPaymentConstants = PolicyBillingLabelsConstants.checkPayment;
  public GenericLabel = GenericLabelConstants;

  constructor(public billingData: PolicyBillingData,
    public zipCodeData: ZipCodeData,
    public applicantData: ApplicantData,
    public applicantNameAddressService: ApplicantNameAddressService,
    public submissionData: SubmissionData,) { }

  ngOnInit() {
    this.getAddresses();
  }

  setZipCode() {
    this.initalizeZipCode(this.checkPaymentForm.get('zip').value);
  }

  initalizeZipCode(zipCode: string) {
    this.checkPaymentForm.get('city').enable();
    const formControlNames = ['state', 'city', 'zip'];
    this.zipCodeData.checkCityList = [];
    Utils.blockUI();
    if (zipCode !== '') {
      this.zipCodeData.getZipCode(zipCode, this.checkPaymentForm, formControlNames, PageSections.CheckPayment);
    } else {
      this.FormUtils.resetFields(this.checkPaymentForm as FormGroup, formControlNames);
      this.zipCodeData.checkCityList = [];

      Utils.unblockUI();
      NotifUtils.showError('Zip code not found, contact Underwriting');
    }
  }

  getAddresses() {
    Utils.blockUI();
    this.applicantNameAddressService.getNameAndAddress(this.submissionData.riskDetail.id).pipe(take(1)).subscribe(data => {
      const mailingAddress = data.addresses.find(a => a.addressTypeId == AddressType.Mailing).address;
      
      this.checkPaymentForm.get('name').setValue(data.businessName);
      this.checkPaymentForm.get('address').setValue(mailingAddress.streetAddress1);
      this.initalizeZipCode(mailingAddress.zipCode.toString());
      this.checkPaymentForm.get('zip').setValue(mailingAddress.zipCode.toString());
      Utils.unblockUI();
    }, (error) => {
      Utils.unblockUI();
      NotifUtils.showError('There was an error on retrieving addresses. Please try again.');
    });
  }
}
