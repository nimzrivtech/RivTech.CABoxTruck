import { Component, OnInit } from '@angular/core';
import { AgencyService } from '../../../core/services/management/agency-service';
import { BaseClass } from '../../../shared/base-class';
import { SearchDTO } from '../../../shared/models/management/search.dto';
import NotifUtils from '../../../shared/utilities/notif-utils';
import Utils from '../../../shared/utilities/utils';
import { environment } from '../../../../environments/environment';
import { Router } from '@angular/router';
import { GenericLabel } from 'app/shared/constants/generic.labels.constants';
import { TableConstants } from 'app/shared/constants/table.constants';
import { AgencyLabelConstants } from '../../../shared/constants/agency.label.constants';
import { RetailerData } from '../data/retailer.data';
import { RetailerService } from '@app/core/services/management/retailer.service';

@Component({
  selector: 'app-retailer',
  templateUrl: './retailer.component.html',
  styleUrls: ['./retailer.component.scss']
})
export class RetailerComponent extends BaseClass implements OnInit {
  AgencyLabelConstants = AgencyLabelConstants;

  isListHidden: boolean = false;

  genericLabel = GenericLabel;
  TableConstants = TableConstants;

  constructor(
    public retailerData: RetailerData,
    private agencyService: AgencyService,
    private retailerService: RetailerService,
    private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.retailerData.callAgencyAPIonLoad();
  }

  addAgency(): void {
    this.retailerData.activeTab = this.AgencyLabelConstants.detailsTab;
    this.retailerData.showingOfSaveUpdateNext(false);
    this.router.navigate(['management', 'retailer', 'new']);
  }

  editAgency(item?: any): void {
    this.retailerData.showingOfSaveUpdateNext(true);
    this.router.navigate(['management', 'retailer', 'edit', item.id]);
  }

  deleteAgency(agencyItem?: any): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete ${agencyItem.entity.companyName}?`,
      () => {
        this.retailerService.deleteRetailer(agencyItem.id).subscribe(res => {
          this.retailerData.callAgencyAPIonLoad();
          //this.retailerData.agencyAuditLog(AgencyLabelConstants.auditLog.delete, res);
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
    // Utils.blockUI();
    // this.retailerData.resetAgencyList();
    // this.retailerData.isAgencyLoading = true;
    // this.agencyService.postSearchList(payload).subscribe((data: any) => {
    //   console.log(data);
    //   this.retailerData.retailerListData = data.map(x => x.agency);
    //   this.retailerData.isSearched = true;
    //   this.retailerData.isAgencyLoading = false;
    //   Utils.unblockUI();
    // }, error => {
    //   Utils.unblockUI();
    //   this.retailerData.isAgencyLoading = false;
    //   NotifUtils.showError(error.message);
    // });
    if (input === '') { input = null; }
    this.retailerData.getRetailers(input);
  }

}


