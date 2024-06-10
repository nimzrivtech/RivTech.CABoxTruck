import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { finalize, take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

import Utils from '../../../../../../../../shared/utilities/utils';
import { VehicleData } from '../../../../../../data/vehicle/vehicle.data';
import { VehicleTextConstants } from '../../../../../../../../shared/constants/vehicle.text.constants';
import { QuoteLimitsData } from '../../../../../../../../modules/submission/data/quote/quote-limits.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { CDK_CONNECTED_OVERLAY_SCROLL_STRATEGY_PROVIDER_FACTORY } from '@angular/cdk/overlay/overlay-directives';

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
              private submissionData: SubmissionData,
              public quoteLimitsData: QuoteLimitsData) { }

  ngOnInit(): void {
    this.parseOptions();
  }

  optionChanged(vehicleId: string, optionId: number, checked: boolean) {
    optionId++;
    const editingVehicle = this.vehicleData.vehicleInfoList.find(v => v.id === vehicleId);
    let optionsArray = (editingVehicle.options ?? '').split(',').map(o => +o);
    if (checked) {
      optionsArray.push(optionId);
    } else if (optionId === 1) {
        optionsArray = [];
    } else {
      optionsArray.splice(optionsArray.indexOf(optionId), 1);
    }
    const options = optionsArray.filter(o => o > 0).sort().join(',');
    editingVehicle.options = options === '' ? null : options;
    if (optionId === 1 && !checked) {
      Utils.blockUI();
      this.vehicleData.pagedItems = [];
      setTimeout(() => {
        this.vehicleData.setPage(this.vehicleData.currentPage);
        Utils.unblockUI();
      }, 200);
    }

    this.vehicleData.update(editingVehicle).pipe(take(1), finalize(() => Utils.unblockUI()))
      .subscribe(() => {
        //this.toastr.success(this.messages.successUpdate, 'Success!');
      }, () => {
        console.error(this.messages.errorUpdate, 'Error!');
      });
  }

  optionsDisabled(vehicleId: string) {
    const editingVehicle = this.vehicleData.vehicleInfoList.find(v => v.id === vehicleId);
    const optionsArray = (editingVehicle.options ?? '').split(',').map(o => +o);
    return optionsArray.indexOf(1) === -1 ? true : false;
  }

  public parseOptions() {
    if (this.optionsCsv) {
      const options = (this.optionsCsv ?? '').split(',').map(o => +o);
      options.forEach(o => {
        this.options[o - 1] = true;
      });
    } else {
      this.options = Array(4).fill(false);
    }
  }
}
