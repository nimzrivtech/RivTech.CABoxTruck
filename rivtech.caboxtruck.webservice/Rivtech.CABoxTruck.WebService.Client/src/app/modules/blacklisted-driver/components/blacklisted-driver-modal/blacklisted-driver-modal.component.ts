import {Component, EventEmitter, OnInit} from '@angular/core';
import {BsModalRef, BsModalService} from 'ngx-bootstrap';
import {AddDriverVerificationModalComponent} from './add-blacklisted-driver-modal/add-driver-verification-modal.component';
import {BlacklistedDriverData} from '../../data/blacklisted-driver.data';
import {BlacklistedDriverDTO} from '../../../../shared/models/blacklisted-driver/blacklisted-driver';
import { TableConstants } from '../../../../shared/constants/table.constants';

@Component({
  selector: 'app-driver-verification-modal',
  templateUrl: './blacklisted-driver-modal.component.html',
  styleUrls: ['./blacklisted-driver-modal.component.scss']
})
export class BlacklistedDriverModalComponent implements OnInit {
  title: any;

  driverName = '';
  licenseNumber = '';
  searchFilter = '';
  TableConstants = TableConstants;
  showAnotherModal: EventEmitter<any> = new EventEmitter();

  constructor(public bsModalRef: BsModalRef,
              public blacklistedDriverData: BlacklistedDriverData) { }

  ngOnInit() {
    this.blacklistedDriverData.fetchDrivers();
    switch (this.blacklistedDriverData.mode) {
      case 'Add': // moved to last page
        this.blacklistedDriverData.setPage(this.blacklistedDriverData.currentPage, true);
        break;
      case 'Edit': // remain on current page
        this.blacklistedDriverData.searchDriver('', '');
        this.blacklistedDriverData.setPage(this.blacklistedDriverData.currentPage, false);
        break;
      default: // reset to page 1
        this.blacklistedDriverData.searchFilter = '';
        this.blacklistedDriverData.searchDriver('', '');
        this.blacklistedDriverData.setPage(1, false);
        break;
    }
  }

  setPage(page: number) {
    this.blacklistedDriverData.setPage(page);
  }

  onAddDriverClick(): void {
    const initialState = {
      title: 'Manage Blacklisted Driver',
      mode: 'Add',
    };
    this.blacklistedDriverData.mode = initialState.mode;
    this.showAnotherModal.emit(initialState);
    this.bsModalRef.hide();
  }

  onEditDriverClick(driver: BlacklistedDriverDTO): void {
    const initialState = {
      title: 'Manage Blacklisted Driver',
      selectedDriver: driver,
      mode: 'Edit',
    };
    this.blacklistedDriverData.mode = initialState.mode;
    this.showAnotherModal.emit(initialState);
    this.bsModalRef.hide();
  }

  onSearchDriverClick(obj) {
    this.blacklistedDriverData.searchDriver('', '');
    this.blacklistedDriverData.setPage(this.blacklistedDriverData.currentPage);
    obj?.preventDefault();
    if (obj) {
      this.blacklistedDriverData.searchFilter = this.searchFilter;
      this.blacklistedDriverData.setPage(1, false);
    }
    return false;
  }

  onClose(): void {
    this.blacklistedDriverData.mode = '';
    this.bsModalRef.hide();
  }
}
