import { Component, Input, OnInit } from '@angular/core';
import { VehicleDto } from '../../../../../../../../shared/models/submission/Vehicle/VehicleDto';
import { ToastrService } from 'ngx-toastr';
import { VehicleData } from '../../../../../../../../modules/submission/data/vehicle/vehicle.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { map, tap } from 'rxjs/operators';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
@Component({
  selector: 'app-vehicles-excel-upload',
  templateUrl: './vehicles-excel-upload.component.html',
  styleUrls: ['./vehicles-excel-upload.component.scss']
})

export class VehiclesExcelUploadComponent implements OnInit {

  @Input() excelVehicles: VehicleDto[] = [];

  constructor(
    private vehicleData: VehicleData,
    private submissionData: SubmissionData,
    private toastr: ToastrService,
    private PolicySummaryData: PolicySummaryData) {

  }

  ngOnInit(): void {
  }

  onSubmit() {
    if (!this.excelVehicles?.length) {
      return this.toastr.success('No Record found to be saved!', 'Success!');
    }

    if (!this.vehicleData.vehicleDropdownsList?.vehicleDescriptionList || this.vehicleData.vehicleDropdownsList.vehicleDescriptionList.length === 0) {
      this.vehicleData.fetchVehicleDescription();
    }

    this.excelVehicles.forEach((o: any) => {
      o.riskDetailId = this.submissionData.riskDetail.id;
      o.registeredState = this.vehicleData.vehicleDropdownsList.stateList.find(f => f.value.toLowerCase() === o.stateFull?.toLowerCase() || f.label.toLowerCase() === o.stateFull?.toLowerCase())?.value;
      o.vehicleDescriptionId = this.vehicleData.vehicleDropdownsList?.vehicleDescriptionList?.find(d => d.label.toLowerCase() === o.description?.trim()?.toLowerCase())?.value,
      o.vehicleBusinessClassId = this.vehicleData.vehicleDropdownsList.vehicleBusinessClassList.find(d => d.label.toLowerCase() === o.businessClass?.trim()?.toLowerCase())?.value;
    });

    this.vehicleData.insertVehicles(this.excelVehicles).pipe(
      map(data => data as VehicleDto[]),
      tap((data: any) => {
        (data as VehicleDto[]).forEach(vehicle => {
          vehicle.isMainRow = true;
          vehicle = this.vehicleData.formatIdsToText(vehicle);
          this.vehicleData.vehicleInfoList.push(vehicle as any);
        });
      }),
    ).subscribe(data => {
      this.PolicySummaryData.setVehicleTypeHeader();
      this.vehicleData.setPage(this.vehicleData.currentPage, true);
      this.toastr.success('Upload Successful!', 'Success!');
    }, (error) => {
      this.toastr.error('Error encounter while saving the data. Please try again.', 'Failed!');
    });
  }

}
