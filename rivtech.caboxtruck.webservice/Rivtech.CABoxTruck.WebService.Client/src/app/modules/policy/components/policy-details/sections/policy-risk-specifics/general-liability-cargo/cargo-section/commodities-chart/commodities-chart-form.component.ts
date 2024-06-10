import { Component, Input, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { RiskSpecificsData } from '../../../../../../../../../modules/submission/data/risk-specifics.data';
import { CommoditiesService } from '../../../../../../../../../core/services/submission/risk-specifics/commodities.service';
import { BaseComponent } from '../../../../../../../../../shared/base-component';
import { CurrencyMaskInputMode } from 'ngx-currency';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-commodities-chart-form',
  templateUrl: './commodities-chart-form.component.html',
  styleUrls: ['./commodities-chart-form.component.scss']
})
export class CommoditiesChartFormComponent extends BaseComponent implements OnInit {
  @Input() modalButton: string;
  @Input() modalTitle: string;
  constructor(
    public data: RiskSpecificsData,
    private commoditiesService: CommoditiesService,
    public modalRef: BsModalRef,
  ) {
    super();
  }

  ngOnInit(): void {
  }

  get commoditiesHauled() {
    return this.data.commoditiesHauled;
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.data.riskSpecificsForms.commoditiesForm; }

  get maskOptions() {
    return {
      align: 'left',
      allowNegative: false,
      max: 100,
      prefix: '',
      suffix: '%',
      precision: 1,
      nullable: true,
      inputMode: CurrencyMaskInputMode.NATURAL
    };
  }

  get commodityList() {
    const addedCommodities = this.data.commoditiesHauled?.map(c => c.commodity).filter(c => c !== this.fg.controls['commodity'].value);
    return this.data.riskSpecificsDropdownsList.commoditiesList?.filter(cl => !addedCommodities.includes(cl.label));
  }

  onCancel() {
    this.fg.markAsUntouched();
  }

  onSubmit() {
    this.modalRef.hide();
    const commodity = this.data.riskSpecificsForms.commoditiesForm.value;
    if (commodity.id) {
      this.commoditiesService.update(commodity).pipe(take(1)).subscribe(data => {
        const editingCommodity = this.commoditiesHauled.find(c => c.id === commodity.id);
        Object.assign(editingCommodity, commodity);
        this.data.setPage(1, this.commoditiesHauled);
      });
    } else {
      commodity.riskDetailId = this.data.submissionData?.riskDetail?.id;
      this.commoditiesService.insert(commodity).pipe(take(1)).subscribe(data => {
        this.commoditiesHauled.push(data);
        this.data.setPage(1, this.commoditiesHauled, true);
      });
    }
  }
}

