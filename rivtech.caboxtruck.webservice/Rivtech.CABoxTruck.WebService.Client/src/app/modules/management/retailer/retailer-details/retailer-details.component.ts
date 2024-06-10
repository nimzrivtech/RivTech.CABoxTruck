import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../shared/enum/click-type.enum';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap';
import { AgencyLabelConstants } from '../../../../shared/constants/agency.label.constants';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { BaseClass } from '../../../../shared/base-class';
import { SubAgencyData } from '../../data/sub-agency.data';
import { AgentData } from '../../data/agent.data';
import { FormGroup } from '@angular/forms';
import { RetailerData } from '../../data/retailer.data';
import { RetailerAgentData } from '../../data/retailer-agent.data';
const Flat = 'F';
@Component({
  selector: 'app-retailer-details',
  templateUrl: './retailer-details.component.html',
  styleUrls: ['./retailer-details.component.scss']
})

export class RetailerDetailsComponent extends BaseClass implements OnInit, OnDestroy {
  @ViewChild('tab') tab: TabsetComponent;

  isHidden: boolean = false;

  AgencyLabelConstants = AgencyLabelConstants;

  isSuccessSave: boolean = false;
  selectedHeading: string;

  constructor(
    public retailerData: RetailerData,
    public subAgencyData: SubAgencyData
    , private router: Router
    , private route: ActivatedRoute
    , private agentData: AgentData
    , private retailerAgentData: RetailerAgentData
  ) { super(); }

  get showSaveCancel() {
    return this.selectedHeading === AgencyLabelConstants.detailsTab || this.selectedHeading === AgencyLabelConstants.addressTab || this.selectedHeading === undefined;
  }

  get agencyFormGroup(): FormGroup {
    return this.retailerData.agencyFormGroup;
  }

  get isEdit() {
    return this.retailerData.agencyId !== null;
  }

  get isFlat(): boolean {
    const commissionType = this.agencyFormGroup.get('commissionType').value;
    return commissionType === Flat || commissionType === '';
  }

  get isUpdated(): boolean {
    return (this.agencyFormGroup.touched && this.agencyFormGroup.dirty) || (this.retailerData.isFlatCommisionsUpdated || this.retailerData.isAddressesUpdated);
  }

  ngOnInit(): void {
    this.initForms();
    this.route.params.pipe(takeUntil(this.stop$)).subscribe(params => {
      if (params.agencyid !== undefined) {
        this.retailerData.agencyId = params.agencyid;
        localStorage.setItem(`agencyId`, params.agencyid);
      } else {
        this.retailerData.agencyId = null;
      }

      if (this.isEdit) {
        this.retailerData.showingOfSaveUpdateNext(true);
        this.callAPIs();
      } else {
        this.resetTable();
        this.retailerData.resetAgencyList();
        this.retailerData.showingOfSaveUpdateNext(false);
      }
    });
  }

  ngOnDestroy(): void {
    this.subAgencyData.isSubAgencyTab = false;
    this.retailerData.resetAgencyUpdatesTrackers();
    this.subAgencyData.resetSubAgencyUpdatesTrackers();
    this.agentData.resetAgentUpdatesTrackers();
  }

  initForms(): void {
    this.retailerData.initializeForms();
    this.agentData.initializeForms();
    this.retailerAgentData.initializeForms();
  }

  callAPIs(): void {
    this.retailerData.getRetailerDetails(this.retailerData.agencyId);
    this.agentData.getAgentList(this.retailerData.agencyId);
    this.retailerAgentData.getAgentList(this.retailerData.agencyId);
  }

  resetTable(): void {
    // Table List
    this.retailerData.agencyFlatCommission = [];
    this.retailerData.addressList = [];
    this.retailerData.regionalSalesManagerList = [];
    this.retailerData.commissionGroupList = [];

    this.agentData.agentList = [];

    // Row Id
    this.retailerData.agencyFlatCommissionRowId = 0;
    this.retailerData.addressListRowId = 0;

    this.agentData.agentRowId = 0;
  }

  toggleAgencyManagement(): void {
    this.isHidden = !this.isHidden;
  }

  onSelect(data: TabDirective): void {
    this.selectedHeading = data.heading;
    this.retailerData.activeTab = data.heading;
    switch (this.selectedHeading) {
      case AgencyLabelConstants.detailsTab:
        this.subAgencyData.isSubAgencyTab = false;
        this.retailerData.showingOfSaveUpdateNext(this.isEdit ? true : false);
        break;
      case AgencyLabelConstants.addressTab:
        this.subAgencyData.isSubAgencyTab = false;
          this.retailerData.showingOfSaveUpdateNext(this.isEdit ? true : false);
        break;
      case AgencyLabelConstants.subAgencyTab:
        this.subAgencyData.isSubAgencyTab = true;
        break;
      case AgencyLabelConstants.agentTab:
        this.subAgencyData.isSubAgencyTab = false;
        break;
      case AgencyLabelConstants.programStateTab:
        this.subAgencyData.isSubAgencyTab = false;
        break;
    }
  }

  onClick(clickType?: ClickTypes): void {
    switch (clickType) {
      case ClickTypes.Cancel:
        // reset all fields
        this.router.navigate(['management', 'retailer']);
        this.retailerData.resetAgencyUpdatesTrackers();
        break;
      case ClickTypes.Save:
        this.retailerData.saveRetailer();
        break;
      case ClickTypes.Update:
        this.retailerData.updateRetailer(this.retailerData.agencyId);
        break;
      case ClickTypes.Next:
        this.tab.tabs[1].active = true;
        break;
    }
  }
}
