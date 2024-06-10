import { Component, OnInit } from '@angular/core';
import { LimitsData } from '../../../../../../../../../modules/submission/data/limits/limits.data';

@Component({
  selector: 'app-auto-liability-chart',
  templateUrl: './auto-liability-chart.component.html',
  styleUrls: ['./auto-liability-chart.component.scss']
})
export class AutoLiabilityChartComponent implements OnInit {
  constructor(public limitsData: LimitsData) { }

  ngOnInit() {
  }

  setTooltip(field, dataList) {
    if (field && dataList) {
      const data = dataList.find(x => x.value == field);
      if (data) {
        return data.label;
      }
    }
  }

  setValue(name) {
    // Temp Hardcoded Id=1 in DB No Coverage
    const inputValue = this.limitsData.limitsForm.limitsPageForm.controls[name].value;
    if (name === 'compDeductibleId' && this.limitsData.limitsForm.limitsPageForm.controls['comprehensiveFire'].value === false) {
      this.limitsData.isComprehensive = true;
      if (inputValue === '1' || inputValue == null) {
        this.limitsData.isComprehensive = false;
      }
      return;
    }

    if (inputValue === '1' || inputValue == null) {
      switch (name) {
        case 'compDeductibleId':
          this.limitsData.isComprehensive = false;
          break;
        case 'collDeductibleId':
          this.limitsData.isCollision = false;
          break;
      }
      return;
    }

    switch (name) {
      case 'compDeductibleId':
        this.limitsData.isComprehensive = true;
        break;
      case 'collDeductibleId':
        this.limitsData.isCollision = true;
        break;
    }
  }

  get f() { return this.limitsData.limitsForm.limitsPageForm.controls; }

  changeAL(isChange: boolean = false) {
    this.limitsData.checkAndSetPdDisplayAndValue(isChange);
    // this.limitsData.checkAndSetCargoAndReeferSymbolValue(true);
    this.limitsData.disableSymbol('autoBILimit');
  }

  // disableSymbol(fieldName) {
  //   switch (fieldName) {
  //     case 'umbiLimit':
  //       this.disableUnInsuredMotoristBI = this.limitsData.umBiLimitList.find(x => x.value == this.f['umbiLimitId'].value).label == 'NONE';
  //       break;
  //     case 'umpdLimit':
  //       this.disableUnInsuredMotoristPD = this.limitsData.umPdLimitList.find(x => x.value == this.f['umpdLimitId'].value).label == 'NONE';
  //       // this.disableUnInsuredMotoristPD = this.f['umpdLimitId'].value == 509;
  //       break;
  //     case 'uimbiLimit':
  //       this.disableUnderinsuredMotoristBI = this.limitsData.uimBiLimitList.find(x => x.value == this.f['uimbiLimitId'].value).label == 'NONE';
  //       // this.disableUnderinsuredMotoristBI = this.f['uimbiLimitId'].value == 243;
  //       break;
  //     case 'uimpdLimit':
  //       this.disableUnderinsuredMotoristPD = this.limitsData.uimPdLimitList.find(x => x.value == this.f['uimpdLimitId'].value).label == 'NONE';
  //       // this.disableUnderinsuredMotoristPD = this.f['uimpdLimitId'].value == 298;
  //       break;
  //     case 'medPayLimit':
  //       this.disableMedicalPayments = this.limitsData.medpayList.find(x => x.value == this.f['medPayLimitId'].value).label == 'NONE';
  //       // this.disableMedicalPayments = this.f['medPayLimitId'].value == 110;
  //       break;
  //     case 'pipLimit':
  //       this.disablePersonalInjuryProtection = this.limitsData.pipList.find(x => x.value == this.f['pipLimitId'].value).label == 'NONE';
  //       // this.disablePersonalInjuryProtection = this.f['pipLimitId'].value == 130;
  //       break;
  //     case 'compDeductible':
  //       // this.disableCompDeductible = this.limitsData.ComprehensiveList.find(x => x.value == this.f['compDeductibleId'].value).label == 'NONE';
  //       this.disableCompDeductible = this.f['compDeductibleId'].value == 76;
  //       if (this.disableCompDeductible) {
  //         this.f['compDeductibleSymbolId'].patchValue(null);
  //       } else {
  //         this.f['compDeductibleSymbolId'].patchValue(10);
  //       }
  //       break;
  //     case 'collDeductible':
  //       // this.disableCollDeductible = this.limitsData.CollisionList.find(x => x.value == this.f['collDeductibleId'].value).label == 'NONE';
  //       this.disableCollDeductible = this.f['collDeductibleId'].value == 65;
  //       if (this.disableCollDeductible) {
  //         this.f['collDeductibleSymbolId'].patchValue(null);
  //       } else {
  //         this.f['collDeductibleSymbolId'].patchValue(10);
  //       }
  //       break;
  //     // case 'checkAll':
  //     //   this.disableUnInsuredMotoristBI = this.f['umbiLimitId'].value == 388;
  //     //   this.disableUnInsuredMotoristPD = this.f['umpdLimitId'].value == 509;
  //     //   this.disableUnderinsuredMotoristBI = this.f['uimbiLimitId'].value == 243;
  //     //   this.disableUnderinsuredMotoristPD = this.f['uimpdLimitId'].value == 298;
  //     //   this.disableMedicalPayments = this.f['medPayLimitId'].value == 110;
  //     //   this.disablePersonalInjuryProtection = this.f['pipLimitId'].value == 130;
  //     //   this.disableCompDeductible = this.f['compDeductibleId'].value == 76;
  //     //   this.disableCollDeductible = this.f['collDeductibleId'].value == 65;
  //     //   break;
  //   }
  // }

  // disableSymbolAll() {
  //   this.disableSymbol('umbiLimit');
  //   this.disableSymbol('umpdLimit');
  //   this.disableSymbol('uimbiLimit');
  //   this.disableSymbol('uimpdLimit');
  //   this.disableSymbol('medPayLimit');
  //   this.disableSymbol('pipLimit');
  //   this.disableSymbol('compDeductible');
  //   this.disableSymbol('collDeductible');
  // }
}
