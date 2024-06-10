import { Component, Input, OnInit } from '@angular/core';
import { TableConstants } from '../../../../../shared/constants/table.constants';
import { AgencyData } from '../../../../../modules/management/data/agency.data';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { AgencyFlatCommissionModalComponent } from './agency-flat-commission-modal/agency-flat-commission-modal.component';
import NotifUtils from '../../../../../shared/utilities/notif-utils';
import { CommissionFlatRate } from '../../../../../shared/models/management/agency-management/agency.request.dto';
import { AuthService } from 'app/core/services/auth.service';
import { SubAgencyData } from '../../../../../modules/management/data/sub-agency.data';
@Component({
  selector: 'app-agency-flat-commissions',
  templateUrl: './agency-flat-commissions.component.html',
  styleUrls: ['./agency-flat-commissions.component.scss']
})
export class AgencyFlatCommissionsComponent implements OnInit {
  @Input() flatCommissionList: CommissionFlatRate[];
  @Input() flatCommissionRowId: number;
  @Input() flatCommissionHeader: string[];
  @Input() isLoading: boolean;

  isFlatHidden: boolean = false;
  TableConstants = TableConstants;
  genericLabel = GenericLabel;

  constructor(public agencyData: AgencyData
              , public subAgencyData: SubAgencyData
              , private modalRef: BsModalRef
              , private modalService: BsModalService
              , private authService: AuthService) { }

  ngOnInit() {
  }

  toggleFlatCommission(): void {
    this.isFlatHidden = !this.isFlatHidden;
  }

  addFlatCommission(): void {
    this.flatCommissionRowId = this.flatCommissionList.length === 0 ? this.flatCommissionRowId : this.flatCommissionRowId + 1;
    const initialState = {
      modalTitle: 'Flat Commissions',
      flatCommissionList: this.flatCommissionList,
      rowId: this.flatCommissionRowId
    };

    this.modalRef = this.showFlatCommissionModal(AgencyFlatCommissionModalComponent, initialState);

  }

  editFlatCommission(item?): void {
    const initialState = {
      modalTitle: 'Flat Commissions',
      flatCommissionList: this.flatCommissionList,
      flatCommissionItem: item,
      isEdit: true
    };

    this.modalRef = this.showFlatCommissionModal(AgencyFlatCommissionModalComponent, initialState);
  }

  showFlatCommissionModal(component?, initialState?): BsModalRef {
    return this.modalService.show(component, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
  }

  deleteFlatCommission(commissionFlatRateItem?: CommissionFlatRate): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete Row #${commissionFlatRateItem.rowId + 1}?`,
      () => {
        this.flatCommissionList.forEach((item, index) => {
          if (item.id === undefined && item.rowId === commissionFlatRateItem.rowId) {
            this.flatCommissionList.splice(index, 1);
          }

          if (item.id !== undefined && item.id === commissionFlatRateItem.id) {
            item.removedProcessDate = this.authService.getCustomDate();
          }
        });

        let ctr = 0;
        this.flatCommissionList.forEach(cfr => {
          if (cfr.removedProcessDate === null || cfr.removedProcessDate === undefined) {
            cfr.rowId = ctr;
            ctr++;
          }
        });
        this.flatCommissionRowId = ctr - 1  ;
        if (!this.subAgencyData.isSubAgencyTab) {
          this.agencyData.isFlatCommisionsUpdated = true;
        } else {
          this.subAgencyData.isFlatCommisionsUpdated = true;
        }
      },
      () => { }
    );
  }

  filter(item?: CommissionFlatRate): boolean {
    return item.removedProcessDate === null || item.removedProcessDate === undefined;
  }

}
