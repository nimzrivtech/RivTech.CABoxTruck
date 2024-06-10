import { Component, OnInit } from '@angular/core';
import { ErrorMessageConstant } from '../../../../../../shared/constants/error-message.constants';
import { AgencyData } from '../../../../../../modules/management/data/agency.data';
import { AgencyLabelConstants } from '../../../../../../shared/constants/agency.label.constants';
import { BsModalRef } from 'ngx-bootstrap';
import { CommissionFlatRate } from '../../../../../../shared/models/management/agency-management/agency.request.dto';
import { Subject } from 'rxjs';
import * as moment from 'moment';
import { FormGroup } from '@angular/forms';
import { SubAgencyData } from '../../../../../../modules/management/data/sub-agency.data';
import { ToastrService } from 'ngx-toastr';
import { takeUntil } from 'rxjs/operators';
import { BaseClass } from '../../../../../../shared/base-class';

@Component({
  selector: 'app-agency-flat-commission-modal',
  templateUrl: './agency-flat-commission-modal.component.html',
  styleUrls: ['./agency-flat-commission-modal.component.scss']
})
export class AgencyFlatCommissionModalComponent extends BaseClass implements OnInit  {
  AgencyLabelConstants = AgencyLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;

  modalTitle: string;

  flatCommissionList: CommissionFlatRate[];
  flatCommissionItem: CommissionFlatRate;

  rowId: number;

  isEdit: boolean;
  isNewBusinessInvalid: boolean = false;
  isRenewalBusinessInvalid: boolean = false;

  public onClose: Subject<boolean>;

  constructor(public agencyData: AgencyData
    , public bsModalRef: BsModalRef
    , public subAgencyData: SubAgencyData
    , private toastr: ToastrService) {
      super();
     }

  get flatCommission(): FormGroup {
    return this.agencyData.flatCommissionFormGroup;
  }

  get isValid(): boolean {
    return this.flatCommission.valid && this.flatCommission.touched && this.flatCommission.dirty && this.isValidExpirationDate 
    && !this.isNewBusinessInvalid && !this.isRenewalBusinessInvalid;
  }

  get isValidExpirationDate(): boolean {
    return moment(this.flatCommission.get('effectiveDate').value?.singleDate?.jsDate).isSameOrBefore(this.flatCommission.get('expirationDate').value?.singleDate?.jsDate);
  }

  get flatCommissionData(): CommissionFlatRate {
    this.rowId = this.isEdit ? this.flatCommissionItem?.rowId : this.flatCommissionList.length;

    const flatCommission: CommissionFlatRate = {
      nbCommissionPercent: +this.flatCommission.get('newBusiness').value,
      rnCommissionPercent: +this.flatCommission.get('renewalBusiness').value,
      effectiveDate: this.flatCommission.get('effectiveDate').value.singleDate?.jsDate,
      expirationDate: this.flatCommission.get('expirationDate').value.singleDate?.jsDate,
      rowId: this.rowId
    };

    return flatCommission;
  }

  ngOnInit() {
    this.onClose = new Subject();
    if (this.isEdit) {
      this.mapEditData(this.flatCommissionItem);
    }
    this.flatCommission.markAllAsTouched();
    this.newAndRenewalBusinessValidation();
  }

  closeDialog(): void {
    this.flatCommission.reset();
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

  save(): void {
    if (this.checkIsFlatComExisting(this.flatCommissionData)){
      this.toastr.error('Flat Commission already exist.', 'Oops..');
      return;
    }

    if (!this.isEdit) {
      this.processAdd(this.flatCommissionData);
    } else {
      this.processEdit();
    }

    if (!this.subAgencyData.isSubAgencyTab) {
      this.agencyData.isFlatCommisionsUpdated = true;
    } else {
      this.subAgencyData.isFlatCommisionsUpdated = true;
    }
    this.onClose.next(true);
    this.flatCommission.reset();
    this.bsModalRef.hide();
  }

  mapEditData(flatCommissionData: CommissionFlatRate): void {
    this.flatCommission.get('newBusiness').setValue(flatCommissionData.nbCommissionPercent);
    this.flatCommission.get('renewalBusiness').setValue(flatCommissionData.rnCommissionPercent);
    this.flatCommission.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(flatCommissionData.effectiveDate) } });
    this.flatCommission.get('expirationDate').setValue({ isRange: false, singleDate: { jsDate: new Date(flatCommissionData.expirationDate) } });
  }

  processAdd(flatCommission: CommissionFlatRate): void {
    this.flatCommissionList.push(flatCommission);
  }

  processEdit(): void {
    if (this.flatCommissionItem.id) { // use item.id => if existing in DB edit
      this.flatCommissionList = this.flatCommissionList.map(data => {
        if (data.id === this.flatCommissionItem.id) {
          this.setFlatCommission(data);
        }
        return data;
      });
    } else {
      this.flatCommissionList = this.flatCommissionList.map(data => {
        if (data.rowId === this.flatCommissionItem.rowId) { // use item.rowId => if newly added edit
          this.setFlatCommission(data);
        }
        return data;
      });
    }
  }

  setFlatCommission(data): void {
    data.nbCommissionPercent = +this.flatCommission.get('newBusiness').value;
    data.rnCommissionPercent = +this.flatCommission.get('renewalBusiness').value;
    data.effectiveDate = this.flatCommission.get('effectiveDate').value.singleDate?.jsDate;
    data.expirationDate = this.flatCommission.get('expirationDate').value.singleDate?.jsDate;
  }

  checkIsFlatComExisting(flatCommission: CommissionFlatRate): boolean {
    const effectiveDate = this.getLocaleDateString(new Date(flatCommission.effectiveDate));
    const expirationDate = this.getLocaleDateString(new Date(flatCommission.expirationDate));

    return this.flatCommissionList.some(add => {
      const efDate = this.getLocaleDateString(new Date(add.effectiveDate));
      const exDate = this.getLocaleDateString(new Date(add.expirationDate));

      return flatCommission.rowId !== add.rowId &&
            flatCommission.nbCommissionPercent === add.nbCommissionPercent &&
            flatCommission.rnCommissionPercent === add.rnCommissionPercent &&
            effectiveDate === efDate &&
            expirationDate === exDate;
    });
  }

  getLocaleDateString(date: Date): string {
    return date.toLocaleDateString();
  }

  newAndRenewalBusinessValidation() {
    this.flatCommission.get('newBusiness').valueChanges.pipe(takeUntil(this.stop$)).subscribe(value => {
      if (value > 100)  {
        this.isNewBusinessInvalid = true;
      } else {
        this.isNewBusinessInvalid = false;
      }
    })

    this.flatCommission.get('renewalBusiness').valueChanges.pipe(takeUntil(this.stop$)).subscribe(value => {
      if (value > 100)  {
        this.isRenewalBusinessInvalid = true;
      } else {
        this.isRenewalBusinessInvalid = false;
      }
    })
  }
  
}
