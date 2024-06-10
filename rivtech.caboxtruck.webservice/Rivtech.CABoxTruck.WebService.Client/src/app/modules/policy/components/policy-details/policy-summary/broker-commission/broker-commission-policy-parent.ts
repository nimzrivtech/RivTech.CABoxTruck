// import { Component } from '@angular/core';
// import { PolicySummaryData } from '../../../../../../modules/policy/data/policy-summary.data';

// @Component({
//   selector: 'broker-commissions-component-policy',
//   template: `
//   <div>
//     <app-child [shouldShowCommission]="shouldShowCommission" [commissions]="commissions"></app-child>
//   </div>`,
// })
// export class BrokerCommissionsDivComponent {
//   shouldShowCommission = false;
//   commissions = {
//     brokerCommisionAL: null,
//     brokerCommisionPD: null,
//   };

//   constructor(public summaryData: PolicySummaryData) { }

// //   ngOninit() {
// //     console.log("parent");
// //     this.setBrokerCommissionsValue(null);
// //   }

//   // ngOnChanges() {
//   //   this.setBrokerCommissionsValue(null);
//   // }
//   // ngDoCheck() {
//   //   this.setBrokerCommissionsValue(null);
//   // }
//   async setBrokerCommissionsValue(from: string | null) {
//     try {
//       await this.summaryData.checkForCommission(from);

//       this.shouldShowCommission = this.summaryData.shouldShowCommission;
//       this.commissions = {
//         brokerCommisionAL: this.summaryData.brokerCommisionAL,
//         brokerCommisionPD: this.summaryData.brokerCommisionPD,
//       };

//       console.log("policy commision", this.shouldShowCommission, this.commissions.brokerCommisionAL, this.commissions.brokerCommisionPD);
//     } catch (error) {
//       console.error("Error in setBrokerCommissionsValue:", error);
//       // Handle the error as appropriate
//     }
//   }
// }