import { Component, OnInit, ViewChild } from '@angular/core';
import { take } from 'rxjs/operators';
import NotifUtils from '../../../../../../../../../shared/utilities/notif-utils';
import { RiskSpecificsData } from '../../../../../../../../../modules/submission/data/risk-specifics.data';
import { CommoditiesService } from '../../../../../../../../../core/services/submission/risk-specifics/commodities.service';
import { CommoditiesHauledDto } from '../../../../../../../../../shared/models/submission/riskspecifics/maintenance-safety/commoditiesHauledDto';
import { TableConstants } from '../../../../../../../../../shared/constants/table.constants';
import { TableData } from '../../../../../../../../../modules/submission/data/tables.data';
import { SubmissionData } from '../../../../../../../../../modules/submission/data/submission.data';

@Component({
  selector: 'app-commodities-chart',
  templateUrl: './commodities-chart.component.html',
  styleUrls: ['./commodities-chart.component.scss']
})
export class CommoditiesChartComponent implements OnInit {
  modalButton: string = 'Add Commodity';
  TableConstants = TableConstants;
  @ViewChild('btnAddCommodity') btnAddCommodity;
  constructor(
    public data: RiskSpecificsData,
    private commoditiesService: CommoditiesService,
    private tableData: TableData,
    public submissionData: SubmissionData
  ) { }

  ngOnInit(): void {
    if (this.commoditiesHauled.length > 0) {
      this.data.setPage(1, this.data.commoditiesHauled);
    }
  }

  get commoditiesHauled() {
    return this.data.commoditiesHauled;
  }

  set commoditiesHauled(value) {
    this.data.commoditiesHauled = value;
  }

  get is100pct() {
    const sum = this.data.commoditiesHauled.reduce((acc, cur) => acc + Number(cur.percentageOfCarry), 0);
    return sum == 100;
  }

  get form() { return this.data.riskSpecificsForms.commoditiesForm; }

  setEdit(formData: CommoditiesHauledDto) {
    this.modalButton = (!formData) ? 'Add Commodity' : 'Save';
    const commoditiesHauled = formData ?? new CommoditiesHauledDto();
    commoditiesHauled.totalPercentageOfCarry = this.getPercentageOfCarry(formData);
    this.data.riskSpecificsForms.commoditiesForm.patchValue(commoditiesHauled);
    this.form.markAsUntouched();
    this.form.markAsPristine();
    this.btnAddCommodity.nativeElement.click();
  }

  delete(commodity) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${commodity?.commodity}'?`, () => {
      this.commoditiesService.delete(commodity.id).pipe(take(1)).subscribe(() => {
        this.commoditiesHauled = this.commoditiesHauled.filter(t => t.id !== commodity.id);
        this.data.setPage(this.data.currentPage, this.data.commoditiesHauled);
      });
    });
  }

  private getPercentageOfCarry(commodity) {
    let commoditiesHauled = this.commoditiesHauled;
    if (commodity) {
      commoditiesHauled = commoditiesHauled.filter(d => d.id !== commodity.id);
    }
    return commoditiesHauled.reduce((sum, current) => +sum + +current.percentageOfCarry, 0);
  }

  sort(targetElement) {
    let sortData = this.tableData.sortPagedData(targetElement, this.data.commoditiesHauled, this.data.currentPage);
    this.data.commoditiesHauled = sortData.sortedData;
    this.data.pagedItems = sortData.pagedItems;
  }

}
