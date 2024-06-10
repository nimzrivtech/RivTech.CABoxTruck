import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { CurrencyPipe } from '@angular/common';
import { GenericAddressData } from '../../../../../../shared/components/generic-address/data/generic-address.data';
import { GenericAddressComponent } from '../../../../../../shared/components/generic-address/generic-address.component';
import { BaseClass } from '../../../../../../shared/base-class';
import { ZipCodeService } from '../../../../../../core/services/management/zip-code.service';
import { EntityAddressDTO } from '../../../../../../shared/models/management/generic-address.model';
import { takeUntil } from 'rxjs/operators';
import NotifUtils from '../../../../../../shared/utilities/notif-utils';
import { UserData } from '../../../../../../modules/management/data/user.data';
import { ErrorMessageConstant } from '../../../../../../shared/constants/error-message.constants';

@Component({
  selector: 'app-user-address',
  templateUrl: './user-address.component.html',
  styleUrls: ['./user-address.component.scss'],
  providers: [CurrencyPipe]
})
export class UserAddressComponent extends BaseClass implements OnInit {
  modalRef: BsModalRef;

  constructor(
    public genericAddressData: GenericAddressData,
    private userData: UserData,
    private modalService: BsModalService,
    private zipCodeService: ZipCodeService) {
    super();
  }

  ngOnInit(): void {

  }

  showAddAddress(): void {
    const initialState = {
      modalTitle: 'Add User Address',
      buttonText: 'Add'
    };

    this.modalRef = this.modalService.show(GenericAddressComponent, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
    });

    this.modalRef.content.event.subscribe((res) => {
      this.genericAddressData.addTableItem(res.data);
    });
  }

  editAddress(rowId: any, tableRow: string): void {
    this.genericAddressData[tableRow].forEach((item) => {
      if (item.id === rowId) {
        this.showEditAddress(this.genericAddressData.addressList[rowId - 1], rowId);
      } else {
        item.edit = false;
      }
    });
  }

  showEditAddress(data: EntityAddressDTO, rowId: any): void {
    const initialState = {
      modalTitle: 'Edit User Address',
      buttonText: 'Save',
      entityAddressModel: data,
      isAdd: false
    };

    this.modalRef = this.modalService.show(GenericAddressComponent, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
    });

    this.modalRef.content.event.pipe(takeUntil(this.stop$)).subscribe((res) => {
      this.updateTableItem(res.data, rowId);
    });
  }

  updateTableItem(row: EntityAddressDTO, rowId: any): void {
    // this.interestSavingData.markInterestForSaving();
    this.genericAddressData.addressTableRows.forEach((item) => {
      if (item.id === rowId) {
        item.tr[0].value = row.addressTypeId;
        item.tr[0].display = this.genericAddressData.addressTypeValue(row.addressTypeId);
        this.zipCodeService.getZipCode(String(row.address[0].cityZipCodeID)).pipe(takeUntil(this.stop$)).subscribe(result => {
          const cityZipCode: any = JSON.parse(result.toString());
          cityZipCode.forEach(add => {
            if (add.city === row.address[0].city) {
              item.tr[1].value = add.city;
              item.tr[1].display = add.city;
              item.tr[2].value = add.stateFullName;
              item.tr[2].display = add.stateFullName;
              item.tr[3].value = row.address[0].cityZipCodeID;
              item.tr[3].display = String(row.address[0].cityZipCodeID);
            }
          });
        }, error => {
          console.log(error);
          NotifUtils.showError(ErrorMessageConstant.zipCodeNotFoundContactUWErrorMessage);
        });
      }
    });

    this.genericAddressData.addressList.forEach((item, index) => {
      if (index === rowId - 1) {
        item.addressTypeId = row.addressTypeId;
        item.effectiveDate = row.effectiveDate;
        item.expirationDate = row.expirationDate;
        item.address[0].streetAddress1 = row.address[0].streetAddress1;
        item.address[0].streetAddress2 = row.address[0].streetAddress2;
        item.address[0].cityZipCodeID = row.address[0].cityZipCodeID;
        item.address[0].zipCode = row.address[0].zipCode;
        item.address[0].city = row.address[0].city;
        item.address[0].stateCode = row.address[0].stateCode;
        item.address[0].isGarageIndoor = row.address[0].isGarageIndoor;
        item.address[0].isGarageOutdoor = row.address[0].isGarageOutdoor;
        item.address[0].isGarageFenced = row.address[0].isGarageFenced;
        item.address[0].isGarageLighted = row.address[0].isGarageLighted;
        item.address[0].isGarageWithSecurityGuard = row.address[0].isGarageWithSecurityGuard;
      }
    });
  }

  deleteTableItem(rowId: any, tableRow: string): void {
    this.genericAddressData[tableRow].forEach((item, index) => {
      if (item.id === rowId) {
        this.genericAddressData[tableRow].splice(index, 1);
      }
    });

    this.genericAddressData.addressList.forEach((item, index) => {
      if (index === rowId - 1) {
        this.genericAddressData.addressList.splice(index, 1);
        if (item.id != null) {
          this.userData.deleteAddresses.push(item.id);
        }
      }
    });
  }

  generateId(length) {
    let result = '';
    const characters =
      'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;

    for (let i = 0; i < length; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    return result;
  }
}
