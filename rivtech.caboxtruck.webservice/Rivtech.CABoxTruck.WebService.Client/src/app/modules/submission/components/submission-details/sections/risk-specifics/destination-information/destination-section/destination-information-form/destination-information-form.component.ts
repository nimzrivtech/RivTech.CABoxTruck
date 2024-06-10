import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RiskSpecificsConstants } from '../../../../../../../../../shared/constants/risk-specifics/risk-specifics.constants';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { ZipcodeService } from '../../../../../../../../../core/services/generics/zipcode.service';
import { DestinationInfoService } from '../../../../../../../../../core/services/submission/risk-specifics/destination-info.service';
import { BaseComponent } from '../../../../../../../../../shared/base-component';
import { CityStateZipDto, ICityStateZipDto } from '../../../../../../../../../shared/models/submission/riskspecifics/cityStateZip.dto';
import { DestinationDto, IDestinationDto } from '../../../../../../../../../shared/models/submission/riskspecifics/destinationDto';
import NotifUtils from '../../../../../../../../../shared/utilities/notif-utils';
import Utils from '../../../../../../../../../shared/utilities/utils';
import { DestinationData } from '../../../../../../../data/riskspecifics/destination.data';

@Component({
  selector: 'app-destination-information-form',
  templateUrl: './destination-information-form.component.html',
  styleUrls: ['./destination-information-form.component.scss']
})
export class DestinationInformationFormComponent extends BaseComponent implements OnInit {

  @ViewChild('btnCloseModal') btnCloseModal;
  submitted = false;
  isEdit = false;
  cityDisabled: boolean = false;
  destination$: Observable<IDestinationDto>;
  validCityStateZipCodes: CityStateZipDto[];
  riskDetailId = null;
  maskOptions = RiskSpecificsConstants.maskOptions.percentage;
  constructor(
    public destinationData: DestinationData,
    private destinationInfoService: DestinationInfoService,
    private route: ActivatedRoute,
    private zipcodeService: ZipcodeService
  ) {
    super();
  }

  @Output() updateDestinationList: EventEmitter<IDestinationDto> = new EventEmitter<IDestinationDto>();

  ngOnInit(): void {
    this.riskDetailId = this.route.parent.parent.parent.snapshot.paramMap.get('id');
    this.destination$ = this.destinationInfoService.selectedDestination$;
    this.destination$.subscribe((destination: IDestinationDto) => {
      this.cityDisabled = false;
      this.validCityStateZipCodes = [];
      if (destination && destination.id) {
        this.isEdit = true;
        this.destinationData.initiateFormGroup(destination);
        this.searchZipCode(true);
      } else {
        this.isEdit = false;
        this.destinationData.initiateFormGroup(destination ?? new DestinationDto());
      }
    });
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.destinationData.destinationForm; }

  onCancel() {
    this.fg.markAsUntouched();
  }

  onSubmit() {
    Utils.blockUI();
    const newDestination: DestinationDto = new DestinationDto(this.destinationData.destinationForm.value);

    let action: Observable<string>;
    if (this.isEdit) {
      newDestination.riskDetailId = this.riskDetailId;
      action = this.destinationInfoService.updateDestination(newDestination);
    } else {
      newDestination.riskDetailId = this.riskDetailId;
      action = this.destinationInfoService.addNewDestination(newDestination);
    }

    action.subscribe((savedDestinationId: string) => {
      newDestination.id = savedDestinationId;
      this.updateDestinationList.emit(newDestination);
    });

    this.btnCloseModal.nativeElement.click();
    this.destinationData.destinationForm.reset();
    Utils.unblockUI();
  }

  searchZipCode(fromEdit?: boolean) {
    this.resetCityState();
    const zipCode = this.fc.zipCode.value;
    if (zipCode === '') return;
    if (!fromEdit) Utils.blockUI();
    this.zipcodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(
      data => {
        Utils.unblockUI();
        this.validCityStateZipCodes = data;
        if (this.validCityStateZipCodes.length > 0) {
          this.fc.state.setValue(data[0].stateCode);
          if (this.validCityStateZipCodes.length === 1) {
            this.fc.city.setValue(data[0].city);
            this.cityDisabled = true;
          } else {
            this.fc.city.setValue(fromEdit ? this.fc.city.value : null);
          }
        } else {
          NotifUtils.showError('Zip code not found, contact Underwriting');
        }
      }
    )
  }

  private resetCityState() {
    this.validCityStateZipCodes = [];
    this.cityDisabled = false;
    this.fc.state.setValue('');
  }

}

