import { Component, OnInit } from '@angular/core';
import { GenericLabel } from '../../../../../../../../shared/constants/generic.labels.constants';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { AgentData } from '../../../../../../../../modules/management/data/agent.data';
import { AgentLabelConstants } from '../../../../../../../../shared/constants/agent.label.constants';
import { SubAgencyDTO } from '../../../../../../../../shared/models/management/agency-management/sub-agencyDto';
import { SearchDTO } from '../../../../../../../../shared/models/management/search.dto';
import { environment } from '../../../../../../../../../environments/environment';
import { SubAgencyData } from '../../../../../../../../modules/management/data/sub-agency.data';
import Utils from '../../../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { AgencySubAgencyService } from '../../../../../../../../core/services/management/agency-sub-agency.service';
import { AgencyData } from '../../../../../../../../modules/management/data/agency.data';

@Component({
  selector: 'app-agent-details-subagency',
  templateUrl: './agent-details-subagency.component.html',
  styleUrls: ['./agent-details-subagency.component.scss']
})
export class AgentDetailsSubagencyComponent implements OnInit {
  TableConstants = TableConstants;
  AgentLabelConstants = AgentLabelConstants;
  genericLabel = GenericLabel;
  constructor(public agentData: AgentData,
              public subAgencyData: SubAgencyData,
              public agencyData: AgencyData,
              private subAgencyService: AgencySubAgencyService) { }

  ngOnInit() {
  }

  onLinkedChecked(item?: SubAgencyDTO, event?): void {
    const isLinkedChecked = event.target.checked;
    const hasPrimary = this.agentData.agentSubAgenciesList.filter(x => x.isPrimary).length;
    this.agentData.isSubAgenciesUpdated = true;
    this.agentData.agentSubAgenciesList.map(value => {
      if (value.id === item.id) {
        value.linked = isLinkedChecked;
        if (hasPrimary === 0) {
          value.isPrimary = isLinkedChecked;
        } else if (value.isPrimary && !isLinkedChecked) {
          value.isPrimary = false;
        }
      }
      return value;
    });
  }

  onPrimaryChecked(item?: SubAgencyDTO): void {
    this.agentData.isSubAgenciesUpdated = true;
    this.agentData.agentSubAgenciesList.map(value => {
      if (value.id === item.id) {
        value.isPrimary = true;
      } else {
        value.isPrimary = false;
      }
      return value;
    });
  }

  searchRecords(input: string): void {
    const payload: SearchDTO = {
      term: input,
      appCode: environment.ApplicationId,
      agencyId: this.agencyData.agencyId
    };
    Utils.unblockUI();
    this.agentData.agentSubAgenciesList = [];
    this.subAgencyService.postSearchSubAgencyList(payload).subscribe((data: any) => {
      this.agentData.agentSubAgenciesList = data.map(x => x.subAgency);
      Utils.unblockUI();
    }, error => {
      Utils.unblockUI();
      NotifUtils.showError(error.message);
    });
  }


}
