import { Component, OnInit, ViewChild , OnDestroy } from '@angular/core';
import { ProgramStateAgentDTO } from '../../../../../../shared/models/management/agent-management/programStateAgentDto';
import { BsModalRef, TabDirective, TabsetComponent } from 'ngx-bootstrap';
import { Subject } from 'rxjs-compat';
import { AgentData } from '../../../../../../modules/management/data/agent.data';
import { ProgramStateData } from '../../../../../../modules/management/data/program-state.data';
import { FormGroup } from '@angular/forms';
import { AgencyData } from '../../../../../../modules/management/data/agency.data';
import { AgencyLabelConstants } from '../../../../../../shared/constants/agency.label.constants';
import { UserNameValidator } from '../../../../../../modules/management/data/validators/usernameValidator';

@Component({
  selector: 'app-agency-agent-modal',
  templateUrl: './agency-agent-modal.component.html',
  styleUrls: ['./agency-agent-modal.component.scss']
})
export class AgencyAgentModalComponent implements OnInit , OnDestroy {
  @ViewChild('tab') tab: TabsetComponent;

  AgencyLabelConstants = AgencyLabelConstants;

  modalTitle: string;

  selectedHeading: string;

  rowId: number;
  agentItem: ProgramStateAgentDTO;
  isEdit: boolean;
  public onClose: Subject<boolean>;

  constructor(public bsModalRef: BsModalRef,
              public agentData: AgentData,
              public agencyData: AgencyData,
              private programStateData: ProgramStateData,
              private userNameValidator: UserNameValidator) { }

  get agentFormGroup(): FormGroup {
    return this.agentData.agentFormGroup;
  }

  get isValid() {
    return this.agentFormGroup.valid && this.agentData.agentSubAgenciesList?.find(x => x.isPrimary)?.id;
  }

  get isUpdated(): boolean {
    return (this.agentFormGroup.touched && this.agentFormGroup.dirty) || (this.agentData.isSubAgenciesUpdated || this.agentData.isAgentLicensesUpdated || this.agentData.isAddressesUpdated || this.agentData.isProgramStatesUpdated);
  }

  ngOnInit() {
    this.onClose = new Subject();
    this.initialize();
    this.agencyData.agentActiveTab = this.AgencyLabelConstants.detailsTab;

    if (this.isEdit) {
      //this.agentData.agentFormGroup.get('username').clearAsyncValidators();
      //this.agentData.agentFormGroup.get('username').updateValueAndValidity();
      this.agentData.getAgent(this.agentItem.agentId);
      this.initializeProgramState(false);
      //this.agentData.agentFormGroup.get('username').setAsyncValidators(this.userNameValidator.usernameValidator());
    } else {
      this.agentData.mapSubAgency();
      this.initializeProgramState(true);
      //this.agentData.agentFormGroup.get('username').clearAsyncValidators();
      //this.agentData.agentFormGroup.get('username').updateValueAndValidity();
      //this.agentData.agentFormGroup.get('username').setAsyncValidators(this.userNameValidator.usernameValidator());
    }
  }

  initialize(): void {
    this.agentFormGroup.reset();
    this.agentData.agentLicenseFormGroup.reset();
    this.agentFormGroup.get('isSystemUser').setValue(false);
    this.agentFormGroup.get('isActive').setValue(false);
    this.agentData.agentSubAgenciesList = [];
    this.agentData.agentLicenseList = [];
    this.agentFormGroup.get('username').enable();
  }

  initializeProgramState(defaultSelected?: boolean): void {
    this.programStateData.programStateList = this.programStateData.programStateListData.map(programState => {
      programState.isSelected = defaultSelected;
      return programState;
    });
  }

  closeDialog(): void {
    this.agentData.resetAgentUpdatesTrackers();
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

  save(): void {
    if (!this.isEdit) {
      this.processAdd();
    } else {
      this.processEdit();
    }

    this.agentData.isUserExist.subscribe(isUserExist => {
      if (!isUserExist) {
        this.onClose.next(true);
        this.agentFormGroup.reset();
        this.agentData.agentDetails = {};
        this.agentData.agentSubAgenciesList = [];
        this.agentData.agentSubAgenciesRowId = 0;
        this.agentData.agentLicenseList = [];
        this.agentData.agentLicenseRowId = 0;
        this.bsModalRef.hide();
      }
    });
  }

  processAdd(): void {
    this.agentData.saveUserAndAgent();
  }

  processEdit(): void {
    this.agentData.updateUserAndAgent();
  }

  onSelect(data: TabDirective): void {
    this.selectedHeading = data.heading;
    this.agencyData.agentActiveTab = data.heading;
  }

  ngOnDestroy(): void {
    this.userNameValidator.oldUserName = '';
  }

}
