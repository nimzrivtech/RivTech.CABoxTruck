import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Injectable } from '@angular/core';
import { Applicant } from '../../../../shared/models/submission/applicant/applicant';
import { SubmissionInsuredInformation } from '../../../../shared/models/submission/applicant/submission-insured-information';
import { SubmissionInsuredContactInformation } from '../../../../shared/models/submission/applicant/submission-insured-contact-information';
import { SubmissionBrokerInformation } from '../../../../shared/models/submission/applicant/submission-broker-information';
import { SubmissionDetails } from '../../../../shared/models/submission/applicant/submission-details';
import { firstYearUnderCurrentInvalidYear, firstYearVsUnderCurrentInvalidYear, yearEstablishedInvalidYear } from '../../helpers/invalidYear.validator';
import { ApplicantFilingsInformationService } from '../../services/applicant-filings-information.service';
import { ApplicantAdditionalInterestService } from '../../services/applicant-additional-interest.service';
import { SelectItem } from '../../../../shared/models/dynamic/select-item';
import { Observable, of, throwError } from 'rxjs';
import { BusinessDetailsDTO } from '../../../../shared/models/submission/applicant/businessDetailsDto';
import { SubmissionData } from '../submission.data';
import { ApplicantBusinessDetailsService } from '../../services/applicant-business-details.service';
import { catchError, finalize, map, switchMap, take, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NameAndAddressDTO } from '../../../../shared/models/submission/applicant/applicant-name-address';
import Utils from '../../../../shared/utilities/utils';
import { ApplicantNameAddressService } from '../../services/applicant-name-address.service';
import { Router } from '@angular/router';
import { SummaryData } from '../summary.data';
import { formatDate } from '@angular/common';
import { RaterApiService } from '../../services/rater-api.service';
import { LayoutService } from '../../../../core/services/layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../components/submission-details/submission-details-navitems';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { FilingsInformationDto } from '../../../../shared/models/submission/applicant/FilingsInformationDto';
import { DriverData } from '../driver/driver.data';
import { VehicleData } from '../vehicle/vehicle.data';
import { LimitsData } from '../limits/limits.data';
import {NavigationService} from '../../../../core/services/navigation/navigation.service';
import { BlanketCoverageDto } from '../../../../shared/models/submission/applicant/BlanketCoverageDto';
import { RiskAdditionalInterestsDTO ,RiskAddnlInterestListRequestDTO} from '../../../../shared/models/submission/RiskAdditionalInterestDto';
import { TableData } from '../tables.data';
import { PagerService } from '../../../../core/services/pager.service';
import FormUtils from '@app/shared/utilities/form.utils';
import { FormsData } from '../forms/forms.data';
import { FeeDetailsDto } from '../../../../shared/models/submission/applicant/FeeDetailsDto';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { QuoteLimitsData } from '../quote/quote-limits.data';
import { CustomValidators } from '@app/shared/validators/custom.validator';
import { AddressType, LvAddressTypeList } from '@app/shared/constants/name-and-address.options.constants';
import { DatasetDTO } from '@app/shared/models/submission/datasetDTO';

export interface ApplicantForms {
  nameAndAddressForm?: FormGroup;
  businessDetails?: FormGroup;
  filingsInformationForm?: FormGroup;
  additionalInterestForm?: FormGroup;
  blanketCoverage?: FormGroup;
  billingInfoForm?: FormGroup;
}

@Injectable()
export class ApplicantData {
  applicantForms: ApplicantForms;
  year = (new Date()).getFullYear();
  vinmainGVWList : any[];
  vinsecondaryBusinessGroupList : any[];
  filingsList: SelectItem[] = [
    { label: 'CA MC 65', value: 1 },
    { label: 'MCS-90/BMC-91', value: 2 },
    { label: 'CA PUC', value: 3 },
    { label: 'BMC-91x', value: 4 },
    { label: 'CO PUC', value: 5 },
    { label: 'MO DOT', value: 6 },
    { label: 'FR-19', value: 7 },
    { label: 'OH PUC', value: 8 },
    { label: 'Form E', value: 9 },
    { label: 'OS-32', value: 10 },
    { label: 'Form F', value: 11 },
    { label: 'PA PPA', value: 12 },
    { label: 'Form H', value: 13 },
    { label: 'VA DMV', value: 14 },
    { label: 'WM ATC', value: 15 },
    { label: 'Other:', value: 16 },
  ];

  entityTypesList: SelectItem[] = [
    { label: 'Additional Insured', value: 1 },
    { label: 'Loss Payee', value: 2 },
    { label: 'Breach of Warranty', value: 3 },
    { label: 'Mortgagee', value: 4 },
    { label: 'Co-Owner', value: 5 },
    { label: 'Owner', value: 6 },
    { label: 'Employee As Lessor', value: 7 },
    { label: 'Registrant', value: 8 },
    { label: 'Leaseback Owner', value: 9 },
    { label: 'Trustee', value: 10 },
    { label: 'Lienholder', value: 11 },
    { label: 'Designated Insured', value: 12 }
  ];

  ApplicantMainGVW: SelectItem[] = [];
  applicantDropdownList: ApplicantInformationDropDowns = new ApplicantInformationDropDowns();
  additionalInterests: RiskAdditionalInterestsDTO[] = [];
  addressList: any[] = [];
  cityList: any[] = [];
  cityListOperations: any[] = [];
  isInitialLoad: boolean = true;
  datepickerOptions: IAngularMyDpOptions;

  subsidiaryList: any[] = [];
  companyList: any[] = [];

  pager: any = {};
  currentPage: number = 1;
  pagedItems: any[];
  loading: boolean;
  itemPerPage: number = 10;
  totalRowCount: number = 9;
  sortField : string = 'AddProcessDate';
  order : string = 'ASC';
  rowCount : number = 0;

  constructor(private fb: FormBuilder,
    private toastr: ToastrService,
    private router: Router,
    public summaryData: SummaryData,
    public submissionData: SubmissionData,
    private filingsInfoService: ApplicantFilingsInformationService,
    private additionalInterestService: ApplicantAdditionalInterestService,
    private applicantBusinessDetailsService: ApplicantBusinessDetailsService,
    private raterApi: RaterApiService,
    private applicantNameAddressService: ApplicantNameAddressService,
    private layoutService: LayoutService,
    public driverData: DriverData,
    public vehicleData: VehicleData,
    public limitsData: LimitsData,
    public formsData: FormsData,
    private navigationService: NavigationService,
    private tableData: TableData,
    private pagerService: PagerService,
    private policySummaryData: PolicySummaryData,
    private quoteLimitsData: QuoteLimitsData
  ) {
  }

  initiateFormFields(): ApplicantForms {
    this.subsidiaryList = [];
    this.companyList = [];
    this.addressList = [];
    this.cityList = [];
    this.additionalInterests = [];
    this.isInitialLoad = true;
    this.applicantForms = {};
    const defaultValueMainGVW = this.submissionData.raterVersionSupport() ? 5 : 1;
    this.summaryData.populateAssignedToList();
    this.datepickerOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
      disableSince: { year: 0, month: 0, day: 0 },
      disableUntil: { year: 0, month: 0, day: 0 }
    };

        // Name and Address
        this.applicantForms.nameAndAddressForm = this.fb.group({
            businessNameInput: [null, Validators.required],
            insuredDbaInput: [null],
            businessAddressInput: [null, Validators.required],
            businessCityDropdown: [null, Validators.required],
            businessStateInput: [null, Validators.required],
            businessZipCodeInput: [null, Validators.required],
            cityOperations: [null, Validators.required],
            stateOperations: [null, Validators.required],
            zipCodeOperations: [null, Validators.required],
            businessPrincipalInput: [null], // Validators.required
            emailInput: [null, [Validators.pattern(
              '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,5}$'
            )]], // Validators.required
            phoneExtInput: [null, CustomValidators.phoneNumberValidator], // Validators.required
            renewal: [false],
            previousPolicyNumber: [null]
          });

        // Business Details
        this.applicantForms.businessDetails = this.fb.group({
            newVentureSwitch: [false], // Validators.required
            yearEstablishedInput: [null, Validators.required],
            firstYearUnderCurrentInput: [null], // Validators.required
            newVenturePreviousExperienceDropdown: [null],
            yearBusinessInput: [null, Validators.required],
            yearUnderCurrentInput: [null], // Validators.required
            businessTypeDropdown: [null, Validators.required],
            mainUseDropdown: [1, Validators.required],
            mainGVW: [defaultValueMainGVW, Validators.required],
            secondaryBusinessGroupDropdown: [1, Validators.required],
            accountCategoryDropdown: [1], // Validators.required
            accountCategoryUWDropdown: [1, Validators.required],
            descOperationsInput: [null],
            federalIdInput: [null, [Validators.minLength(9)]], // Validators.required
            naicsCodeInput: [null],
            pucNumberInput: [null],
            socialSecurityNumber: [null, [Validators.minLength(9)]],
            anySubsidiariesCompaniesSwitch: [false],
            companyPastSwitch: [false],
          },
          {
            validator : [
              yearEstablishedInvalidYear(this.year, 'yearEstablishedInput'),
              firstYearUnderCurrentInvalidYear(this.year, 'firstYearUnderCurrentInput'),
              firstYearVsUnderCurrentInvalidYear('yearEstablishedInput', 'firstYearUnderCurrentInput'),
              this.validateMainUse,
              this.validateMainGVW,
              this.validateSecondaryBusinessGroup,
              this.validateAccountCategoryUW
            ]
          });

        // Filings Information
        this.applicantForms.filingsInformationForm = this.fb.group({
          riskDetailId: '',
          isARatedCarrierRequired: [false], // Validators.required
          isFilingRequired: [false], // Validators.required
          requiredFilingsList: [[]],
          requiredFilingIds: [[]],
          usdotNumber: [''],
          mcNumber: ['', Validators.required],
          isOthersAllowedToOperateCurrent: [false], // Validators.required
          isOthersAllowedToOperatePast: [false], // Validators.required
          isOperatorsWorkOnlyForInsured: [true], // Validators.required
          isInsuredHasMotorCarrierNo: [false],
          isDifferentUSDOTNumberLastFiveYears: [false], // Validators.required
          dotNumberLastFiveYearsDetail: [''],
        });

    // Addition Interest
    this.applicantForms.additionalInterestForm = this.fb.group({
      id: [''],
      additionalInterestTypeId: [null, Validators.required],
      name: ['', Validators.required],
      address: [''],
      city: [''],
      state: [''],
      zipCode: [''],
      effectiveDate: ['', Validators.required],
      expirationDate: ['', Validators.required],
      isWaiverOfSubrogation: [false],
      isPrimaryNonContributory: [false],
      isAutoLiability: [false],
      isGeneralLiability: [false],
      is30DayNotice: [false],
      currentInterests: [[]]
    }, { validators: [this.validateStartEndDate, this.validateDuplicateInterest] });

    this.applicantForms.blanketCoverage = this.fb.group({
      riskDetailId: [''],
      isAdditionalInsured: [null],
      isWaiverOfSubrogation: [null],
      isPrimaryNonContributory: [null]
    });

    this.applicantForms.billingInfoForm = this.fb.group({
      contactName: [null, Validators.required],
      phone: [null, [Validators.required, CustomValidators.phoneNumberValidator]],
      email: [null, [Validators.required, Validators.pattern(
        '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,5}$'
      )]]
    });
    return this.applicantForms;
  }
  retrieveDropdowValues() {
    this.fetchApplicantMainGVWList().subscribe(data => {
      this.vinmainGVWList = (data as DatasetDTO[]).map(d => ({ label: d.descriptionInUI, value: d.value }));
    });
    
    this.fetchApplicantSecondaryBusGroupList().subscribe(data => {
      this.vinsecondaryBusinessGroupList = (data as DatasetDTO[]).map(d => ({ label: d.descriptionInUI, value: d.value }));
    });

    return this.applicantDropdownList = {
      mainGVWList:  [],
      secondaryBusinessGroupList:[]
    };
  }
  populateNameAndAddressFields() {
    // Name and Address
    this.applicantForms.nameAndAddressForm.controls['businessNameInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.businessName);
    this.applicantForms.nameAndAddressForm.controls['insuredDbaInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.insuredDBA);
    this.applicantForms.nameAndAddressForm.controls['businessAddressInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.businessAddress);
    this.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].setValue(this.submissionData.riskDetail?.nameAndAddress?.city ?? null);
    this.applicantForms.nameAndAddressForm.controls['businessStateInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.state);
    this.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.zipCode);
    this.applicantForms.nameAndAddressForm.controls['cityOperations'].setValue(this.submissionData.riskDetail?.nameAndAddress?.cityOperations ?? null);
    this.applicantForms.nameAndAddressForm.controls['stateOperations'].setValue(this.submissionData.riskDetail?.nameAndAddress?.stateOperations);
    this.applicantForms.nameAndAddressForm.controls['zipCodeOperations'].setValue(this.submissionData.riskDetail?.nameAndAddress?.zipCodeOperations);

    this.applicantForms.nameAndAddressForm.controls['businessPrincipalInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.businessPrincipal);
    this.applicantForms.nameAndAddressForm.controls['emailInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.email);
    this.applicantForms.nameAndAddressForm.controls['phoneExtInput'].setValue(this.submissionData.riskDetail?.nameAndAddress?.phoneExt);

    // Populate Billing Info
    const billingInfo = this.submissionData.riskDetail?.riskPolicyContacts?.find(x => x.typeId == AddressType.Billing);
    this.applicantForms.billingInfoForm.controls['contactName'].setValue(billingInfo?.fullName);
    this.applicantForms.billingInfoForm.controls['phone'].setValue(billingInfo?.phone);
    this.applicantForms.billingInfoForm.controls['email'].setValue(billingInfo?.email);

    this.applicantForms.nameAndAddressForm.controls['renewal'].patchValue(this.submissionData.riskDetail.isRenewal);
    this.applicantForms.nameAndAddressForm.controls['previousPolicyNumber'].patchValue(this.submissionData.riskDetail.previousPolicyNumber);

    console.log('IsRenewal:', this.submissionData.riskDetail.isRenewal);
    if (this.submissionData.riskDetail.isRenewal) {
      this.setAsRequiredNameAndAddress('previousPolicyNumber');
    }
  }

  populateBusinessDetailsFields() {
    // Name and Address
    this.applicantForms.businessDetails.controls['newVentureSwitch'].setValue(this.submissionData.riskDetail?.businessDetails?.isNewVenture);
    this.applicantForms.businessDetails.controls['yearEstablishedInput'].setValue(this.submissionData.riskDetail?.businessDetails?.yearEstablished);
    this.applicantForms.businessDetails.controls['firstYearUnderCurrentInput'].setValue(this.submissionData.riskDetail?.businessDetails?.firstYearUnderCurrentManagement);
    this.applicantForms.businessDetails.controls['newVenturePreviousExperienceDropdown'].setValue(this.submissionData.riskDetail?.businessDetails?.newVenturePreviousExperienceId ?? null);
    this.applicantForms.businessDetails.controls['yearBusinessInput'].setValue(this.submissionData.riskDetail?.businessDetails?.yearsInBusiness);
    this.applicantForms.businessDetails.controls['yearUnderCurrentInput'].setValue(this.submissionData.riskDetail?.businessDetails?.yearsUnderCurrentManagement);
    this.applicantForms.businessDetails.controls['businessTypeDropdown'].setValue(this.submissionData.riskDetail?.businessDetails?.businessTypeId ?? null);
    // this.applicantForms.businessDetails.controls['mainUseDropdown'].setValue(this.submissionData.riskDetail?.businessDetails?.mainUseId ?? null);
    // this.applicantForms.businessDetails.controls['secondaryBusinessGroupDropdown'].setValue(this.submissionData.riskDetail?.businessDetails?.secondaryBusinessGroupId ?? null);
    // this.applicantForms.businessDetails.controls['accountCategoryDropdown'].setValue(this.submissionData.riskDetail?.businessDetails?.accountCategoryId ?? null);
    // this.applicantForms.businessDetails.controls['accountCategoryUWDropdown'].setValue(this.submissionData.riskDetail?.businessDetails?.accountCategoryUWId ?? null);
    this.applicantForms.businessDetails.controls['descOperationsInput'].setValue(this.submissionData.riskDetail?.businessDetails?.descOperations);
    this.applicantForms.businessDetails.controls['federalIdInput'].setValue(this.submissionData.riskDetail?.businessDetails?.federalIDNumber);
    this.applicantForms.businessDetails.controls['naicsCodeInput'].setValue(this.submissionData.riskDetail?.businessDetails?.naicsCode);
    this.applicantForms.businessDetails.controls['pucNumberInput'].setValue(this.submissionData.riskDetail?.businessDetails?.pUCNumber);
    this.applicantForms.businessDetails.controls['socialSecurityNumber'].setValue(this.submissionData.riskDetail?.businessDetails?.socialSecurityNumber);
    this.applicantForms.businessDetails.controls['anySubsidiariesCompaniesSwitch'].setValue(this.submissionData.riskDetail?.businessDetails?.hasSubsidiaries);
    this.applicantForms.businessDetails.controls['companyPastSwitch'].setValue(this.submissionData.riskDetail?.businessDetails?.hasCompanies);
  }

  populateFilingsInformationFields() {
    this.applicantForms.filingsInformationForm.patchValue(this.submissionData?.riskDetail?.filingsInformation);
  }

  populateBlanketCoverageFields() {
    this.applicantForms.blanketCoverage.patchValue(this.submissionData?.riskDetail?.blanketCoverage);
  }

  get formvalidateRequired() {
    return this.validateRequired();
  }

  validateRequired() {
    return this.applicantForms.nameAndAddressForm.controls['businessNameInput'].valid
      && this.applicantForms.nameAndAddressForm.controls['businessAddressInput'].valid && this.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].value != null
      && this.applicantForms.nameAndAddressForm.controls['businessStateInput'].valid && this.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].valid;
  }

  validateStartEndDate(form: FormGroup) {
    const startDate = new Date(form.controls['effectiveDate'].value?.singleDate?.jsDate);
    const endDate = new Date(form.controls['expirationDate'].value?.singleDate?.jsDate);

    return startDate >= endDate ? { 'invalidEffectiveExpirationDate': true } : null;
  }

  validateMainUse(form: FormGroup) {
    const formValue = form.controls['mainUseDropdown']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidMainUseDropdown': true } : null;
  }

  validateMainGVW(form: FormGroup) {
    const formValue = form.controls['mainGVW']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidMainGVW': true } : null;
  }

  validateSecondaryBusinessGroup(form: FormGroup) {
    const formValue = form.controls['secondaryBusinessGroupDropdown']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidSecondaryBusinessGroup': true } : null;
  }

  validateAccountCategoryUW(form: FormGroup) {
    const formValue = form.controls['accountCategoryUWDropdown']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidAccountCategoryUWDropdown': true } : null;
  }

  validateDuplicateInterest(form: FormGroup) {
    const currentInterestNames = form.controls['currentInterests']?.value as any[];
    const interestTypeId = form.controls['additionalInterestTypeId']?.value;
    const interestName = form.controls['name']?.value?.toLowerCase();
    const interestAddress = form.controls['address']?.value?.toLowerCase() + form.controls['city']?.value?.toLowerCase();

    const savedName = currentInterestNames.find(i => i.additionalInterestTypeId == interestTypeId
      && i.name?.toLowerCase() === interestName
      && i.address?.toLowerCase() + i.city?.toLowerCase() === interestAddress);
    return savedName ? { 'interestDuplicate': true } : null;
  }

  getFormValue(): Applicant {
    const formValues = new Applicant({
      submissionInsuredInformation: new SubmissionInsuredInformation,
      submissionInsuredContactInformation: new SubmissionInsuredContactInformation,
      submissionBrokerInformation: new SubmissionBrokerInformation,
      submissionDetails: new SubmissionDetails
    });
    return formValues;
  }

  formsAreDirty(): boolean {
    throw new Error('Method not implemented.');
  }

  setHeaderDetails() {
    const nameAddressForm = this.applicantForms.nameAndAddressForm.getRawValue();
    const formValue = this.summaryData.form?.value;
    this.summaryData.name = nameAddressForm.businessNameInput;
    this.summaryData.address = `${nameAddressForm.businessAddressInput}, ${nameAddressForm.businessCityDropdown}, ${nameAddressForm.businessStateInput} ${nameAddressForm.businessZipCodeInput}`;
    this.summaryData.quoteNumber = this.submissionData.riskDetail.submissionNumber;
    this.summaryData.quoteStatus = formValue.status;
    this.summaryData.policyNumber = this.submissionData.riskDetail.policyNumber;
    this.summaryData.updateStatus(formValue.status, formValue.assignedToId);
  }

  sortAddressList() {
    this.addressList.sort(function (a, b) {
      if (a.addressTypeId === b.addressTypeId) {
        return a.address.state > b.address.state ? 1 : -1;
      }
      return a.addressTypeId > b.addressTypeId ? 1 : -1;
    });
  }

  saveNameAndAddress(nextUrl: string) {
    var values =  this.applicantForms.nameAndAddressForm.getRawValue();
    const data = new NameAndAddressDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      submissionNumber: this.submissionData.riskDetail.submissionNumber,
      businessName: this.applicantForms.nameAndAddressForm.value.businessNameInput,
      insuredDBA: this.applicantForms.nameAndAddressForm.value.insuredDbaInput,
      city: this.applicantForms.nameAndAddressForm.value.businessCityDropdown,
      state: this.applicantForms.nameAndAddressForm.value.businessStateInput,
      zipCode: this.applicantForms.nameAndAddressForm.value.businessZipCodeInput,
      cityOperations: this.applicantForms.nameAndAddressForm.value.cityOperations,
      stateOperations: this.applicantForms.nameAndAddressForm.value.stateOperations,
      zipCodeOperations: this.applicantForms.nameAndAddressForm.value.zipCodeOperations,
      businessPrincipal: this.applicantForms.nameAndAddressForm.value.businessPrincipalInput,
      email: this.applicantForms.nameAndAddressForm.value.emailInput,
      phoneExt: this.applicantForms.nameAndAddressForm.value.phoneExtInput,
      addresses: this.addressList,
      isRenewal: values.renewal,
      previousPolicyNumber: values.previousPolicyNumber
    });

    // Update submission data ahead of saving
    if (this.submissionData.riskDetail.nameAndAddress) {
      if (data.state !== this.submissionData.riskDetail.nameAndAddress.state) {
        console.log('State value changed ', this.submissionData.riskDetail.nameAndAddress.state, data.state);
        if (this.submissionData.riskDetail.riskCoverage != null) {
          this.submissionData.riskDetail.riskCoverage.autoBILimitId = null;
          this.submissionData.riskDetail.riskCoverage.autoPDLimitId = null;
          this.submissionData.riskDetail.riskCoverage.umbiLimitId = null;
          this.submissionData.riskDetail.riskCoverage.umpdLimitId = null;
          this.submissionData.riskDetail.riskCoverage.uimbiLimitId = null;
          this.submissionData.riskDetail.riskCoverage.uimpdLimitId = null;
          this.submissionData.riskDetail.riskCoverage.pipLimitId = null;
          this.submissionData.riskDetail.riskCoverage.medPayLimitId = null;
        }

        if (this.submissionData.riskDetail.riskCoverages !== null && this.submissionData.riskDetail.riskCoverages.length > 0) {
          console.log('clear coverages');
          this.submissionData.riskDetail.riskCoverages.forEach(item => {
            item.autoBILimitId = null;
            item.autoPDLimitId = null;
            item.umbiLimitId = null;
            item.umpdLimitId = null;
            item.uimbiLimitId = null;
            item.uimpdLimitId = null;
            item.pipLimitId = null;
            item.medPayLimitId = null;
          });
        }
        this.submissionData.riskDetail.nameAndAddress.state = data.state;
        this.limitsData.populateLimitsFields();
        this.limitsData.setDefaults();
        this.limitsData.setDefaultRiskCoverageLimit();
        this.formsData.setDefaultSurplusLineNumber();
      } else {
        this.submissionData.riskDetail.nameAndAddress.state = data.state;
      }
    }

    this.setFeeDetails(data.state);

    Utils.blockUI();
    if (this.submissionData.isNew) {
      this.applicantNameAddressService.saveNameAndAddress(data).pipe(take(1)).subscribe(result => {
        Utils.unblockUI();
        this.submissionData.isNew = false;
        this.submissionData.riskDetail.id = result.riskDetailId;
        this.submissionData.riskDetail.riskId = result.riskId;
        this.submissionData.riskDetail.submissionNumber = result.submissionNumber;
        this.submissionData.riskDetail.policyNumber = result.policyNumber;
        this.submissionData.riskDetail.isRenewal = result.isRenewal;
        this.submissionData.riskDetail.previousPolicyNumber = result.previousPolicyNumber;
        data.riskDetailId = result.riskDetailId;
        this.submissionData.riskDetail.nameAndAddress = result;
        this.submissionData.riskDetail.fieldVersions = result.fieldVersions;
        this.summaryData.form.patchValue({status: 'Received', assignedToId: this.summaryData.userId});
        this.setHeaderDetails();
        this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id));
        //this.createExperienceRaterFile(); // trigger create rater file.
        console.log('Submission Created!');
        this.router.navigate([this.navigationService.navigateRoute(nextUrl, this.submissionData.riskDetail.id)]);
      }, (error) => {
        console.error(error);
        Utils.unblockUI();
        this.toastr.error(ErrorMessageConstant.savingErrorMsg);
      });
    } else {
      this.applicantNameAddressService.updateNameAndAddress(data).pipe(take(1)).subscribe(result => {
        Utils.unblockUI();
        this.submissionData.riskDetail.nameAndAddress = result;
        this.submissionData.riskDetail.policyNumber = result.policyNumber;
        this.submissionData.riskDetail.isRenewal = result.isRenewal;
        this.submissionData.riskDetail.previousPolicyNumber = result.previousPolicyNumber;
        this.driverData.defaultPolicyState = result.addresses.filter(t => (t.address).isMainGarage)[0].address?.state;
        this.vehicleData.updateAddresses(result);
        this.formsData.updateDefaultStates();
        this.setHeaderDetails();
        console.log('Submission Saved!');
      }, (error) => {
        console.error(error);
        Utils.unblockUI();
        this.toastr.error(ErrorMessageConstant.savingErrorMsg);
      });
    }
  }

  saveNameAndAddress$(nextUrl: string): Observable<any> {
    var values =  this.applicantForms.nameAndAddressForm.getRawValue();
    const data = new NameAndAddressDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      submissionNumber: this.submissionData.riskDetail.submissionNumber,
      businessName: this.applicantForms.nameAndAddressForm.value.businessNameInput,
      insuredDBA: this.applicantForms.nameAndAddressForm.value.insuredDbaInput,
      city: this.applicantForms.nameAndAddressForm.value.businessCityDropdown,
      state: this.applicantForms.nameAndAddressForm.value.businessStateInput,
      zipCode: this.applicantForms.nameAndAddressForm.value.businessZipCodeInput,
      cityOperations: this.applicantForms.nameAndAddressForm.value.cityOperations,
      stateOperations: this.applicantForms.nameAndAddressForm.value.stateOperations,
      zipCodeOperations: this.applicantForms.nameAndAddressForm.value.zipCodeOperations,
      businessPrincipal: this.applicantForms.nameAndAddressForm.value.businessPrincipalInput,
      email: this.applicantForms.nameAndAddressForm.value.emailInput,
      phoneExt: this.applicantForms.nameAndAddressForm.value.phoneExtInput,
      addresses: this.addressList,
      isRenewal: values.renewal,
      previousPolicyNumber: values.previousPolicyNumber
    });

    // Update submission data ahead of saving
    if (this.submissionData.riskDetail.nameAndAddress) {
      if (data.state !== this.submissionData.riskDetail.nameAndAddress.state) {
        console.log('State value changed ', this.submissionData.riskDetail.nameAndAddress.state, data.state);
        if (this.submissionData.riskDetail.riskCoverage != null) {
          this.submissionData.riskDetail.riskCoverage.autoBILimitId = null;
          this.submissionData.riskDetail.riskCoverage.autoPDLimitId = null;
          this.submissionData.riskDetail.riskCoverage.umbiLimitId = null;
          this.submissionData.riskDetail.riskCoverage.umpdLimitId = null;
          this.submissionData.riskDetail.riskCoverage.uimbiLimitId = null;
          this.submissionData.riskDetail.riskCoverage.uimpdLimitId = null;
          this.submissionData.riskDetail.riskCoverage.pipLimitId = null;
          this.submissionData.riskDetail.riskCoverage.medPayLimitId = null;
        }

        if (this.submissionData.riskDetail.riskCoverages !== null && this.submissionData.riskDetail.riskCoverages.length > 0) {
          console.log('clear coverages');
          this.submissionData.riskDetail.riskCoverages.forEach(item => {
            item.autoBILimitId = null;
            item.autoPDLimitId = null;
            item.umbiLimitId = null;
            item.umpdLimitId = null;
            item.uimbiLimitId = null;
            item.uimpdLimitId = null;
            item.pipLimitId = null;
            item.medPayLimitId = null;
          });
        }
        this.submissionData.riskDetail.nameAndAddress.state = data.state;
        this.limitsData.populateLimitsFields();
        this.limitsData.setDefaults();
        this.limitsData.setDefaultRiskCoverageLimit();
        this.formsData.setDefaultSurplusLineNumber();
      } else {
        this.submissionData.riskDetail.nameAndAddress.state = data.state;
      }
    }

    this.setFeeDetails(data.state);

    if (this.submissionData.isNew) {
      return this.applicantNameAddressService.saveNameAndAddress(data).pipe(
        (take(1)),
        tap(result => { 
          this.submissionData.isNew = false;
          this.submissionData.riskDetail.id = result.riskDetailId;
          this.submissionData.riskDetail.riskId = result.riskId;
          this.submissionData.riskDetail.submissionNumber = result.submissionNumber;
          this.submissionData.riskDetail.policyNumber = result.policyNumber;
          data.riskDetailId = result.riskDetailId;
          this.submissionData.riskDetail.nameAndAddress = result;
          this.submissionData.riskDetail.isRenewal = result.isRenewal;
          this.submissionData.riskDetail.previousPolicyNumber = result.previousPolicyNumber;
          this.summaryData.form.patchValue({status: 'Received', assignedToId: this.summaryData.userId});
          this.setHeaderDetails();
          this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id));
          // this.createExperienceRaterFile(); // trigger create rater file.
          console.log('Submission Created!');
          // this.router.navigate([this.navigationService.navigateRoute(nextUrl, this.submissionData.riskDetail.id)]);
        }), 
        switchMap((result) => this.raterApi.initializeRaterVersion$(result.riskId).pipe(
          tap((data) => {
            console.log('data ', data);
            this.submissionData.riskDetail.raterVersionId = data;
            console.log('Rater version initialized. 1');
          })
          ).catch((err) => {
            this.toastr.error("Error occured while setting intial rater version.");
            console.error(err);
            return of(err);
          })
        ),
        catchError((err) => {
          console.error(err);
          this.toastr.error(ErrorMessageConstant.savingErrorMsg);
          return of(err);
        })
      );
    } else {
      return this.applicantNameAddressService.updateNameAndAddress(data).pipe(
        take(1),
        tap(result => { 
          console.log(result);
          const oldPolicyNumber = this.submissionData?.riskDetail?.policyNumber;
          this.submissionData.riskDetail.nameAndAddress = result;
          this.submissionData.riskDetail.policyNumber = result.policyNumber;
          this.submissionData.riskDetail.isRenewal = result.isRenewal;
          this.submissionData.riskDetail.previousPolicyNumber = result.previousPolicyNumber;
          this.driverData.defaultPolicyState = result.addresses.filter(t => (t.address).isMainGarage)[0].address?.state;
          this.vehicleData.updateAddresses(result);
          this.formsData.updateDefaultStates();
          this.submissionData.updatePolicyNumber(oldPolicyNumber);
          this.setHeaderDetails();
          console.log('Submission Saved!');
        }), 
        switchMap((result) => this.raterApi.initializeRaterVersion$(result.riskId)
          .pipe(
            tap((data) => {
              console.log('data ', data);
              this.submissionData.riskDetail.raterVersionId = data;
              console.log('Rater version initialized. 2');
            }),
            catchError((err) => {
              this.toastr.error("Error occured while setting intial rater version.");
              console.error(err);
              return of(err);
            })
          )
        ),
        catchError((err) => {
          console.error(err);
          this.toastr.error(ErrorMessageConstant.savingErrorMsg);
          return of(err);
        })
      );
    }
  }

  updateNameAndAddressMidterm() {
    const data = new NameAndAddressDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      submissionNumber: this.submissionData.riskDetail.submissionNumber,
      businessName: this.applicantForms.nameAndAddressForm.value.businessNameInput,
      insuredDBA: this.applicantForms.nameAndAddressForm.value.insuredDbaInput,
      city: this.applicantForms.nameAndAddressForm.value.businessCityDropdown ?? this.submissionData.riskDetail.nameAndAddress.city,
      state: this.applicantForms.nameAndAddressForm.value.businessStateInput,
      zipCode: this.applicantForms.nameAndAddressForm.value.businessZipCodeInput,
      cityOperations: this.applicantForms.nameAndAddressForm.value.cityOperations,
      stateOperations: this.applicantForms.nameAndAddressForm.value.stateOperations,
      zipCodeOperations: this.applicantForms.nameAndAddressForm.value.zipCodeOperations,
      businessPrincipal: this.applicantForms.nameAndAddressForm.value.businessPrincipalInput,
      email: this.applicantForms.nameAndAddressForm.value.emailInput,
      phoneExt: this.applicantForms.nameAndAddressForm.value.phoneExtInput,
      addresses: this.addressList,
    });

    Utils.blockUI();
    this.applicantNameAddressService.updateNameAndAddressMidterm(data).pipe(take(1)).subscribe(result => {
      Utils.unblockUI();
      this.submissionData.riskDetail.nameAndAddress = result;
      this.policySummaryData.name = data?.businessName;
      console.log('Name and Address Saved!');
      this.policySummaryData.updateSubtatusForPendingEndorsement();
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
  }

  updateNameAndAddressMidterm$(): Observable<any>{
    const nameAndAddressData = new NameAndAddressDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      submissionNumber: this.submissionData.riskDetail.submissionNumber,
      businessName: this.applicantForms.nameAndAddressForm.value.businessNameInput,
      insuredDBA: this.applicantForms.nameAndAddressForm.value.insuredDbaInput,
      businessAddress: this.applicantForms.nameAndAddressForm.value.businessAddressInput,
      city: this.applicantForms.nameAndAddressForm.value.businessCityDropdown ?? this.submissionData.riskDetail.nameAndAddress.city,
      state: this.applicantForms.nameAndAddressForm.value.businessStateInput,
      zipCode: this.applicantForms.nameAndAddressForm.value.businessZipCodeInput,
      cityOperations: this.applicantForms.nameAndAddressForm.value.cityOperations,
      stateOperations: this.applicantForms.nameAndAddressForm.value.stateOperations,
      zipCodeOperations: this.applicantForms.nameAndAddressForm.value.zipCodeOperations,
      businessPrincipal: this.applicantForms.nameAndAddressForm.value.businessPrincipalInput,
      email: this.applicantForms.nameAndAddressForm.value.emailInput,
      phoneExt: this.applicantForms.nameAndAddressForm.value.phoneExtInput,
      addresses: this.addressList,
    });

    return this.applicantNameAddressService.updateNameAndAddressMidterm(nameAndAddressData).pipe(
        (take(1)),
        tap(result => { 
          this.submissionData.riskDetail.nameAndAddress = result;
          this.policySummaryData.name = nameAndAddressData?.businessName;
          console.log('Name and Address Saved!');
          this.policySummaryData.updatePendingEndorsementObservable();
        }), 
        catchError((err) => {
          this.toastr.error(ErrorMessageConstant.savingErrorMsg);
          return of(err);
        })
    );
  }

  saveBusinessDetails(callbackFn?: Function) {
    const data = new BusinessDetailsDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      isNewVenture: this.applicantForms.businessDetails.getRawValue().newVentureSwitch,
      yearEstablished: this.applicantForms.businessDetails.value.yearEstablishedInput ? parseInt(this.applicantForms.businessDetails.value.yearEstablishedInput) : null,
      firstYearUnderCurrentManagement: this.applicantForms.businessDetails.value.firstYearUnderCurrentInput ? parseInt(this.applicantForms.businessDetails.value.firstYearUnderCurrentInput) : null,
      newVenturePreviousExperienceId: this.applicantForms.businessDetails.value.newVenturePreviousExperienceDropdown ? parseInt(this.applicantForms.businessDetails.value.newVenturePreviousExperienceDropdown) : null,
      yearsInBusiness: this.applicantForms.businessDetails.value.yearBusinessInput == null ? null : parseInt(this.applicantForms.businessDetails.value.yearBusinessInput),
      yearsUnderCurrentManagement: this.applicantForms.businessDetails.value.yearUnderCurrentInput == null ? null : parseInt(this.applicantForms.businessDetails.value.yearUnderCurrentInput),
      businessTypeId: this.applicantForms.businessDetails.value.businessTypeDropdown ? parseInt(this.applicantForms.businessDetails.value.businessTypeDropdown) : null,
      mainUseId: this.submissionData.riskDetail.businessDetails?.mainUseId,
      mainGVWId: this.submissionData.riskDetail.businessDetails?.mainGVWId,
      secondaryBusinessGroupId: this.submissionData.riskDetail.businessDetails?.secondaryBusinessGroupId,
      accountCategoryId: this.submissionData.riskDetail.businessDetails?.accountCategoryId,
      accountCategoryUWId: this.submissionData.riskDetail.businessDetails?.accountCategoryUWId,
      descOperations: this.applicantForms.businessDetails.value.descOperationsInput,
      federalIDNumber: this.applicantForms.businessDetails.value.federalIdInput,
      naicsCode: this.applicantForms.businessDetails.value.naicsCodeInput,
      pUCNumber: this.applicantForms.businessDetails.value.pucNumberInput,
      socialSecurityNumber:  this.applicantForms.businessDetails.value.socialSecurityNumber,
      hasSubsidiaries: this.applicantForms.businessDetails.value.anySubsidiariesCompaniesSwitch,
      hasCompanies: this.applicantForms.businessDetails.value.companyPastSwitch,
    });
    Utils.blockUI();
    this.applicantBusinessDetailsService.saveBusinessDetails(data).pipe(take(1)).subscribe(result => {
      Utils.unblockUI();
      this.submissionData.riskDetail.businessDetails = result;
      this.submissionData.riskDetail.businessDetails.subsidiaries = this.subsidiaryList;
      this.submissionData.riskDetail.businessDetails.companies = this.companyList;

      if (this.submissionData.riskDetail.riskForms?.length > 0) {
        const form = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'CourierCoverageEndorsement');
        if (form != null) {
          const index = this.submissionData.riskDetail.riskForms.indexOf(form);
          this.submissionData.riskDetail.riskForms[index].isSelected = false;
          if (result.mainUseId == 1) {
            this.submissionData.riskDetail.riskForms[index].isSelected = true;
          }
        }
      }

      if (callbackFn) { callbackFn(); }
      console.log('Business Details Saved!');
    }, (error) => {
      Utils.unblockUI();
      if (callbackFn) { callbackFn(); }
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
  }

  saveFilingsInformation() {
    Utils.blockUI();
    const formValue = this.applicantForms.filingsInformationForm.value;
    const filingsInfo = new FilingsInformationDto(formValue);
    filingsInfo.riskDetailId = this.submissionData.riskDetail.id;
    filingsInfo.usdotNumber = filingsInfo.usdotNumber === '' ? null : filingsInfo.usdotNumber;
    filingsInfo.dotNumberLastFiveYearsDetail = filingsInfo.dotNumberLastFiveYearsDetail === '' || !filingsInfo.isDifferentUSDOTNumberLastFiveYears
      ? null : filingsInfo.dotNumberLastFiveYearsDetail;

    filingsInfo.requiredFilingIds = filingsInfo.isFilingRequired ? formValue.requiredFilingsList.map(rf => +rf.value) : [];
    this.filingsInfoService.put(filingsInfo.riskDetailId, filingsInfo).pipe(take(1), finalize(() => Utils.unblockUI()))
      .subscribe(() => {
        this.submissionData.riskDetail.filingsInformation = filingsInfo;
      }, (error) => {
        console.error('Filings Information update error: ', error);
      });
  }

  saveBlanketCoverage() {
      const blanketCoverage = new BlanketCoverageDto(this.applicantForms.blanketCoverage.value);
      if (!blanketCoverage.riskDetailId || blanketCoverage.riskDetailId === '') {
        blanketCoverage.riskDetailId = this.submissionData.riskDetail?.id;
      }
      Utils.blockUI();
      this.additionalInterestService
      .putBlanketCoverage(blanketCoverage).pipe(take(1),
      finalize(() => {
        this.refreshQuoteOptionsAndSummaryHeader();
        Utils.unblockUI();
      }))
      .subscribe(() => {
        this.submissionData.riskDetail.blanketCoverage = blanketCoverage;
        this.additionalInterests.forEach(ai => {
          if (blanketCoverage.isWaiverOfSubrogation) {
            ai.isWaiverOfSubrogation = true;
          }
          if (blanketCoverage.isPrimaryNonContributory) {
            ai.isPrimaryNonContributory = true;
          }
        });
      }, (error) => {
        console.error('Blanket Coverage update error: ', error);
      });
  }

  saveAdditionalInterest(additionalInterest, fromEndorsement: boolean = false) {
    additionalInterest.riskDetailId = this.submissionData.riskDetail.id;
    if (additionalInterest.id == '') {
      this.additionalInterestService.post(additionalInterest).subscribe(data => {
        this.getAdditionalInterests(fromEndorsement, true);
        if (fromEndorsement) { this.policySummaryData.updateSubtatusForPendingEndorsement(); }
      });
    } else {
      this.additionalInterestService.put(additionalInterest).subscribe(data => {
        this.getAdditionalInterests(fromEndorsement);
        if (fromEndorsement) { this.policySummaryData.updateSubtatusForPendingEndorsement(); }
      });
    }
    this.submissionData.riskDetail.additionalInterest = this.additionalInterests;
  }

  saveNewEntityAddress(entityAddress) {
    Utils.blockUI();
    entityAddress.riskDetailId = this.submissionData?.riskDetail.id;
    this.applicantNameAddressService.insertEntityAddress(entityAddress).pipe(take(1)).subscribe(() => {
      //this.addressList.push(entityAddress);
      this.refreshAndAddAddressList().pipe().subscribe(() => {
        this.sortAddressList();
        this.setPage(1, this.addressList, true);
        Utils.unblockUI();  
      })
    });
  }

  createExperienceRaterFile() {
    const id: string = this.summaryData.quoteNumber;
    const raterVersionId = this.submissionData.riskDetail.raterVersionId;

    this.raterApi.test().subscribe(file => {
      console.log('Connection to Rater server is established.');
    }, error => {
      this.toastr.error('Unable to contact Rater server');
    });

    this.raterApi.create(id, raterVersionId).subscribe(file => {
      console.log(`Rater file for Policy Number: ${id} is created!`);
    }, (error) => {
      console.log('There was an error on creating Rater excel file. Please try again. ' + error);
    });
  }

  getEntityTypeName(id: number) {
    const entityType = this.entityTypesList.find(et => et.value == id); // strict (===) comparison results to undefined
    return entityType?.label ?? '';
  }

  getValidated(item: RiskAdditionalInterestsDTO) {
    return !this.isEmpty(item.address) && !this.isEmpty(item.city) && !this.isEmpty(item.state) && !this.isEmpty(item.zipCode);
  }

  isEmpty(str) {
      return (!str || str.length === 0 );
  }

  getAdditionalInterests(fromEndorsement: boolean = false, isNew: boolean = false) {
    this.getAiList(this.getAIListPayload(this.currentPage, this.itemPerPage, fromEndorsement), isNew);
  }

  refreshAndAddAddressList() : Observable<any>{
    return this.applicantNameAddressService.getNameAndAddress(this.submissionData.riskDetail.id).pipe(
        take(1),
        tap((data) => {
          data.addresses.forEach(address => {
            const addressFromUI = this.addressList.filter(x => x.id == address.id);
            if (addressFromUI == null || addressFromUI.length == 0)
            {
              address.addressType = this.getAddressTypeName(address.addressTypeId);
              this.addressList.push(address);
            }
          });
    }));
  }

  getAddressTypeName(id) { return LvAddressTypeList.find(x => x.id === id).value; }

  sort(targetElement, data) {
    this.tableData.sort(targetElement, data, 1);
  }

  sortAddress(targetElement) {
    let sortData = this.tableData.sortPagedData(targetElement, this.addressList, this.currentPage);
    this.addressList = sortData.sortedData;
    this.pagedItems = sortData.pagedItems;
  }

  sortAdditionalInterests(targetElement) {
    const property = targetElement.getAttribute('data-name');
    this.sortField =property;
    this.order= this.order =='ASC' ? 'DESC' : 'ASC';
    this.getAiList(this.getAIListPayload(this.currentPage, this.itemPerPage, this.submissionData?.riskDetail?.isPolicy));
  }

  setPage(page: number, data: any[], isNew: boolean = false) {
    this.pager = {};
    this.pagedItems = [];
    if (page < 1) {
      return;
    }

    this.pager = this.pagerService.getPager(data.length, page);

    if (isNew) {
      this.currentPage = this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }
      this.currentPage = page;
    }

    this.pagedItems = data.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  get isBusinessDetailsValidToRate() {
    const rateFields = ['yearBusinessInput', 'yearUnderCurrentInput', 'businessTypeDropdown',
      'mainUseDropdown', 'secondaryBusinessGroupDropdown', 'accountCategoryDropdown', 'accountCategoryUWDropdown']
    const a = rateFields.every(rf => FormUtils.validateFormControl(this.applicantForms.businessDetails.controls[rf]));
    return a;
  }

  setFeeDetails(state: string): void {
    let feeDetails: FeeDetailsDto[] = [];
    const result = this.applicantNameAddressService.getFeeDetailsByState(state);
    result.forEach((data) => {
      let feeDetail = new FeeDetailsDto();
      feeDetail.stateCode = data?.stateCode;
      feeDetail.feeKindId = data?.feeKindId;
      feeDetail.description = data?.description;
      feeDetail.amount = data?.amount;
      feeDetail.isActive = data?.isActive;
      feeDetails.push(feeDetail);
    });

    this.submissionData.riskDetail.feeDetails = feeDetails;
  }

  private sortByAddProcessDate(cv1, cv2) {
    return new Date(cv1.addProcessDate).getTime() - new Date(cv2.addProcessDate).getTime();
  }

  get nameAddressFormControls() { return this.applicantForms.nameAndAddressForm.controls; }
  get businessDetailsFormControls() { return this.applicantForms.businessDetails.controls; }
  get billingInfoFormControls() { return this.applicantForms.billingInfoForm.controls; }
  get additionalInterestFormControls() { return this.applicantForms.additionalInterestForm.controls; }

  private resetFieldNameAndAddress(fieldName: string) {
    this.nameAddressFormControls[fieldName].reset();
    this.nameAddressFormControls[fieldName].clearValidators();
    this.nameAddressFormControls[fieldName].updateValueAndValidity();
  }

  private setAsRequiredNameAndAddress(fieldName: string, type?: string) {
    let validator = [Validators.required];
    if (type == 'email') {
      validator.push(Validators.pattern(
        '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,5}$'
      ));
    } else if (type == 'phone') {
      validator.push(CustomValidators.phoneNumberValidator);
    }
    this.nameAddressFormControls[fieldName].setValidators(validator);
    this.nameAddressFormControls[fieldName].updateValueAndValidity();
  }

  private resetFieldBusinessDetails(fieldName: string) {
    this.businessDetailsFormControls[fieldName].reset();
    this.businessDetailsFormControls[fieldName].clearValidators();
    this.businessDetailsFormControls[fieldName].updateValueAndValidity();
  }

  private setAsRequiredBusinessDetails(fieldName: string) {
    this.businessDetailsFormControls[fieldName].setValidators(Validators.required);
    this.businessDetailsFormControls[fieldName].updateValueAndValidity();
  }

  private resetFieldAdditionalInterest(fieldName: string) {
    this.additionalInterestFormControls[fieldName].reset();
    this.additionalInterestFormControls[fieldName].clearValidators();
    this.additionalInterestFormControls[fieldName].updateValueAndValidity();
  }

  private setAsRequiredAdditionalInterest(fieldName: string) {
    this.additionalInterestFormControls[fieldName].setValidators(Validators.required);
    this.additionalInterestFormControls[fieldName].updateValueAndValidity();
  }


  enableBindRequiredFields(enable: boolean) {
    if (enable) {
      this.setAsRequiredNameAndAddress('businessPrincipalInput');
      this.setAsRequiredNameAndAddress('emailInput', 'email');
      this.setAsRequiredNameAndAddress('phoneExtInput', 'phone');

      this.setAsRequiredBusinessDetails('federalIdInput');

      this.setAsRequiredAdditionalInterest('address');
      this.setAsRequiredAdditionalInterest('city');
      this.setAsRequiredAdditionalInterest('state');
      this.setAsRequiredAdditionalInterest('zipCode');

      return;
    }

    this.resetFieldNameAndAddress('businessPrincipalInput');
    this.resetFieldNameAndAddress('emailInput');
    this.resetFieldNameAndAddress('phoneExtInput');

    this.resetFieldBusinessDetails('federalIdInput');

    this.resetFieldAdditionalInterest('address');
    this.resetFieldAdditionalInterest('city');
    this.resetFieldAdditionalInterest('state');
    this.resetFieldAdditionalInterest('zipCode');
  }

  enableRequiredFields() {
    this.setAsRequiredAdditionalInterest('address');
    this.setAsRequiredAdditionalInterest('city');
    this.setAsRequiredAdditionalInterest('state');
    this.setAsRequiredAdditionalInterest('zipCode');
    return;
  }

  public refreshQuoteOptionsAndSummaryHeader(): void {
    this.quoteLimitsData.loadQuoteOptions().subscribe(data => {
      if (this.submissionData.isIssued) {
        this.policySummaryData.getBillingSummary();
      } else {
        const bindOptionId = 1;
        const riskCoverage = data?.riskCoverages.find(coverage => coverage.optionNumber == bindOptionId);
        this.summaryData.estimatedPremium = riskCoverage?.riskCoveragePremium?.premium;
      }
    });
  }

  getAiList(payload: RiskAddnlInterestListRequestDTO = {}, isNew: boolean = false): void {
    const riskDetailId = this.submissionData.riskDetail?.id;
    Utils.blockUI();
    this.additionalInterestService.getAdditionalInterestsWithPaging(payload)
      .pipe(take(1), finalize(() => {
          this.refreshQuoteOptionsAndSummaryHeader();
        Utils.unblockUI();  
      })).subscribe(data => {
        this.totalRowCount = data.paginationDetails.totalCount;
        this.rowCount = data.paginationDetails.totalCount;
        this.additionalInterests = data.additionalInterest;
        this.currentPage = isNew? data.paginationDetails.totalPages  : data.paginationDetails.currentPage;
        this.additionalInterests.forEach(ai => {
            ai.entityType = this.getEntityTypeName(ai.additionalInterestTypeId);
            ai.isValidated = this.getValidated(ai);
        Utils.unblockUI();  
        });
      this.pagedItems = this.additionalInterests;
      this.submissionData.riskDetail.additionalInterest = this.additionalInterests;
      
    });
   
  }

  getAIListPayload(pageNumber: number = 1, pageSize: number = this.itemPerPage, fromEndorsement:boolean): RiskAddnlInterestListRequestDTO {
    const payload: RiskAddnlInterestListRequestDTO = {
      pageNumber: pageNumber,
      pageSize: pageSize,
      riskDetailId : this.submissionData.riskDetail?.id,
      fromEndorsement : fromEndorsement,
      sortField: this.sortField,
      order: this.order
    };
    return payload;
  }
  
  public setApplicantDataset() {
    this.SetApplicantMainGVWList();
    this.SetApplicantSecondaryBusGroupList();
  }

  public fetchApplicantMainGVWList() {
    const isSupported = this.vehicleData.raterVersionSupport();
    return this.applicantBusinessDetailsService.getApplicantMainGVW(isSupported);
  }
  public fetchApplicantSecondaryBusGroupList() {
    const isSupported = this.vehicleData.raterVersionSupport();
    return this.applicantBusinessDetailsService.getApplicantSecondaryBusGroup(isSupported);
  }

  public SetApplicantMainGVWList() {
    this.fetchApplicantMainGVWList().subscribe(data => {
      this.applicantDropdownList.mainGVWList = (data as DatasetDTO[]).map(d => ({ label: d.descriptionInUI, value: d.value }));
    });
  }
  public SetApplicantSecondaryBusGroupList() {
    this.fetchApplicantSecondaryBusGroupList().subscribe(data => {
      this.applicantDropdownList.secondaryBusinessGroupList = (data as DatasetDTO[]).map(d => ({ label: d.descriptionInUI, value: d.value }));
    });
  }

  
}

export class ApplicantInformationDropDowns {
  mainGVWList: SelectItem[];
  secondaryBusinessGroupList: SelectItem[];
}
