import {Component, OnDestroy, OnInit} from '@angular/core';
import { GenericAddressData } from '../../shared/components/generic-address/data/generic-address.data';
import { AuthService } from '../../core/services/auth.service';
import { LayoutService } from '../../core/services/layout/layout.service';
import { AgencyData } from './data/agency.data';
import { createManagementMenuItems } from './management-navitems';
import { SubAgencyData } from './data/sub-agency.data';
import { RetailerData } from './data/retailer.data';

@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.scss']
})
export class ManagementComponent implements OnInit, OnDestroy {

  constructor(private layoutService: LayoutService, private auth: AuthService, 
    private agencyData: AgencyData,
    private retailerData: RetailerData,
    private genericAddressData: GenericAddressData, private subAgencyData: SubAgencyData) { }

  ngOnInit() {
    this.callAgencyAPIonLoad();
    this.layoutService.updateMenu(createManagementMenuItems(this.auth.userType.value));
  }

  ngOnDestroy(): void {
    this.layoutService.clearMenu();
  }

  callAgencyAPIonLoad(): void {
    this.agencyData.getAgencies();
    this.genericAddressData.getAddressType();
    this.retailerData.getAgencies();
    this.retailerData.getRetailers();
  }

}
