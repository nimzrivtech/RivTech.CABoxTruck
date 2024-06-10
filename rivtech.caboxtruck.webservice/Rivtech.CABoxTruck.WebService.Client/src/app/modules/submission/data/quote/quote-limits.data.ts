import { Injectable } from '@angular/core';
import { AbstractControl, Form, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { map, switchMap, take, tap } from 'rxjs/operators';
import Utils from '../../../../shared/utilities/utils';
import { RiskCoverageDTO, RiskCoverageListDTO } from '../../../../shared/models/submission/limits/riskCoverageDto';
import { SubmissionData } from '../submission.data';
import { ToastrService } from 'ngx-toastr';
import { QuoteOptionsService } from '../../services/quote-options.service';
import { fourColumnRequired, oneColumnRequired, threeColumnRequired, twoColumnRequired } from '../../helpers/quoteOptionsRequired.validator';
import { LimitsData } from '../limits/limits.data';
import { LimitsService } from '../../services/coverage-limits.service';
import { LayoutService } from '../../../../core/services/layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../components/submission-details/submission-details-navitems';
import { RiskSpecificsData } from '../risk-specifics.data';
import FormUtils from '../../../../shared/utilities/form.utils';
import { RaterApiData } from '../rater-api.data';
import { DriverService } from '../../services/driver.service';
import { DriverData } from '../driver/driver.data';
import { VehicleService } from '../../services/vehicle.service';
import { SelectItem } from '../../../../shared/models/dynamic/select-item';
import { LimitsDefaultValues } from '../../../../shared/constants/limits-defaults.constants';
import { BindingDto } from '../../../../shared/models/submission/binding/BindingDto';
import { BindingService } from '../../services/binding.service';
import { Observable } from 'rxjs-compat/Observable';
import { RiskCoveragePremiumDTO } from '@app/shared/models/submission/limits/riskCoveragePremiumDto';
import { data } from 'jquery';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { Guid } from 'guid-typescript';
import { FactorModSettingsFactory } from './factor-mod-settings/factor-mod-settings-factory';
import { ErrorMessageConstant } from '@app/shared/constants/error-message.constants';

export interface QuoteLimitsForm {
  limitsForm?: FormGroup;
}

@Injectable({
  providedIn: 'root'
})
export class QuoteLimitsData {

  readonly DEFAULT_AUTO_PD_LIMIT_ID = 30;
  readonly RATER_VERSION_SUPPORT = 7;

  /**
  * The FormGroup object for the Quote Options screen.
  */
  quoteOptionsForm: FormGroup;
  premiumAvgForm: FormGroup;
  has4RiskCoverages: Boolean = false;
  disabledList: any[] = [];

  get cityState() {
    return this.submissionData.riskDetail.nameAndAddress.state;
  }

  get optionsArray(): FormArray {
    return this.quoteOptionsForm.get('optionItems') as FormArray;
  }

  get optionsPremiumArray(): FormArray {
    return this.premiumAvgForm.get('optionItems') as FormArray;
  }

  get options(): RiskCoverageDTO[] {
    let items: RiskCoverageDTO[] = [];
    this.optionsArray.controls.forEach(fgOption => {
      items.push(this.toRiskCoverageDTO(fgOption.value));
    });
    return items;
  }

  public loadQuoteOptionsPremiumAvg(): Observable<any> {
    return this.quoteOptionsService.getQuoteOptions(this.submissionData.riskDetail.id)
      .pipe(
        tap(data => {
            if (data?.riskCoverages?.length > 0) {
              this.has4RiskCoverages = data?.riskCoverages?.length === 4;
              this.submissionData.riskDetail.riskCoverages = data.riskCoverages;
            let forms: OptionPremiumForm[] = [];
            forms = data.riskCoverages.map(rc => this.toOptionPremiumForm(rc));
            this.updatePremiumForm(forms);
          }
        })
      );
  }

  public loadPremiumAvgMetrics(): Observable<any> {
    return this.quoteOptionsService.getPremium(this.submissionData.riskDetail.riskId)
      .pipe(
        tap(data => {
          console.log(data);
          if (data?.riskCoverages?.length > 0) {
              let forms: OptionPremiumForm[] = [];
              forms = data.riskCoverages.map(rc => this.toOptionPremiumForm(rc));
              this.updatePremiumForm(forms.slice().sort((a, b) => b.optionNumber - a.optionNumber));
          }
        })
      );
  }

  public loadQuoteOptions(): Observable<any> {
    return this.quoteOptionsService.getQuoteOptions(this.submissionData.riskDetail.id)
      .pipe(
        tap(data => {
          if (data?.riskCoverages?.length > 0) {
            this.has4RiskCoverages = data?.riskCoverages?.length === 4;
            this.submissionData.riskDetail.riskCoverages = data.riskCoverages;
            let forms: OptionForm[] = [];
            forms = data.riskCoverages.map(rc => this.toOptionForm(rc));
            this.updateForm(forms);
          }
        }),
        switchMap(() => this.setQuoteDefaultsData())
      );
  }

  public saveOptions(): Observable<any> {
    let options: RiskCoverageDTO[] = this.optionsArray.value.map(x => this.toRiskCoverageDTO(x));

    let option1cargoLimitId = 0;
    let option1refCargoLimitId = 0;

    options.forEach(opt => {
      if (opt.riskCoveragePremium?.id === null) {
        opt.riskCoveragePremium = null;
      }

      // Always align option 1 to limits page
      if (opt.optionNumber === 1) {
        opt.autoBILimitId = this.submissionData.riskDetail.riskCoverage.autoBILimitId;
        opt.autoPDLimitId = this.submissionData.riskDetail.riskCoverage.autoPDLimitId;
        opt.umbiLimitId = this.submissionData.riskDetail.riskCoverage.umbiLimitId;
        opt.umpdLimitId = this.submissionData.riskDetail.riskCoverage.umpdLimitId;
        opt.uimbiLimitId = this.submissionData.riskDetail.riskCoverage.uimbiLimitId;
        opt.uimpdLimitId = this.submissionData.riskDetail.riskCoverage.uimpdLimitId;
        opt.medPayLimitId = this.submissionData.riskDetail.riskCoverage.medPayLimitId;
        opt.pipLimitId = this.submissionData.riskDetail.riskCoverage.pipLimitId;
        opt.liabilityDeductibleId = this.submissionData.riskDetail.riskCoverage.liabilityDeductibleId;
        opt.collDeductibleId = this.submissionData.riskDetail.riskCoverage.collDeductibleId;
        opt.glbiLimitId = this.submissionData.riskDetail.riskCoverage.glbiLimitId; 
        opt.trailerInterchangeLimitId = this.submissionData.riskDetail.riskCoverage.trailerInterchangeLimitId;
        opt.nonOwnedTrailerLimitId = this.submissionData.riskDetail.riskCoverage.nonOwnedTrailerLimitId;
        opt.pollutionCovLimitId = this.submissionData.riskDetail.riskCoverage.pollutionCovLimitId;
        opt.trailerTypeLimitId = this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId;   
        opt.cargoLimitId = this.submissionData.riskDetail.riskCoverage.cargoLimitId;
        opt.refCargoLimitId = this.submissionData.riskDetail.riskCoverage.refCargoLimitId;
        opt.compDeductibleId = this.submissionData.riskDetail.riskCoverage.compDeductibleId;
        opt.fireDeductibleId = this.submissionData.riskDetail.riskCoverage.fireDeductibleId;
        opt.alSymbolId = this.submissionData.riskDetail.riskCoverage.alSymbolId;
        opt.pdSymbolId = this.submissionData.riskDetail.riskCoverage.pdSymbolId;

        if (parseInt(this.submissionData.riskDetail.binding.creditedOfficeId, 10) === 3) {
          opt.arsCommission = opt.brokerCommisionAL >= 12 ? 1 : 2.5;
        } else {
          opt.arsCommission = 0;
        }

        option1cargoLimitId  = opt.cargoLimitId;
        option1refCargoLimitId  = opt.refCargoLimitId;
      }

      if(option1cargoLimitId == 53)
      {
        opt.cargoLimitId = 53;
        opt.refCargoLimitId = 157;
      }

      if(option1refCargoLimitId == 157)
      {
        opt.refCargoLimitId = 157;
      }

      if (opt.optionNumber > 1) {
        if(opt.trailerInterchangeLimitId == LimitsDefaultValues.triId &&
           opt.nonOwnedTrailerLimitId == LimitsDefaultValues.noId) {
          opt.trailerTypeLimitId = null;
        } else {
          opt.trailerTypeLimitId = (this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId) ? this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId : LimitsDefaultValues.trtId;
        }
      }

      opt.cargoLimitSymbolId = this.submissionData.riskDetail.riskCoverage.cargoLimitSymbolId;
      opt.refCargoLimitSymbolId =  this.submissionData.riskDetail.riskCoverage.refCargoLimitSymbolId;
      opt.trailerInterchangeLimitSymbolId = this.raterVersionSupport(8) ? null : this.submissionData.riskDetail.riskCoverage.trailerInterchangeLimitSymbolId;
      opt.nonOwnedTrailerLimitSymbolId = this.raterVersionSupport(8) ? null : this.submissionData.riskDetail.riskCoverage.nonOwnedTrailerLimitSymbolId;
    });
    return this.quoteOptionsService.saveQuoteOptions({
      riskDetailId: this.submissionData.riskDetail.id,
      riskCoverages: options
    }).pipe(
      tap((data: RiskCoverageListDTO) => this.submissionData.riskDetail.riskCoverages = data.riskCoverages)
    );
  }

  public saveOptionsWithParam(riskCoverages: RiskCoverageDTO[]): Observable<any> {
    let options = riskCoverages;

    let option1cargoLimitId = 0;
    let option1refCargoLimitId = 0;

    options.forEach(opt => {
      if (opt.riskCoveragePremium?.id === null) {
        opt.riskCoveragePremium = null;
      }

      // Always align option 1 to limits page
      if (opt.optionNumber === 1) {
        opt.autoBILimitId = this.submissionData.riskDetail.riskCoverage.autoBILimitId;
        opt.autoPDLimitId = this.submissionData.riskDetail.riskCoverage.autoPDLimitId;
        opt.umbiLimitId = this.submissionData.riskDetail.riskCoverage.umbiLimitId;
        opt.umpdLimitId = this.submissionData.riskDetail.riskCoverage.umpdLimitId;
        opt.uimbiLimitId = this.submissionData.riskDetail.riskCoverage.uimbiLimitId;
        opt.uimpdLimitId = this.submissionData.riskDetail.riskCoverage.uimpdLimitId;
        opt.medPayLimitId = this.submissionData.riskDetail.riskCoverage.medPayLimitId;
        opt.pipLimitId = this.submissionData.riskDetail.riskCoverage.pipLimitId;
        opt.liabilityDeductibleId = this.submissionData.riskDetail.riskCoverage.liabilityDeductibleId;
        opt.collDeductibleId = this.submissionData.riskDetail.riskCoverage.collDeductibleId;
        opt.glbiLimitId = this.submissionData.riskDetail.riskCoverage.glbiLimitId; 
        opt.trailerInterchangeLimitId = this.submissionData.riskDetail.riskCoverage.trailerInterchangeLimitId;
        opt.nonOwnedTrailerLimitId = this.submissionData.riskDetail.riskCoverage.nonOwnedTrailerLimitId;
        opt.pollutionCovLimitId = this.submissionData.riskDetail.riskCoverage.pollutionCovLimitId;
        opt.trailerTypeLimitId = this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId;   
        opt.cargoLimitId = this.submissionData.riskDetail.riskCoverage.cargoLimitId;
        opt.refCargoLimitId = this.submissionData.riskDetail.riskCoverage.refCargoLimitId;
        opt.compDeductibleId = this.submissionData.riskDetail.riskCoverage.compDeductibleId;
        opt.fireDeductibleId = this.submissionData.riskDetail.riskCoverage.fireDeductibleId;
        opt.alSymbolId = this.submissionData.riskDetail.riskCoverage.alSymbolId;
        opt.pdSymbolId = this.submissionData.riskDetail.riskCoverage.pdSymbolId;

        if (parseInt(this.submissionData.riskDetail.binding.creditedOfficeId, 10) === 3) {
          opt.arsCommission = opt.brokerCommisionAL >= 12 ? 1 : 2.5;
        } else {
          opt.arsCommission = 0;
        }

        option1cargoLimitId  = opt.cargoLimitId;
        option1refCargoLimitId  = opt.refCargoLimitId;
      }

      if(option1cargoLimitId == 53)
      {
        opt.cargoLimitId = 53;
        opt.refCargoLimitId = 157;
      }

      if(option1refCargoLimitId == 157)
      {
        opt.refCargoLimitId = 157;
      }

      if (opt.optionNumber > 1) {
        if(opt.trailerInterchangeLimitId == LimitsDefaultValues.triId &&
           opt.nonOwnedTrailerLimitId == LimitsDefaultValues.noId) {
          opt.trailerTypeLimitId = null;
        } else {
          opt.trailerTypeLimitId = (this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId) ? this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId : LimitsDefaultValues.trtId;
        }
      }

      opt.cargoLimitSymbolId = this.submissionData.riskDetail.riskCoverage.cargoLimitSymbolId;
      opt.refCargoLimitSymbolId =  this.submissionData.riskDetail.riskCoverage.refCargoLimitSymbolId;
      opt.trailerInterchangeLimitSymbolId = this.raterVersionSupport(8) ? null : this.submissionData.riskDetail.riskCoverage.trailerInterchangeLimitSymbolId;
      opt.nonOwnedTrailerLimitSymbolId = this.raterVersionSupport(8) ? null : this.submissionData.riskDetail.riskCoverage.nonOwnedTrailerLimitSymbolId;
    });
    return this.quoteOptionsService.saveQuoteOptions({
      riskDetailId: this.submissionData.riskDetail.id,
      riskCoverages: options
    }).pipe(
      tap((data: RiskCoverageListDTO) => this.submissionData.riskDetail.riskCoverages = data.riskCoverages)
    );
  }

  public copyOption(fgOption: FormGroup): Observable<any> {
    return this.quoteOptionsService.copyOption(this.submissionData.riskDetail.id, fgOption.value.optionNumber)
      .pipe(
        switchMap(() => this.loadQuoteOptions())
      );
  }

  public deleteOption(fgOption: FormGroup): Observable<any> {
    let dto = this.toRiskCoverageDTO(fgOption.value);
    return this.quoteOptionsService.deleteOption(this.submissionData.riskDetail.id, dto)
      .pipe(switchMap(() => this.loadQuoteOptions()));
  }

  isSplit(stringValue: any): boolean {
    return stringValue != null && stringValue.includes("/");
  }

  /**
  * Initializes the FormGroup where quote options will be applied.
  */
  public initializeForms() {
    this.quoteOptionsForm = null;
    this.quoteOptionsForm = this.formBuilder.group({
      alPremiumInput: null,
      pdPremiumInput: null,
      glPremiumInput: null,
      cargoPremiumInput: null,
      trailerInterchangePremiumInput: null,
      nonOwnedTrailerPremiumInput: null,
      optionItems: this.formBuilder.array([])
    });
  }

  public initializePremiumForms() {
    this.premiumAvgForm = null;
    this.premiumAvgForm = this.formBuilder.group({
      optionItems: this.formBuilder.array([])
    });
  }

  public updatePremiumForm(forms: OptionPremiumForm[]): void {
    forms?.forEach(form => {
      let fgOption = this.optionsPremiumArray.controls.find(fgOpt => fgOpt.value.optionNumber == form.optionNumber) as FormGroup;
      fgOption = fgOption || this.formBuilder.group(form);
      fgOption.patchValue(form);
      this.optionsPremiumArray.push(fgOption);
    });
  }

  /**
   * Displays each quote options with form builder.
   * @param options Each option column to display.
   */
  public updateForm(forms: OptionForm[]): void {
    this.initializeForms();

    let option: OptionForm = forms[0];
    // console.log(option1);
    this.quoteOptionsForm.patchValue({
      alPremiumInput: option.autoLiabilityPremium,
      pdPremiumInput: option.physicalDamagePremium,
      glPremiumInput: option.generalLiabilityPremium,
      cargoPremiumInput: option.cargoPremium,
      trailerInterchangePremiumInput: option.trailerInterchangePremiumAL,
      nonOwnedTrailerPremiumInput: option.nonOwnedTrailerPremium
    });

    forms?.forEach(form => {
      form.cargoLimitId = option.cargoLimitId == 53 ? 53: form.cargoLimitId;
      form.refCargoLimitId = option.cargoLimitId == 53 || option.refCargoLimitId == 157 ? 157: form.refCargoLimitId;

      let fgOption = this.createOptionFormGroup(form);
      this.optionsArray.push(fgOption);
    });
    // console.log(this.quoteOptionsForm);
  }

  /**
   * Creates a FormGroup from OptionForm model.
   */
  private createOptionFormGroup(optionForm: OptionForm): FormGroup {
    let fgOption = this.optionsArray.controls.find(fgOpt => fgOpt.value.optionNumber == optionForm.optionNumber) as FormGroup;
    fgOption = fgOption || this.formBuilder.group(optionForm);
    this.retrieveDropDownValues(false, fgOption, optionForm.optionNumber);
    this.setOnALLimitInputChanged(fgOption);
    this.setOnDepositPercentageInputChanged(fgOption);
    fgOption.patchValue(optionForm);
    this.autoComputeAdditionalInterest(fgOption);
    //this.autoComputeRiskManagementFee(fgOption, optionForm.optionNumber - 1);
    
    this.autoComputeInstallmentFee(fgOption, optionForm.optionNumber - 1);
    
    return fgOption;
  }

  /**
   * Setup behavioral changes when Deposit Percentage changed.
   * @param fgOption A FormGroup where deposit percentage control is included.
   */
  private setOnDepositPercentageInputChanged(fgOption: FormGroup): void {
    fgOption.controls["depositPct"].valueChanges.subscribe(newValue => {
      let numOfInstallmentsInput = fgOption.controls["numberOfInstallments"];
      let optionForm = fgOption.value as OptionForm;

      if (newValue < 100 && optionForm.numberOfInstallments == null) {
        numOfInstallmentsInput.enable();
        numOfInstallmentsInput.patchValue(10);
      } else if (newValue == 100) {
        numOfInstallmentsInput.disable();
        numOfInstallmentsInput.patchValue(null);
      }
    });
  }

  /**
   * Setup behavioral changes when Auto Liability input changed.
   * @param fgOption A FormGroup where Auto Liability control is included.
   */
  private setOnALLimitInputChanged(fgOption: FormGroup): void {
    fgOption.controls["autoBILimitId"].valueChanges.subscribe(newValue => {
      let optionForm = fgOption.value as OptionForm;
      let stringValue = this.biLimitList?.find(l => l.value == newValue)?.label ?? null;
      let pdLimitCtrl = fgOption.controls["autoPDLimitId"];

      if (this.isSplit(stringValue)) {
        pdLimitCtrl.enable();
        if (optionForm.autoPDLimitId == null)
          optionForm.autoPDLimitId = this.DEFAULT_AUTO_PD_LIMIT_ID; // Default PD Limit value id.
      } else {
        optionForm.autoPDLimitId = optionForm.autoBILimitId;
        pdLimitCtrl.disable();
      }
      pdLimitCtrl.patchValue(optionForm.autoPDLimitId);
    });
  }

  private toOptionPremiumForm(option: RiskCoverageDTO): OptionPremiumForm {
    let optionForm = new OptionPremiumForm();
    
    optionForm.optionNumber = option.optionNumber;
    optionForm.avgAlVehicleTotalPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgAlVehicleTotalPremium;
    optionForm.avgAlVehiclePowerPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgAlVehiclePowerPremium;
    optionForm.avgAlVehicleTrailerOtherPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgAlVehicleTrailerOtherPremium;
    optionForm.avgApdVehicleTotalPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgApdVehicleTotalPremium;
    optionForm.avgApdVehiclePowerPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgApdVehiclePowerPremium;
    optionForm.avgApdVehicleTrailerOtherPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgApdVehicleTrailerOtherPremium;
    optionForm.avgMtcRefVehicleTotalPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgMtcRefVehicleTotalPremium;
    optionForm.avgMtcRefVehiclePowerPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgMtcRefVehiclePowerPremium;
    optionForm.avgMtcRefVehicleTrailerOtherPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgMtcRefVehicleTrailerOtherPremium;
    optionForm.avgTotalVehiclePremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgTotalVehiclePremium;
    optionForm.avgTotalVehiclePowerPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgTotalVehiclePowerPremium;
    optionForm.avgTotalVehicleTrailerOtherPremium = option.riskCoveragePremium?.riskCoveragePremiumAvg?.avgTotalVehicleTrailerOtherPremium;

    return optionForm;
  }

  /**
   * Converts the given RiskCoverageDTO model to OptionForm view model and sets default values.
   * @param option The model which contains the info of a Quote Option.
   * @returns An OptionForm object converted from option parameter. 
   */
  private toOptionForm(option: RiskCoverageDTO): OptionForm {
    let optionForm = new OptionForm();

    for (let key in option) {
      if (optionForm.hasOwnProperty(key)) {
        optionForm[key] = option[key];
      } else {
        switch (key) {
          case 'riskCoveragePremium':
            for (let rcpKey in option[key]) {
              if (rcpKey === 'id') {
                optionForm['riskCoveragePremiumId'] = option['riskCoveragePremium']?.id;
              } else if (rcpKey ==='trailerInterchangeAL') {
                optionForm['trailerInterchangePremiumAL'] = option[key]['trailerInterchangeAL'];
              } else {
                optionForm[rcpKey] = option[key][rcpKey];
              }
            }
            break;
          case 'compDeductibleId':
          case 'fireDeductibleId':
            if (optionForm.compFireTheftDeductibleId != null)
              continue;

            optionForm.comprehensiveFire = option.compDeductibleId !== null;
            if (optionForm.comprehensiveFire) {
              optionForm.compFireTheftDeductibleId = option.compDeductibleId
            } else {
              optionForm.compFireTheftDeductibleId = option.fireDeductibleId;
            }

            break;
          default:
            optionForm[key] = option[key];
            break;
        }
      }
    }

    const yearsInBusiness = this.submissionData?.riskDetail?.businessDetails?.yearsInBusiness ?? 0;

    if (yearsInBusiness < 1) {
      optionForm.setDefaultExperienceRatingFactors();
    }
    optionForm.setDefaultBrokerCommission();
    optionForm.setDefaultPremiumRatingFactors();
    optionForm.setDefaultDeposit();

    return optionForm;
  }

  /**
   * Converts OptionForm view model to RiskCoverageDTO model. 
   * @param optionForm The OptionForm object to be converted.
   * @returns The RiskCoverageDTO model converted from OptionForm view model.
   */
  public toRiskCoverageDTO(optionForm: OptionForm): RiskCoverageDTO {
    let optionData = new RiskCoverageDTO();
    let riskCovPrem = new RiskCoveragePremiumDTO();
    for (let key in optionForm) {
      if (riskCovPrem.hasOwnProperty(key) && key !== 'id') {
        riskCovPrem[key] = optionForm[key];
      } else {
        switch (key) {
          case 'compFireTheftDeductibleId':
          case 'compDeductibleId':
          case 'fireDeductibleId':
            break;
          case 'riskCoveragePremiumId':
            riskCovPrem['id'] = optionForm.riskCoveragePremiumId;
            break;
          case 'comprehensiveFire':
            if (optionForm.comprehensiveFire) {
              optionData.compDeductibleId = optionForm.compFireTheftDeductibleId as number;
              //joptionData.fireDeductibleId = null;
            } else {
              optionData.fireDeductibleId = optionForm.compFireTheftDeductibleId as number;
              //optionData.compDeductibleId = null;
            }
            break;
          default:
            optionData[key] = optionForm[key];
            break;
        }
      }
    }

    if (!optionForm.hasOwnProperty('autoPDLimitId')) {
      let biLimit = this.limitsData.biLimitList.find(x => x.value == optionData.autoBILimitId);
      optionData.autoPDLimitId = this.limitsData.pdLimitList.find(x => x.label == biLimit?.label)?.value;
    }

    optionData.riskCoveragePremium = riskCovPrem;
    return optionData;
  }




  isUmBiAvailable: boolean;
  isUmPdAvailable: boolean;
  isUimBiAvailable: boolean;
  isUimPdAvailable: boolean;
  isPipAvailable: boolean;
  fieldArray: Array<Number> = [0];

  pdView: Array<boolean> = [false, false, false, false];

  // Limits dropdown lookup
  biLimitList: any[];
  pdLimitList: any[];
  liabDeductibleList: any[];
  umBiLimitList: any[];
  umPdLimitList: any[];
  uimBiLimitList: any[];
  uimPdLimitList: any[];
  medpayList: any[];
  pipList: any[];
  CauseOfLossList: any[];
  ComprehensiveList: any[];
  CollisionList: any[];
  glList: any[];
  cargoList: any[];
  refBreakList: any[];
  trailerInterchangeList: any[];
  nonOwnedTrailerList: any[];
  pollutionCovList: any[];
  selectedSubjects = [];
  isCalculatePremium: boolean = false;
  yearsInBusiness: number = 0;

  installmentCountList: any[] = [];

  symbolList: any[] = [
    { label: '7, 8 & 9', value: '1' },
    { label: '7 & 8', value: '13' },
    { label: '7 & 9', value: '2' },
    { label: '2, 8 & 9', value: '3' },
    { label: '1) Any Auto', value: '4' },
    { label: '2) All Owned Autos', value: '5' },
    { label: '3) Owned Private Passenger Autos', value: '6' },
    { label: '4) Owned Autos Other Than Private Passenger', value: '7' },
    { label: '5) All Owned Autos Which Require No-Fault Coverage', value: '8' },
    { label: '6) Owned Autos Subject to Compulsory UM Law', value: '9' },
    { label: '7) Autos Specified on  Schedule', value: '10' },
    { label: '8) Hired Autos', value: '11' },
    { label: '9) Non-Owned Autos', value: '12' },
  ];

  get alSymbolList(): any[] {
    var symbolList = this.symbolList;

    if(this.raterVersionSupport()) {
      symbolList = symbolList.concat([
        { label: '10 - Use 2, 8 & 9 Pricing', value: '14' },
        { label: '10 - Use 7, 8 & 9 Pricing', value: '15' }
      ]);
    }

    return symbolList;
  };
  
  // i created entry of generic option for this using this  url and
  // this.baseUrl + `/api/KeyValue/generic/subjectivities`
  subjectivities: any[] = [
    { 'value': 1, 'label': '300 Mileage Limitation' },
    { 'value': 2, 'label': 'Unlisted Driver Exclusion' },
    { 'value': 3, 'label': 'Occupant Hazard Exclusion' },
    { 'value': 4, 'label': 'No drivers under 23 years old permitted.' },
    { 'value': 5, 'label': 'All 23 and 24 year old drivers must have a clean MVR.' },
    { 'value': 6, 'label': 'All 25+ year old drivers can only have 1 moving violation or accident in previous 36 months.' },
    { 'value': 7, 'label': 'Proof of deposit or signed financed agreement required to bind.' },
    { 'value': 8, 'label': 'If the vehicle count changes at time of binding, price per unit could change.' },
    { 'value': 9, 'label': 'The quote is based on MVRs provided. The premium is subject to change if any of the driver information changes.' },
    { 'value': 10, 'label': 'The following are required to bind: Updated signed application that matches the terms of the accepted quote, signed quote, signed um/uim coverage selection/rejection form, signed affidavit / diligent search form, and signed Trucking Verification Form.' },
    { 'value': 11, 'label': 'Copy of the contract with the logistics company required for the requested coverages selected.' },
    { 'value': 12, 'label': 'Proof of safety devices, GPS, and/or telematics required.' },
    { 'value': 13, 'label': 'Updated Excel Spreadsheet of vehicles required at time of bind.' },
    { 'value': 14, 'label': 'Updated Excel Spreadsheet of drivers required at time of bind.' },
    { 'value': 15, 'label': 'Currently valued loss runs required to bind.' },
    { 'value': 16, 'label': 'Currently valued MVRs required to bind.' }
  ];

  // All forms
  quoteLimitsForm: QuoteLimitsForm;

  // field format
  premiumCurrencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 2,
    nullable: true
  };

  feesCurrencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 0,
    nullable: true
  };

  factorModsSettings: any;


  get controls() {
    return this.quoteLimitsForm.limitsForm.controls;
  }

  constructor(
    private quoteOptionsService: QuoteOptionsService,
    private formBuilder: FormBuilder,
    public submissionData: SubmissionData,
    public limitsData: LimitsData,
    private limitService: LimitsService,
    private toastr: ToastrService,
    private layoutService: LayoutService,
    private riskSpecificsData: RiskSpecificsData,
    private driverService: DriverService,
    private vehicleService: VehicleService,
    public driverData: DriverData,
    private service: BindingService,
  ) {
    this.initializeForms();
    for (let i = 2; i <= 10; i++) {
      this.installmentCountList.push({ label: i, value: i });
    }
  }

  initiateFormFields() {
    this.initializeForms();

    this.quoteLimitsForm = {};
    this.quoteLimitsForm.limitsForm = this.formBuilder.group({
      autoBILimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      alSymbolId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      autoPDLimitId: this.formBuilder.array([null, null, null, null]),
      pdSymbolId: this.formBuilder.array([null, null, null, null]),
      umbiLimitId: this.formBuilder.array([null, null, null, null]),
      umpdLimitId: this.formBuilder.array([null, null, null, null]),
      uimbiLimitId: this.formBuilder.array([null, null, null, null]),
      uimpdLimitId: this.formBuilder.array([null, null, null, null]),
      medPayLimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      pipLimitId: this.formBuilder.array([null, null, null, null]),
      liabilityDeductibleId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      compDeductibleId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      collDeductibleId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      glbiLimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      cargoLimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      refCargoLimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      trailerInterchangeLimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      nonOwnedTrailerLimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      pollutionCovLimitId: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      comprehensiveFire: this.formBuilder.array([true, true, true, true]),

      brokerCommisionAL: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      brokerCommisionPD: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      LiabilityScheduleRatingFactor: this.formBuilder.array([null, null, null, null]),
      LiabilityExperienceRatingFactor: this.formBuilder.array([null, null, null, null]),
      glScheduleRF: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      glExperienceRF: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      cargoScheduleRF: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      cargoExperienceRF: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      apdScheduleRF: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      apdExperienceRF: this.formBuilder.array([null, null, null, null], oneColumnRequired),
      depositPct: this.formBuilder.array([null, null, null, null]), // oneColumnRequired
      depositPremium: this.formBuilder.array([null, null, null, null]), // oneColumnRequired
      numberOfInstallments: this.formBuilder.array([null, null, null, null]), // oneColumnRequired

      autoLiabilityPremium: this.formBuilder.array([null, null, null, null]),
      cargoPremium: this.formBuilder.array([null, null, null, null]),
      physicalDamagePremium: this.formBuilder.array([null, null, null, null]),
      vehicleLevelPremium: this.formBuilder.array([null, null, null, null]),
      generalLiabilityPremium: this.formBuilder.array([null, null, null, null]),
      trailerInterchangePremium: this.formBuilder.array([null, null, null, null]),
      nonOwnedTrailerPremium: this.formBuilder.array([null, null, null, null]),
      additionalInsuredPremium: this.formBuilder.array([null, null, null, null]),
      waiverOfSubrogationPremium: this.formBuilder.array([null, null, null, null]),
      primaryNonContributoryPremium: this.formBuilder.array([null, null, null, null]),
      riskManagementFee_AL: this.formBuilder.array([null, null, null, null]),
      riskManagementFee_PD: this.formBuilder.array([null, null, null, null]),
      avgPerVehicle: this.formBuilder.array([null, null, null, null]),
      premium: this.formBuilder.array([null, null, null, null]),
      fees: this.formBuilder.array([null, null, null, null]),
      taxesAndStampingFees: this.formBuilder.array([null, null, null, null]),
      depositAmount: this.formBuilder.array([null, null, null, null]), // oneColumnRequired
      installmentFee: this.formBuilder.array([null, null, null, null]),
      perInstallment: this.formBuilder.array([null, null, null, null]),
      totalAmounDueFull: this.formBuilder.array([null, null, null, null]),
      totalAmountDueInstallment: this.formBuilder.array([null, null, null, null]),
      alPremiumInput: [null],
      pdPremiumInput: [null],
      glPremiumInput: [null],
      cargoPremiumInput: [null],
      trailerInterchangePremiumInput: [null],
      nonOwnedTrailerPremiumInput: [null],

      subjectivitiesId: [[]],
      subjectivitiesList: [[]]
    });
  }

  getQuoteOptions(): Observable<any> {
    return this.quoteOptionsService.getQuoteOptions(this.submissionData.riskDetail.id)
      .pipe(map(data => this.submissionData.riskDetail.riskCoverages = data.riskCoverages));
  }

  populateQuoteFields() {
    this.fieldArray = [];
    if (this.submissionData.riskDetail?.riskCoverages?.length > 0) {
      const autoBILimitId = [];
      const autoPDLimitId = [];
      const alSymbolId = [];
      const pdSymbolId = [];
      const umbiLimitId = [];
      const umpdLimitId = [];
      const uimbiLimitId = [];
      const uimpdLimitId = [];
      const medPayLimitId = [];
      const pipLimitId = [];
      const liabilityDeductibleId = [];
      const comprehensiveFire = [];
      const compDeductibleId = [];
      const collDeductibleId = [];
      const glbiLimitId = [];
      const cargoLimitId = [];
      const refCargoLimitId = [];
      const trailerInterchangeLimitId = [];
      const nonOwnedTrailerLimitId = [];
      const pollutionCovLimitId = [];

      const brokerCommisionAL = [];
      const brokerCommisionPD = [];
      const glScheduleRF = [];
      const glExperienceRF = [];
      const cargoScheduleRF = [];
      const cargoExperienceRF = [];
      const apdScheduleRF = [];
      const apdExperienceRF = [];
      const depositPct = [];
      const depositPremium = [];
      const numberOfInstallments = [];

      const LiabSchedRF = [];
      const LiabExpRF = [];

      const autoLiabilityPremium = [];
      const cargoPremium = [];
      const physicalDamagePremium = [];
      const vehicleLevelPremium = [];
      const generalLiabilityPremium = [];
      const additionalInsuredPremium = [];
      const waiverOfSubrogationPremium = [];
      const primaryNonContributoryPremium = [];
      const riskManagementFee_AL = [];
      const riskManagementFee_PD = [];
      const premium = [];
      const fees = [];

      for (let i = 0; i < this.submissionData.riskDetail.riskCoverages.length; i++) {
        this.fieldArray.push(i);
      }
      this.submissionData.riskDetail.riskCoverages.forEach(property => {
        autoBILimitId.splice(property.optionNumber - 1, 0, property.autoBILimitId);
        alSymbolId.splice(property.optionNumber - 1, 0, property.alSymbolId);
        autoPDLimitId.splice(property.optionNumber - 1, 0, property.autoPDLimitId);
        pdSymbolId.splice(property.optionNumber - 1, 0, property.pdSymbolId);
        umbiLimitId.splice(property.optionNumber - 1, 0, property.umbiLimitId);
        umpdLimitId.splice(property.optionNumber - 1, 0, property.umpdLimitId);
        uimbiLimitId.splice(property.optionNumber - 1, 0, property.uimbiLimitId);
        uimpdLimitId.splice(property.optionNumber - 1, 0, property.uimpdLimitId);
        medPayLimitId.splice(property.optionNumber - 1, 0, property.medPayLimitId);
        pipLimitId.splice(property.optionNumber - 1, 0, property.pipLimitId);
        liabilityDeductibleId.splice(property.optionNumber - 1, 0, property.liabilityDeductibleId);

        if (property.compDeductibleId == null) {
          comprehensiveFire.splice(property.optionNumber - 1, 0, false);
          compDeductibleId.splice(property.optionNumber - 1, 0, property.fireDeductibleId);
        } else {
          comprehensiveFire.splice(property.optionNumber - 1, 0, true);
          compDeductibleId.splice(property.optionNumber - 1, 0, property.compDeductibleId);
        }

        collDeductibleId.splice(property.optionNumber - 1, 0, property.collDeductibleId);
        glbiLimitId.splice(property.optionNumber - 1, 0, property.glbiLimitId);
        cargoLimitId.splice(property.optionNumber - 1, 0, property.cargoLimitId);
        refCargoLimitId.splice(property.optionNumber - 1, 0, property.refCargoLimitId);
        trailerInterchangeLimitId.splice(property.optionNumber - 1, 0, property.trailerInterchangeLimitId);
        nonOwnedTrailerLimitId.splice(property.optionNumber - 1, 0, property.nonOwnedTrailerLimitId);
        pollutionCovLimitId.splice(property.optionNumber - 1, 0, property.pollutionCovLimitId);

        brokerCommisionAL.splice(property.optionNumber - 1, 0, property.brokerCommisionAL);
        brokerCommisionPD.splice(property.optionNumber - 1, 0, property.brokerCommisionPD);
        glScheduleRF.splice(property.optionNumber - 1, 0, property.glScheduleRF);
        glExperienceRF.splice(property.optionNumber - 1, 0, property.glExperienceRF);
        cargoScheduleRF.splice(property.optionNumber - 1, 0, property.cargoScheduleRF);
        cargoExperienceRF.splice(property.optionNumber - 1, 0, property.cargoExperienceRF);
        apdScheduleRF.splice(property.optionNumber - 1, 0, property.apdScheduleRF);
        apdExperienceRF.splice(property.optionNumber - 1, 0, property.apdExperienceRF);
        depositPct.splice(property.optionNumber - 1, 0, property.depositPct);
        depositPremium.splice(property.optionNumber - 1, 0, property.depositPremium);
        numberOfInstallments.splice(property.optionNumber - 1, 0, property.numberOfInstallments);

        LiabExpRF.splice(property.optionNumber - 1, 0, property.liabilityExperienceRatingFactor);
        LiabSchedRF.splice(property.optionNumber - 1, 0, property.liabilityScheduleRatingFactor);

        if (property?.riskCoveragePremium !== null) {
          // risk coverage premium
          autoLiabilityPremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.autoLiabilityPremium);
          cargoPremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.cargoPremium);
          physicalDamagePremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.physicalDamagePremium);
          vehicleLevelPremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.vehicleLevelPremium);
          generalLiabilityPremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.generalLiabilityPremium);
          additionalInsuredPremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.additionalInsuredPremium);
          waiverOfSubrogationPremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.waiverOfSubrogationPremium);
          primaryNonContributoryPremium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.primaryNonContributoryPremium);
          riskManagementFee_AL.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.riskMgrFeeAL);
          riskManagementFee_PD.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.riskMgrFeePD);
          premium.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.premium);
          fees.splice(property.optionNumber - 1, 0, property.riskCoveragePremium?.fees);
          if (property.optionNumber === 1) {
            this.quoteLimitsForm.limitsForm.controls['alPremiumInput'].patchValue(property.riskCoveragePremium?.autoLiabilityPremium);
            this.quoteLimitsForm.limitsForm.controls['pdPremiumInput'].patchValue(property.riskCoveragePremium?.physicalDamagePremium);
            this.quoteLimitsForm.limitsForm.controls['glPremiumInput'].patchValue(property.riskCoveragePremium?.generalLiabilityPremium);
            this.quoteLimitsForm.limitsForm.controls['cargoPremiumInput'].patchValue(property.riskCoveragePremium?.cargoPremium);

            // if (property.subjectivitiesId !== null) {
            //   const getSubjectivityId: number[] = $.map(property.subjectivitiesId.split(','), (value) => parseInt(value));
            //   this.selectedSubjects = this.subjectivities.filter(subjectivity => getSubjectivityId.includes(+subjectivity.value));
            // }
          }
        }
      });

      this.quoteLimitsForm.limitsForm.patchValue({
        autoBILimitId: autoBILimitId,
        alSymbolId: alSymbolId,
        autoPDLimitId: autoPDLimitId,
        pdSymbolId: pdSymbolId,
        umbiLimitId: umbiLimitId,
        umpdLimitId: umpdLimitId,
        uimbiLimitId: uimbiLimitId,
        uimpdLimitId: uimpdLimitId,
        medPayLimitId: medPayLimitId,
        pipLimitId: pipLimitId,
        liabilityDeductibleId: liabilityDeductibleId,
        comprehensiveFire: comprehensiveFire,
        compDeductibleId: compDeductibleId,
        collDeductibleId: collDeductibleId,
        glbiLimitId: glbiLimitId,
        cargoLimitId: cargoLimitId,
        refCargoLimitId: refCargoLimitId,
        trailerInterchangeLimitId: trailerInterchangeLimitId,
        nonOwnedTrailerLimitId: nonOwnedTrailerLimitId,
        pollutionCovLimitId: pollutionCovLimitId,
        brokerCommisionAL: brokerCommisionAL,
        brokerCommisionPD: brokerCommisionPD,
        glScheduleRF: glScheduleRF,
        glExperienceRF: glExperienceRF,
        cargoScheduleRF: cargoScheduleRF,
        cargoExperienceRF: cargoExperienceRF,
        apdScheduleRF: apdScheduleRF,
        apdExperienceRF: apdExperienceRF,
        depositPct: depositPct,
        depositPremium: depositPremium,
        numberOfInstallments: numberOfInstallments,

        LiabilityScheduleRatingFactor: LiabSchedRF,
        LiabilityExperienceRatingFactor: LiabExpRF,

        autoLiabilityPremium: autoLiabilityPremium,
        cargoPremium: cargoPremium,
        physicalDamagePremium: physicalDamagePremium,
        vehicleLevelPremium: vehicleLevelPremium,
        generalLiabilityPremium: generalLiabilityPremium,
        additionalInsuredPremium: additionalInsuredPremium,
        waiverOfSubrogationPremium: waiverOfSubrogationPremium,
        primaryNonContributoryPremium: primaryNonContributoryPremium,
        riskManagementFee_AL: riskManagementFee_AL,
        riskManagementFee_PD: riskManagementFee_PD,
        premium: premium,
        fees: fees
      });
    }
  }

  populateQuoteFieldsByOption(optionId: number) {
    if (this.submissionData.riskDetail?.riskCoverages[optionId - 1].riskCoveragePremium !== null && this.submissionData.riskDetail?.riskCoverages?.length > 0) {
      const updatedRiskCoveragePremium = this.submissionData.riskDetail?.riskCoverages[optionId - 1].riskCoveragePremium;
      if (optionId === 1) {
        this.quoteLimitsForm.limitsForm.controls['alPremiumInput'].patchValue(updatedRiskCoveragePremium?.autoLiabilityPremium);
        this.quoteLimitsForm.limitsForm.controls['pdPremiumInput'].patchValue(updatedRiskCoveragePremium?.physicalDamagePremium);
        this.quoteLimitsForm.limitsForm.controls['glPremiumInput'].patchValue(updatedRiskCoveragePremium?.generalLiabilityPremium);
        this.quoteLimitsForm.limitsForm.controls['cargoPremiumInput'].patchValue(updatedRiskCoveragePremium?.cargoPremium);
      }

      const autoLiabilityPremium = this.quoteLimitsForm.limitsForm.value.autoLiabilityPremium;
      const cargoPremium = this.quoteLimitsForm.limitsForm.value.cargoPremium;
      const physicalDamagePremium = this.quoteLimitsForm.limitsForm.value.physicalDamagePremium;
      const vehicleLevelPremium = this.quoteLimitsForm.limitsForm.value.vehicleLevelPremium;
      const avgPerVehicle = this.quoteLimitsForm.limitsForm.value.avgPerVehicle;
      const generalLiabilityPremium = this.quoteLimitsForm.limitsForm.value.generalLiabilityPremium;
      const additionalInsuredPremium = this.quoteLimitsForm.limitsForm.value.additionalInsuredPremium;
      const waiverOfSubrogationPremium = this.quoteLimitsForm.limitsForm.value.waiverOfSubrogationPremium;
      const primaryNonContributoryPremium = this.quoteLimitsForm.limitsForm.value.primaryNonContributoryPremium;
      const riskManagementFee_AL = this.quoteLimitsForm.limitsForm.value.riskManagementFee_AL;
      const riskManagementFee_PD = this.quoteLimitsForm.limitsForm.value.riskManagementFee_PD;
      const premium = this.quoteLimitsForm.limitsForm.value.premium;
      const fees = this.quoteLimitsForm.limitsForm.value.fees;

      autoLiabilityPremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.autoLiabilityPremium);
      cargoPremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.cargoPremium);
      physicalDamagePremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.physicalDamagePremium);
      vehicleLevelPremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.vehicleLevelPremium);
      avgPerVehicle.splice(optionId - 1, 1, updatedRiskCoveragePremium?.avgPerVehicle);
      generalLiabilityPremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.generalLiabilityPremium);
      additionalInsuredPremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.additionalInsuredPremium);
      waiverOfSubrogationPremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.waiverOfSubrogationPremium);
      primaryNonContributoryPremium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.primaryNonContributoryPremium);
      riskManagementFee_AL.splice(optionId - 1, 1, updatedRiskCoveragePremium?.riskMgrFeeAL);
      riskManagementFee_PD.splice(optionId - 1, 1, updatedRiskCoveragePremium?.riskMgrFeePD);
      premium.splice(optionId - 1, 1, updatedRiskCoveragePremium?.premium);
      fees.splice(optionId - 1, 1, updatedRiskCoveragePremium?.fees);

      this.quoteLimitsForm.limitsForm.controls['autoLiabilityPremium'].patchValue(autoLiabilityPremium);
      this.quoteLimitsForm.limitsForm.controls['cargoPremium'].patchValue(cargoPremium);
      this.quoteLimitsForm.limitsForm.controls['physicalDamagePremium'].patchValue(physicalDamagePremium);
      this.quoteLimitsForm.limitsForm.controls['vehicleLevelPremium'].patchValue(vehicleLevelPremium);
      this.quoteLimitsForm.limitsForm.controls['avgPerVehicle'].patchValue(avgPerVehicle);
      this.quoteLimitsForm.limitsForm.controls['generalLiabilityPremium'].patchValue(generalLiabilityPremium);
      this.quoteLimitsForm.limitsForm.controls['additionalInsuredPremium'].patchValue(additionalInsuredPremium);
      this.quoteLimitsForm.limitsForm.controls['waiverOfSubrogationPremium'].patchValue(waiverOfSubrogationPremium);
      this.quoteLimitsForm.limitsForm.controls['primaryNonContributoryPremium'].patchValue(primaryNonContributoryPremium);
      this.quoteLimitsForm.limitsForm.controls['riskManagementFee_AL'].patchValue(riskManagementFee_AL);
      this.quoteLimitsForm.limitsForm.controls['riskManagementFee_PD'].patchValue(riskManagementFee_PD);
      this.quoteLimitsForm.limitsForm.controls['premium'].patchValue(premium);
      this.quoteLimitsForm.limitsForm.controls['fees'].patchValue(fees);
    }
  }

  setQuoteDefaultsData(): Observable<any> {
    const businessDetail = this.submissionData?.riskDetail?.businessDetails;
    let riskCovs = this.submissionData?.riskDetail?.riskCoverages;
    if (businessDetail.isNewVenture) {
      if (riskCovs != null) {
        riskCovs.forEach((item) => {
          if (item.liabilityExperienceRatingFactor == null)
            item.liabilityExperienceRatingFactor = 1;

          if (item.liabilityScheduleRatingFactor == null)
            item.liabilityScheduleRatingFactor = 1;
        });
      }
    }

    riskCovs = riskCovs?.sort(riskCov => riskCov.optionNumber);
    riskCovs.forEach((riskCov) => {
      let isOption1 = riskCov.optionNumber == 1;
      let riskCovOption1 = riskCovs.find(rc => rc.optionNumber == 1);

        riskCov.brokerCommisionAL = riskCov.brokerCommisionAL ?? (isOption1 ? 10.5 : riskCovOption1.brokerCommisionAL);
        riskCov.brokerCommisionPD = riskCov.brokerCommisionPD ?? (isOption1 ? 10.0 : riskCovOption1.brokerCommisionPD);

        riskCov.arsCommission = riskCov.arsCommission ?? (riskCov.brokerCommisionAL >= 12 ? 1 : 2.5);

        riskCov.glScheduleRF = riskCov.glScheduleRF ?? (isOption1 ? 1.00 : riskCovOption1.glScheduleRF);
        riskCov.glExperienceRF = riskCov.glExperienceRF ?? (isOption1 ? 1.00 : riskCovOption1.glExperienceRF);

        riskCov.cargoScheduleRF = riskCov.cargoScheduleRF ?? (isOption1 ?  1.00 : riskCovOption1.cargoScheduleRF);
        riskCov.cargoExperienceRF = riskCov.cargoExperienceRF ?? (isOption1 ?  1.00 : riskCovOption1.cargoExperienceRF);

        riskCov.apdScheduleRF = riskCov.apdScheduleRF ?? (isOption1 ? 1.00 : riskCovOption1.apdScheduleRF);
        riskCov.apdExperienceRF = riskCov.apdExperienceRF ?? (isOption1 ? 1.00 : riskCovOption1.apdExperienceRF);

        riskCov.depositPct = riskCov.depositPct ?? (isOption1 ? 10.00 : riskCovOption1.depositPct);

        riskCov.cargoLimitSymbolId = riskCovs[0].cargoLimitSymbolId;
        riskCov.refCargoLimitSymbolId = riskCovs[0].refCargoLimitSymbolId;

        if(this.raterVersionSupport(8)){
          riskCov.trailerInterchangeLimitSymbolId = null;
          riskCov.nonOwnedTrailerLimitSymbolId = null;
        }
    });
    
    let riskCoverageListDTO: RiskCoverageListDTO = {
      riskDetailId: this.submissionData.riskDetail.id,
      riskCoverages: riskCovs
    };
    return this.quoteOptionsService.saveQuoteOptions(riskCoverageListDTO);
  }

  setQuoteDefaults() {
    const index = 0;

    //const yearsInBusiness = this.submissionData?.riskDetail?.businessDetails?.yearsInBusiness ?? 0;
    const businessDetail = this.submissionData?.riskDetail?.businessDetails;
    const riskCovs = this.submissionData?.riskDetail?.riskCoverages;
    if (businessDetail.isNewVenture) {
      const liabilityExperienceRatingFactor = this.quoteLimitsForm.limitsForm.value.LiabilityExperienceRatingFactor;
      const liabilityScheduleRatingFactor = this.quoteLimitsForm.limitsForm.value.LiabilityScheduleRatingFactor;
      liabilityExperienceRatingFactor.splice(index, 1, 1.00);
      liabilityScheduleRatingFactor.splice(index, 1, 1.00);

      if (riskCovs != null) {
        riskCovs.forEach((item) => {
          if (item.liabilityExperienceRatingFactor == null)
            item.liabilityExperienceRatingFactor = 1;

          if (item.liabilityScheduleRatingFactor == null)
            item.liabilityScheduleRatingFactor = 1;
        });
      }

      this.quoteLimitsForm.limitsForm.controls['LiabilityExperienceRatingFactor'].patchValue(liabilityExperienceRatingFactor);
      this.quoteLimitsForm.limitsForm.controls['LiabilityScheduleRatingFactor'].patchValue(liabilityScheduleRatingFactor);
    }

    // Do not apply defaults if value exist for these fields
    if (this.quoteLimitsForm.limitsForm.value.brokerCommisionAL[index] || this.quoteLimitsForm.limitsForm.value.brokerCommisionPD[index] || 
      this.quoteLimitsForm.limitsForm.value.glScheduleRF[index] || this.quoteLimitsForm.limitsForm.value.glExperienceRF[index] || 
      this.quoteLimitsForm.limitsForm.value.cargoScheduleRF[index] || this.quoteLimitsForm.limitsForm.value.cargoExperienceRF[index] || 
      this.quoteLimitsForm.limitsForm.value.apdScheduleRF[index] || this.quoteLimitsForm.limitsForm.value.apdExperienceRF[index] || 
      this.quoteLimitsForm.limitsForm.value.depositPct[index] || this.quoteLimitsForm.limitsForm.value.depositPremium[index]) {
      return;
    }

    const brokerCommisionAL = this.quoteLimitsForm.limitsForm.value.brokerCommisionAL;
    brokerCommisionAL.splice(index, 1, 10.5);
    const brokerCommisionPD = this.quoteLimitsForm.limitsForm.value.brokerCommisionPD;
    brokerCommisionPD.splice(index, 1, 10.0);

    const glScheduleRF = this.quoteLimitsForm.limitsForm.value.glScheduleRF;
    glScheduleRF.splice(index, 1, 1.00);
    const glExperienceRF = this.quoteLimitsForm.limitsForm.value.glExperienceRF;
    glExperienceRF.splice(index, 1, 1.00);
    const cargoScheduleRF = this.quoteLimitsForm.limitsForm.value.cargoScheduleRF;
    cargoScheduleRF.splice(index, 1, 1.00);
    const cargoExperienceRF = this.quoteLimitsForm.limitsForm.value.cargoExperienceRF;
    cargoExperienceRF.splice(index, 1, 1.00);
    const apdScheduleRF = this.quoteLimitsForm.limitsForm.value.apdScheduleRF;
    apdScheduleRF.splice(index, 1, 1.00);
    const apdExperienceRF = this.quoteLimitsForm.limitsForm.value.apdExperienceRF;
    apdExperienceRF.splice(index, 1, 1.00);

    const depositPct = this.quoteLimitsForm.limitsForm.value.depositPct;
    depositPct.splice(index, 1, 10.0);

    this.quoteLimitsForm.limitsForm.controls['brokerCommisionAL'].patchValue(brokerCommisionAL);
    this.quoteLimitsForm.limitsForm.controls['brokerCommisionPD'].patchValue(brokerCommisionPD);
    this.quoteLimitsForm.limitsForm.controls['glScheduleRF'].patchValue(glScheduleRF);
    this.quoteLimitsForm.limitsForm.controls['glExperienceRF'].patchValue(glExperienceRF);
    this.quoteLimitsForm.limitsForm.controls['cargoScheduleRF'].patchValue(cargoScheduleRF);
    this.quoteLimitsForm.limitsForm.controls['cargoExperienceRF'].patchValue(cargoExperienceRF);
    this.quoteLimitsForm.limitsForm.controls['apdScheduleRF'].patchValue(apdScheduleRF);
    this.quoteLimitsForm.limitsForm.controls['apdExperienceRF'].patchValue(apdExperienceRF);
    this.quoteLimitsForm.limitsForm.controls['depositPct'].patchValue(depositPct);
  }

  retrieveDropDownValues(isInitial: boolean = false, fgOption?: FormGroup, column?: number) {
    this.disabledList = [];
    if (this.submissionData?.isIssued) { return; }
    this.umBiLimitList = [];
    this.umPdLimitList = [];
    this.uimBiLimitList = [];
    this.uimPdLimitList = [];
    this.pipList = [];
    this.trailerInterchangeList = [];
    this.nonOwnedTrailerList = [];
    this.pollutionCovList = [];
    
    console.log(this.submissionData?.riskDetail?.raterVersionId);
    console.log('issuported',this.raterVersionSupport());

    this.isUmBiAvailable = false;
    this.isUmPdAvailable = false;
    this.isUimBiAvailable = false;
    this.isUimPdAvailable = false;
    this.isPipAvailable = false;
    const state = this.cityState;
    // Autoliability/Property
    this.limitService.getLimits(state, this.submissionData.riskDetail.raterVersionId).pipe(take(1)).subscribe(data => {
      if (data.umbiLimits && data.umbiLimits.length > 0) {
        this.umBiLimitList = this.customConvertEnumerableListToSelectItemList(data.umbiLimits);
        this.isUmBiAvailable = true;
      }

      if (data.umpdLimits && data.umpdLimits.length > 0) {
        this.umPdLimitList = this.customConvertEnumerableListToSelectItemList(data.umpdLimits);
        this.isUmPdAvailable = true;
      }

      if (data.uimbiLimits && data.uimbiLimits.length > 0) {
        this.uimBiLimitList = this.customConvertEnumerableListToSelectItemList(data.uimbiLimits);
        this.isUimBiAvailable = true;
      }

      if (data.uimpdLimits) {
        if (data.uimpdLimits.length > 0) {
          this.uimPdLimitList = this.customConvertEnumerableListToSelectItemList(data.uimpdLimits);
          this.isUimPdAvailable = true;  
        }
        else {
          const selectItemList: SelectItem[] = [];
          selectItemList.push({
            value: 1,
            label: 'NONE'
          });

          this.uimPdLimitList = selectItemList;
          this.disabledList.push('uimpdLimits');
        }
      }

      if (data.pipLimits) {
        if (data.pipLimits.length > 0) {
          this.pipList = this.customConvertEnumerableListToSelectItemList(data.pipLimits);
          this.isPipAvailable = true;  
        }
        else {
          const selectItemList: SelectItem[] = [];
          selectItemList.push({
            value: 1,
            label: 'NONE'
          });

          this.pipList = selectItemList;
          this.disabledList.push('pipLimits');
        }
      }

      if (data.medPayLimits) {
        if (data.medPayLimits.length > 0) {
          if (['FL','OR','VA'].includes(state)) {
            this.disabledList.push('medPayLimits');
          } else {
            this.medpayList = this.customConvertEnumerableListToSelectItemList(data.medPayLimits);
          }
        }
      }

      this.biLimitList = this.customConvertEnumerableListToSelectItemList(data.autoBILimits);
      this.pdLimitList = this.customConvertEnumerableListToSelectItemList(data.autoPDLimits);
      this.liabDeductibleList = this.customConvertEnumerableListToSelectItemList(data.liabilityDeductibleLimits);
      this.ComprehensiveList = this.customConvertEnumerableListToSelectItemList(data.comprehensiveLimits);
      this.CollisionList = this.customConvertEnumerableListToSelectItemList(data.collisionLimits);

      this.glList = this.sortGlList(this.customConvertEnumerableListToSelectItemList(data.glLimits));
      this.cargoList = this.customConvertEnumerableListToSelectItemList(data.cargoLimits);
      this.refBreakList = this.customConvertEnumerableListToSelectItemList(data.refLimits);

      if (this.raterVersionSupport()) {
        this.trailerInterchangeList = this.customConvertEnumerableListToSelectItemList(data.trailerInterchangeLimits);
        this.nonOwnedTrailerList = this.customConvertEnumerableListToSelectItemList(data.nonOwnedTrailerLimits);
        this.pollutionCovList = this.customConvertEnumerableListToSelectItemList(data.pollutionCovLimits);  
      }

      this.setFieldsValidations(fgOption, column);

      // Manual Update of menu badge because of this async http method
      if (!isInitial) { // If not new
        this.submissionData.validationList.quoteOptions = FormUtils.validateForm(this.quoteOptionsForm);
        this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList,
          this.riskSpecificsData.hiddenNavItems));
      }

      this.fieldArray.forEach(x => {
        this.checkAndSetPdDisplayAndValue(false, x);
      });

      this.updateAllColumnPdDisplayValue();
    });
  }

  public disabledField(field: string) {
    return this.disabledList?.some(x => x == field);
  }

  // Remove coma in value
  customConvertEnumerableListToSelectItemList(list: any[]): SelectItem[] {
    const convertedList: SelectItem[] = [];
    const noneList: SelectItem[] = [];
    if (list) {
      for (const item of list) {
        if (item.limitDisplay != 'NONE') {
          convertedList.push({
            label: item.limitDisplay,
            value: item.id
          });
        } else {
          noneList.push({
            label: item.limitDisplay,
            value: item.id
          });
        }
      }
    }

    return convertedList.concat(noneList);
  }

  setFieldsValidations(fgOption: FormGroup, column: number) {
    const optionFormCtrl = fgOption.controls;
    this.resetField(optionFormCtrl['autoBILimitId']);
    this.resetField(optionFormCtrl['alSymbolId']);
    this.resetField(optionFormCtrl['medPayLimitId']);
    this.resetField(optionFormCtrl['liabilityDeductibleId']);
    this.resetField(optionFormCtrl['compFireTheftDeductibleId']);
    this.resetField(optionFormCtrl['collDeductibleId']);
    this.resetField(optionFormCtrl['glbiLimitId']);
    this.resetField(optionFormCtrl['cargoLimitId']);
    this.resetField(optionFormCtrl['refCargoLimitId']);

    this.resetField(optionFormCtrl['brokerCommisionAL']);
    this.resetField(optionFormCtrl['brokerCommisionPD']);
    this.resetField(optionFormCtrl['glScheduleRF']);
    this.resetField(optionFormCtrl['glExperienceRF']);
    this.resetField(optionFormCtrl['cargoScheduleRF']);
    this.resetField(optionFormCtrl['cargoExperienceRF']);
    this.resetField(optionFormCtrl['apdScheduleRF']);
    this.resetField(optionFormCtrl['apdExperienceRF']);
    // this.resetField('depositPct');
    // this.resetField('depositAmount');
    // this.resetField('numberOfInstallments');

    this.setAsRequired(optionFormCtrl['autoBILimitId'], column);
    this.setAsRequired(optionFormCtrl['alSymbolId'], column);
    this.setAsRequired(optionFormCtrl['medPayLimitId'], column);
    this.setAsRequired(optionFormCtrl['liabilityDeductibleId'], column);
    this.setAsRequired(optionFormCtrl['compFireTheftDeductibleId'], column);
    this.setAsRequired(optionFormCtrl['collDeductibleId'], column);
    this.setAsRequired(optionFormCtrl['glbiLimitId'], column);
    this.setAsRequired(optionFormCtrl['cargoLimitId'], column);
    this.setAsRequired(optionFormCtrl['refCargoLimitId'], column);

    this.setAsRequired(optionFormCtrl['brokerCommisionAL'], column);
    this.setAsRequired(optionFormCtrl['brokerCommisionPD'], column);

    this.setRFsAsRequiredAndMinValue(optionFormCtrl['glScheduleRF'], column);
    this.setRFsAsRequiredAndMinValue(optionFormCtrl['glExperienceRF'], column);
    this.setRFsAsRequiredAndMinValue(optionFormCtrl['cargoScheduleRF'], column);
    this.setRFsAsRequiredAndMinValue(optionFormCtrl['cargoExperienceRF'], column);
    this.setRFsAsRequiredAndMinValue(optionFormCtrl['apdScheduleRF'], column);
    this.setRFsAsRequiredAndMinValue(optionFormCtrl['apdExperienceRF'], column);
    // this.setAsRequired('depositPct', column);
    // this.setAsRequired('depositAmount', column);
    // this.setAsRequired('numberOfInstallments', column);

    this.setAsRequired(optionFormCtrl['autoPDLimitId'], column);
    this.setAsRequired(optionFormCtrl['pdSymbolId'], column);

    if (this.raterVersionSupport()) { this.setAsRequired(optionFormCtrl['trailerInterchangeLimitId'], column); } else { this.resetField(optionFormCtrl['trailerInterchangeLimitId']); }
    if (this.raterVersionSupport()) { this.setAsRequired(optionFormCtrl['nonOwnedTrailerLimitId'], column); } else { this.resetField(optionFormCtrl['nonOwnedTrailerLimitId']); }
    if (this.raterVersionSupport()) { this.setAsRequired(optionFormCtrl['pollutionCovLimitId'], column); } else { this.resetField(optionFormCtrl['pollutionCovLimitId']); }

    if (this.isUmBiAvailable) { this.setAsRequired(optionFormCtrl['umbiLimitId'], column); } else { this.resetField(optionFormCtrl['umbiLimitId']); }
    if (this.isUmPdAvailable) { this.setAsRequired(optionFormCtrl['umpdLimitId'], column); } else { this.resetField(optionFormCtrl['umpdLimitId']); }
    if (this.isUimBiAvailable) { this.setAsRequired(optionFormCtrl['uimbiLimitId'], column); } else { this.resetField(optionFormCtrl['uimbiLimitId']); }
    if (this.isUimPdAvailable) { this.setAsRequired(optionFormCtrl['uimpdLimitId'], column); } else { this.resetField(optionFormCtrl['uimpdLimitId']); }
    if (this.isPipAvailable) { this.setAsRequired(optionFormCtrl['pipLimitId'], column); } else { this.resetField(optionFormCtrl['pipLimitId']); }
  }

  get f() { return this.quoteLimitsForm.limitsForm.controls; }
  private resetField(optionFormCtrlField: AbstractControl) {
    optionFormCtrlField.clearValidators();
    optionFormCtrlField.updateValueAndValidity();
  }

  private setAsRequired(optionFormCtrlField: AbstractControl, columnLength: number) {
    switch (columnLength) {
      case 1:
        optionFormCtrlField.setValidators(oneColumnRequired);
        break;
      case 2:
        optionFormCtrlField.setValidators(twoColumnRequired);
        break;
      case 3:
        optionFormCtrlField.setValidators(threeColumnRequired);
        break;
      case 4:
        optionFormCtrlField.setValidators(fourColumnRequired);
        break;
      default:
        break;
    }
    optionFormCtrlField.updateValueAndValidity();
  }

  private setRFsAsRequiredAndMinValue(optionFormCtrlField: AbstractControl, columnLength: number) {
    switch (columnLength) {
      case 1:
        optionFormCtrlField.setValidators([Validators.min(0.0001), oneColumnRequired]);
        break;
      case 2:
        optionFormCtrlField.setValidators([Validators.min(0.0001), twoColumnRequired]);
        break;
      case 3:
        optionFormCtrlField.setValidators([Validators.min(0.0001), threeColumnRequired]);
        break;
      case 4:
        optionFormCtrlField.setValidators([Validators.min(0.0001), fourColumnRequired]);
        break;
      default:
        break;
    }
    optionFormCtrlField.updateValueAndValidity();
  }



  updateDriver(isAdded: boolean = true): any {
    if (this.submissionData.riskDetail?.drivers?.length <= 0) { return; }

    // using this code prevent reference to the main source.
    const objDriverLst = this.submissionData.riskDetail?.drivers?.map(x => Object.assign({}, x));
    if (!objDriverLst) { return; }


    // const covOption = this.submissionData?.risk?.riskCoverages?.find(x => x.optionNumber === optionId);
    const covOption = this.submissionData?.riskDetail?.riskCoverages?.map(x => x.optionNumber);
    //#region put validation for options

    // get the last saved option number
    const optNumber = covOption.reverse()[0];
    objDriverLst.forEach(o => {
      let arrOption = o.options?.split(',') ?? [];
      if (isAdded) {
        const validateAgeOptions = this.driverData.optAgeValidation(o);
        if (validateAgeOptions != null && !o.isExcludeKO) {
          arrOption = [];
        } else {
          arrOption.push(optNumber.toString());
        }
      } else {
        if (arrOption.indexOf((optNumber + 1).toString()) !== -1) {
          arrOption.splice(arrOption.indexOf((optNumber + 1).toString()), 1);
        }
      }

      o.options = Array.from(new Set(arrOption)).join(','); // filter unique
      o.options = o.options?.length === 0 || !o.options?.startsWith('1') ? null : o.options;
      o.riskDetailId = this.submissionData.riskDetail.id;
    });

    //#endregion
    const objDriver = objDriverLst.map(o => ({ riskDetailId: o.riskDetailId, id: o.id, options: o.options }));
    return this.driverService.PutListOptions(objDriver).subscribe(result => {
      this.submissionData.riskDetail.drivers = objDriverLst.map(x => Object.assign([], x));
      return result;
    });
  }

  updateVehicle(isAdded: boolean = true): any {
    if (this.submissionData.riskDetail?.vehicles?.length <= 0) { return; }

    // using this code prevent reference to the main source.
    const objVehLst = this.submissionData.riskDetail?.vehicles?.map(x => Object.assign({}, x));
    if (!objVehLst) { return; }

    // const covOption = this.submissionData?.risk?.riskCoverages?.find(x => x.optionNumber === optionId);
    const covOption = this.submissionData?.riskDetail?.riskCoverages?.map(x => x.optionNumber);
    //#region put validation for options

    // get the last saved option number
    const optNumber = covOption.reverse()[0];
    objVehLst.forEach(o => {
      const arrOption = o.options?.split(',') ?? [];
      if (isAdded) {
        if (arrOption.length !==0 && arrOption[0] !== "" && optNumber !== 1)
        {
          arrOption.push(optNumber.toString());
        }
      } else {
        if (arrOption.indexOf((optNumber + 1).toString()) !== -1) {
          arrOption.splice(arrOption.indexOf((optNumber + 1).toString()), 1);
        }
      }

      o.options = Array.from(new Set(arrOption)).join(','); // filter unique
      o.options = o.options?.length === 0 ? null : o.options;
    });

    //#endregion
    const objVeh = objVehLst.map(o => ({ riskDetailId: o.riskDetailId, id: o.id, options: o.options }));
    return this.vehicleService.PutListOptions(objVeh).subscribe(result => {
      this.submissionData.riskDetail.vehicles = objVehLst.map(x => Object.assign({}, x));
      if (isAdded) {
        let fgOption = this.optionsArray.controls.find(fgOpt => fgOpt.value.optionNumber == covOption.length) as FormGroup;
        this.autoComputeRiskManagementFee(fgOption, covOption.length - 1);
      }
      return result;
    });
  }

  updateBinding() {
    const bindingOption = this.submissionData?.riskDetail?.binding?.bindOptionId ?? null;
    if (bindingOption && this.optionIsDisabled(bindingOption)) {
      const binding: BindingDto = Object.assign({}, this.submissionData?.riskDetail?.binding);
      //binding.bindOptionId = Defaults.BindingDefaults.bindOptionId?.toString(); // deselect if binding is disabled;
      binding.bindOptionId = '1'; // circular dependency detected so we direct this to specific value;

      try {
        // tslint:disable-next-line: deprecation
        this.service.addUpdateBinding(binding).pipe(take(1)).subscribe(data => {
          this.submissionData.riskDetail.binding = data;
        }, (error) => {
        });
      } catch {
      }
    }
  }

  optionIsDisabled(opt): boolean {
    const riskDetailId = this.submissionData?.riskDetail?.id;
    const riskCoverage = this.submissionData?.riskDetail?.riskCoverages?.find(x => x.optionNumber === opt && x.riskDetailId === riskDetailId);
    return riskCoverage === undefined;
  }

  updateAllColumnPdDisplayValue() {
    this.fieldArray.forEach(x => {
      this.checkAndSetPdDisplayAndValue(false, x);
    });
  }

  checkAndSetPdDisplayAndValue(isChange: boolean = false, index: any) {
    const al = this.quoteLimitsForm.limitsForm.controls['autoBILimitId'].value;
    const alSymbol = this.quoteLimitsForm.limitsForm.controls['alSymbolId'].value;

    const pd = this.quoteLimitsForm.limitsForm.controls['autoPDLimitId'].value;
    const pdSymbol = this.quoteLimitsForm.limitsForm.controls['pdSymbolId'].value;
    if (al[index] == null || al[index] === '') {
      this.pdView[index] = false;
      pd.splice(index, 1, null);
      pdSymbol.splice(index, 1, null);
      this.quoteLimitsForm.limitsForm.controls['autoPDLimitId'].patchValue(pd);
      this.quoteLimitsForm.limitsForm.controls['pdSymbolId'].patchValue(pdSymbol);
      return;
    }

    const alLabel = this.biLimitList.find(x => x.value == al[index])?.label;

    if (alLabel) {
      if (alLabel.toString().indexOf('/') !== -1) {
        this.pdView[index] = true;
        if (isChange) {
          pd.splice(index, 1, LimitsDefaultValues.autoPDSplitLimitId);
          pdSymbol.splice(index, 1, LimitsDefaultValues.defaultSymbol);
          this.quoteLimitsForm.limitsForm.controls['autoPDLimitId'].patchValue(pd);
          this.quoteLimitsForm.limitsForm.controls['pdSymbolId'].patchValue(pdSymbol);
        }
      } else {
        this.pdView[index] = false;
        const pdValue = this.pdLimitList.find(x => x.label == alLabel)?.value;

        pd.splice(index, 1, pdValue);
        pdSymbol.splice(index, 1, alSymbol[index]);

        this.quoteLimitsForm.limitsForm.controls['autoPDLimitId'].patchValue(pd);
        this.quoteLimitsForm.limitsForm.controls['pdSymbolId'].patchValue(pdSymbol);
      }
    }
  }

  updatePaymentOption(fgOption: FormGroup): Observable<any> {
    let optionForm = fgOption.value as OptionForm;
    return this.saveOptions()
      .pipe(
        switchMap(() => this.quoteOptionsService.updatePaymentOption(this.submissionData.riskDetail.id, optionForm.optionNumber)),
        switchMap(() => this.loadQuoteOptions())
      );
  }

  autoComputeRiskManagementFee(fgOption: FormGroup, index: number): void {
    let riskManagementFeeALVal = 0;
    let riskManagementFeePDVal = 0;
    let totalRiskManagementFee = 0;
    let totalInstallmentFee = 0;
    const vehicles = this.submissionData?.riskDetail?.vehicles?.filter(x => x.isValidated && x.options?.split(',').includes(`${index + 1}`));
    if (vehicles?.length > 0) {
      const RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT = this.submissionData.riskDetail?.feeDetails?.find(feeDetail => feeDetail?.description === 'Service Fee - Single Units (AL)')?.amount;
      const RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT = this.submissionData.riskDetail?.feeDetails?.find(feeDetail => feeDetail?.description === 'Service Fee - Multiple Units (AL)')?.amount;
      const RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT = this.submissionData.riskDetail?.feeDetails?.find(feeDetail => feeDetail?.description === 'Service Fee - Single Units (PD)')?.amount;
      const RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT = this.submissionData.riskDetail?.feeDetails?.find(feeDetail => feeDetail?.description === 'Service Fee - Multiple Units (PD)')?.amount;
      let pdUnits = 0;
      vehicles?.forEach((vehicle) => {
        let hasCovCollision = vehicle?.isCoverageCollision == true && vehicle.coverageCollisionDeductibleId !== 65;
        let hasCovFireTheft = vehicle?.isCoverageFireTheft == true && vehicle.coverageFireTheftDeductibleId !== 76;

        if (hasCovCollision || hasCovFireTheft) {
          pdUnits++;
        }
      });

      riskManagementFeeALVal = (vehicles?.length > 1) ? (vehicles?.length * RISK_MANAGEMENT_FEE_AL_MULTIPLE_UNIT) : (vehicles?.length == 1) ? RISK_MANAGEMENT_FEE_AL_SINGLE_UNIT : 0;
      riskManagementFeePDVal = (vehicles?.length > 1) ? (pdUnits * RISK_MANAGEMENT_FEE_PD_MULTIPLE_UNIT) : (pdUnits == 1) ? RISK_MANAGEMENT_FEE_PD_SINGLE_UNIT : 0;
      totalRiskManagementFee = riskManagementFeeALVal + riskManagementFeePDVal;
    }

    totalInstallmentFee = this.submissionData?.riskDetail?.riskCoverages[index]?.riskCoveragePremium?.totalInstallmentFee;

    fgOption.patchValue({
      riskMgrFeeAL: riskManagementFeeALVal,
      riskMgrFeePD: riskManagementFeePDVal,
      fees: (totalRiskManagementFee + totalInstallmentFee)
    });

    this.autoComputeInstallmentFee(fgOption, index);
  }

  autoComputeInstallmentFee(fgOption: FormGroup, index: number): void {
    let installmentFeeWithTax = 0;

    const installmentFee = this.submissionData?.riskDetail?.riskCoverages[index]?.riskCoveragePremium?.installmentFee;
    const installmentTax = this.submissionData?.riskDetail?.riskCoverages[index]?.riskCoveragePremium?.perInstallmentTax;
    if (installmentFee > 0) {
      installmentFeeWithTax = installmentFee + installmentTax;
    }

    fgOption.patchValue({
      installmentFee: installmentFeeWithTax
    });
  }

  autoComputeAdditionalInterest(fgOption: FormGroup): void {
    const additionalInterests = this.submissionData?.riskDetail?.additionalInterest;
    let additionalInsuredPremiumVal = 0;
    let waiverOfSubrogationPremiumVal = 0;
    let primaryNonContributoryPremiumVal = 0;
    const isBlanketCoverageAdditionalInsured = this.submissionData?.riskDetail?.blanketCoverage?.isAdditionalInsured;
    const isBlanketCoverageWaiverOfSubrogation = this.submissionData?.riskDetail?.blanketCoverage?.isWaiverOfSubrogation;
    const isBlanketCoveragePrimaryAndNonContributory = this.submissionData?.riskDetail?.blanketCoverage?.isPrimaryNonContributory;
    const ADDITIONAL_INSURED_RATE = 50;
    const WAIVER_OF_SUBROGATION_RATE = 250;
    const PRIMARY_AND_NONCONTRIBUTORY_RATE = 500;
    const ADDITIONAL_INSURED_FLAT_RATE = 500;
    const WAIVER_OF_SUBROGATION_FLAT_RATE = 1000;
    const PRIMARY_NON_CONTRIBUTORY_FLAT_RATE = 1500;
    if (additionalInterests?.length > 0) {
      additionalInsuredPremiumVal = isBlanketCoverageAdditionalInsured ? ADDITIONAL_INSURED_FLAT_RATE : (additionalInterests?.filter(x => x.additionalInterestTypeId == 1).length * ADDITIONAL_INSURED_RATE);
      waiverOfSubrogationPremiumVal = isBlanketCoverageWaiverOfSubrogation ? WAIVER_OF_SUBROGATION_FLAT_RATE : (additionalInterests?.filter(x => x.isWaiverOfSubrogation).length * WAIVER_OF_SUBROGATION_RATE);
      primaryNonContributoryPremiumVal = isBlanketCoveragePrimaryAndNonContributory ? PRIMARY_NON_CONTRIBUTORY_FLAT_RATE : (additionalInterests?.filter(x => x.isPrimaryNonContributory).length * PRIMARY_AND_NONCONTRIBUTORY_RATE);
    } else {
      additionalInsuredPremiumVal = isBlanketCoverageAdditionalInsured ? ADDITIONAL_INSURED_FLAT_RATE : 0;
      waiverOfSubrogationPremiumVal = isBlanketCoverageWaiverOfSubrogation ? WAIVER_OF_SUBROGATION_FLAT_RATE : 0;
      primaryNonContributoryPremiumVal = isBlanketCoveragePrimaryAndNonContributory ? PRIMARY_NON_CONTRIBUTORY_FLAT_RATE : 0;
    }

    fgOption.patchValue({
      additionalInsuredPremium: additionalInsuredPremiumVal,
      waiverOfSubrogationPremium: waiverOfSubrogationPremiumVal,
      primaryNonContributoryPremium: primaryNonContributoryPremiumVal
    });
  }

  private sortGlList(glList: any[]) {
    glList = glList.sort((a, b) => b.value === 87 || a.value < b.value ? 1 : -1);
    return glList;
  }

  public updateFactorModSettings(raterVersionId) {
    var factorModSettingsFactory = new FactorModSettingsFactory();
    this.factorModsSettings = factorModSettingsFactory.get(raterVersionId);
  }

  public raterVersionSupport(id: number = this.RATER_VERSION_SUPPORT): boolean {
    return this.submissionData?.riskDetail?.raterVersionId == null ||
      this.submissionData?.riskDetail?.raterVersionId >= id ||
      this.submissionData?.riskDetail?.raterVersionId == 0;
  }
}

export class OptionPremiumForm {
  optionNumber: number = null;
  avgAlVehicleTotalPremium: number = null;
  avgAlVehiclePowerPremium: number = null;
  avgAlVehicleTrailerOtherPremium: number = null;
  avgApdVehicleTotalPremium: number = null;
  avgApdVehiclePowerPremium: number = null;
  avgApdVehicleTrailerOtherPremium: number = null;
  avgMtcRefVehicleTotalPremium: number = null;
  avgMtcRefVehiclePowerPremium: number = null;
  avgMtcRefVehicleTrailerOtherPremium: number = null;
  avgTotalVehiclePremium: number = null;
  avgTotalVehiclePowerPremium: number = null;
  avgTotalVehicleTrailerOtherPremium: number = null;
}
/**
 * A view model which is derived from RiskCoverageDTO and will be supplied for per Quote Option FormGroups.
 */
export class OptionForm {
  public static create(obj: any): OptionForm {
    let newOptionForm = new OptionForm();
    for (let key in obj) {
      newOptionForm[key] = obj[key];
    }
    return newOptionForm;
  }

  id: string = null;
  optionNumber: number = null;
  autoBILimitId: number = null;
  alSymbolId: number = null;
  autoPDLimitId: number = null;
  pdSymbolId: number = null;
  umbiLimitId: number = null;
  umpdLimitId: number = null;
  uimbiLimitId: number = null;
  uimpdLimitId: number = null;
  medPayLimitId: number = null;
  pipLimitId: number = null;
  liabilityDeductibleId: number = null;
  comprehensiveFire: boolean = null;
  compFireTheftDeductibleId: number = null;
  fireDeductibleId?: number = null;
  collDeductibleId: number = null;
  glbiLimitId: number = null;
  cargoLimitId: number = null;
  refCargoLimitId: number = null;
  trailerInterchangeLimitId: number = null;
  nonOwnedTrailerLimitId: number = null;
  pollutionCovLimitId: number = null;
  brokerCommisionAL?: string = null;
  brokerCommisionPD: string = null;
  arsCommission: string = null;
  glScheduleRF: number = null;
  glExperienceRF: number = null;
  cargoScheduleRF: number = null;
  cargoExperienceRF: number = null;
  apdScheduleRF: number = null;
  apdExperienceRF: number = null;
  depositPct: number = null;
  depositPremium: number = null;
  numberOfInstallments: number = null;

  liabilityScheduleRatingFactor: number = null;
  liabilityExperienceRatingFactor: number = null;

  avgPerVehicle: number = null;
  autoLiabilityPremium: number = null;
  cargoPremium: number = null;
  physicalDamagePremium: number = null;
  vehicleLevelPremium: number = null;
  generalLiabilityPremium: number = null;
  trailerInterchangePremium: number = null;
  trailerInterchangePremiumAL: number = null;
  nonOwnedTrailerPremium: number = null;
  additionalInsuredPremium: number = null;
  waiverOfSubrogationPremium: number = null;
  primaryNonContributoryPremium: number = null;
  riskMgrFeeAL: number = null;
  riskMgrFeePD: number = null;
  premium: number = null;
  fees: number = null;
  taxesAndStampingFees: number = null;
  totalAmounDueFull: number = null;
  totalAmountDueInstallment: number = null;
  perInstallment: number = null;
  installmentFee: number = null;
  depositAmount: number = null;
  riskCoveragePremiumId: string = null;

  public setDefaultBrokerCommission(): void {
    this.brokerCommisionAL = this.brokerCommisionAL ?? '10.5';
    this.brokerCommisionPD = this.brokerCommisionPD ?? '10.0';
    this.arsCommission = this.arsCommission ?? '1';
  }

  public setDefaultExperienceRatingFactors(): void {
    this.liabilityExperienceRatingFactor = this.liabilityExperienceRatingFactor ?? 1.00;
    this.liabilityScheduleRatingFactor = this.liabilityScheduleRatingFactor ?? 1.00;
  }

  public setDefaultPremiumRatingFactors(): void {
    this.glScheduleRF = this.glScheduleRF ?? 1.00;
    this.glExperienceRF = this.glExperienceRF ?? 1.00;

    this.cargoScheduleRF = this.cargoScheduleRF ?? 1.00;
    this.cargoExperienceRF = this.cargoExperienceRF ?? 1.00;

    this.apdScheduleRF = this.apdScheduleRF ?? 1.00;
    this.apdExperienceRF = this.apdExperienceRF ?? 1.00;
  }

  public setDefaultDeposit(): void {
    this.depositPct = this.depositPct ?? 10.0;
    if (this.numberOfInstallments == null)
      this.numberOfInstallments = this.depositPct === 100 ? null : 10;
  }

  public recalculateDepositPremium(): void {
    if (this.premium == null) {
      this.depositPremium = null;
      return;
    }
    this.depositPremium = (this.depositPct / 100) * this.premium;
  }

  public recalculateDepositPercentage() {
    if (this.premium == null) {
      return;
    }
    let newPercentage = (this.depositPremium / this.premium) * 100;
    this.depositPct = Math.floor(newPercentage * 10) / 10; // Cut to 1 decimal.
  }

  public equalizePremiumAndDepositPremium() {
    this.depositPremium = this.premium;
    this.depositPct = 100;
  }

}

