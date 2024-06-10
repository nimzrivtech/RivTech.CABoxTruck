import {Component, EventEmitter, OnInit} from '@angular/core';
import {BsModalRef, BsModalService} from 'ngx-bootstrap';
import Utils from '../../../../../shared/utilities/utils';
import {finalize} from 'rxjs/operators';
import {BlacklistedDriverData} from '@app/modules/blacklisted-driver/data/blacklisted-driver.data';
import {BlacklistedDriverDTO} from '@app/shared/models/blacklisted-driver/blacklisted-driver';
import NotifUtils from '@app/shared/utilities/notif-utils';

@Component({
  selector: 'app-add-driver-verification-modal',
  templateUrl: './add-driver-verification-modal.component.html',
  styleUrls: ['./add-driver-verification-modal.component.scss']
})

export class AddDriverVerificationModalComponent implements OnInit {
  title: string;
  selectedDriver: BlacklistedDriverDTO;
  mode = 'Add';
  modalRef: BsModalRef | null;

  onClose: EventEmitter<any> = new EventEmitter();

  constructor(public bsModalRef: BsModalRef, private modalService: BsModalService,
              public blacklistedDriverData: BlacklistedDriverData) { }

  get form() { return this.fg; }
  get fg() { return this.blacklistedDriverData.blackListedDriverForm; }
  get fc() { return this.blacklistedDriverData.blackListedDriverForm.controls; }

  ngOnInit() {
    this.blacklistedDriverData.initializeFormFields();
    if (this.selectedDriver != null) {
      this.blacklistedDriverData.populateFields(this.selectedDriver);
    }
  }

  onCancel(): void {
    this.blacklistedDriverData.mode = ''; // reset mode
    this.bsModalRef.hide();
    this.onClose.emit();
  }

  onSubmit() {
    const blackListedDriver = this.blacklistedDriverData.blackListedDriverForm.value;
    this.update(blackListedDriver);
  }

  update(blackListedDriver: BlacklistedDriverDTO): void {
    Utils.blockUI();

    this.blacklistedDriverData.update(blackListedDriver).pipe(finalize(() => {
      Utils.unblockUI();
      this.blacklistedDriverData.blacklistedDrivers.sort((a, b) => a?.firstName.localeCompare(b?.firstName)).push(blackListedDriver);
      this.onClose.emit();
      this.bsModalRef.hide();
    }))
      .subscribe(() => {

      }, (error) => {
        console.error(error);
      });
  }
}
