import { Component, OnInit } from '@angular/core';
import { GenericLabel } from '../../../../../../shared/constants/generic.labels.constants';
import { TableConstants } from '../../../../../../shared/constants/table.constants';
import { AgentData } from '../../../../../../modules/management/data/agent.data';
import { AgentLabelConstants } from '../../../../../../shared/constants/agent.label.constants';
import { SubAgencyDTO } from '../../../../../../shared/models/management/agency-management/sub-agencyDto';
import { SearchDTO } from '../../../../../../shared/models/management/search.dto';
import { environment } from '../../../../../../../environments/environment';
import { SubAgencyData } from '../../../../../../modules/management/data/sub-agency.data';
import Utils from '../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../shared/utilities/notif-utils';
import { AgencySubAgencyService } from '../../../../../../core/services/management/agency-sub-agency.service';
import { RetailerData } from '@app/modules/management/data/retailer.data';
import { AgencyData } from '@app/modules/management/data/agency.data';
import { AgencyService } from '@app/core/services/management/agency-service';

@Component({
  selector: 'app-retailer-info-agency',
  templateUrl: './retailer-info-agency.component.html',
  styleUrls: ['./retailer-info-agency.component.scss']
})
export class RetailerInfoAgencyComponent implements OnInit {
  TableConstants = TableConstants;
  AgentLabelConstants = AgentLabelConstants;
  genericLabel = GenericLabel;
  constructor(public agentData: AgentData,
              public subAgencyData: SubAgencyData,
              public retailerData: RetailerData,
              private agencyData: AgencyData,
              private agencyService: AgencyService,
              private subAgencyService: AgencySubAgencyService) { }

  ngOnInit() {
    if (this.isNew) { this.searchRecords(''); }
  }

  get isNew() {
    return this.retailerData.agencyId === null;
  }

  onLinkedChecked(item?: SubAgencyDTO, event?): void {
    const isLinkedChecked = event.target.checked;
    this.retailerData.retailerAgenciesList.map(value => {
      if (value.id === item.id) {
        value.linked = isLinkedChecked;
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
      appCode: environment.ApplicationId
    };
    Utils.blockUI();
    this.retailerData.resetAgencyList();
    this.agencyService.postSearchList(payload).subscribe((data: any) => {
      this.retailerData.retailerAgenciesList = data.map(x => x.agency);
      Utils.unblockUI();
    }, error => {
      Utils.unblockUI();
      this.agencyData.isAgencyLoading = false;
      NotifUtils.showError(error.message);
    });
  }

}
