import { Component, OnInit } from '@angular/core';
import { AgencyService } from '../../../core/services/management/agency-service';
import { BaseClass } from '../../../shared/base-class';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { SearchDTO } from '../../../shared/models/management/search.dto';
import NotifUtils from '../../../shared/utilities/notif-utils';
import Utils from '../../../shared/utilities/utils';
import { environment } from '../../../../environments/environment';
import { AgencyData } from '../data/agency.data';
import { Router } from '@angular/router';
import { GenericLabel } from 'app/shared/constants/generic.labels.constants';
import { TableConstants } from 'app/shared/constants/table.constants';
import { AgencyLabelConstants } from '../../../shared/constants/agency.label.constants';

@Component({
  selector: 'app-agency',
  templateUrl: './agency.component.html',
  styleUrls: ['./agency.component.scss']
})
export class AgencyComponent extends BaseClass implements OnInit {
  AgencyLabelConstants = AgencyLabelConstants;

  isListHidden: boolean = false;

  genericLabel = GenericLabel;
  TableConstants = TableConstants;

  constructor(
    public agencyData: AgencyData,
    private agencyService: AgencyService,
    private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.agencyData.callAgencyAPIonLoad();
  }



  addAgency(): void {
    this.agencyData.activeTab = this.AgencyLabelConstants.detailsTab;
    this.agencyData.showingOfSaveUpdateNext(false);
    this.router.navigate(['management', 'agency', 'new']);
  }

  editAgency(item?: any): void {
    this.agencyData.showingOfSaveUpdateNext(true);
    this.router.navigate(['management', 'agency', 'edit', item.id]);
  }

  deleteAgency(agencyItem?: any): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete ${agencyItem.entity.companyName}?`,
      () => {
        this.agencyService.deleteAgencyDetails(agencyItem.id).subscribe(res => {
          this.agencyData.callAgencyAPIonLoad();
          this.agencyData.agencyAuditLog(AgencyLabelConstants.auditLog.delete, res);
        });
      },
      () => { }
    );
  }

  toggleAgencyList(): void {
    this.isListHidden = !this.isListHidden;
  }

  searchRecords(input: string): void {
    // const payload: SearchDTO = {
    //   term: input,
    //   programId: environment.ApplicationId
    // };
    this.agencyData.resetAgencyList();
    this.agencyData.searchAgencyRecords(input.toLowerCase());
    // Utils.blockUI();
    // this.agencyData.isAgencyLoading = true;
    // this.agencyService.postSearchList(payload).subscribe((data: any) => {
    //   this.agencyData.agencyListData = data.map(x => x.agency);
    //   this.agencyData.isSearched = true;
    //   this.agencyData.isAgencyLoading = false;
    //   Utils.unblockUI();
    // }, error => {
    //   Utils.unblockUI();
    //   this.agencyData.isAgencyLoading = false;
    //   NotifUtils.showError(error.message);
    // });
  }

}


