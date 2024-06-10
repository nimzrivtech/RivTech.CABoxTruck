import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { GenericLabel } from '../../../../../../../../shared/constants/generic.labels.constants';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { AgentData } from '../../../../../../../../modules/management/data/agent.data';
import { AgentLabelConstants } from '../../../../../../../../shared/constants/agent.label.constants';
import { AgentLicenseDTO } from '../../../../../../../../shared/models/data/dto/agent/agentuser-response.dto';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { AgentDetailsLicensesModalComponent } from './agent-details-licenses-modal/agent-details-licenses-modal.component';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { Subject } from 'rxjs-compat';
import { AuthService } from 'app/core/services/auth.service';


@Component({
  selector: 'app-agent-details-licenses',
  templateUrl: './agent-details-licenses.component.html',
  styleUrls: ['./agent-details-licenses.component.scss']
})
export class AgentDetailsLicensesComponent implements OnInit, OnChanges {

  @Input() licenseList: AgentLicenseDTO[];
  @Input() licenseRowId: number;
  @Input() licenseListHeader: string[];
  @Input() isLoading: boolean;

  AgentLabelConstants = AgentLabelConstants;
  TableConstants = TableConstants;
  genericLabel = GenericLabel;

  licenseListLocal: AgentLicenseDTO[] = [];

  public onClose: Subject<boolean>;

  constructor(public agentData: AgentData
            , private modalService: BsModalService
            , private modalRef: BsModalRef
            , private authService: AuthService) { }

  ngOnInit() {
  }

  ngOnChanges() {
    this.licenseListLocal = this.licenseList.filter(x =>  x.removedProcessDate === null || x.removedProcessDate === undefined);
  }

  addLicense(): void {
    this.licenseRowId = this.licenseListLocal.length === 0 ? this.licenseRowId : this.licenseRowId + 1;
    const initialState = {
      modalTitle: 'Add License',
      licenseList: this.licenseList,
      rowId: this.licenseRowId
    };

    this.modalRef = this.showAgentLicenseModal(AgentDetailsLicensesModalComponent, initialState);

    this.modalRef.content.onClose.subscribe(result => {
      switch (result) {
        case true:
          this.licenseListLocal = this.licenseList.filter(x => x.removedProcessDate === null || x.removedProcessDate === undefined);
          this.remapLicenseList();
          break;
        case false:
          break;
      }
    });
  }

  editLicense(item: AgentLicenseDTO): void {
    const initialState = {
      modalTitle: 'Edit License',
      licenseList: this.licenseList,
      licenseItem: item,
      isEdit: true
    };

    this.modalRef = this.showAgentLicenseModal(AgentDetailsLicensesModalComponent, initialState);

    this.modalRef.content.onClose.subscribe(result => {
      switch (result) {
        case true:
          this.licenseListLocal = this.licenseList.filter(x =>  x.removedProcessDate === null || x.removedProcessDate === undefined);
          this.remapLicenseList();
          break;
        case false:
          break;
      }
    });
  }

  deleteLicense(licenseItem: AgentLicenseDTO): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete Row #${licenseItem.rowId + 1}?`,
      () => {
        this.licenseList.forEach((item, index) => {
          if (item.id === undefined && item.rowId === licenseItem.rowId) {
            this.licenseList.splice(index, 1);
          }

          if (item.id !== undefined && item.id === licenseItem.id) {
            item.removedProcessDate = this.authService.getCustomDate();
          }

        });
        this.licenseListLocal = this.licenseList.filter(x =>  x.removedProcessDate === null || x.removedProcessDate === undefined);

        this.remapLicenseList();

        this.agentData.isAgentLicensesUpdated = true;
        },
      () => { }
    );
  }

  remapLicenseList(): void {
    let ctr = 0;
    this.licenseListLocal.forEach(cfr => {
      if (cfr.removedProcessDate === null || cfr.removedProcessDate === undefined) {
        cfr.rowId = ctr;
        ctr++;
      }
    });
    this.licenseRowId = ctr - 1  ;
  }


  showAgentLicenseModal(component?, initialState?): BsModalRef {
    return this.modalService.show(component, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
  }

  filter(item?: AgentLicenseDTO): boolean {
    return item.removedProcessDate === null || item.removedProcessDate === undefined;
  }



}
