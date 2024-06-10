import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'broker-commissions-component-policy',
  template: `
    <div *ngIf="shouldShowCommission" class="row">
      <div class="col-6 form-type">
        <label class="font-weight-light sub-title-label">AL Commission</label>
        <h6 class="field-text">
          {{ brokerCommisionAL !== null && brokerCommisionAL !== undefined ? brokerCommisionAL + '%' : '&nbsp;' }}
        </h6>
      </div>
      <div class="col-6 form-type">
        <label class="font-weight-light sub-title-label">APD Commission</label>
        <h6 class="field-text">
          {{ brokerCommisionPD !== null && brokerCommisionPD !== undefined ? brokerCommisionPD + '%' : '&nbsp;' }}
        </h6>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BrokerCommissionsDivChildComponent {
  @Input() shouldShowCommission: boolean;
  @Input() brokerCommisionAL: number;
  @Input() brokerCommisionPD: number;
}
