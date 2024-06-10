import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../shared/enum/click-type.enum';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap';
import { AgencyLabelConstants } from '../../../../shared/constants/agency.label.constants';
import { AgencyData } from '../../data/agency.data';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { BaseClass } from '../../../../shared/base-class';
import { SubAgencyData } from '../../data/sub-agency.data';
import { AgentData } from '../../data/agent.data';
import { FormGroup } from '@angular/forms';
const Flat = 'F';
@Component({
  selector: 'app-agency-details',
  templateUrl: './agency-details.component.html',
  styleUrls: ['./agency-details.component.scss']
})

export class AgencyDetailsComponent extends BaseClass implements OnInit, OnDestroy {
  @ViewChild('tab') tab: TabsetComponent;

  isHidden: boolean = false;

  AgencyLabelConstants = AgencyLabelConstants;

  isSuccessSave: boolean = false;
  selectedHeading: string;

  constructor(
    public agencyData: AgencyData,
    public subAgencyData: SubAgencyData
    , private router: Router
    , private route: ActivatedRoute
    , private agentData: AgentData
  ) { super(); }

  get showSaveCancel() {
    return this.selectedHeading === AgencyLabelConstants.detailsTab || this.selectedHeading === AgencyLabelConstants.addressTab || this.selectedHeading === undefined;
  }

  get agencyFormGroup(): FormGroup {
    return this.agencyData.agencyFormGroup;
  }

  get isEdit() {
    return this.agencyData.agencyId !== null;
  }

  get isFlat(): boolean {
    const commissionType = this.agencyFormGroup.get('commissionType').value;
    return commissionType === Flat || commissionType === '';
  }

  get isUpdated(): boolean {
    return (this.agencyFormGroup.touched && this.agencyFormGroup.dirty) || (this.agencyData.isFlatCommisionsUpdated || this.agencyData.isAddressesUpdated);
  }

  ngOnInit(): void {
    this.initForms();
    this.route.params.pipe(takeUntil(this.stop$)).subscribe(params => {
      if (params.agencyid !== undefined) {
        this.agencyData.agencyId = params.agencyid;
        localStorage.setItem(`agencyId`, params.agencyid);
      } else {
        this.agencyData.agencyId = null;
      }

      if (this.isEdit) {
        this.agencyData.showingOfSaveUpdateNext(true);
        this.callAPIs();
      } else {
        this.resetTable();
        this.agencyData.showingOfSaveUpdateNext(false);
      }
    });
  }

  ngOnDestroy(): void {
    this.subAgencyData.isSubAgencyTab = false;
    this.agencyData.resetAgencyUpdatesTrackers();
    this.subAgencyData.resetSubAgencyUpdatesTrackers();
    this.agentData.resetAgentUpdatesTrackers();
  }

  initForms(): void {
    this.agencyData.initializeForms();
    this.agentData.initializeForms();
  }

  callAPIs(): void {
    this.agencyData.getAgencyDetails(this.agencyData.agencyId);
    this.agentData.getAgentList(this.agencyData.agencyId);
  }

  resetTable(): void {
    // Table List
    this.agencyData.agencyFlatCommission = [];
    this.agencyData.addressList = [];
    this.agencyData.regionalSalesManagerList = [];
    this.agencyData.commissionGroupList = [];

    this.agentData.agentList = [];

    // Row Id
    this.agencyData.agencyFlatCommissionRowId = 0;
    this.agencyData.addressListRowId = 0;

    this.agentData.agentRowId = 0;
  }

  toggleAgencyManagement(): void {
    this.isHidden = !this.isHidden;
  }

  onSelect(data: TabDirective): void {
    this.selectedHeading = data.heading;
    this.agencyData.activeTab = data.heading;
    switch (this.selectedHeading) {
      case AgencyLabelConstants.detailsTab:
        this.subAgencyData.isSubAgencyTab = false;
        this.agencyData.showingOfSaveUpdateNext(this.isEdit ? true : false);
        break;
      case AgencyLabelConstants.addressTab:
        this.agencyData.agencyFormGroup.markAllAsTouched();
        this.subAgencyData.isSubAgencyTab = false;
        this.agencyData.showingOfSaveUpdateNext(this.isEdit ? true : false);
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
        this.router.navigate(['management', 'agency']);
        this.agencyData.resetAgencyUpdatesTrackers();
        break;
      case ClickTypes.Save:
        // to do save
        this.agencyData.saveAgency();
        break;
      case ClickTypes.Update:
        this.agencyData.updateAgency(this.agencyData.agencyId);
        break;
      case ClickTypes.Next:
        this.agencyData.agencyFormGroup.markAllAsTouched();
        this.tab.tabs[1].active = true;
        break;
    }
  }
}
