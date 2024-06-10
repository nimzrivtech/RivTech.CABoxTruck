import { Component, OnInit } from '@angular/core';
import { ProgramStateConstants } from '../../../../../shared/constants/program-state.constants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import { TableConstants } from '../../../../../shared/constants/table.constants';
import { ProgramStateData } from '../../../data/program-state.data';
import { AgencyProgramStateModalComponent } from './agency-program-state-modal/agency-program-state-modal.component';
import NotifUtils from '../../../../../shared/utilities/notif-utils';

@Component({
  selector: 'app-agency-program-state',
  templateUrl: './agency-program-state.component.html',
  styleUrls: ['./agency-program-state.component.scss']
})
export class AgencyProgramStateComponent implements OnInit {
  modalRef: BsModalRef;
  tableConstants = TableConstants;
  genericLabel = GenericLabel;
  programStateConstants = ProgramStateConstants;

  constructor(
    public programStateData: ProgramStateData,
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    this.programStateData.getProgramStatesList();
    this.programStateData.getProgramStates();
  }

  addProgramState(): void {
    this.modalRef = this.modalService.show( AgencyProgramStateModalComponent, {
      initialState: {
        modalTitle: this.programStateConstants.programStateAddModalTitle,
      },
      class: 'modal-lg',
      backdrop: 'static',
      keyboard: false,
      ignoreBackdropClick: true,
    });

    this.modalRef.content.event.subscribe((res: Array<any>) => {
      if (res.length > 0) {
        if(this.programStateData.programStateListData.length === 0){
          this.programStateData.programStateListData = res;
        } else {
          res.forEach(a => {
            if(!this.programStateData.programStateListData.some(b => b.id === a.id)){
              this.programStateData.programStateListData.push(a);
            }
          });
        }
      }
    });
  }

  editProgramState(item?: any): void {
    NotifUtils.showConfirmMessage(`${this.programStateConstants.updateProgramStateConfirmationMessage} "${item.isActive ? this.programStateConstants.programStateStatus.active : this.programStateConstants.programStateStatus.inactive}" to "${!item.isActive ? this.programStateConstants.programStateStatus.active : this.programStateConstants.programStateStatus.inactive}"?`,
      () => {
        this.programStateData.updateProgramState(item.id, item.isActive);
      },
      () => { }
    );
  }

  deleteProgramState(id: string): void {
    NotifUtils.showConfirmMessage(`${this.programStateConstants.deleteProgramStateConfirmationMessage}`,
      () => {
        this.programStateData.deleteProgramState(id);
      },
      () => { }
    );
  }
}
