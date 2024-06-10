import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { SummaryData } from '../../../../../../../modules/submission/data/summary.data';

@Component({
  selector: 'broker-commissions-component2',
  templateUrl: './broker-commission.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BrokerCommissionsDivComponent2 {

  shouldShowCommission: boolean = false;
  brokerCommisionAL: number;
  brokerCommisionPD: number;

  constructor(
    public summaryData: SummaryData,

  ) { }

  ngOnInit() {
    console.log(" ngOnInit() {");
    this.setBrokerCommissionsValue(null);
  }

  setBrokerCommissionsValue(from: string | null) {
    this.summaryData.checkForCommission(from);

    this.shouldShowCommission = this.summaryData.shouldShowCommission;
    this.brokerCommisionAL = this.summaryData.brokerCommisionAL;
    this.brokerCommisionPD = this.summaryData.brokerCommisionPD;


    console.log("policy commision", this.shouldShowCommission, this.brokerCommisionAL, this.brokerCommisionPD);
  }

  reloadComponent(from: string | null) {
    this.setBrokerCommissionsValue(from);
    console.log("reloadComponent", from);
    // const currentUrl = this.router.url;
    // const refreshedUrl = currentUrl.includes('?') ? `${currentUrl}&_refresh=${new Date().getTime()}` : `${currentUrl}?_refresh=${new Date().getTime()}`;

    // // Navigate to the updated URL
    // this.router.navigateByUrl(refreshedUrl);

  }

  showBroker() {
    this.shouldShowCommission = true;
  }

}