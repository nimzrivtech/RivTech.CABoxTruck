import { Component, OnInit } from '@angular/core';
import { AgentData } from '../../../../../../../../modules/management/data/agent.data';
import { ProgramStateData } from '../../../../../../../../modules/management/data/program-state.data';

@Component({
  selector: 'app-agent-details-program-state',
  templateUrl: './agent-details-program-state.component.html',
  styleUrls: ['./agent-details-program-state.component.scss']
})
export class AgentDetailsProgramStateComponent implements OnInit {

  constructor(public programStateData: ProgramStateData, private agentData: AgentData) { }

  ngOnInit() {
  }

  addProgramState(programState: any, event?: any): void {
    const isSelected = event.target.checked;

    this.programStateData.programStateList.map(value => {
      if (value.id === programState.id) {
        value.isSelected = isSelected;
      }

      return value;
    });
    this.agentData.isProgramStatesUpdated = true;
  }

  isAllCheckBoxChecked(event: any): void {
    this.programStateData.programStateList.map(value => {
      value.isSelected = event?.target?.checked;
      return value;
    });
    this.agentData.isProgramStatesUpdated = true;
  }

}
