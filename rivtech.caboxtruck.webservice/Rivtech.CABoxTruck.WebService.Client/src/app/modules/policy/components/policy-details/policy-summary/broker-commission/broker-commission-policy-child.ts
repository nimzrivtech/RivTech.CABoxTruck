// import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

// @Component({
//   selector: 'app-child',
//   template: `
//     <div *ngIf="shouldShowCommission" class="row">
//       <div class="col-6 form-type">
//         <label class="font-weight-light sub-title-label">AL Commission</label>
//         <h6 class="field-text">
//           {{ commissions.brokerCommisionAL !== null && commissions.brokerCommisionAL !== undefined ? commissions.brokerCommisionAL + '%' : '&nbsp;' }}
//         </h6>
//       </div>
//       <div class="col-6 form-type">
//         <label class="font-weight-light sub-title-label">APD Commission</label>
//         <h6 class="field-text">
//           {{ commissions.brokerCommisionPD !== null && commissions.brokerCommisionPD !== undefined ? commissions.brokerCommisionPD + '%' : '&nbsp;' }}
//         </h6>
//       </div>
//     </div>
//   `,
//   changeDetection: ChangeDetectionStrategy.OnPush
// })
// export class BrokerCommissionsDivChildComponent {
//   @Input() shouldShowCommission: boolean;
//   @Input() commissions: { brokerCommisionAL: number; brokerCommisionPD: number; };
// }
