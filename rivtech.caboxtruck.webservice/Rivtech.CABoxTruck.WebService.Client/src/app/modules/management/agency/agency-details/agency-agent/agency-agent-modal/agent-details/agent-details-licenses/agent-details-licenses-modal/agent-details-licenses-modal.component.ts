import { Component, OnInit } from '@angular/core';
import { AgentData } from '../../../../../../../../../modules/management/data/agent.data';
import { AgentLicenseDTO } from '../../../../../../../../../shared/models/data/dto/agent/agentuser-response.dto';
import { BsModalRef } from 'ngx-bootstrap';
import { AgentLabelConstants } from '../../../../../../../../../shared/constants/agent.label.constants';
import { ErrorMessageConstant } from '../../../../../../../../../shared/constants/error-message.constants';
import { Subject } from 'rxjs-compat';
import { ProgramStateData } from '../../../../../../../../../modules/management/data/program-state.data';
import { GenericLabel } from '../../../../../../../../../shared/constants/generic.labels.constants';
import { FormGroup } from '@angular/forms';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-agent-details-licenses-modal',
  templateUrl: './agent-details-licenses-modal.component.html',
  styleUrls: ['./agent-details-licenses-modal.component.scss']
})
export class AgentDetailsLicensesModalComponent implements OnInit {
  AgentLabelConstants = AgentLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;
  GenericLabel = GenericLabel;

  modalTitle: string;

  licenseList: AgentLicenseDTO[];
  licenseItem: AgentLicenseDTO;

  rowId: number;

  isEdit: boolean;

  public onClose: Subject<boolean>;

  constructor(public agentData: AgentData
    , public bsModalRef: BsModalRef
    , public programState: ProgramStateData
    , private toastr: ToastrService
    , private authService: AuthService) { }

  get agentLicenseFormGroup(): FormGroup {
    return this.agentData.agentLicenseFormGroup;
  }

  get isValid() {
    return this.agentLicenseFormGroup.valid;
  }

  get isUpdated(): boolean {
    return this.agentLicenseFormGroup.touched && this.agentLicenseFormGroup.dirty;
  }

  get isValidExpirationDate(): boolean {
    return moment(this.agentLicenseFormGroup.get('licenseEffectiveDate').value?.singleDate?.jsDate).isSameOrBefore(this.agentLicenseFormGroup.get('licenseExpirationDate').value?.singleDate?.jsDate);
  }

  get isSavingDisabled(): boolean {
    return !this.isValid || !this.isUpdated || !this.isValidExpirationDate;
  }

  get license(): AgentLicenseDTO {
    this.rowId = this.isEdit ? this.licenseItem?.rowId : this.licenseList.length;

    const license: AgentLicenseDTO = {
      stateCode: this.agentLicenseFormGroup.get('stateCode').value,
      licenseNumber: this.agentLicenseFormGroup.get('licenseNumber').value,
      licenseEffectiveDate:  this.agentLicenseFormGroup.get('licenseEffectiveDate').value.singleDate?.jsDate,
      licenseExpirationDate: this.agentLicenseFormGroup.get('licenseExpirationDate').value.singleDate?.jsDate,
      rowId: this.rowId,
      removedProcessDate: null
    };

    return license;
  }

  ngOnInit() {
    this.onClose = new Subject();
    if (this.isEdit) {
      this.mapEditData(this.licenseItem);
    }
  }

  closeDialog(): void {
    this.agentLicenseFormGroup.reset();
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

  save(): void {
    if (this.checkIsLicenseExisting(this.license)){
      this.toastr.error('License already exist.', 'Oops..');
      return;
    }

    if (!this.isEdit) {
      this.processAdd(this.license);
    } else {
      this.processEdit();
    }

    this.agentData.isAgentLicensesUpdated = true;
    this.onClose.next(true);
    this.agentLicenseFormGroup.reset();
    this.bsModalRef.hide();
  }

  mapEditData(licenseData: AgentLicenseDTO): void {
    this.agentLicenseFormGroup.get('stateCode').setValue(licenseData.stateCode);
    this.agentLicenseFormGroup.get('licenseNumber').setValue(licenseData.licenseNumber);
    // this.agentLicenseFormGroup.get('isSurplusLines').setValue(licenseData.isSurplusLines ?? true);
    this.agentLicenseFormGroup.get('licenseEffectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(licenseData.licenseEffectiveDate) } });
    this.agentLicenseFormGroup.get('licenseExpirationDate').setValue({ isRange: false, singleDate: { jsDate: new Date(licenseData.licenseExpirationDate) } });
    this.agentLicenseFormGroup.markAllAsTouched();
  }

  processAdd(license: AgentLicenseDTO): void {
    this.licenseList.push(license);
  }

  processEdit(): void {
    if (this.licenseItem.id) { // use item.id => if existing in DB edit
      this.licenseList = this.licenseList.map(data => {
        if (data.id === this.licenseItem.id) {
          this.setLicenses(data);
        }
        return data;
      });
    } else {
      this.licenseList = this.licenseList.map(data => {
        if (data.rowId === this.licenseItem.rowId) { // use item.rowId => if newly added edit
          this.setLicenses(data);
        }
        return data;
      });
    }
  }

  setLicenses(data?): void {
    data.stateCode = this.agentLicenseFormGroup.get('stateCode').value;
    data.licenseNumber = this.agentLicenseFormGroup.get('licenseNumber').value;
    data.licenseEffectiveDate = this.agentLicenseFormGroup.get('licenseEffectiveDate').value.singleDate?.jsDate;
    data.licenseExpirationDate = this.agentLicenseFormGroup.get('licenseExpirationDate').value.singleDate?.jsDate;
  }

  checkIsLicenseExisting(license: AgentLicenseDTO): boolean {
    const effectiveDate = this.getLocaleDateString(new Date(license.licenseEffectiveDate));
    const expirationDate = this.getLocaleDateString(new Date(license.licenseExpirationDate));

    return this.licenseList.some(add => {
      const efDate = this.getLocaleDateString(new Date(add.licenseEffectiveDate));
      const exDate = this.getLocaleDateString(new Date(add.licenseExpirationDate));

      return license.rowId !== add.rowId &&
            license.stateCode === add.stateCode &&
            license.licenseNumber === add.licenseNumber &&
            effectiveDate === efDate &&
            expirationDate === exDate &&
            add.removedProcessDate === null;
    });
  }

  getLocaleDateString(date: Date): string {
    return date.toLocaleDateString();
  }
}
