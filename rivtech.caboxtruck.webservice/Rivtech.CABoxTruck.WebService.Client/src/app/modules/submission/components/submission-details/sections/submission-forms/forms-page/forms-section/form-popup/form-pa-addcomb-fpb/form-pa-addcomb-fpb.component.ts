import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { FormsData } from '../../../../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../../../shared/utilities/utils';
import { finalize } from 'rxjs/operators';
import { FormsService } from '../../../../../../../../../../modules/submission/services/forms.service';
import { FormOtherInfoDTO } from '@app/shared/models/submission/forms/FormOtherInfoDto';
import FormUtils from '@app/shared/utilities/form.utils';
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'app-form-pa-addcomb-fpb',
  templateUrl: './form-pa-addcomb-fpb.component.html',
  styleUrls: ['./form-pa-addcomb-fpb.component.scss']
})
export class FormPAaddcombFPBComponent implements OnInit {
  formId: '';
  other: '';
  default: any;
  private onDestroy$ = new Subject<void>();

  public onClose: Subject<boolean>;
  constructor(
    public modalRef: BsModalRef,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private formsService: FormsService
  ) {
    this.default = {
      medicalExpenseBenefits: 25000,
      workLossUpTo: 15000,
      workLossPerMonth: 1000,
      funeralExpenseBenefits: 1500,
      accidentalDeathBenefits: 5000,
      maximumTotalLimitAllBenefits: 50000,
      combFuneralExpenseBenefits: 2500,
      combAccidentalDeathBenefits: 10000,
    };
  }

  ngOnInit() {
    this.formsData.paAddCombFPBForm.reset();
    this.initCheckboxes();
    this.initValues();
  }

  initValues() {
    if (this.other) {
      let data = JSON.parse(this.other);
      this.patchFormValue(data);
    }

    this.setDefaultsAdded();
    this.setDefaultsComb();
  }

  setDefaultsAdded() {
    this.f.medicalExpenseBenefits.setValue(this.f.medicalExpenseBenefits.value ?? this.default.medicalExpenseBenefits);
    this.f.workLossUpTo.setValue(this.f.workLossUpTo.value ?? this.default.workLossUpTo);
    this.f.workLossPerMonth.setValue(this.f.workLossPerMonth.value ?? this.default.workLossPerMonth);
    this.f.funeralExpenseBenefits.setValue(this.f.funeralExpenseBenefits.value ?? this.default.funeralExpenseBenefits);
    this.f.accidentalDeathBenefits.setValue(this.f.accidentalDeathBenefits.value ?? this.default.accidentalDeathBenefits);
  }

  setDefaultsComb() {
    this.f.maximumTotalLimitAllBenefits.setValue(this.f.maximumTotalLimitAllBenefits.value ?? this.default.maximumTotalLimitAllBenefits);
    this.f.combFuneralExpenseBenefits.setValue(this.f.combFuneralExpenseBenefits.value ?? this.default.combFuneralExpenseBenefits);
    this.f.combAccidentalDeathBenefits.setValue(this.f.combAccidentalDeathBenefits.value ?? this.default.combAccidentalDeathBenefits);
  }


  get f() { return this.formsData.paAddCombFPBForm.controls; }

  saveForm() {
    Utils.blockUI();
    const addedFirstParty = this.f.addedFirstPartyBenefits.value ?? false;
    if (addedFirstParty)
      this.clearCombFPB();
    else
      this.clearAddedFPB();
    const otherInfo = new FormOtherInfoDTO(this.formsData.paAddCombFPBForm.value);
    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    otherInfo.addedFirstPartyBenefits = otherInfo.addedFirstPartyBenefits ?? false;
    otherInfo.combinationFirstPartyBenefits = otherInfo.combinationFirstPartyBenefits ?? false;
    
    this.formsService.updateStatePAaddcombFPB(otherInfo)
      .pipe(
        finalize(() => {
          Utils.unblockUI();
          this.modalRef.hide();
        })
      )
      .subscribe(data => {
        const riskForm = this.formsData.forms.find(f => f.id === data.id);
        riskForm.other = data.other;
      });
  }

  onCancel() {
    const riskForm = this.formsData.forms.find(f => f.form.id === this.formId);
    riskForm.isSelected = false;
    this.formsData.updateRiskForm(riskForm);
    this.modalRef.hide();
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
  }

  initCheckboxes() {
    this.f.addedFirstPartyBenefits.patchValue(false, {emitEvent: false, onlySelf: true});
    this.f.combinationFirstPartyBenefits.patchValue(false, {emitEvent: false, onlySelf: true});
    this.f.addedFirstPartyBenefits.valueChanges
    .takeUntil(this.onDestroy$)
    .subscribe((enabled) => {
      if (enabled) {
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'medicalExpenseBenefits');
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'workLossUpTo');
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'workLossPerMonth');
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'funeralExpenseBenefits');
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'accidentalDeathBenefits');
        
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'maximumTotalLimitAllBenefits');
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'combFuneralExpenseBenefits');
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'combAccidentalDeathBenefits');
        this.initValues();
        this.f.combinationFirstPartyBenefits.patchValue(false, {emitEvent: false, onlySelf: true});
      } else {
        this.f.addedFirstPartyBenefits.patchValue(true, {emitEvent: false, onlySelf: true});
        this.f.combinationFirstPartyBenefits.patchValue(false, {emitEvent: false, onlySelf: true});
      }
    });

    this.f.combinationFirstPartyBenefits.valueChanges
    .takeUntil(this.onDestroy$)
    .subscribe((enabled) => {
      if (enabled) {
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'maximumTotalLimitAllBenefits');
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'combFuneralExpenseBenefits');
        FormUtils.addRequiredValidator(this.formsData.paAddCombFPBForm, 'combAccidentalDeathBenefits');
        
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'medicalExpenseBenefits');
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'workLossUpTo');
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'workLossPerMonth');
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'funeralExpenseBenefits');
        FormUtils.clearValidator(this.formsData.paAddCombFPBForm, 'accidentalDeathBenefits');
        this.initValues();
        this.f.addedFirstPartyBenefits.patchValue(false, {emitEvent: false, onlySelf: true});
      } else {
        this.f.combinationFirstPartyBenefits.patchValue(true, {emitEvent: false, onlySelf: true});
        this.f.addedFirstPartyBenefits.patchValue(false, {emitEvent: false, onlySelf: true});
      }
    });
  }

  patchFormValue(data: any) {
    if (data.addedFirstPartyBenefits) {
      this.f.addedFirstPartyBenefits.patchValue(data.addedFirstPartyBenefits, {emitEvent: false, onlySelf: true});
      this.f.medicalExpenseBenefits.setValue(data.medicalExpenseBenefits);
      this.f.workLossUpTo.setValue(data.workLossUpTo);
      this.f.workLossPerMonth.setValue(data.workLossPerMonth);
      this.f.funeralExpenseBenefits.setValue(data.funeralExpenseBenefits);
      this.f.accidentalDeathBenefits.setValue(data.accidentalDeathBenefits);
    } else {
      this.f.combinationFirstPartyBenefits.patchValue(data.combinationFirstPartyBenefits, {emitEvent: false, onlySelf: true});
      this.f.maximumTotalLimitAllBenefits.setValue(data.maximumTotalLimitAllBenefits);
      this.f.combFuneralExpenseBenefits.setValue(data.combFuneralExpenseBenefits);
      this.f.combAccidentalDeathBenefits.setValue(data.combAccidentalDeathBenefits);
    }
  }

  clearAddedFPB() {
    this.f.medicalExpenseBenefits.setValue(null);
    this.f.workLossUpTo.setValue(null);
    this.f.workLossPerMonth.setValue(null);
    this.f.funeralExpenseBenefits.setValue(null);
    this.f.accidentalDeathBenefits.setValue(null);
  }

  clearCombFPB() {
    this.f.maximumTotalLimitAllBenefits.setValue(null);
    this.f.combFuneralExpenseBenefits.setValue(null);
    this.f.combAccidentalDeathBenefits.setValue(null);
  }

  resetFields(formGroup?: FormGroup, formControls?) {
    Object.keys(formControls).forEach(formControl => {
      if (formGroup.get(formControl).value == 0) {
        formGroup.get(formControl).setValue(null);
      }
    });
  }
}
