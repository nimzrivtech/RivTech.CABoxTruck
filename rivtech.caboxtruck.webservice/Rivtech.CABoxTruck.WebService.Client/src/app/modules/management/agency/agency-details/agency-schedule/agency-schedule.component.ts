import { Component, Input, OnInit } from '@angular/core';
import { AgencyData } from '../../../../../modules/management/data/agency.data';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import { TableConstants } from '../../../../../shared/constants/table.constants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-agency-schedule',
  templateUrl: './agency-schedule.component.html',
  styleUrls: ['./agency-schedule.component.scss']
})
export class AgencyScheduleComponent implements OnInit {
  @Input() scheduleList: any[];
  @Input() scheduleRowId: number;
  @Input() scheduleHeader: string[];
  @Input() isLoading: boolean;

  isScheduleHidden: boolean = false;
  TableConstants = TableConstants;
  genericLabel = GenericLabel;

  constructor(public agencyData: AgencyData
              , private modalRef: BsModalRef
              , private modalService: BsModalService) { }

  ngOnInit() {
  }

  toggleFlatCommission(): void {
    this.isScheduleHidden = !this.isScheduleHidden;
  }

  addFlatCommission(): void {
  }

  editFlatCommission(item?): void {
  }

  showFlatCommissionModal(component?, initialState?): BsModalRef {
    return this.modalService.show(component, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
  }

  deleteSchedule(item?): void {
  }

  filter(item?: any): boolean {
    return item.removedProcessDate === null || item.removedProcessDate === undefined;
  }

}
