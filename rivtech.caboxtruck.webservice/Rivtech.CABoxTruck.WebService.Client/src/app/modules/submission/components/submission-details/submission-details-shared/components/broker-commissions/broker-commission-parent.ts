import { Component } from '@angular/core';
import { SummaryData } from '../../../../../../../modules/submission/data/summary.data';

@Component({
  selector: 'broker-commissions-component',
  template: `
  <div>
    <app-child [shouldShowCommission]="shouldShowCommission" [commissions]="commissions"></app-child>
  </div>`,
})
export class BrokerCommissionsDivComponent {
  shouldShowCommission = false;
  commissions = {
    brokerCommisionAL: null,
    brokerCommisionPD: null,
  };

  constructor(public summaryData: SummaryData) { }

  // setBrokerCommissionsValue(from: string | null) 
  // {
  //     this.summaryData.checkForCommission(from);
  //     this.shouldShowCommission = this.summaryData.shouldShowCommission;
  //     this.commissions = {
  //         brokerCommisionAL: this.summaryData.brokerCommisionAL,
  //         brokerCommisionPD: this.summaryData.brokerCommisionPD,
  //     }
  //     console.log("policy commision", this.shouldShowCommission, this.commissions.brokerCommisionAL, this.commissions.brokerCommisionPD);
  // }

  ngOninit() {
    console.log("parent");
    this.setBrokerCommissionsValue(null);
  }

  // ngOnChanges() {
  //   this.setBrokerCommissionsValue(null);
  // }
  // ngDoCheck() {
  //   this.setBrokerCommissionsValue(null);
  // }
  async setBrokerCommissionsValue(from: string | null) {
    try {
      await this.summaryData.checkForCommission(from);

      this.shouldShowCommission = this.summaryData.shouldShowCommission;
      this.commissions = {
        brokerCommisionAL: this.summaryData.brokerCommisionAL,
        brokerCommisionPD: this.summaryData.brokerCommisionPD,
      };

      console.log("policy commision", this.shouldShowCommission, this.commissions.brokerCommisionAL, this.commissions.brokerCommisionPD);
    } catch (error) {
      console.error("Error in setBrokerCommissionsValue:", error);
      // Handle the error as appropriate
    }
  }
}