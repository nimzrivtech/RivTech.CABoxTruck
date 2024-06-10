import { Component, Input, OnInit } from '@angular/core';
import { AgentLabelConstants } from '../../../../../../../shared/constants/agent.label.constants';
import { ErrorMessageConstant } from '../../../../../../../shared/constants/error-message.constants';
import { AgentData } from '../../../../../../../modules/management/data/agent.data';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { PasswordFormConstants } from '../../../../../../../shared/constants/login.labels.constants';
import Utils from '../../../../../../../shared/utilities/utils';
import { environment } from '../../../../../../../../environments/environment';
import { AccountService } from '../../../../../../../core/services/account.service';
import { AuthService } from '../../../../../../../core/services/auth.service';


@Component({
  selector: 'app-agent-details',
  templateUrl: './agent-details.component.html',
  styleUrls: ['./agent-details.component.scss']
})
export class AgentDetailsComponent implements OnInit {
  AgentLabelConstants = AgentLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;

  @Input() isEdit: boolean;
  constructor(
    public agentData: AgentData,
    private accountService: AccountService,
    private authService: AuthService) { }

  ngOnInit() {
  }

  onSystemUserChanged(): void {
    this.agentData.systemUserLogic(this.isEdit);
  }

  resetAgentPassword(): void {
    NotifUtils.showConfirmMessage(PasswordFormConstants.resetPasswordConfirmation,
      () => {
        Utils.blockUI();

        const emailAddress = this.agentData.agentFormGroup.get('emailAddress').value;
        const payload = {
          appCode: environment.ApplicationId,
          userName: this.agentData.agentFormGroup.get('username').value
        };

        this.accountService.userResetPassword(payload).subscribe(() => {
          Utils.unblockUI();
          this.authService.savePasswordAuditLog(PasswordFormConstants.auditLog.action.successReset, payload.userName);
          NotifUtils.showSuccessWithConfirmationFromUser(PasswordFormConstants.resetPasswordRequestSuccess.replace('{0}', emailAddress));
        }, error => {
          Utils.unblockUI();
          NotifUtils.showError(PasswordFormConstants.resetPasswordError.replace('{0}', emailAddress));
        });
      },
      () => { }
    );
  }
}
