import { Injectable } from '@angular/core';
import { IProgramStateDTO, IProgramStateRequestDTO } from '../../../shared/models/management/agency-management/programStateAgencyDto';
import { Subject } from 'rxjs';
import { BaseClass } from '../../../shared/base-class';
import { ProgramStateTableConstants } from '../../../shared/constants/agency.table.constants';
import { ProgramstateDetailService } from '../../../core/services/management/programstate-detail.service';
import { AgencyData } from './agency.data';
import { distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { ProgramstateService } from '../../..//core/services/management/programstate.service';
import { ProgramStateConstants } from '../../../shared/constants/program-state.constants';
import Utils from '../../../shared/utilities/utils';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { AuditLogService } from '../../../core/services/management/auditLog.service';
import { AuthService } from '../../../core/services/auth.service';
import { AgencyLabelConstants } from '../../../shared/constants/agency.label.constants';

@Injectable({
    providedIn: 'root'
})
export class ProgramStateData extends BaseClass {
    addedProgramState: Array<any> = [];
    programStateListData: Array<any> = [];
    programStatesModalList: Array<any> = [];
    programStates = new Subject<IProgramStateDTO[]>();
    isProgramStateLoading: boolean;
    isProgramStateDetailLoading: boolean;
    programStateConstants = ProgramStateConstants;

    statesList: Array<string>;
    programStateList: Array<any> = [];

    programStateListHeader: any[] = [
        {
            name: ProgramStateTableConstants.mainProgram,
            columnName: 'programState.program.masterProgramCode'
        },
        {
            name: ProgramStateTableConstants.programName,
            columnName: 'programState.program.programName'
        },
        {
            name: ProgramStateTableConstants.formType,
            columnName: 'programState.program.formTypeCode'
        },
        {
            name: ProgramStateTableConstants.state,
            columnName: 'programState.stateCode'
        },
        {
            name: ProgramStateTableConstants.status,
            columnName: 'isActive'
        }
    ];

    programStateModalListHeader: any[] = [
        {
            name: ProgramStateTableConstants.mainProgram,
            columnName: 'program.masterProgramCode'
        },
        {
            name: ProgramStateTableConstants.programName,
            columnName: 'program.programName'
        },
        {
            name: ProgramStateTableConstants.formType,
            columnName: 'program.formTypeCode'
        },
        {
            name: ProgramStateTableConstants.state,
            columnName: 'stateCode'
        }
    ];

    constructor(
        private programStateService: ProgramstateService,
        private programStateDetailService: ProgramstateDetailService,
        public agencyData: AgencyData,
        private auditLogService: AuditLogService,
        private authService: AuthService
    ) {
        super();
    }

    getProgramStates() {
        this.isProgramStateLoading = true;
        this.isProgramStateDetailLoading = true;

        if (this.agencyData.agencyId) {
            this.programStateDetailService.getProgramStatesWithAgencyId(this.agencyData.agencyId).pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
                this.programStateListData = data;
                this.programStates.next(this.programStateListData);
                this.isProgramStateLoading = false;
                this.isProgramStateDetailLoading = false;
            });
        }
    }

    getProgramStatesList() {
        this.isProgramStateLoading = true;

        this.programStateService.getProgramStatesList().pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
            this.programStatesModalList = data;

            this.statesList = this.programStatesModalList.map(x => x.stateCode)
                .filter((value, index, self) => self.indexOf(value) === index);

            this.isProgramStateLoading = false;
        });
    }

    saveAddedProgramStates(): void {
        const payload: IProgramStateRequestDTO = this.getProgramStatePayload();
        this.isProgramStateLoading = true;
        Utils.blockUI();

        this.programStateDetailService.postProgramStateDetails(payload).subscribe(() => {
            Utils.unblockUI();
            this.programStateAuditLog(AgencyLabelConstants.auditLog.add, payload.agencyId);
            NotifUtils.showSuccess(this.programStateConstants.programStateAddedSuccessfulyMessage, () => {
                this.getProgramStates();
                this.isProgramStateLoading = false;
            });
        }, err => { });
    }

    updateProgramState(id: string, status: boolean): void {
        const payload: IProgramStateRequestDTO = this.getProgramStatePayload(true, id, status);
        Utils.blockUI();

        this.programStateDetailService.postProgramStateStatusDetails(payload).subscribe((res) => {
            Utils.unblockUI();
            this.programStateAuditLog(AgencyLabelConstants.auditLog.edit, res);
            NotifUtils.showSuccess(this.programStateConstants.programStateUpdatedSuccessfulyMessage, () => {
                this.getProgramStates();
            });
        }, err => { });
    }

    deleteProgramState(id: string): void {
        Utils.blockUI();

        this.programStateDetailService.deleteProgramStateStatusDetails(id).subscribe((res) => {
            if (res) {
                Utils.unblockUI();
                this.programStateAuditLog(AgencyLabelConstants.auditLog.delete, res);
                NotifUtils.showSuccess(this.programStateConstants.programStateDeletedSuccessfulyMessage, () => {
                    this.getProgramStates();
                });
            }
        }, err => { });
    }

    getProgramStatePayload(isUpdate: boolean = false, id?: string, status?: boolean): IProgramStateRequestDTO {
        if (!isUpdate) {
            const programStateIds = this.addedProgramState.map(x => x.id);
            return {
                programStateIds,
                agencyId: this.agencyData.agencyId
            };
        } else {
            return {
                id,
                isActive: !status
            };
        }
    }

    programStateAuditLog(action: string, entityId: string): void {
        const payload = {
          userId: this.authService.getUserId(),
          keyId: entityId,
          auditType: '',
          description: '',
          method: ''
        };

        switch (action) {
          case AgencyLabelConstants.auditLog.add:
              payload.auditType = AgencyLabelConstants.auditLog.add;
              payload.description = ProgramStateConstants.programStateAddedSuccessfulyMessage;
              payload.method = ProgramStateConstants.methodName.add;
            break;
          case AgencyLabelConstants.auditLog.edit:
              payload.auditType = AgencyLabelConstants.auditLog.edit;
              payload.description = ProgramStateConstants.programStateUpdatedSuccessfulyMessage;
              payload.method = ProgramStateConstants.methodName.edit;
            break;
          case AgencyLabelConstants.auditLog.delete:
              payload.auditType = AgencyLabelConstants.auditLog.delete;
              payload.description = ProgramStateConstants.programStateDeletedSuccessfulyMessage;
              payload.method = ProgramStateConstants.methodName.delete;
            break;
        }

        this.auditLogService.insertToAuditLog(payload).subscribe();
    }

}