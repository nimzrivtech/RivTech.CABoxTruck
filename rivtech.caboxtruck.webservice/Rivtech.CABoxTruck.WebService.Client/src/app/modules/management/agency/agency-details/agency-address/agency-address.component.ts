import { Component, Input, OnInit } from '@angular/core';
import { BaseClass } from '../../../../../shared/base-class';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { AgencyData } from '../../../../../modules/management/data/agency.data';
import { TableConstants } from '../../../../../shared/constants/table.constants';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import { AgencyAddressModalComponent } from './agency-address-modal/agency-address-modal.component';
import { GenericAddressData } from '../../../../../shared/components/generic-address/data/generic-address.data';
import NotifUtils from '../../../../../shared/utilities/notif-utils';
import { AgencyAddressEntityAddressDTO } from '../../../../../shared/models/management/agency-address.dto';
import { SubAgencyData } from '../../../../../modules/management/data/sub-agency.data';
import { AgencyLabelConstants } from '../../../../../shared/constants/agency.label.constants';
import { AgentData } from '../../../../../modules/management/data/agent.data';

import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-agency-address',
  templateUrl: './agency-address.component.html',
  styleUrls: ['./agency-address.component.scss']
})
export class AgencyAddressComponent extends BaseClass implements OnInit {
  isFlatHidden: boolean = false;
  TableConstants = TableConstants;
  genericLabel = GenericLabel;
  AgencyLabelConstants = AgencyLabelConstants;
  subAgencyId: string;

  @Input() addressList: AgencyAddressEntityAddressDTO[];
  @Input() addressHeaders: string[];
  @Input() addressRowId: number;
  @Input() isLoading: boolean;

  constructor(
    public agencyData: AgencyData,
    public subAgencyData: SubAgencyData,
    private modalService: BsModalService,
    private modalRef: BsModalRef,
    private genericAddressData: GenericAddressData,
    private agentData: AgentData,
    private authService: AuthService) {
    super();
  }

  get isEdit() {
    return this.agencyData.agencyId !== null;
  }

  get isEditSubAgency() {
    this.subAgencyId = localStorage.getItem('subAgencyId');
    return this.subAgencyId !== '';
  }

  get isEditAgent() {
    this.subAgencyId = localStorage.getItem('agentId');
    return this.subAgencyId !== '';
  }

  ngOnInit() {
  }

  toggleAddress(): void {
    this.isFlatHidden = !this.isFlatHidden;
  }

  addAddress(): void {
    this.addressRowId = this.addressList.length === 0 ? this.addressRowId : this.addressRowId + 1;
    const initialState = {
      modalTitle: 'Address',
      buttonText: 'Save',
      addressList: this.addressList,
      rowId: this.addressRowId
    };

    this.modalRef = this.showAddressModal(AgencyAddressModalComponent, initialState);
  }

  editAddress(item?): void {
    const initialState = {
      modalTitle: 'Address',
      addressList: this.addressList,
      addressItem: item,
      isEdit: true
    };

    this.modalRef = this.showAddressModal(AgencyAddressModalComponent, initialState);
    // this.modalRef.content.onClose.subscribe(result => {
    //   switch (result) {
    //     case true:
    //       this.flatCommissionList = this.agencyData.agencyFlatCommission;
    //       break;
    //     case false:
    //       break;
    //   }
    // });
  }

  showAddressModal(component?, initialState?): BsModalRef {
    return this.modalService.show(component, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
  }

  deleteAddress(addressItem: AgencyAddressEntityAddressDTO): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete ${addressItem.addressTypeDescription}?`,
      () => {
        this.addressList.forEach((item, index) => {
          if (item.id === undefined && item.rowId === addressItem.rowId) {
            this.addressList.splice(index, 1);
          }

          if (item.id !== undefined && item.id === addressItem.id) {
            item.removedProcessDate = this.authService.getCustomDate();
          }
          
          switch (this.agencyData.activeTab) {
            case this.AgencyLabelConstants.subAgencyTab:
              if (this.addressList.length === 0) {
                this.subAgencyData.showingOfSaveUpdateNext(this.isEditSubAgency ? true : false);
              } else {
                this.subAgencyData.showingOfSaveUpdateNext(this.isEditSubAgency ? true : false);
              }
              break;
            case this.AgencyLabelConstants.addressTab:
              if (this.addressList.length === 0) {
                this.agencyData.showingOfSaveUpdateNext(this.isEdit ? true : false);
              } else {
                this.agencyData.showingOfSaveUpdateNext(this.isEdit ? true : false);
              }
              break;
            }
        });

        this.updateAddressTracker(this.agencyData.activeTab);
      },
      () => { }
    );
  }

  filter(item?: AgencyAddressEntityAddressDTO): boolean {
    item.addressTypeDescription = this.genericAddressData?.addressTypeValue(item.addressTypeId);
    return item.removedProcessDate === null || item.removedProcessDate === undefined;
  }

  updateAddressTracker(activeTab: string): void {
    switch (activeTab) {
      case AgencyLabelConstants.addressTab:
        this.agencyData.isAddressesUpdated = true;
        this.agencyData.showingOfSaveUpdateNext(this.isEdit ? true : false);
        break;
      case AgencyLabelConstants.subAgencyTab:
        this.subAgencyData.isAddressesUpdated = true;
        this.subAgencyData.showingOfSaveUpdateNext(this.isEditSubAgency ? true : false);
        break;
      case AgencyLabelConstants.agentTab:
        this.agentData.isAddressesUpdated = true;
        break;
      default:
        this.agencyData.isAddressesUpdated = true;
    }
  }
}
