import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { LimitsData } from '../../../../../../../../../modules/submission/data/limits/limits.data';

@Component({
  selector: 'app-other-coverages-chart',
  templateUrl: './other-coverages-chart.component.html',
  styleUrls: ['./other-coverages-chart.component.scss']
})
export class OtherCoveragesChartComponent implements OnInit {
  form: FormGroup;
  disableRefrigeration: boolean = true;
  disableTrailerType: boolean = true;
  constructor(public limitsData: LimitsData) {
  }

  ngOnInit() {
    this.checkCargoLimits();
    // this.checkTrailerInterchangeLimits();
  }

  setTooltip(field, dataList) {
    if (field && dataList) {
      const data = dataList.find(x => x.value == field);
      if (data) {
        return data.label;
      }
    }
  }

  setValue(name){
    // Temp Hardcoded Id=1 in DB No Coverage
    if (this.f[name].value === '1' || this.f[name].value == null) {
      switch (name) {
        case 'cargoLimitId':
          this.limitsData.isCargo = false;
          break;
        case 'refCargoLimitId':
          this.limitsData.isRefrigeration = false;
          break;
      }
      return;
    }

    switch (name) {
      case 'cargoLimitId':
        this.limitsData.isCargo = true;
        break;
      case 'refCargoLimitId':
        this.limitsData.isRefrigeration = true;
        break;
    }

    this.checkCargoLimits();
    this.checkTrailerInterchangeLimits();
  }

  checkCargoLimits() {
    this.disableRefrigeration = (this.f['cargoLimitId'].value == "53");
    if (this.disableRefrigeration) {
      this.limitsData.isRefrigeration = false;
      this.f['refCargoLimitId'].patchValue(157); // set value to NONE
    }
  }

  checkTrailerInterchangeLimits() {
    this.disableTrailerType = (
      (this.f['trailerInterchangeLimitId'].value == null || this.f['trailerInterchangeLimitId'].value == "822") && 
      (this.f['nonOwnedTrailerLimitId'].value == null || this.f['nonOwnedTrailerLimitId'].value == "839")
    );
    if (this.disableTrailerType) {
      this.limitsData.isTrailerType = false;
      //this.f['trailerTypeLimitId'].patchValue(859);
    } else {
      this.limitsData.isTrailerType = true;
    }
  }

  get f() { return this.limitsData.limitsForm.limitsPageForm.controls; }
}
