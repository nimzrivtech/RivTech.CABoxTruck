import { Component, OnInit } from '@angular/core';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import { TableConstants } from '../../../../../shared/constants/table.constants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { SearchDTO } from '../../../../../shared/models/management/search.dto';
import { environment } from '../../../../../../environments/environment';
import Utils from '../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../shared/utilities/notif-utils';
import { AgencyService } from '../../../../../core/services/management/agency-service';
import { AgencyData } from '../../../../../modules/management/data/agency.data';
import { AgencyLabelConstants } from '../../../../../shared/constants/agency.label.constants';
import { AgentLabelConstants } from '../../../../../shared/constants/agent.label.constants';
import { RetailerAgentModalComponent } from './retailer-agent-modal/retailer-agent-modal.component';
import { RetailerData } from '@app/modules/management/data/retailer.data';
import { RetailerAgentData } from '@app/modules/management/data/retailer-agent.data';
@Component({
  selector: 'app-retailer-agent',
  templateUrl: './retailer-agent.component.html',
  styleUrls: ['./retailer-agent.component.scss']
})
export class RetailerAgentComponent implements OnInit {
  isHidden: boolean = false;
  TableConstants = TableConstants;
  genericLabel = GenericLabel;
  AgentLabelConstants = AgentLabelConstants;

  constructor(
    public retailerAgentData: RetailerAgentData,
    public agencyData: AgencyData,
    public retailerData: RetailerData,
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
    this.retailerAgentData.agentRowId = this.retailerAgentData.agentRowId + 1;
    const initialState = {
      modalTitle: 'Add Agent',
      agentList: this.retailerAgentData.agentList,
      rowId: this.retailerAgentData.agentRowId
    };

    this.modalRef = this.showAgentModal(RetailerAgentModalComponent, initialState);
    localStorage.setItem('agentId', '');
  }

  editAgent(item?): void {
    localStorage.setItem('agentId', item.Id);
    this.retailerAgentData.agentRowId = this.retailerAgentData.agentRowId + 1;
    const initialState = {
      modalTitle: 'Update Agent',
      agentList: this.retailerAgentData.agentList,
      rowId: this.retailerAgentData.agentRowId,
      agentItem: item,
      isEdit: true
    };

    this.modalRef = this.showAgentModal(RetailerAgentModalComponent, initialState);
  }

  deleteAgent(agentItem?): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete ${agentItem?.entity?.fullName}?`,
    () => {
      this.retailerAgentData.retailerAgentService.deleteAgent(agentItem.id).subscribe(res => {
        this.retailerAgentData.callAgentAPIonLoad();
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
    //   agencyId: this.retailerData.agencyId
    // };
    // Utils.unblockUI();
    // this.retailerAgentData.agentList = [];
    // this.agencyService.postSearchAgentList(payload).subscribe((data: any) => {
    //   console.log(data);
    //   this.retailerAgentData.agentList = data;
    //   Utils.unblockUI();
    // }, error => {
    //   Utils.unblockUI();
    //   NotifUtils.showError(error.message);
    // });
    this.retailerAgentData.getAgentList(this.retailerData.agencyId, null, input);
  }

}
