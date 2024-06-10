import { OnInit, EventEmitter, ViewChild } from '@angular/core';
import { Component } from '@angular/core';
import { BsModalRef, TabDirective, TabsetComponent } from 'ngx-bootstrap';
import { SubAgencyData } from '../../../../../../modules/management/data/sub-agency.data';
import { AgencyData } from '../../../../../../modules/management/data/agency.data';
import { CommissionFlatRate } from '../../../../../../shared/models/management/agency-management/agency.request.dto';
import { FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
import { AgencyLabelConstants } from '../../../../../../shared/constants/agency.label.constants';
import { CustomValidators } from '../../../../../../shared/validators/custom.validator';
import { GeneralValidationService } from '../../../../../../core/services/submission/validations/general-validation.service';
import { BaseClass } from '../../../../../../shared/base-class';
import { takeUntil } from 'rxjs/operators';

const Flat = 'F';
@Component({
  selector: 'app-sub-agency-modal',
  templateUrl: './sub-agency-modal.component.html',
  styleUrls: []
})
export class SubAgencyModalComponent extends BaseClass implements OnInit {
  @ViewChild('tab') tab: TabsetComponent;

  AgencyLabelConstants = AgencyLabelConstants;
  modalTitle: string;
  agencyTitle: string;
  agencyId: string;
  isEdit: boolean = false;
  agencyFlatCommissionClone: CommissionFlatRate[] = [];
  public event: EventEmitter<any> = new EventEmitter<any>();

  constructor(private modalRef: BsModalRef,
    public subAgencyData: SubAgencyData,
    private agencyData: AgencyData,
    private generalValidationService: GeneralValidationService
  ) {
    super();
   }

  get subAgencyFormGroup(): FormGroup {
    return this.subAgencyData.subAgencyFormGroup;
  }

  get isFlat(): boolean {
    return this.subAgencyFormGroup.get('commissionType').value === Flat;
  }

  get isUpdated(): boolean {
    return (this.subAgencyFormGroup.touched && this.subAgencyFormGroup.dirty) || this.subAgencyData.isAddressesUpdated;
  }

  get isValidExpirationDate(): boolean {
    return  moment(this.subAgencyFormGroup.get('licenseEffectiveDate').value?.singleDate?.jsDate).isSameOrBefore(this.subAgencyFormGroup.get('licenseExpirationDate').value?.singleDate?.jsDate);
  }

  get isSavingDisabled(): boolean {
    return !this.subAgencyFormGroup.valid || this.subAgencyData.isAddressListEmpty || !this.isValidExpirationDate;
  }

  ngOnInit(): void {
    this.subAgencyData.initializeForms();
    // this.isEftAvailableChildValidation();

    if (!this.isEdit) {
      this.agencyFlatComCloneUponCreate();
      this.subAgencyData.showingOfSaveUpdateNext(false);
    } else {
      this.agencyData.activeTab = this.AgencyLabelConstants.subAgencyTab;
      this.subAgencyData.showingOfSaveUpdateNext(true);
    }
  }

  closeDialog(): void {
    this.subAgencyData.resetSubAgencyUpdatesTrackers();
    this.modalRef.hide();
  }

  save(): void {
    this.closeDialog();
    this.subAgencyData.saveSubAgency();
    this.agencyData.subAgencyActiveTab = this.AgencyLabelConstants.subAgencyTab;
  }

  update(): void {
    const subAgencyId = localStorage.getItem('subAgencyId');
    this.closeDialog();
    this.subAgencyData.updateSubAgency(subAgencyId);
  }

  next() {
    this.subAgencyData.subAgencyFormGroup.markAllAsTouched();
    this.tab.tabs[1].active = true;
  }

  onSelect(data: TabDirective): void {
    this.agencyData.subAgencyActiveTab = data.heading;
    this.subAgencyData.subAgencyFormGroup.markAllAsTouched();
    this.subAgencyData.showingOfSaveUpdateNext(this.isEdit ? true : false);
  }

  agencyFlatComCloneUponCreate() {
    this.agencyFlatCommissionClone = this.agencyData.agencyFlatCommission;
    this.agencyFlatCommissionClone.forEach(data => {
      delete data.agencyId;
      delete data.commmissionGroupId;
      delete data.createdDate;
      delete data.id;
      delete data.removedProcessDate;
      delete data.subAgencyId;
      this.subAgencyData.subAgencyFlatCommission.push(data);
    });
  }

  isEftAvailableChildValidation() {
    this.subAgencyData.subAgencyFormGroup.get('isEftAvailable').valueChanges.pipe(takeUntil(this.stop$)).subscribe(value => {
      if (!value) {
        this.generalValidationService.clearValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftAccountNumber');
        this.generalValidationService.clearValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftAccountType');
        this.generalValidationService.clearValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftRoutingNumber');
        this.generalValidationService.clearValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftCommissionEmail');
      } else {
        this.generalValidationService.resetValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftAccountNumber', [Validators.required]);
        this.generalValidationService.resetValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftAccountType', [Validators.required]);
        this.generalValidationService.resetValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftRoutingNumber', [Validators.required]);
        this.generalValidationService.resetValidatorFormControl(this.subAgencyData.subAgencyFormGroup, 'eftCommissionEmail', [Validators.required, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$'), CustomValidators.spaceValidator]);
      }
    });

  }
}