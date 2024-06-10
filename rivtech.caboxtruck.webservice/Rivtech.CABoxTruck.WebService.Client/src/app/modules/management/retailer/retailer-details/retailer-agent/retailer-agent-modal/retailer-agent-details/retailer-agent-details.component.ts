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
import { RetailerAgentData } from '@app/modules/management/data/retailer-agent.data';


@Component({
  selector: 'app-retailer-agent-details',
  templateUrl: './retailer-agent-details.component.html',
  styleUrls: ['./retailer-agent-details.component.scss']
})
export class RetailerAgentDetailsComponent implements OnInit {
  AgentLabelConstants = AgentLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;

  @Input() isEdit: boolean;
  constructor(
    public retailerAgentData: RetailerAgentData) { }

  ngOnInit() {
  }

  onSystemUserChanged(): void {
  }

  resetAgentPassword(): void {
  }
}
