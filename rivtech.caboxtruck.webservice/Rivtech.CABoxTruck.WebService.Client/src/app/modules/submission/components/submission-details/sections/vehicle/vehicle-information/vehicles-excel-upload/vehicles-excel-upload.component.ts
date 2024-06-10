import { Component, Input, OnInit } from '@angular/core';
import { VehicleData } from '../../../../../../data/vehicle/vehicle.data';
import { VehicleDto } from '../../../../../../../../shared/models/submission/Vehicle/VehicleDto';
import { VehicleModel } from '../../../../../../../../shared/models/submission/Vehicle/vehicle.model';
import { ToastrService } from 'ngx-toastr';
import { map, tap } from 'rxjs/operators';
import { SummaryData } from '../../../../../../../../modules/submission/data/summary.data';

@Component({
  selector: 'app-vehicles-excel-upload',
  templateUrl: './vehicles-excel-upload.component.html',
  styleUrls: ['./vehicles-excel-upload.component.scss']
})

export class VehiclesExcelUploadComponent implements OnInit {

  @Input() excelVehicles: VehicleDto[] = [];

  constructor(
    private vehicleData: VehicleData, 
    private toastr: ToastrService,
    private summaryData: SummaryData) { }

  ngOnInit(): void {
  }

  onSubmit() {
    if (!this.excelVehicles?.length) {
      return this.toastr.success('No Record found to be saved!', 'Success!');
    }



    this.excelVehicles.forEach(o => {
      const vehDefault = this.vehicleData.setDefaults(o);
      o.registeredState = this.vehicleData.isEmpty(o.stateFull) ? vehDefault.registeredState : this.vehicleData.vehicleDropdownsList.stateList.find(f => f.value.toLowerCase() === o.stateFull?.toLowerCase() || f.label.toLowerCase() === o.stateFull?.toLowerCase())?.value;
      o.vehicleDescriptionId = this.vehicleData.checkVehicleDescriptionIfSupported(o.description);
      o.vehicleBusinessClassId = this.vehicleData.vehicleDropdownsList.vehicleBusinessClassList.find(d => d.label.toLowerCase() === o.businessClass?.trim()?.toLowerCase())?.value === undefined ?
        vehDefault.vehicleBusinessClassId :
        this.vehicleData.vehicleDropdownsList.vehicleBusinessClassList.find(d => d.label.toLowerCase() === o.businessClass?.trim()?.toLowerCase())?.value;
      o.radius = isNaN(o.radius) || this.isEmpty(o.radius) ? vehDefault.radius : o.radius;
      o.coverageFireTheft = this.coverageValidation(o.coverageFireTheft, vehDefault.isCoverageFireTheft);
      o.coverageFireTheftDeductibleId = o.coverageFireTheftDeductibleId ?? vehDefault.coverageFireTheftDeductibleId;
      o.coverageCollisionDeductibleId = o.coverageCollisionDeductibleId ?? vehDefault.coverageCollisionDeductibleId;
      o.coverageFireTheftDesc = o.coverageFireTheft.valueOf() == "Yes" ? "" : this.vehicleData.vehicleDropdownsList.comprehensiveDeductibleList.find(d => d.value === o.coverageFireTheftDeductibleId) === undefined ?
        this.vehicleData.vehicleDropdownsList.comprehensiveDeductibleList.find(d => d.value === vehDefault.coverageFireTheftDeductibleId)?.label :
        this.vehicleData.vehicleDropdownsList.comprehensiveDeductibleList.find(d => d.value === o.coverageFireTheftDeductibleId)?.label;
      o.coverageCollisionDeductible = o.coverageFireTheft.valueOf() == "Yes" ? "" : this.vehicleData.vehicleDropdownsList.collisionDeductibleList.find(d => d.value === o.coverageCollisionDeductibleId) === undefined ?
        this.vehicleData.vehicleDropdownsList.collisionDeductibleList.find(d => d.value === vehDefault.coverageCollisionDeductibleId)?.label :
        this.vehicleData.vehicleDropdownsList.collisionDeductibleList.find(d => d.value === o.coverageCollisionDeductibleId)?.label;
      o.grossVehicleWeightId = this.vehicleData.vehicleDropdownsList.grossVehicleWeightList.find(d => d.value === o.grossVehicleWeightId)?.label === undefined ?
        this.vehicleData.vehicleDropdownsList.grossVehicleWeightList[0].value :
        this.vehicleData.vehicleDropdownsList.grossVehicleWeightList.find(d => d.value === o.grossVehicleWeightId)?.value;
      o.isCoverageFireTheft = o.coverageFireTheft.valueOf() == "Yes" ? true : false;
      o.coverageCollision = this.coverageValidation(o.coverageCollision, vehDefault.isCoverageCollision);
      o.isCoverageCollision = o.coverageCollision.valueOf() == "Yes" ? true : false;
      o.coverageRefrigeration = this.coverageValidation(o.coverageRefrigeration, vehDefault.hasCoverageRefrigeration);
      o.coverageCargo = this.coverageValidation(o.coverageCargo, vehDefault.hasCoverageCargo);
      o.hasCoverageCargo = o.coverageCargo.valueOf() == "Yes";
      o.hasCoverageRefrigeration = o.hasCoverageCargo == false ? false : o.coverageRefrigeration.valueOf() == "Yes";
      o.vehicleType = this.isEmpty(o.vehicleType) || this.vehicleData.vehicleDropdownsList.vehicleTypeList.find(f => f.value.toLowerCase() === o.vehicleType.toLowerCase()) === undefined ? this.vehicleData.vehicleDropdownsList.vehicleTypeList[0].value : o.vehicleType;

    });

    this.vehicleData.insertVehicles(this.excelVehicles).pipe(
      map(data => data as VehicleDto[]),
      tap((data: any) => {
        (data as VehicleDto[]).forEach(vehicle => {
          vehicle = this.formatIddropdown(vehicle);
          this.vehicleData.vehicleInfoList.push(vehicle as any);
        });
        this.vehicleData.renderVehicleList();
        
      }),
    ).subscribe(data => {
      this.summaryData.setVehicleTypeHeader();
      this.vehicleData.setPage(this.vehicleData.currentPage, true);
      this.toastr.success('Upload Successful!', 'Success!');
    }, (error) => {
      this.toastr.error('Error encounter while saving the data. Please try again.', 'Failed!');
    });

  }

  private isEmpty(str) {
    return (!str || str.length === 0 );
  }

  private coverageValidation(coverage: string, hasCoverage: boolean = false) {
    var retVal: string;
    if (this.isEmpty(coverage)){
      retVal = hasCoverage ? "Yes" : "No";
    }
    else if(!hasCoverage)
    {
      retVal= "No"
    }
    else {
      if (coverage.toLocaleUpperCase() === "YES"|| coverage.toLocaleUpperCase() === "NO")
        retVal = coverage;
      else
        retVal = hasCoverage ? "Yes" : "No";
    }
    return retVal;
  }

  private formatIddropdown(vehicle: any) {
    const lists = this.vehicleData.vehicleDropdownsList;

    if (!this.vehicleData.vehicleDropdownsList?.vehicleDescriptionList || this.vehicleData.vehicleDropdownsList.vehicleDescriptionList.length === 0) {
      this.vehicleData.fetchVehicleDescription();
    }

    vehicle.vehicleDescription = lists.vehicleDescriptionList?.find(vd => vd.value === vehicle.vehicleDescriptionId)?.label.trim();
    vehicle.stateCode = lists.garagingAddressList?.find(g => g.value === vehicle.garagingAddressId)?.state;
    vehicle.grossVehicleWeight = lists.grossVehicleWeightList?.find(vd => vd.value === vehicle.grossVehicleWeight)?.label
    vehicle.coverageFireTheftDeductibleId = lists.comprehensiveDeductibleList?.find(vd => vd.value === vehicle.coverageFireTheftDeductibleId)?.label
    vehicle.coverageCollisionDeductibleId = lists.collisionDeductibleList?.find(vd => vd.value === vehicle.coverageCollisionDeductibleId)?.label
    vehicle.radius = lists.radiusList?.find(vd => vd.value === vehicle.radius)?.label
    return vehicle;
  }

  private vehicleDescriptionParser() {
    const vehicleDescriptionListV7 = this.vehicleData.vehicleDropdownsList.vehicleDescriptionListV7.map(item => item);
    const vehicleDescriptionListBeforeV7 = this.vehicleData.vehicleDropdownsList.vehicleDescriptionListBeforeV7.map(item => item);
    const mergedVehicleDescriptionList = vehicleDescriptionListV7.concat(vehicleDescriptionListBeforeV7);
    return mergedVehicleDescriptionList;
  }
}
