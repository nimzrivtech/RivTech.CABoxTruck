import { Injectable, Injector } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BindingDto } from '../../../../shared/models/submission/binding/BindingDto';
import { BindingRequirementsDto } from '../../../../shared/models/submission/binding/BindingRequirementsDto';
import { SelectItem } from '../../../../shared/models/dynamic/select-item';
import { BindingService } from '../../services/binding.service';
import { finalize, switchMap, take, tap } from 'rxjs/operators';
import { SubmissionData } from '../submission.data';
import { Observable, of  } from 'rxjs';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { FileUploadDocumentDto } from '../../../../shared/models/submission/binding/FileUploadDocumentDto';
import Utils from '../../../../shared/utilities/utils';
import { ToastrService } from 'ngx-toastr';
import { QuoteLimitsData } from '../quote/quote-limits.data';
import { QuoteConditionsDto } from '../../../../shared/models/submission/binding/QuoteConditionsDto';
import { TableData } from '../tables.data';
import { PagerService } from '../../../../core/services/pager.service';
import { fileSizeValidatorMultiple, fileTypeValidatorMultiple } from '../../helpers/file.validator';
import { RiskCoverageDTO, RiskCoverageListDTO } from '@app/shared/models/submission/limits/riskCoverageDto';
import { QuoteOptionsService } from '../../services/quote-options.service';
import { SummaryData } from '../summary.data';
import { DocumentData } from '../../../../modules/documents/document.data';
import { PathConstants } from '../../../../shared/constants/path.constants';
import { AuthService } from '@app/core/services/auth.service';
import { FormDTO } from '@app/shared/models/submission/forms/FormDto';
import { QuoteConditionRequestDTO } from '@app/shared/models/submission/binding/QuoteConditionRequestDTO';

export abstract class BindFormGroup {
  formBinding?: FormGroup;
  formBindingRequirements?: FormGroup;
  formQuoteConditons?: FormGroup;
  formFileUpload?: FormGroup;
}

@Injectable()
export class BindingData extends BindFormGroup {
  //public bindFormGroup: BindFormGroup = new BindFormGroup();
  public dropdownList: BindingDropDowns = new BindingDropDowns();
  public bindingRequirementsList: Array<BindingRequirementsDto> = new Array<BindingRequirementsDto>();
  public quoteConditionsList: Array<QuoteConditionsDto> = new Array<QuoteConditionsDto>();

  requirementsPager: any = {};
  requirementsCurrentPage: number = 1;
  requirementsPagedItems: any[];
  loadingRequirements: boolean;
  loadingConditions: boolean;
  conditionsPager: any = {};
  conditionsCurrentPage: number = 1;
  conditionsPagedItems: any[];
  formsSelections: FormDTO[];
  
  currentTotalItemsRequirements: number = 1;
  currentTotalItemsConditions: number = 1;
  showRowsNumberRequirements: number = 10;
  showRowsNumberConditions: number = 10;

  constructor(private formBuilder: FormBuilder,
    private service: BindingService,
    private submissionData: SubmissionData,
    private toastr: ToastrService,
    public quoteLimitsData: QuoteLimitsData,
    private tableData: TableData,
    private pagerService: PagerService,
    private quoteOptionsService: QuoteOptionsService,
    private documentData: DocumentData,
    private summaryData: SummaryData,
    private authServer: AuthService,
    private injector: Injector
  ) {
    super(); // must call super()
  }

  initiateBindingFormGroup(obj?: BindingDto): FormGroup {
    this.formBinding = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      bindOptionId: [this.setInitialValue(obj.bindOptionId, null)],
      paymentTypesId: [this.setInitialValue(obj.paymentTypesId, null)],
      creditedOfficeId: [this.setInitialValue(obj.creditedOfficeId, null)],
      surplusLIneNum: [this.setInitialValue(obj.surplusLIneNum, null)],
      producerSLNumber: [this.setInitialValue(obj.producerSLNumber, null)],
      slState: [this.setInitialValue(obj.slState, null)],
      slaNum: [this.setInitialValue(obj.slaNum, null)],
      minimumEarned: [this.setInitialValue(obj.minimumEarned, null)],
      createdBy: [this.setInitialValue(obj.createdBy, null)],
      createdDate: [this.setInitialValue(obj.createdDate, null)],
      updatedDate: [this.setInitialValue(obj.updatedDate, null)],
    }, {
    });

    if (this.submissionData.isQuoteIssued || this.submissionData.isIssued) {
      this.enableBindRequiredFields(true);
    }

    return this.formBinding;
  }

  initiateBindingRequirementsFormGroup(obj?: BindingRequirementsDto): FormGroup {
    this.formBindingRequirements = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      bindRequirementsOptionId: [this.setInitialValue(obj.bindRequirementsOptionId, null), Validators.required],
      isPreBind: [this.setInitialValue(obj.isPreBind, false)],
      bindStatusId: [this.setInitialValue(obj.bindStatusId, null), Validators.required],
      describe: [this.setInitialValue(obj.describe, null)],
      comments: [this.setInitialValue(obj.comments, null)],
      relevantDocumentDesc: [this.setInitialValue(obj.relevantDocumentDesc, null)],

      riskDetailId: [this.setInitialValue(obj.riskDetailId, null)],
      createdBy: [this.setInitialValue(obj.createdBy, null)],
      createdDate: [this.setInitialValue(obj.createdDate, null)],
      updatedDate: [this.setInitialValue(obj.updatedDate, null)],
      IsActive: [this.setInitialValue(obj.IsActive, null)],
      deletedDate: [this.setInitialValue(obj.updatedDate, null)],

      fileUploads: [this.setInitialValue(obj.fileUploads, null)],
    }, {
    });

    return this.formBindingRequirements;
  }

  initiateFileUploadDocumentDtoFormGroup(obj?: FileUploadDocumentDto): FormGroup {
    this.formFileUpload = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      tableRefId: [this.setInitialValue(obj.tableRefId, null)],
      fileCategoryId: [this.setInitialValue(obj.fileCategoryId, null)],
      description: [this.setInitialValue(obj.description, null), Validators.required],
      fileName: [this.setInitialValue(obj.fileName, null)],
      filePath: [this.setInitialValue(obj.filePath, null)],
      fileExtension: [this.setInitialValue(obj.fileExtension, null)],
      mimeType: [this.setInitialValue(obj.mimeType, null)],
      isConfidential: [this.setInitialValue(obj.isConfidential, null)],
      isUploaded: [this.setInitialValue(obj.isUploaded, null)],
      createdBy: [this.setInitialValue(obj.createdBy, null)],
      createdDate: [this.setInitialValue(obj.createdDate, null)],
      updatedDate: [this.setInitialValue(obj.updatedDate, null)],

      isActive: [this.setInitialValue(obj.isActive, null)],
      removedBy: [this.setInitialValue(obj.removedBy, null)],
      isUserUploaded: [this.setInitialValue(obj.isUserUploaded, null)],

      deletedDate: [this.setInitialValue(obj.deletedDate, null)],
      fileInfo: [null],
      fileValidation: [null, [fileTypeValidatorMultiple, fileSizeValidatorMultiple]],
    }, {
    });

    if (obj.createdDate && !isNaN(new Date(obj.createdDate).getTime())) { this.formFileUpload.get('createdDate').setValue({ isRange: false, singleDate: { jsDate: new Date(obj?.createdDate) } }); }
    if (obj.updatedDate) { this.formFileUpload.get('createdDate').setValue({ isRange: false, singleDate: { jsDate: new Date(obj?.updatedDate) } }); }
    if (obj.fileInfo) { this.formFileUpload.get('fileInfo').setValue(obj?.fileInfo); }

    return this.formFileUpload;
  }

  initiateQuoteConditionFormGroup(obj?: QuoteConditionsDto): FormGroup {
    this.formQuoteConditons = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      quoteConditionsOptionId: [this.setInitialValue(obj.quoteConditionsOptionId, null), Validators.required],
      describe: [this.setInitialValue(obj.describe, null)],
      comments: [this.setInitialValue(obj.comments, null)],

      relevantDocumentDesc: [this.setInitialValue(obj.relevantDocumentDesc, null)],

      riskDetailId: [this.setInitialValue(obj.riskDetailId, null)],
      createdBy: [this.setInitialValue(obj.createdBy, null)],
      createdDate: [this.setInitialValue(obj.createdDate, null)],
      updatedDate: [this.setInitialValue(obj.updatedDate, null)],
      IsActive: [this.setInitialValue(obj.IsActive, null)],
      deletedDate: [this.setInitialValue(obj.updatedDate, null)],

      fileUploads: [this.setInitialValue(obj.fileUploads, null)],
    }, {
    });

    return this.formQuoteConditons;
  }

  retrieveDropdownValues(callbackFn?: Function) {
    const hasSelectedAPD = (this.submissionData?.riskDetail?.riskCoverage?.compDeductibleId ?? 76) !== 76
                          || (this.submissionData?.riskDetail?.riskCoverage?.collDeductibleId ?? 65) !== 65
                          || (this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId ?? 76) !== 76;
    const hasSymbol10 = (this.submissionData?.riskDetail?.riskCoverage?.alSymbolId && this.submissionData?.riskDetail?.riskCoverage?.alSymbolId == 14 ||
      this.submissionData?.riskDetail?.riskCoverage?.alSymbolId && this.submissionData?.riskDetail?.riskCoverage?.alSymbolId == 15);
    const alSymbolId = this.submissionData?.riskDetail?.riskCoverage?.alSymbolId;
    // tslint:disable-next-line: deprecation
    this.service.getBindOption().pipe(take(1)).subscribe(data => {
      this.dropdownList.bindOptionList = data.filter(f => !this.quoteLimitsData.optionIsDisabled(f.id)
        || (this.submissionData?.riskDetail?.riskCoverages?.length == 0 && f.id == 1)
        || (this.submissionData?.riskDetail?.riskCoverages?.length == undefined && f.id == 1)
      ).map(s => ({
        value: s.id,
        label: s.description?.toString()
      }) as SelectItem) as SelectItem[];
    });

    // this.service.getPricingOption().pipe(take(1)).subscribe(data => {
    //   this.dropdownList.pricingOptionList = data.map(s => ({
    //     value: s.id?.toString(),
    //     label: s.description?.toString()
    //   }) as SelectItem) as SelectItem[];
    // });

    this.service.getpaymentTypesList().pipe(take(1)).subscribe(data => {
      this.dropdownList.paymentTypesList = data.map(s => ({
        value: s.id?.toString(),
        label: s.description?.toString()
      }) as SelectItem) as SelectItem[];
    });

    this.service.getCreditedOffices().pipe(take(1)).subscribe(data => {
      this.dropdownList.creditOfficeList = data.map(s => ({
        value: s.id?.toString(),
        label: s.description?.toString()
      }) as SelectItem) as SelectItem[];
    });

    this.service.getStates().pipe(take(1)).subscribe(data => {
      const surplusLineStateFulleName: string = data.find(state => state.stateCode === this.defaultPolicyState)?.fullStateName?.toString()?.replace('[E]', '');
      this.formBinding?.controls.slState.patchValue(surplusLineStateFulleName);
    });

    this.service.getSubjectivities().pipe(take(1)).subscribe(data => {
      this.dropdownList.descriptionList = data
        .map(s => ({
          value: s.id?.toString(),
          label: s.description?.toString()
        }) as SelectItem) as SelectItem[];
    });

    this.service.getbindRequirementsOption().pipe(take(1)).subscribe(data => {
      this.dropdownList.bindRequirementList = data
        .map(s => ({
          value: s.id?.toString(),
          label: s.description?.toString()
        }) as SelectItem) as SelectItem[];
    });

    this.service.getBindStatus().pipe(take(1)).subscribe(data => {
      this.dropdownList.bindStatusList = data.map(s => ({
        value: s.id?.toString(),
        label: s.description?.toString()
      }) as SelectItem) as SelectItem[];
    });

    this.service.getQuoteConditionsOption().pipe(take(1)).subscribe(data => {
      if (!hasSelectedAPD) {
        data = data.filter(opt => opt?.id !== 13);
      }
      if (!hasSymbol10) {
        data = data.filter(opt => opt?.id !== 23);
      }
      this.dropdownList.quoteConditionList = data.map(s => ({
        value: s.id?.toString(),
        label: this.formatQuoteConditionLabel(s, alSymbolId),
        isActive: s.isActive
      }) as SelectItem) as SelectItem[];
    });

    if (callbackFn) { callbackFn(); }
  }

  private formatQuoteConditionLabel(condition: any, alSymbolId: any): string {
    switch(condition.id){
      case 14:
        return condition.description?.replace('{VAL}', this.submissionData?.riskDetail?.binding?.minimumEarned)?.toString();
      case 23:
        if(alSymbolId == 14)
          return condition.description?.replace('7/8/9', '2/8/9')?.toString();
        else if (alSymbolId == 15)
          return condition.description?.replace('2/8/9', '7/8/9')?.toString();
        else
          return condition.description?.toString();
      default:
        return condition.description?.toString();
    }
  }

  initializeForms() {
    if (this.submissionData.riskDetail?.binding) {
      this.initiateBindingFormGroup(this.submissionData.riskDetail?.binding);
    } else {
      this.initiateBindingFormGroup(new BindingDto());
    }

    if (this.submissionData.riskDetail?.bindingRequirements) {
      this.bindingRequirementsList = this.submissionData.riskDetail?.bindingRequirements?.map(x => Object.assign({}, x));
    } else {
      this.bindingRequirementsList = Array<BindingRequirementsDto>();
    }
    this.initiateBindingRequirementsFormGroup(new BindingRequirementsDto());

    if (this.submissionData.riskDetail?.quoteConditions) {
      this.quoteConditionsList = this.submissionData.riskDetail?.quoteConditions?.map(x => Object.assign({}, x));
    } else {
      this.quoteConditionsList = Array<QuoteConditionsDto>();
    }
    this.initiateQuoteConditionFormGroup(new QuoteConditionsDto());
    this.initiateFileUploadDocumentDtoFormGroup(new FileUploadDocumentDto());
    this.initiateARSCommissionFields();
  }

  onMinimumEarnedInputChange(event: any) {
    let value = parseFloat(event.target.value?.replace('%', ''));

    if(value > 100) value = 100;

    this.updateMinimumEarnedOnConditionsList(value);
  }

  updateMinimumEarnedOnConditionsList(value: any){
    if(!value) return;
    
    const quoteCondition = this.dropdownList.quoteConditionList.find(q => q.value == '14');
    quoteCondition.label = `Policy Subject to ${value}% Minimum Premium`;

    const fg = this.formBinding;
    fg.controls['minimumEarned'].patchValue(value?.toString());

    this.setConditionsPage(this.conditionsCurrentPage, []);
    setTimeout(() => {
      this.setConditionsPage(this.conditionsCurrentPage, this.quoteConditionsList);
      this.saveBinding();
    });
  }

  public onChangeBindOption(value: any) {
    // update selected option column
    this.updateBindOptionPaymentPlan("1");
  }

  public onChangepaymentTypes(value: any) {
    // update selected option column
    this.updateBindOptionPaymentPlan(value);
  }

  public onChangeCreditOffice(value: any) {
    this.getSurplusLineDefault(this.formBinding.controls.creditedOfficeId?.value, this.defaultPolicyState).subscribe(data => {
      this.formBinding.controls.surplusLIneNum.setValue(data.licenseNumber);
      this.formBinding.controls.producerSLNumber.setValue(data.producerSLNumber);
      this.saveBinding();
      this.saveARSCommission();
    });
  }

  public surplusLIneNumChanged() {
    if (this.formBinding.controls.surplusLIneNum.value != null) {
      this.saveBinding();
    }
  }

  public slaNumChanged() {
    this.saveBinding();
  }

  public initiateARSCommissionFields() {
    this.formBinding.addControl('arsCommissionPercent', new FormControl(''));
    this.formBinding.addControl('arsCommission', new FormControl(''));
  }

  public initiateARSCommissionValues() {
    const riskCoverages = this.submissionData.riskDetail?.riskCoverages;

    if (riskCoverages) {
      this.formBinding.get('arsCommissionPercent').setValue(riskCoverages[0]?.arsCommission);
      this.formBinding.get('arsCommission').setValue(riskCoverages[0]?.riskCoveragePremium?.arsCommissionAmount);
    }
    else{
      this.formBinding.get('arsCommissionPercent').setValue(0);
      this.formBinding.get('arsCommission').setValue(0);
    }
  }

  public saveBinding() {
    
    // if the SLA is conditional to Name and address policy. im thinking if need to trigger this on changing non NJ.
    const fg = this.formBinding;
    const binding: BindingDto = new BindingDto(fg.value);
    const riskDetail = this.submissionData.riskDetail;

    if (!binding.riskId) { binding.riskId = riskDetail?.riskId; }
    if (!binding.minimumEarned) {
      binding.minimumEarned = riskDetail?.binding?.minimumEarned || riskDetail?.binding?.minimumEarned == 0 ? riskDetail?.binding?.minimumEarned : Defaults.BindingDefaults.minimumEarned;
    }

    if (!this.showSLA && binding.slaNum?.length > 0) {
      binding.slaNum = null;
    }

    // Set default bindOption and paymentTypesId if current and previous is null
    if (!binding.bindOptionId) {
      binding.bindOptionId = riskDetail?.binding?.bindOptionId ? riskDetail?.binding?.bindOptionId : Defaults.BindingDefaults.bindOptionId.toString();
    } // Option 1 };
    if (!binding.creditedOfficeId) {
      binding.creditedOfficeId = riskDetail?.binding?.creditedOfficeId ? riskDetail?.binding?.creditedOfficeId : Defaults.BindingDefaults.creditedOfficeId.toString();
    } // Applied
    if (!binding.paymentTypesId) {
      if (riskDetail?.binding?.paymentTypesId) {
        binding.paymentTypesId = riskDetail?.binding?.paymentTypesId;
      } else {
        if (this.submissionData.riskDetail?.riskCoverages?.length > 0) {
          if (this.submissionData.riskDetail?.riskCoverages[0]?.depositPct == 100) { // If BindOption default value is Option 1
            binding.paymentTypesId = '1'; // Direct Bill Full-Pay
          } else {
            binding.paymentTypesId = Defaults.BindingDefaults.paymentTypesId.toString(); // direct bill installments
          }
        }
      }
    }

    if(this.formBinding.value.paymentTypesId != null)
    {
      binding.paymentTypesId =this.formBinding.value.paymentTypesId;
    }
    binding.surplusLIneNum = binding.surplusLIneNum?.length === 0 ? null : binding.surplusLIneNum?.trim();
    //binding.surplusLIneNum = binding.surplusLIneNum?.length === 0 ? null : binding.surplusLIneNum?.trim() ===this.formBinding.controls.surplusLIneNum.value.trim()? binding.surplusLIneNum?.trim()  : this.formBinding.controls.surplusLIneNum.value.trim();
    if (!binding.surplusLIneNum && riskDetail?.binding?.surplusLIneNum) {
      binding.surplusLIneNum = riskDetail?.binding?.surplusLIneNum;
    }

    binding.slaNum = binding.slaNum?.length === 0 ? null : binding.slaNum;

    // Update form values before saving
    fg.controls['bindOptionId'].patchValue(binding.bindOptionId);
    fg.controls['creditedOfficeId'].patchValue(binding.creditedOfficeId);
    fg.controls['paymentTypesId'].patchValue(binding.paymentTypesId);
    fg.controls['surplusLIneNum'].patchValue(binding.surplusLIneNum);
    fg.controls['minimumEarned'].patchValue(binding.minimumEarned);
    fg.controls['slaNum'].patchValue(binding.slaNum);
    

    if (binding.surplusLIneNum) {
      this.savingBinding(binding);
    } else {
      binding.surplusLIneNum = '91510'; // Temp Fix default surplusLineNum for Applied
      this.formBinding.controls.surplusLIneNum.setValue('91510'); // Temp Fix default surplusLineNum for Applied
      this.getSurplusLineDefault(this.formBinding.controls.creditedOfficeId?.value, this.defaultPolicyState).subscribe(data => {
        this.formBinding.controls.surplusLIneNum.setValue(data.licenseNumber);
        this.formBinding.controls.producerSLNumber.setValue(data.producerSLNumber);
        binding.surplusLIneNum = data.licenseNumber;
        binding.producerSLNumber = data.producerSLNumber;
        this.savingBinding(binding);
      });
    }
  }

  savingBinding(binding) {
    const fg = this.formBinding;
    Utils.blockUI();
    try {
      // tslint:disable-next-line: deprecation
      this.service.addUpdateBinding(binding).pipe(take(1)).subscribe(data => {
        Utils.unblockUI();
        this.submissionData.riskDetail.binding = data;
        // this.summaryData.updateEstimatedPremium();
        // this.initiateBindingFormGroup(data);
        fg.controls['id'].patchValue(data.id);
      }, (error) => {
        Utils.unblockUI();
        if (this.submissionData.riskDetail.binding) {
          this.initiateBindingFormGroup(this.submissionData.riskDetail?.binding);
        }
      });
    } catch {
      Utils.unblockUI();
      if (this.submissionData.riskDetail.binding) {
        this.initiateBindingFormGroup(this.submissionData.riskDetail?.binding);
      }
    }
  }

  saveARSCommission() {
    const fg = this.formBinding;
    const binding: BindingDto = new BindingDto(fg.value);

    this.submissionData?.riskDetail?.riskCoverages.forEach (option => {
        option.arsCommission = parseInt(binding.creditedOfficeId, 10) === 3 ? (option.brokerCommisionAL >= 12 ? 2.5 : 1) : 0;
        option.riskCoveragePremium.arsCommissionAmount = parseInt(binding.creditedOfficeId, 10) === 3 ? option.riskCoveragePremium.arsCommissionAmount : 0;
      }
    );

    fg.controls['arsCommissionPercent'].setValue(this.submissionData?.riskDetail?.riskCoverages[0].arsCommission);
    fg.controls['arsCommission'].setValue(this.submissionData?.riskDetail?.riskCoverages[0].riskCoveragePremium?.arsCommissionAmount);

    Utils.blockUI();
    this.quoteOptionsService.saveQuoteOptions({
      riskDetailId: this.submissionData.riskDetail.id,
      riskCoverages: this.submissionData?.riskDetail?.riskCoverages
    }).pipe(
      switchMap(() => this.quoteLimitsData.loadQuoteOptions()),
      finalize(() => Utils.unblockUI())
    ).subscribe();  
  }

  SubmitBindRequirements(data: BindingRequirementsDto, callback?: Function) {
    const formData = new FormData();

    const fg = this.formBindingRequirements;
    const bindingReq: BindingRequirementsDto = data ?? new BindingRequirementsDto(fg.value);
    if (!bindingReq.riskDetailId) { bindingReq.riskDetailId = this.submissionData.riskDetail?.id; }

    if (!bindingReq.riskDetailId || !this.defaultPolicyState) {
      NotifUtils.showError('The assigned Submission Id is not detected. Please try again');
      return;
    }

    if (!bindingReq.riskDetailId || !this.defaultPolicyState) {
      NotifUtils.showError('Policy State is not detected. Please try again');
      return;
    }

    const isNew: boolean = bindingReq.id == null;
    const fgFile = this.formFileUpload;
    bindingReq.fileUploads = new Array<FileUploadDocumentDto>();
    if (fgFile?.valid) {
      formData.append('file', fgFile.controls.fileInfo.value);

      const fileInfo = fgFile.controls.fileInfo.value;
      if (fileInfo) {
        Array.from(fileInfo).forEach(file => {
          if (file instanceof File) {
            formData.append('file', (<File>file), (<File>file).name);
            const f: FileUploadDocumentDto = new FileUploadDocumentDto();
            f.fileCategoryId = 4; // Bind Requirement
            f.fileName = (<File>file).name;
            f.createdDate = fgFile.get('createdDate').value?.singleDate?.jsDate;
            f.createdBy = Number(this.authServer.getCurrentUserId());
            f.description = fgFile.get('description').value;
            f.isUserUploaded = true;
            bindingReq.fileUploads.push(f);
          }
        });
      }
    }
    //remove describe if not "Others" = 16
    if (bindingReq.bindRequirementsOptionId != "16") {
      bindingReq.describe = null;
    }

    // This is EnumerationDto fields in backend.
    // Temporarily removed the values.
    bindingReq.bindRequirementsOption = null;
    bindingReq.bindStatus = null;

    formData.append('data', JSON.stringify(bindingReq));
    formData.append('states', this.defaultPolicyState); // get sates from policy States.

    Utils.blockUI();
    this.service.addUpdateBindingRequirementsWithFile(formData)
      .pipe(finalize(() => {  }), take(1))
      .subscribe(result => {
        let obj = ClassConverter.convertPropertyNames(result.data, ClassConverter.pascalCaseToCamelCase);
        obj.bindRequirementsOption = this.dropdownList.bindRequirementList?.find(x => x.value == obj?.bindRequirementsOptionId)?.label;
        obj.bindStatus = this.dropdownList.bindStatusList?.find(x => x.value == obj?.bindStatusId)?.label;
        if (isNew) {
          this.bindingRequirementsList?.push(obj);
        } else {
          this.bindingRequirementsList = this.bindingRequirementsList?.filter((value, key) => {
            if (value.id === bindingReq.id) {
              Object.assign(value, obj);
            }
            return true;
          });
        }

        Utils.unblockUI();
        if (result.message?.length > 0) {
          // this.toastr.error(result.message);
          NotifUtils.showError(result.message);
          return;
        }
        this.submissionData.riskDetail.bindingRequirements = this.bindingRequirementsList.map(x => Object.assign({}, x));
        this.setRequirementsPage(1, this.bindingRequirementsList, isNew);
        if (callback) { callback(); }
      }, (error) => {
        setTimeout(() => {
          NotifUtils.showError('There was an error trying to submit the data. Please try again');
        }, 50);
      });
  }

  DeleteRequirements(obj) {
    const bindDesc = this.dropdownList?.bindRequirementList?.find(x => x.value == obj?.bindRequirementsOptionId);
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${bindDesc?.label}'?`, () => {
      this.service.delete(obj.id).pipe(take(1)).subscribe(data => {
        this.bindingRequirementsList = this.bindingRequirementsList.filter(t => t.id !== obj.id);
        this.submissionData.riskDetail.bindingRequirements = this.bindingRequirementsList.map(x => Object.assign({}, x));
        this.setRequirementsPage(this.requirementsCurrentPage, this.bindingRequirementsList);
        this.documentData.loadDocuments();
      }, (error) => {
        NotifUtils.showError('There was an error trying to delete the record. Please try again');
      });
    });
  }

  DeleteFileRequirements(tableRefId) {
    if (tableRefId != null) {
      const obj = this.bindingRequirementsList.find(x => x.id === tableRefId);
      if (obj != null) {
        console.log('update bind');
        obj.filePath = null;
        this.submissionData.riskDetail.bindingRequirements = this.bindingRequirementsList.map(x => Object.assign({}, x));
        this.setRequirementsPage(this.requirementsCurrentPage, this.bindingRequirementsList);
      }
    }
  }

  DeleteFileQuoteCondition(tableRefId) {
    if (tableRefId != null) {
      const obj = this.quoteConditionsList.find(x => x.id === tableRefId);
      if (obj != null) {
        console.log('update quote');
        obj.filePath = null;
        this.submissionData.riskDetail.quoteConditions = this.quoteConditionsList.map(x => Object.assign({}, x));
        this.setConditionsPage(this.conditionsCurrentPage, this.quoteConditionsList);
      }
    }
  }

  getFileUploadByRefId(id: string) {
    this.formBindingRequirements.controls.relevantDocumentDesc.setValue('');
    this.formQuoteConditons.controls.relevantDocumentDesc.setValue('');
    this.service.getFileUploadByRefId(id).pipe(take(1)).subscribe(data => {
      if (data.length) {
        var activeFile = data.filter(f => f.isActive != false);
        this.formFileUpload = this.initiateFileUploadDocumentDtoFormGroup(activeFile[0] ?? new FileUploadDocumentDto());
        this.formFileUpload.controls.fileName.setValue(activeFile.map(x => x.fileName).join('\n'));
        this.formBindingRequirements.controls.relevantDocumentDesc.setValue(this.formFileUpload.controls.description.value);
        this.formQuoteConditons.controls.relevantDocumentDesc.setValue(this.formFileUpload.controls.description.value);
      }
    }, (error) => {
    });
  }

  SubmitQuoteOptions(data: QuoteConditionsDto, callback?: Function) {
    const formData = new FormData();

    const fg = this.formQuoteConditons;
    const quoteConditionOpt: QuoteConditionsDto = data ?? new QuoteConditionsDto(fg.value);
    if (!quoteConditionOpt.riskDetailId) { quoteConditionOpt.riskDetailId = this.submissionData.riskDetail?.id; }

    if (!quoteConditionOpt.riskDetailId || !this.defaultPolicyState) {
      NotifUtils.showError('The assigned Submission Id is not detected. Please try again');
      return;
    }

    if (!quoteConditionOpt.riskDetailId || !this.defaultPolicyState) {
      NotifUtils.showError('Policy State is not detected. Please try again');
      return;
    }

    const isNew: boolean = quoteConditionOpt.id == null;
    const fgFile = this.formFileUpload;
    quoteConditionOpt.fileUploads = new Array<FileUploadDocumentDto>();
    if (fgFile?.valid) {
      formData.append('file', fgFile.controls.fileInfo.value);

      const fileInfo = fgFile.controls.fileInfo.value;
      if (fileInfo) {
        Array.from(fileInfo).forEach(file => {
          if (file instanceof File) {
            formData.append('file', (<File>file), (<File>file).name);
            const f: FileUploadDocumentDto = new FileUploadDocumentDto();
            f.fileCategoryId = 5; // Quote condition
            f.fileName = (<File>file).name;
            f.createdDate = fgFile.get('createdDate').value?.singleDate?.jsDate;
            f.createdBy = Number(this.authServer.getCurrentUserId());
            f.description = fgFile.get('description').value;
            f.isUserUploaded = true;
            quoteConditionOpt.fileUploads.push(f);
          }
        });
      }
    }

    //remove describe if not "Others" = 10
    if (quoteConditionOpt.quoteConditionsOptionId != "10") {
      quoteConditionOpt.describe = null;
    }

    formData.append('data', JSON.stringify(quoteConditionOpt));
    formData.append('states', this.defaultPolicyState); // get sates from policy States.
    Utils.blockUI();
    this.service.addUpdateQuoteConditionsWithFile(formData)
      .pipe(finalize(() => {  }), take(1))
      .subscribe(result => {
        let obj = ClassConverter.convertPropertyNames(result.data, ClassConverter.pascalCaseToCamelCase);
        if (isNew) {
          this.quoteConditionsList.push(obj);
        } else {
          this.quoteConditionsList = this.quoteConditionsList.filter((value, key) => {
            if (value.id === quoteConditionOpt.id) {
              Object.assign(value, obj);
            }
            return true;
          });
        }

        Utils.unblockUI();
        if (result.message?.length > 0) {
          // this.toastr.error(result.message);
          NotifUtils.showError(result.message);
          return;
        }
        this.submissionData.riskDetail.quoteConditions = this.quoteConditionsList.map(x => Object.assign({}, x));
        const quoteConditionsOptionEl: HTMLElement = document.querySelector('[id=quoteConditionsOption]');
        quoteConditionsOptionEl?.setAttribute('data-order', 'asc');
        this.sortQuoteConditions(quoteConditionsOptionEl);
        this.setConditionsPage(1, this.quoteConditionsList);
        if (callback) { callback(); }
      }, (error) => {
        setTimeout(() => {
          NotifUtils.showError('There was an error trying to submit the data. Please try again');
        }, 50);
      });
  }

  DeleteQuoteConditions(obj: QuoteConditionsDto) {
    const bindDesc = this.dropdownList?.quoteConditionList?.find(x => x.value == obj?.quoteConditionsOptionId);
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${bindDesc?.label}'?`, () => {
      this.service.deleteQuote(obj.id).pipe(take(1)).subscribe(data => {
        this.quoteConditionsList = this.quoteConditionsList.filter(t => t.id !== obj.id);
        this.submissionData.riskDetail.quoteConditions = this.quoteConditionsList.map(x => Object.assign({}, x));
        this.setConditionsPage(this.conditionsCurrentPage, this.quoteConditionsList);
        this.documentData.loadDocuments();
      }, (error) => {
        NotifUtils.showError('There was an error trying to delete the record. Please try again');
      });
    });
  }

  setRequirementsPage(page: number, data: any[], isNew: boolean = false) {
    if (page < 1) {
      return;
    }

    this.requirementsCurrentPage = (isNew) ? Math.ceil(data.length / this.showRowsNumberRequirements) : page;
    this.requirementsPager = this.pagerService.getPagerMod(data.length, this.requirementsCurrentPage, this.showRowsNumberRequirements);

    if (isNew) {
      this.requirementsPager.currentPage = this.requirementsPager.totalPages;
      this.requirementsPager.endIndex = this.requirementsPager.totalItems;
      this.requirementsPager.startIndex = ((this.requirementsPager.currentPage * this.requirementsPager.pageSize) - this.requirementsPager.pageSize);
    } else {
      while (this.requirementsPager.totalItems <= ((this.requirementsPager.currentPage * this.requirementsPager.pageSize) - this.requirementsPager.pageSize)) {
        this.requirementsPager.currentPage--;
        this.requirementsPager.startIndex = ((this.requirementsPager.currentPage * this.requirementsPager.pageSize) - this.requirementsPager.pageSize);
      }

      this.requirementsCurrentPage = page;
    }

    this.requirementsPagedItems = data.slice(this.requirementsPager.startIndex, this.requirementsPager.endIndex + 1);

    this.currentTotalItemsRequirements = this.requirementsPager.totalItems;
  }

  changeShowRowsNumberRequirements(page: number, value: number) {
    if (this.showRowsNumberRequirements === value) { return; }

    this.showRowsNumberRequirements = value;
    let bindingRequirements = Object.assign([], this.bindingRequirementsList);

    this.requirementsPager = this.pagerService.getPagerMod(bindingRequirements.length, page, value);
    this.requirementsPagedItems = bindingRequirements.slice(this.requirementsPager.startIndex, this.requirementsPager.endIndex + 1);
    this.requirementsCurrentPage = page;
    this.currentTotalItemsRequirements = this.requirementsPager.totalItems;
  }

  setConditionsPage(page: number, data: any[], isNew: boolean = false) {
    if (page < 1) {
      return;
    }

    this.conditionsCurrentPage = (isNew) ? Math.ceil(data.length / this.showRowsNumberConditions) : page;
    this.conditionsPager = this.pagerService.getPagerMod(data.length, this.conditionsCurrentPage, this.showRowsNumberConditions);

    if (isNew) {
      this.conditionsPager.currentPage = this.conditionsPager.totalPages;
      this.conditionsPager.endIndex = this.conditionsPager.totalItems;
      this.conditionsPager.startIndex = ((this.conditionsPager.currentPage * this.conditionsPager.pageSize) - this.conditionsPager.pageSize);
    } else {
      while (this.conditionsPager.totalItems <= ((this.conditionsPager.currentPage * this.conditionsPager.pageSize) - this.conditionsPager.pageSize)) {
        this.conditionsPager.currentPage--;
        this.conditionsPager.startIndex = ((this.conditionsPager.currentPage * this.conditionsPager.pageSize) - this.conditionsPager.pageSize);
      }

      this.conditionsCurrentPage = page;
    }

    this.conditionsPagedItems = data.slice(this.conditionsPager.startIndex, this.conditionsPager.endIndex + 1);
    this.currentTotalItemsConditions = this.conditionsPager.totalItems;
  }

  changeShowRowsNumberQuoteConditions(page: number, value: number) {
    if (this.showRowsNumberConditions === value) { return; }

    this.showRowsNumberConditions = value;
    let quoteConditions = Object.assign([], this.quoteConditionsList);

    this.conditionsPager = this.pagerService.getPagerMod(quoteConditions.length, page, value);
    this.conditionsPagedItems = quoteConditions.slice(this.conditionsPager.startIndex, this.conditionsPager.endIndex + 1);
    this.conditionsCurrentPage = page;
    this.currentTotalItemsConditions = this.conditionsPager.totalItems;
  }

  //#region Function Helpers
  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length == 0) ? val : obj;
  }

  getDesc(val) {
    return this.dropdownList.descriptionList.find(x => x.value === val)?.label;
  }
  getStatus(val) { return this.dropdownList.bindStatusList.find(x => x.value === val)?.label; }

  public formatDateForPicker(dateValue?: any) {
    const dateSet = dateValue ? new Date(dateValue) : new Date();
    return { isRange: false, singleDate: { jsDate: dateSet } };
  }

  public bindingDefault() {
    const riskDetail = this.submissionData?.riskDetail;
    if (!this.submissionData.riskDetail?.binding) {
      this.formBinding.controls.bindOptionId.setValue(Defaults.BindingDefaults.bindOptionId); // Option 1
      this.formBinding.controls.creditedOfficeId.setValue(Defaults.BindingDefaults.creditedOfficeId); // Applied
      this.formBinding.controls.minimumEarned.setValue(Defaults.BindingDefaults.minimumEarned); // 25%

      if (riskDetail?.riskCoverages?.length > 0) {
        this.formBinding.controls.arsCommissionPercent.setValue(riskDetail?.riskCoverages[0]?.arsCommission);
        this.formBinding.controls.arsCommissionPercent.setValue(riskDetail?.riskCoverages[0]?.riskCoveragePremium.arsCommissionAmount);
      }
      this.getSurplusLineDefault(this.formBinding.controls.creditedOfficeId?.value, this.defaultPolicyState).subscribe(data => {
        this.formBinding.controls.surplusLIneNum.setValue(data.licenseNumber);
        this.formBinding.controls.producerSLNumber.setValue(data.producerSLNumber);
      });
    }

    if(riskDetail?.riskCoverages?.length > 0)
    {
        if(riskDetail?.riskCoverages[0]?.depositPct == 100) { 
          this.formBinding.controls.paymentTypesId.setValue(this.formBinding.controls.paymentTypesId.value ===3 ? 3 :1)// Direct Bill Full-Pay or Premium Financed
        } else {
          this.formBinding.controls.paymentTypesId.setValue(Defaults.BindingDefaults.paymentTypesId); // direct bill installments
        }
    }
   
   
    if (this.formBinding?.controls.slState.value === null) {
      this.formBinding?.controls.slState.patchValue(this.defaultPolicyState);
    }
  }

  public saveBindingDefault() {
    const riskDetail = this.submissionData?.riskDetail;
    if (this.formBinding?.controls.slState.value === null) {
      this.formBinding?.controls.slState.patchValue(this.defaultPolicyState);
    }
    if (!this.submissionData.riskDetail?.binding) {
      this.formBinding.controls.bindOptionId.setValue(Defaults.BindingDefaults.bindOptionId); // Option 1
      this.formBinding.controls.creditedOfficeId.setValue(Defaults.BindingDefaults.creditedOfficeId); // Applied
      this.formBinding.controls.minimumEarned.setValue(Defaults.BindingDefaults.minimumEarned); // 25%

      if (riskDetail?.riskCoverages?.length > 0) {
        this.formBinding.controls.arsCommissionPercent.setValue(riskDetail?.riskCoverages[0]?.arsCommission);
        this.formBinding.controls.arsCommissionPercent.setValue(riskDetail?.riskCoverages[0]?.riskCoveragePremium.arsCommissionAmount);  

        if (riskDetail?.riskCoverages[0]?.depositPct == 100) { // If BindOption default value is Option 1
          this.formBinding.controls.paymentTypesId.setValue(1); // Direct Bill Full-Pay
        } else {
          this.formBinding.controls.paymentTypesId.setValue(Defaults.BindingDefaults.paymentTypesId); // direct bill installments
        }
      }

      this.getSurplusLineDefault(this.formBinding.controls.creditedOfficeId?.value, this.defaultPolicyState).subscribe(data => {
        this.formBinding.controls.surplusLIneNum.setValue(data.licenseNumber);
        this.formBinding.controls.producerSLNumber.setValue(data.producerSLNumber);

        // Save Binding
        this.saveBinding();
      });
    }
  }

  public bindRequirementListFiltered(bindingReqId): any[] {
    const getCurrent = Array
      .from(new Set(this.bindingRequirementsList
        .map(s => s.bindRequirementsOptionId)))
      .filter(f => f != bindingReqId && f != '16'); // 16 = 'others'

    return this.dropdownList.bindRequirementList.filter(f => getCurrent.findIndex(x => x == f.value) == -1);
  }

  public quoteConditionListFiltered(quoteConditionId): any[] {
    const getCurrent = Array
      .from(new Set(this.quoteConditionsList
        .map(s => s.quoteConditionsOptionId)))
      .filter(f => f != quoteConditionId && f != '10'); // 10 = 'others'
    return this.dropdownList.quoteConditionList.filter(f => f.isActive == true && getCurrent.findIndex(x => x == f.value) == -1);
  }

  public truncate(str, maxLength: number) {
    if (str?.length > maxLength) {
      return str.slice(0, maxLength) + '...';
    } else {
      return str;
    }
  }

  public showTruncateTitle(val, dataset) {

    const str = dataset?.find(f => f.value === val)?.label;
    const strTruncate = this.truncate(str, 70);

    //i should put the value of maxlenght 70 to variable
    if (strTruncate?.length > 70) {
      return str;
    } else {
      return '';
    }
  }

  public getSurplusLineDefault(creditId: string, state: string): Observable<any> {
    // this.service.getSurplusLineDefault(creditId, state).pipe(take(1)).subscribe(data => {
    //   return data.licenseNumber;
    // });
    return this.service.getSurplusLineDefault(creditId, state).pipe(take(1));
  }

  public loadBindReqDefaults(sort: boolean = true) {
    if (this.submissionData?.riskDetail?.bindingRequirements?.length > 0) { return; }
    Defaults.BindingDefaults.bindRequirements.forEach((bindReqOptId) => {
      const dto = new BindingRequirementsDto();
      dto.bindRequirementsOptionId = `${bindReqOptId}`;
      dto.bindStatusId = '1'; // set default status to pending
      dto.isDefault = true;
      this.SubmitBindRequirements(dto, () => {
        // sort
        if (sort) {
          const bindRequirementsOptionEl: HTMLElement = document.querySelector('[id=bindRequirementsOption]');
          bindRequirementsOptionEl?.setAttribute('data-order', 'asc');
          this.sortBindRequirements(bindRequirementsOptionEl);
          this.setRequirementsPage(1, this.bindingRequirementsList);
        }
      });
    });
  }

  public loadQuoteConditionDefaults(previousRiskCoverageDTO: RiskCoverageDTO = null, isPolicy: boolean = false) {
    const request = new QuoteConditionRequestDTO();
    request.isPolicy = isPolicy;
    request.id = this.submissionData.riskDetail.id;
    request.riskCoverage = this.submissionData.riskDetail.riskCoverage;
    request.previousRiskCoverage = previousRiskCoverageDTO != null ? previousRiskCoverageDTO : this.submissionData.riskDetail.riskCoverage;
    
    this.service.getQuoteConditionOptions(request).subscribe(data => {
      this.quoteConditionsList = data;
      const quoteConditionsOptionEl: HTMLElement = document.querySelector('[id=quoteConditionsOption]');
      quoteConditionsOptionEl?.setAttribute('data-order', 'asc');
      this.submissionData.riskDetail.quoteConditions = data;
      this.sortQuoteConditions(quoteConditionsOptionEl);
      this.setConditionsPage(1, this.quoteConditionsList);
      this.updateMinimumEarnedOnConditionsList(this.submissionData.riskDetail?.binding?.minimumEarned?.toString());
    });
  }

  sortBindRequirements(targetElement) {
    if (targetElement) {
      const sortData = this.tableData.sortPagedData(targetElement, this.bindingRequirementsList, this.requirementsCurrentPage, this.showRowsNumberRequirements);
      this.bindingRequirementsList = sortData.sortedData;
      this.requirementsPagedItems = sortData.pagedItems;
    }
  }

  sortQuoteConditions(targetElement) {
    if (targetElement) {
      const sortData = this.tableData.sortPagedData(targetElement, this.quoteConditionsList, this.conditionsCurrentPage, this.showRowsNumberConditions);
      this.quoteConditionsList = sortData.sortedData;
      this.conditionsPagedItems = sortData.pagedItems;
    }
  }

  private updateBindOptionPaymentPlan(flag: string): void {


    if (this.submissionData?.isIssued) { return; }
    let optionNumber: number = Number(this.formBinding.value.bindOptionId);
    let paymendPlanId: number = Number(this.formBinding.value.paymentTypesId);
    if (optionNumber === 0 || paymendPlanId === 0) { return; }
    let quoteLimitsOptionsArray = this.quoteLimitsData.optionsArray;
    let fgOption = quoteLimitsOptionsArray.controls.find(fgOpt => fgOpt.value.optionNumber == optionNumber) as FormGroup;
    let depositPct = fgOption.controls['depositPct'];
    let numberOfInstallments = fgOption.controls['numberOfInstallments'];

    if (flag ==="1" || flag ==="3") {
      
      this.formBinding.controls.paymentTypesId.setValue(flag ==="1"? 1 : 3); // Direct Bill Full-Pay and Premium Financed
      
      fgOption.controls['depositPct'].setValue(100);
      fgOption.controls['numberOfInstallments'].setValue(null);
      
      this.saveBinding();
     }
    if (flag === "2") {
      
      if (paymendPlanId == 1 && depositPct.value == 100) { return; }
      if (paymendPlanId == 2 && depositPct.value < 100) { return; }
      if (paymendPlanId == 3 && depositPct.value == 100) { return; }

      
      this.formBinding.controls.paymentTypesId.setValue(2); 
      fgOption.controls['depositPct'].setValue(10);
      fgOption.controls['numberOfInstallments'].setValue(10);
      this.saveBinding();
    }

  
    let options: RiskCoverageDTO[] = quoteLimitsOptionsArray.value.map(x => this.quoteLimitsData.toRiskCoverageDTO(x));
      options.forEach(opt => {
        if (opt.riskCoveragePremium?.id === null) { opt.riskCoveragePremium = null; }
      });
      Utils.blockUI();
      this.quoteOptionsService.saveQuoteOptions({
        riskDetailId: this.submissionData.riskDetail.id,
        riskCoverages: options
      }).pipe(
        switchMap(() => this.quoteOptionsService.updatePaymentOption(this.submissionData.riskDetail.id, optionNumber)),
        switchMap(() => this.quoteLimitsData.loadQuoteOptions()),
        finalize(() => Utils.unblockUI())
      ).subscribe();
  }

  private resetFieldBinding(fieldName: string) {
    this.formBinding.controls[fieldName].reset();
    this.formBinding.controls[fieldName].clearValidators();
    this.formBinding.controls[fieldName].updateValueAndValidity();
  }

  private setAsRequiredBinding(fieldName: string) {
    this.formBinding.controls[fieldName].setValidators(Validators.required);
    this.formBinding.controls[fieldName].updateValueAndValidity();
  }

  enableBindRequiredFields(enable: boolean) {
    if (enable) {
      this.setAsRequiredBinding('bindOptionId');
      this.setAsRequiredBinding('paymentTypesId');
      this.setAsRequiredBinding('minimumEarned');
      this.setAsRequiredBinding('surplusLIneNum');

      return;
    }

    this.resetFieldBinding('bindOptionId');
    this.resetFieldBinding('paymentTypesId');
    this.resetFieldBinding('minimumEarned');
    this.resetFieldBinding('surplusLIneNum');
  }

  get defaultPolicyState(): string { return this.submissionData.riskDetail?.nameAndAddress?.state; }

  get creditedOffice(): string { return this.submissionData.riskDetail?.binding?.creditedOfficeId; }

  get acceptedFileTypes(): string {
    let acceptedFileTypes: string = '';

    FileUploadFunctions.f.acceptedFileTypes.forEach(item => {
      acceptedFileTypes += `".${item}", `;
    });

    return acceptedFileTypes;
  }

  get fileSizeLimit(): number {
    return FileUploadFunctions.f.fileSizeLimit;
  }

  get invalidFileTypeMessage(): string {
    return FileUploadFunctions.f.invalidFileTypeErrorMessage
      .replace('{0}', this.formFileUpload.controls.fileName.value)
      .replace('{1}', this.acceptedFileTypes.substring(0, this.acceptedFileTypes.length - 2)) + '>';
  }

  get maxFileSizeExceeded(): string {
    return FileUploadFunctions.f.maxFileSizeExceeded;
  }

  get getFileName(): string {
    const fname = this.formFileUpload.controls.fileName.value ?? '';
    return fname.length === 0 ? '' : fname;
  }

  get getSelectedFileCount(): string {
    const fname = this.formFileUpload.controls.fileName.value ?? '';
    const selectedFiles = (fname !== '') ? (fname.split('\n')) : [];
    return (selectedFiles.length === 0) ? 'No File Chosen' : `(${selectedFiles.length}) File(s) Chosen`;
  }

  get showSLA(): boolean {
    return this.defaultPolicyState?.toLowerCase() == 'nj';
  }

  get showARSCommissionFields(): boolean {
    return this.creditedOffice == '3' ? true : false;
  }
  //#endregion

  public resaveBindInfo$(): Observable<any> {
    //this.formInfo.markAllAsTouched();
  //   var hasBindingInfo: boolean = this.formInfo !== undefined ;
  //  if (hasBindingInfo) {
     return this.resaveBindInfo();
    // }
  }


  private resaveBindInfo(){
    // if the SLA is conditional to Name and address policy. im thinking if need to trigger this on changing non NJ.
    const fg = this.formBinding;
    const binding: BindingDto = new BindingDto(fg.value);
    const riskDetail = this.submissionData.riskDetail;

    if (!binding.riskId) { binding.riskId = riskDetail?.riskId; }
    if (!binding.minimumEarned) {
      binding.minimumEarned = riskDetail?.binding?.minimumEarned || riskDetail?.binding?.minimumEarned == 0 ? riskDetail?.binding?.minimumEarned : Defaults.BindingDefaults.minimumEarned;
    }

    if (!this.showSLA && binding.slaNum?.length > 0) {
      binding.slaNum = null;
    }

    // Set default bindOption and paymentTypesId if current and previous is null
    if (!binding.bindOptionId) {
      binding.bindOptionId = riskDetail?.binding?.bindOptionId ? riskDetail?.binding?.bindOptionId : Defaults.BindingDefaults.bindOptionId.toString();
    } // Option 1 };
    if (!binding.creditedOfficeId) {
      binding.creditedOfficeId = riskDetail?.binding?.creditedOfficeId ? riskDetail?.binding?.creditedOfficeId : Defaults.BindingDefaults.creditedOfficeId.toString();
    } // Applied
    if (!binding.paymentTypesId) {
      if (riskDetail?.binding?.paymentTypesId) {
        binding.paymentTypesId = riskDetail?.binding?.paymentTypesId;
      } else {
        if (this.submissionData.riskDetail?.riskCoverages?.length > 0) {
          this.formBinding.controls.arsCommissionPercent.setValue(riskDetail?.riskCoverages[0]?.arsCommission);
          this.formBinding.controls.arsCommissionPercent.setValue(riskDetail?.riskCoverages[0]?.riskCoveragePremium.arsCommissionAmount);  
  
          if (this.submissionData.riskDetail?.riskCoverages[0]?.depositPct == 100) { // If BindOption default value is Option 1
            binding.paymentTypesId = '1'; // Direct Bill Full-Pay
          } else {
            binding.paymentTypesId = Defaults.BindingDefaults.paymentTypesId.toString(); // direct bill installments
          }
        }
      }
    }

    binding.surplusLIneNum = binding.surplusLIneNum?.length === 0 ? null : binding.surplusLIneNum?.trim() ===this.formBinding.controls.surplusLIneNum.value.trim()? binding.surplusLIneNum?.trim()  : this.formBinding.controls.surplusLIneNum.value.trim();
    if (!binding.surplusLIneNum && riskDetail?.binding?.surplusLIneNum) {
      binding.surplusLIneNum = riskDetail?.binding?.surplusLIneNum;
    }

    binding.slaNum = binding.slaNum?.length === 0 ? null : binding.slaNum;

    // Update form values before saving
    fg.controls['bindOptionId'].patchValue(binding.bindOptionId);
    fg.controls['creditedOfficeId'].patchValue(binding.creditedOfficeId);
    fg.controls['paymentTypesId'].patchValue(binding.paymentTypesId);
    fg.controls['surplusLIneNum'].patchValue(binding.surplusLIneNum);
    fg.controls['minimumEarned'].patchValue(binding.minimumEarned);
    fg.controls['slaNum'].patchValue(binding.slaNum);

    if (!binding.surplusLIneNum) {
      binding.surplusLIneNum = '91510'; // Temp Fix default surplusLineNum for Applied
      this.formBinding.controls.surplusLIneNum.setValue('91510'); // Temp Fix default surplusLineNum for Applied
        this.getSurplusLineDefault(this.formBinding.controls.creditedOfficeId?.value, this.defaultPolicyState).subscribe(data => {
        this.formBinding.controls.surplusLIneNum.setValue(data.licenseNumber);
        this.formBinding.controls.producerSLNumber.setValue(data.producerSLNumber);
        binding.surplusLIneNum = data.licenseNumber;
        binding.producerSLNumber = data.producerSLNumber;
      });
      }
      // tslint:disable-next-line: deprecation
      
      Utils.blockUI();

      return this.service.addUpdateBinding(binding).pipe(
        take(1),
        tap(data => {
        Utils.unblockUI();
        this.submissionData.riskDetail.binding = data;
        fg.controls['id'].patchValue(data.id);
      }, (error) => {
        Utils.unblockUI();
        if (this.submissionData.riskDetail.binding) {
          this.initiateBindingFormGroup(this.submissionData.riskDetail?.binding);
        }
      })
      );
    }

    // field format
    premiumCurrencySettings: any = {
      align: 'left',
      allowNegative: false,
      precision: 2,
      nullable: true
    };
  }
// THIS FUNCTION IS USED WHEN RETURN JSON OBJECT WITH pascal property. covert to lowercasePascal
export abstract class ClassConverter {
  public static pascalCasePattern() {
    return new RegExp('^([A-Z])([a-z]+)');
  }

  public static pascalCaseToCamelCase(propname) {
    if (ClassConverter.pascalCasePattern().test(propname)) {
      return propname.charAt(0).toLowerCase() + propname.slice(1);
    } else {
      return propname;
    }
  }

  public static convertPropertyNames(obj, converterFn) {
    let r, value;
    const t = Object.prototype.toString.apply(obj);

    if (t == '[object Object]') {
      r = {};
      for (let propname in obj) {
        value = obj[propname];
        r[converterFn(propname)] = ClassConverter.convertPropertyNames(value, converterFn);
      }
      return r;
    } else if (t == '[object Array]') {
      r = [];
      for (var i = 0, L = obj.length; i < L; ++i) {
        value = obj[i];
        r[i] = ClassConverter.convertPropertyNames(value, converterFn);
      }
      return r;
    }
    return obj;
  }
}

export abstract class FileUploadFunctions {
  public static f = {
    // fileSizeLimit: 22000000,

    fileSizeLimit: 22000000,
    acceptedFileTypes: [
      'pdf',
      'doc',
      'docx',
      'msg',
      'jpg',
      'jpeg',
      'bmp',
      'png',
      'xls',
      'xlsx',
      'txt',
      'zip',
    ],
    maxFileSize: 'Maximum upload file size: 22MB',
    maxFileSizeExceeded: 'The file being uploaded exceeds the maximum allowable size of 22MB.',
    invalidFileTypeErrorMessage: `The document {0} could not be uploaded. The file type is invalid.
      Note: Only the following file types are valid and can be attached – <list of valid file types e.g. {1}`
  };


  public static requiredFileFormat(type: string[]) {
    return function (control: AbstractControl) {
      const fileInfo = control.value;
      if (fileInfo) {
        for (const file of fileInfo) {
          if (file instanceof File) {
            const extension = (<File>file).name.split('.').slice(-1)[0].toLowerCase();
            if (!type.includes(extension.toLowerCase())) {
              return {
                requiredFileType: true
              };
              break;
            }
            return null;
          }
        }

        // Array.from(fileInfo).forEach(file => {
        //   if (file instanceof File) {
        //     const extension = (<File>file).name.split('.').slice(-1)[0].toLowerCase();
        //     if (!type.includes(extension.toLowerCase())) {
        //       return {
        //         requiredFileType: true
        //       };
        //     }
        //     return null;
        //   }
        // });
      }
      return null;
    };
  }

  public static requiredFileSize() {
    return function (control: AbstractControl) {
      const fileInfo = control.value;
      if (fileInfo) {

        for (const file of fileInfo) {
          if (file instanceof File) {
            const isExceeded = file.size > FileUploadFunctions.f.fileSizeLimit ? true : false;
            if (isExceeded) {
              return {
                requiredFileSize: true
              };
              break;
            }
            return null;
          }
        }

        // Array.from(fileInfo).forEach(file => {
        //   if (file instanceof File) {
        //     const isExceeded = file.size > FileUploadFunctions.f.fileSizeLimit ? true : false;
        //     if (isExceeded) {
        //       return {
        //         requiredFileSize: true
        //       };
        //     }
        //     return null;
        //   }
        // });
      }
      return null;
    };
  }
}

export abstract class Defaults {
  public static BindingDefaults = {
    bindOptionId: 1,
    paymentTypesId: 2,
    creditedOfficeId: 3,
    minimumEarned: 25,
    bindRequirements: [2, 3, 4, 5, 6] // default bind req rows
  };
}

export class BindingDropDowns {
  bindOptionList: any[];
  //pricingOptionList: any[];
  creditOfficeList: any[];
  //subjectivities
  descriptionList: any[];
  bindRequirementList: any[];
  bindStatusList: any[];

  quoteConditionList: any[];

  paymentTypesList: any[];
}
