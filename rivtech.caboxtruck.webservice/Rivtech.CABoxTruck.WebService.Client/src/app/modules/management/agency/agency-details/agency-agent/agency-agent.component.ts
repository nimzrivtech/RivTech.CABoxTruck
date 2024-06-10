import { Component, OnInit } from '@angular/core';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import { TableConstants } from '../../../../../shared/constants/table.constants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { AgentData } from '../../../../../modules/management/data/agent.data';
import { AgencyAgentModalComponent } from './agency-agent-modal/agency-agent-modal.component';
import { SearchDTO } from '../../../../../shared/models/management/search.dto';
import { environment } from '../../../../../../environments/environment';
import Utils from '../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../shared/utilities/notif-utils';
import { AgencyService } from '../../../../../core/services/management/agency-service';
import { AgencyData } from '../../../../../modules/management/data/agency.data';
import { AgencyLabelConstants } from '../../../../../shared/constants/agency.label.constants';
import { AgentLabelConstants } from '../../../../../shared/constants/agent.label.constants';
@Component({
  selector: 'app-agency-agent',
  templateUrl: './agency-agent.component.html',
  styleUrls: ['./agency-agent.component.scss']
})
export class AgencyAgentComponent implements OnInit {
  isHidden: boolean = false;
  TableConstants = TableConstants;
  genericLabel = GenericLabel;
  AgentLabelConstants = AgentLabelConstants;

  constructor(public agentData: AgentData,
    public agencyData: AgencyData,
    private modalRef: BsModalRef,
    private modalService: BsModalService,
    private agencyService: AgencyService) { }

  ngOnInit() {
  }

  toggleAgent(): void {
    this.isHidden = !this.isHidden;
  }

  addAgent(): void {
    this.agencyData.agentActiveTab = AgencyLabelConstants.agentTab;
    this.agentData.agentRowId = this.agentData.agentRowId + 1;
    const initialState = {
      modalTitle: 'Add Agent',
      agentList: this.agentData.agentList,
      rowId: this.agentData.agentRowId
    };

    this.modalRef = this.showAgentModal(AgencyAgentModalComponent, initialState);
    localStorage.setItem('agentId', '');
  }

  editAgent(item?): void {
    localStorage.setItem('agentId', item.agentId);
    this.agentData.agentRowId = this.agentData.agentRowId + 1;
    const initialState = {
      modalTitle: 'Update Agent',
      agentList: this.agentData.agentList,
      rowId: this.agentData.agentRowId,
      agentItem: item,
      isEdit: true
    };

    this.modalRef = this.showAgentModal(AgencyAgentModalComponent, initialState);
  }

  deleteAgent(agentItem?): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete ${agentItem.agent?.entity?.fullName}?`,
    () => {
      this.agentData.agentService.deleteAgent(agentItem.agentId).subscribe(res => {
        this.agentData.callAgentAPIonLoad();
        this.agentData.agentAuditLog(AgencyLabelConstants.auditLog.delete, res);
      }, err => {
        console.log(err);
      });
    },
    () => { }
  );
  }

  showAgentModal(component?, initialState?): BsModalRef {
    return this.modalService.show(component, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-xl',
    });
  }

  searchRecords(input: string): void {
    // const payload: SearchDTO = {
    //   term: input,
    //   programId: environment.ApplicationId,
    //   agencyId: this.agentData.subAgencyData.agencyData.agencyId
    // };
    // Utils.unblockUI();
    // this.agentData.agentList = [];
    // this.agencyService.postSearchAgentList(payload).subscribe((data: any) => {
    //   this.agentData.agentList = data;
    //   Utils.unblockUI();
    // }, error => {
    //   Utils.unblockUI();
    //   NotifUtils.showError(error.message);
    // });
    this.agentData.getAgentList(this.agencyData.agencyId, null, input.toLowerCase());
  }

}
