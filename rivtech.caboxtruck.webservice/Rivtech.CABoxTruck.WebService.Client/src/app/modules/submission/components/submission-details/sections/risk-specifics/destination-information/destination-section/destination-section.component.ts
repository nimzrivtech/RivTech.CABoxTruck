import { EventEmitter, Output, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { DestinationInfoService } from '../../../../../../../../core/services/submission/risk-specifics/destination-info.service';
import { DestinationDto, IDestinationDto } from '../../../../../../../../shared/models/submission/riskspecifics/destinationDto';
import Utils from '../../../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { RadiusOfOperationsData } from '../../../../../../../../modules/submission/data/riskspecifics/radius-of-operations.data';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { TableData } from '../../../../../../../../modules/submission/data/tables.data';
import { PagerService } from '../../../../../../../../core/services/pager.service';
import { DestinationData } from '../../../../../../../../modules/submission/data/riskspecifics/destination.data';
import { ZipcodeService } from '@app/core/services/generics/zipcode.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-destination-section',
  templateUrl: './destination-section.component.html',
  styleUrls: ['./destination-section.component.scss']
})
export class DestinationSectionComponent implements OnInit {

  destinations: IDestinationDto[] = [];
  selectedDestination$: Observable<IDestinationDto>;
  riskDetailId = null;
  navigationEnd;
  routePathParam;
  TableConstants = TableConstants;
  @ViewChild('btnAddDestination') btnAddDestination;

  pager: any = {};
  currentPage: number = 0;
  pagedItems: any[];
  loading: boolean;

  addNewItem: boolean = false;
  editItem: boolean = false;
  isEdit: boolean = false;
  selectedRow: number = -1;

  @Output() statesUpdated = new EventEmitter<string[]>();
  constructor(private destinationInfoService: DestinationInfoService,
    private route: ActivatedRoute,
    public submissionData: SubmissionData,
    public radiusOfOperationsData: RadiusOfOperationsData,
    private pagerService: PagerService,
    private zipcodeService: ZipcodeService,
    private tableData: TableData,
    private destinationData: DestinationData) { }

  ngOnInit() {
    this.renderDestinationList();
    this.selectedDestination$ = this.destinationInfoService.selectedDestination$;
    this.destinationData.initiateFormGroup(new DestinationDto());
  }

  renderDestinationList() {
    if (this.submissionData.isNew) { return; }
    this.riskDetailId = this.route.parent.parent.parent.snapshot.paramMap.get('id');
    this.destinationInfoService.getAllDestinations(this.riskDetailId).subscribe((destinations: IDestinationDto[]) => {
      this.destinations = destinations ? destinations : [];
      this.submissionData.riskDetail.riskSpecificDestinations = destinations;
      this.radiusOfOperationsData.destinationList = this.destinations;
      this.setPage(1, this.destinations);
      this.statesUpdated.emit(this.destinations.map(d => d.state));
    });
  }

  deleteDestination(destinationToDelete: IDestinationDto) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete 'Destination'?`, () => {
      Utils.blockUI();
      this.destinationInfoService.deleteDestination(destinationToDelete.id).subscribe((id: string) => {
        const NOT_FOUND = -1;
        const index = this.destinations.findIndex(destination => destination.id === id);
        if (index !== NOT_FOUND) {
          this.destinations.splice(index, 1);
          this.submissionData.riskDetail.riskSpecificDestinations = this.destinations;
          this.radiusOfOperationsData.destinationList = this.destinations;
          this.setPage(this.currentPage, this.destinations);
          this.statesUpdated.emit(this.destinations.map(d => d.state));
        }
        this.addNewItem = false;
        this.isEdit = false;
      });
      Utils.unblockUI();
    });

  }

  updateDestinationList(newDestination: IDestinationDto) {
    const NOT_FOUND = -1;
    let isNew = false;
    const index = this.destinations.findIndex(destination => destination.id === newDestination.id);
    if (index === NOT_FOUND) {
      this.destinations.push(newDestination);
      isNew = true;
    } else {
      this.destinations[index] = newDestination;
    }
    this.setPage(this.currentPage, this.destinations, isNew);
    this.statesUpdated.emit(this.destinations.map(d => d.state));
    this.submissionData.riskDetail.riskSpecificDestinations = this.destinations;
    this.radiusOfOperationsData.destinationList = this.destinations;
  }

  onCreateNewItem() {
    this.setValidationSource();
    this.addNewItem = true;
    if (this.pagedItems.length == 0) {
      // Default to 100% for initial
      this.destinationData.destinationForm.controls['city'].patchValue(this.submissionData.riskDetail.nameAndAddress?.cityOperations);
      this.destinationData.destinationForm.controls['state'].patchValue(this.submissionData.riskDetail.nameAndAddress?.stateOperations);
      this.destinationData.destinationForm.controls['zipCode'].patchValue(this.submissionData.riskDetail.nameAndAddress?.zipCodeOperations);
      this.destinationData.destinationForm.controls['percentageOfTravelGiven'].patchValue(100);
      this.destinationData.destinationForm.controls['percentageOfTravelActual'].patchValue(100);
    }
  }

  onEditItem(item, index) {
    if (this.addNewItem) { return; }
    this.setValidationSource(item.id);
    this.isEdit = true;
    this.editItem = item;
    this.selectedRow = index;
    this.fg.patchValue(item);
  }

  setValidationSource(id?: string) {
    let destinations = this.destinations;
    if (id) { destinations = this.destinations.filter(d => d.id !== id); }
    this.fc['totalPercentageOfTravelGiven'].patchValue(destinations.reduce((sum, current) => sum + Number(current.percentageOfTravelGiven), 0));
    this.fc['totalPercentageOfTravelActual'].patchValue(destinations.reduce((sum, current) => sum + Number(current.percentageOfTravelActual), 0));
    this.fc['currentDestinations'].patchValue(destinations.map(d => d.city?.toLowerCase()?.trim() + d.state + d.zipCode.trim()));
  }

  onCancel(value: any): void {
    NotifUtils.showConfirmMessage('You have unsaved changes that will be lost', () => {
        this.fg.reset();
        this.addNewItem = false;
        this.isEdit = false;
      },
      () => { }
    );
  }

  saveDestination() {
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
      this.updateDestinationList(newDestination);
      this.fg.reset();
      this.addNewItem = false;
      this.isEdit = false;
    });

    Utils.unblockUI();
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.destinationData.destinationForm; }
  get is100pct() {
    let sum = this.destinations?.reduce((acc, cur) => acc + Number(cur.percentageOfTravelActual), 0);
    sum = Math.round(sum * 100) / 100;
    return sum == 100;
  }

  sort(targetElement) {
    let sortData = this.tableData.sortPagedData(targetElement, this.destinations, this.currentPage);
    this.destinations = sortData.sortedData;
    this.pagedItems = sortData.pagedItems;
  }

  setPage(page: number, data: any[], isNew: boolean = false) {
    if (page < 1) {
      return;
    }

    this.pager = this.pagerService.getPager(data.length, page);

    if (isNew) {
      this.currentPage = this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }

      this.currentPage = page;
    }

    this.pagedItems = data.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  setCityStateZipCode() {
    const zipCode = this.destinationData.destinationForm.controls['zipCode'].value;
    if (zipCode !== '') {
      // Set City dromdownlist values
      Utils.blockUI();
      this.zipcodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(data => {
        // Map to dropdownList
        if (data.length > 0) {
          this.destinationData.destinationForm.controls['state'].patchValue(data[0].stateCode);
          Utils.unblockUI();
        } else {
          Utils.unblockUI();
          NotifUtils.showError('Zip code not found, contact Underwriting');
          this.resetZipCode();
        }
      });
    } else {
      this.resetZipCode();
    }
  }

  resetZipCode() {
    this.destinationData.destinationForm.controls['state'].patchValue(null);
    this.destinationData.destinationForm.controls['zipCode'].patchValue(null);
    this.destinationData.destinationForm.markAsTouched();
  }

  pctChanged() {
    const pctGiven = this.destinationData.destinationForm.controls['percentageOfTravelGiven'].value;
    if (pctGiven !== '') {
      this.destinationData.destinationForm.controls['percentageOfTravelActual'].patchValue(pctGiven);
    }
  }
}
