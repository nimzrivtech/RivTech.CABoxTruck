import { Component, OnInit, ViewChild , OnDestroy } from '@angular/core';
import { ProgramStateAgentDTO } from '../../../../../../shared/models/management/agent-management/programStateAgentDto';
import { BsModalRef, TabDirective, TabsetComponent } from 'ngx-bootstrap';
import { Subject } from 'rxjs-compat';
import { ProgramStateData } from '../../../../../../modules/management/data/program-state.data';
import { FormGroup } from '@angular/forms';
import { AgencyData } from '../../../../../../modules/management/data/agency.data';
import { AgencyLabelConstants } from '../../../../../../shared/constants/agency.label.constants';
import { UserNameValidator } from '../../../../../../modules/management/data/validators/usernameValidator';
import { RetailerAgentData } from '@app/modules/management/data/retailer-agent.data';
import { RetailerData } from '@app/modules/management/data/retailer.data';

@Component({
  selector: 'app-retailer-agent-modal',
  templateUrl: './retailer-agent-modal.component.html',
  styleUrls: ['./retailer-agent-modal.component.scss']
})
export class RetailerAgentModalComponent implements OnInit , OnDestroy {
  @ViewChild('tab') tab: TabsetComponent;

  AgencyLabelConstants = AgencyLabelConstants;

  modalTitle: string;

  selectedHeading: string;

  rowId: number;
  agentItem: ProgramStateAgentDTO;
  isEdit: boolean;
  public onClose: Subject<boolean>;

  constructor(public bsModalRef: BsModalRef,
              public retailerAgentData: RetailerAgentData,
              public agencyData: AgencyData,
              public retailerData: RetailerData,
              private programStateData: ProgramStateData,
              private userNameValidator: UserNameValidator) { }

  get agentFormGroup(): FormGroup {
    return this.retailerAgentData.agentFormGroup;
  }

  get isValid() {
    return this.agentFormGroup.valid;
  }

  get isUpdated(): boolean {
    //return (this.agentFormGroup.touched && this.agentFormGroup.dirty) || (this.retailerAgentData.isSubAgenciesUpdated || this.retailerAgentData.isAgentLicensesUpdated || this.retailerAgentData.isAddressesUpdated || this.retailerAgentData.isProgramStatesUpdated);
    return true;
  }

  ngOnInit() {
    this.onClose = new Subject();
    this.initialize();
    this.agencyData.agentActiveTab = this.AgencyLabelConstants.detailsTab;

    if (this.isEdit) {
      this.retailerAgentData.getAgent(this.agentItem.id);
      this.initializeProgramState(false);
    } else {
      this.retailerAgentData.mapSubAgency();
      this.initializeProgramState(true);
    }
  }

  initialize(): void {
    this.agentFormGroup.reset();
    this.retailerAgentData.agentLicenseFormGroup.reset();
    this.agentFormGroup.get('isSystemUser').setValue(false);
    this.agentFormGroup.get('isActive').setValue(true);
    this.retailerAgentData.agentSubAgenciesList = [];
    this.retailerAgentData.agentLicenseList = [];
    this.agentFormGroup.get('username').enable();
  }

  initializeProgramState(defaultSelected?: boolean): void {
    this.programStateData.programStateList = this.programStateData.programStateListData.map(programState => {
      programState.isSelected = defaultSelected;
      return programState;
    });
  }

  closeDialog(): void {
    this.retailerAgentData.resetAgentUpdatesTrackers();
    this.onClose.next(false);
    this.bsModalRef.hide();
  }

  save(): void {
    if (!this.isEdit) {
      this.processAdd();
    } else {
      this.processEdit();
    }

    this.retailerAgentData.isUserExist.subscribe(isUserExist => {
      if (!isUserExist) {
        this.onClose.next(true);
        this.agentFormGroup.reset();
        this.retailerAgentData.agentDetails = {};
        this.retailerAgentData.agentSubAgenciesList = [];
        this.retailerAgentData.agentSubAgenciesRowId = 0;
        this.retailerAgentData.agentLicenseList = [];
        this.retailerAgentData.agentLicenseRowId = 0;
        this.bsModalRef.hide();
      }
    });
  }

  processAdd(): void {
    this.retailerAgentData.saveUserAndAgent(this.retailerData.agencyId);
  }

  processEdit(): void {
    this.retailerAgentData.updateUserAndAgent();
  }

  onSelect(data: TabDirective): void {
    this.selectedHeading = data.heading;
    this.agencyData.agentActiveTab = data.heading;
  }

  ngOnDestroy(): void {
    this.userNameValidator.oldUserName = '';
  }

}
