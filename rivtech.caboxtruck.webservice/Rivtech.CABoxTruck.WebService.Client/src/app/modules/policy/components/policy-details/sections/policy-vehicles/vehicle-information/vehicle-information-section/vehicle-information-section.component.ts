import { Component, Input, OnInit } from '@angular/core';
import { finalize, map, take, tap } from 'rxjs/operators';

import Utils from '../../../../../../../../shared/utilities/utils';
import FormUtils from '../../../../../../../../shared/utilities/form.utils';
import { VehicleData, VehicleInformationDropDowns } from '../../../../../../../../modules/submission/data/vehicle/vehicle.data';
import { VehicleDto } from '../../../../../../../../shared/models/submission/Vehicle/VehicleDto';
import { VehicleTextConstants } from '../../../../../../../../shared/constants/vehicle.text.constants';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { LimitsData } from '../../../../../../../../modules/submission/data/limits/limits.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { VehicleModel } from '@app/shared/models/submission/Vehicle/vehicle.model';

@Component({
  selector: 'app-vehicle-information-section',
  templateUrl: './vehicle-information-section.component.html',
  styleUrls: ['./vehicle-information-section.component.scss']
})

export class VehicleInformationSectionComponent extends BaseComponent implements OnInit {
  @Input() modalButton: string;
  messages = VehicleTextConstants.messages;
  lists: VehicleInformationDropDowns;
  initialRadiusTerritory: any;
  initialStateCode: any;

  currencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 0,
    nullable: true,
  };
  constructor(
    public vehicleData: VehicleData,
    public submissionData: SubmissionData,
    private policySummaryData: PolicySummaryData,
    private limitsData: LimitsData,
  ) {
    super();
  }

  ngOnInit(): void {
    this.vehicleData.fetchVehicleDatasets();
    this.vehicleData.riskDetailId = this.submissionData.riskDetail?.id;
    this.lists = this.vehicleData.vehicleDropdownsList;
    this.organizeVehicleDescriptionList(); // to trim labels for display
    this.form.get('year').valueChanges.subscribe(selectedYear => {
      this.onYearChange(selectedYear);
    });
    this.form.get('grossVehicleWeightId').valueChanges.subscribe(selectedWeightId => {
      this.convertGVWIdIfSupported(selectedWeightId);
    });

    // this.form.get('primaryOperatingZipCode').valueChanges.subscribe(zipCode => {
    //   if (zipCode) {
    //     const state = this.lists.zipCodeList.find(z => z.value == zipCode)?.state;
    //     this.fc.state.setValue(state);
    //   }
    // });
    this.form.get('primaryOperatingZipCode').valueChanges.subscribe(zipCode => {
      this.fc.primaryOperatingZipCode.setValue(zipCode);
    });this.form.get('state').valueChanges.subscribe(state => {
      this.fc.state.setValue(state);
    });

    const fireTheft = this.form.get('isCoverageFireTheft');
    fireTheft.valueChanges.subscribe(() => {
      this.toggleRequired(fireTheft.value, 'coverageFireTheftDeductibleId');
    });

    const collision = this.form.get('isCoverageCollision');
    collision.valueChanges.subscribe(() => {
      this.toggleRequired(collision.value, 'coverageCollisionDeductibleId');
    });

    if (!this.limitsData.hasCargo) {
      this.form.get('hasCoverageCargo').disable();
      this.form.get('hasCoverageRefrigeration').disable();
    }

    if (!this.limitsData.hasRefrigeration) {
      this.form.get('hasCoverageRefrigeration').disable();
    }

    if (!this.limitsData.hasComprehensive) {
      this.form.get('isCoverageFireTheft').disable();
    }

    if (!this.limitsData.hasCollision) {
      this.form.get('isCoverageCollision').disable();
    }

    console.log(this.lists.garagingAddressList);
    console.log(this.lists.oldGaragingAddressList);
  }

  onYearChange(selectedYear: number) {
    let age = this.vehicleData.recalculateAge(selectedYear);
    this.fc.age.setValue(age);
  }

  get form() { return this.fg; }
  get fc() { return this.vehicleData.form.controls; }
  get fg() { return this.vehicleData.form; }
  get coverage() { return this.limitsData.coverage; }

  onCancel() {
    this.fg.markAsUntouched();
  }

  onSubmit() {
    const vehicle = this.vehicleData.form.value;
    // Always Yes. Required to have liability
    vehicle.isAutoLiability = true;

    this.fg.markAllAsTouched();
    if (this.fg.valid) {
      if (vehicle.id == null) {
        vehicle.options = this.vehicleData.getOptionQuote;
        vehicle.isValidated = true;
        this.addVehicle(vehicle);
      } else {
        vehicle.isValidated = true;
        this.updateVehicle(vehicle);
      }
    }
  }

  toggleCoverage(coverageName: string, checked: boolean) {
    if (coverageName === 'FireTheft') {
      let compFireTheftDeductibleId = checked ? this.coverage?.compDeductibleId ?? this.coverage?.fireDeductibleId : null; 
      this.form.patchValue({
        coverageFireTheftDeductibleId: compFireTheftDeductibleId,
      });
    } else if (coverageName === 'Collision') {
      let collDeductibleId = checked ? this.coverage && this.coverage.collDeductibleId : null;
      this.form.patchValue({
        coverageCollisionDeductibleId: collDeductibleId,
      });
    }

    this.vehicleData.checkStatedAmountLimits();
    this.vehicleData.form.get('statedAmount').updateValueAndValidity();
  }

  toggleCargo(checked: boolean) {
    if (!checked || !this.limitsData.hasRefrigeration) {
      this.fc.hasCoverageRefrigeration.setValue(false);
      this.fc.hasCoverageRefrigeration.disable();
    } else {
      this.fc.hasCoverageRefrigeration.enable();
    }
  }

  private addVehicle(vehicle: VehicleDto) {
    Utils.blockUI();
    vehicle.riskDetailId = this.vehicleData.riskDetailId;

    vehicle.statedAmount = vehicle.statedAmount ?? 0;

    this.vehicleData.insert(vehicle).pipe(
      map(data => data as VehicleDto),
      tap((data: any) => {
        (data as VehicleDto).isMainRow = true;
        this.vehicleData.vehicleInfoList.push(data);
      }),
      finalize(() => Utils.unblockUI())
    ).subscribe(() => {
      this.vehicleData.renderPolicyVehicleList(true, true);
      this.policySummaryData.updateSubtatusForPendingEndorsement();
      this.policySummaryData.setVehicleTypeHeader();
    }, (error) => {
      console.error(this.messages.errorAdd, error);
    });
  }

  private updateVehicle(vehicle: VehicleDto) {
    Utils.blockUI();
    vehicle.isEndorsement = true;
    vehicle.statedAmount = vehicle.statedAmount ?? 0;
    this.vehicleData.update(vehicle)
      .pipe(finalize(() => Utils.unblockUI()))
      .subscribe(() => {
        this.vehicleData.renderPolicyVehicleList();
        this.policySummaryData.updateSubtatusForPendingEndorsement();
        this.policySummaryData.setVehicleTypeHeader();
      }, (error) => {
        console.error(this.messages.errorUpdate, error);
      });
  }

  toggleRequired(required: boolean, targetControlName: string) {
    if (required) {
      FormUtils.addRequiredValidator(this.form, targetControlName);
    } else {
      FormUtils.clearValidator(this.form, targetControlName);
    }
  }

  private organizeVehicleDescriptionList() {
    this.lists.vehicleDescriptionList?.forEach((vehicleDescription) => {
      vehicleDescription.label = vehicleDescription.label.trim();
      vehicleDescription.value = vehicleDescription.value;
    });
  }

  get isViewOnly(): boolean {
    return this.modalButton === 'View';
  }

  public convertGVWIdIfSupported(selectedWeightId) {
    const isSupported = this.submissionData.raterVersionSupport();
    let mapWeightId;
    if (isSupported) {
      switch (selectedWeightId) {
        case '1':
        case 1:
          mapWeightId = '5';
          break;
        case '2':
        case 2:
          mapWeightId = '6';
          break;
        case '3':
        case 3:
          mapWeightId = '7';
          break;
        case '4':
        case 4:
          mapWeightId = '8';
          break;
      }
    }
    else {
      mapWeightId = selectedWeightId;
    }

    this.fc.grossVehicleWeightCategoryId.setValue(mapWeightId);
    console.log(selectedWeightId, typeof selectedWeightId);
    console.log(mapWeightId, typeof mapWeightId);
  }

  onRadiusTerritoryChange(event: Event){
    //@ts-ignore:next-line
    this.formControl.setValue(this.component.vehicleData.currentSelectedVehicle.radiusTerritory, { emitEvent: false });
  }

  onStateCodeChange(event: Event){
    this.fc.state.setValue(this.vehicleData.currentSelectedVehicle.state, { emitEvent: false });
  }
}
