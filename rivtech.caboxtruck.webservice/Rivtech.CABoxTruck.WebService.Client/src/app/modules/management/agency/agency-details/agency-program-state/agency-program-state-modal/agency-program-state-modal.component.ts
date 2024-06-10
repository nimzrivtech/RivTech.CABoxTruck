import { Component, EventEmitter, OnInit } from '@angular/core';
import { ProgramStateConstants } from '../../../../../../shared/constants/program-state.constants';
import { BsModalRef } from 'ngx-bootstrap';
import { ProgramStateData } from '../../../../data/program-state.data';
import { environment } from '../../../../../../../environments/environment';
import { SearchDTO } from '../../../../../../shared/models/management/search.dto';
import Utils from '../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../shared/utilities/notif-utils';
import { AgencySubAgencyService } from '../../../../../../core/services/management/agency-sub-agency.service';

@Component({
  selector: 'app-agency-program-state-modal',
  templateUrl: './agency-program-state-modal.component.html',
  styleUrls: ['./agency-program-state-modal.component.scss']
})
export class AgencyProgramStateModalComponent implements OnInit {
  public event: EventEmitter<any> = new EventEmitter<any>();
  modalTitle: string;
  programStateConstants = ProgramStateConstants;

  constructor(
    public programStateData: ProgramStateData,
    private subAgencyService: AgencySubAgencyService,
    public modalRef: BsModalRef
  ) { }

  ngOnInit() {
    this.checkExistingProgramStates(this.programStateData.programStateListData);
  }

  saveProgramStateModal(): void {
    this.event.emit(this.programStateData.addedProgramState);
    this.programStateData.saveAddedProgramStates();
    this.closeProgramStateModal();
  }

  closeProgramStateModal(): void {
    this.programStateData.addedProgramState = [];
    this.modalRef.hide();
    this.resetCheckBoxes();
  }

  onSearch(): void { }

  isAllCheckBoxChecked(event: any): void {
    this.programStateData.programStatesModalList.forEach(item => {
      const isExisting: boolean = this.programStateData.programStateListData.some(a => a.programState.id === item.id);
      item.isSelected = event.target.checked;

      if (event?.target?.checked) {
        if (!isExisting) {
          this.addProgramState(item, event);
        }
      } else {
        this.programStateData.addedProgramState = [];
      }
    });
  }

  checkExistingProgramStates(currentProgramStates: any[]): void {
    this.programStateData.programStatesModalList.forEach(item => {
      const isExisting: boolean = currentProgramStates.some(a => a.programState.id === item.id);

      if (isExisting) {
        item.isSelected = true;
        this.programStateData.addedProgramState.push(item);
      }
    });
  }

  resetCheckBoxes(): void {
    this.programStateData.programStatesModalList.forEach(item => {
      item.isSelected = false;
    });
  }

  addProgramState(programState: any, event: any): void {
    const isExisting: boolean = this.programStateData.addedProgramState.some(a => a.id === programState.id);

    if (event?.target?.checked) {
      if (isExisting) {
        this.programStateData.addedProgramState.forEach((obj, i) => {
          if (obj.id === programState.id) {
            this.programStateData.addedProgramState.splice(i, 1);
          }
        });
      } else {
        programState.isSelected = true;
        this.programStateData.addedProgramState.push(programState);
      }
    } else {
      this.programStateData.addedProgramState.forEach((obj, i) => {
        if (obj.id === programState.id) {
          this.programStateData.addedProgramState.splice(i, 1);
          programState.isSelected = false;
        }
      });
    }
  }

  resetProgramStateModalList() {
    this.programStateData.programStatesModalList = [];
  }

  searchRecords(input: string): void {
    const payload: SearchDTO = {
      term: input
    };
    Utils.unblockUI();
    this.resetProgramStateModalList();
    this.subAgencyService.postSearchProgramStateModalList(payload).subscribe((data: any) => {
      this.programStateData.programStatesModalList = data;
      this.programStateData.statesList = this.programStateData.programStatesModalList.map(x => x.stateCode)
        .filter((value, index, self) => self.indexOf(value) === index);
      this.programStateData.isProgramStateLoading = false;
      Utils.unblockUI();
    }, error => {
      Utils.unblockUI();
      NotifUtils.showError(error.message);
    });
  }

}
