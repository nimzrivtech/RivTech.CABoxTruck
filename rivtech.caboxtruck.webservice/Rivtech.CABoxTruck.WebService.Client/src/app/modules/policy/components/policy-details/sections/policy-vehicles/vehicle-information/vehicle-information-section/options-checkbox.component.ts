import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { finalize, take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

import Utils from '../../../../../../../../shared/utilities/utils';
import { VehicleTextConstants } from '../../../../../../../../shared/constants/vehicle.text.constants';
import { QuoteLimitsData } from '../../../../../../../../modules/submission/data/quote/quote-limits.data';
import { VehicleData } from '../../../../../../../../modules/submission/data/vehicle/vehicle.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
  selector: 'app-options-checkbox',
  templateUrl: './options-checkbox.component.html',
  styleUrls: ['./options-checkbox.component.scss']
})

export class OptionsCheckboxComponent implements OnInit {

  @Input() vehicleId: number;
  @Input() optionsCsv: string;
  options: Boolean[] = Array(4).fill(false);
  messages = VehicleTextConstants.messages;

  constructor(private vehicleData: VehicleData,
    private toastr: ToastrService,
    private policySummaryData: PolicySummaryData,
    public quoteLimitsData: QuoteLimitsData) { }

  ngOnInit(): void {
    this.parseOptions();
  }

  optionChanged(vehicleId: string, optionId: number, checked: boolean) {
    optionId++;
    const editingVehicle = this.vehicleData.vehicleInfoList.find(v => v.id === vehicleId);
    const optionsArray = (editingVehicle.options ?? '').split(',').map(o => +o);
    if (checked) {
      optionsArray.push(optionId);
    } else {
      optionsArray.splice(optionsArray.indexOf(optionId), 1);
    }
    const options = optionsArray.filter(o => o > 0).sort().join(',');
    editingVehicle.options = options === '' ? null : options;

    this.vehicleData.update(editingVehicle).pipe(take(1), finalize(() => Utils.unblockUI()))
      .subscribe(() => {
        this.policySummaryData.updateSubtatusForPendingEndorsement();
      }, () => {
        console.error(this.messages.errorUpdate, 'Error!');
      });
  }

  public parseOptions() {
    var options = (this.optionsCsv ?? '').split(',').map(o => +o);
    options.forEach(o => {
      this.options[o - 1] = true;
    });
  }
}
