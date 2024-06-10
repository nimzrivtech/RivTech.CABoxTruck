import { Component, OnInit } from '@angular/core';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { ClaimsHistoryValidationService } from '../../../../../../core/services/validations/claims-history-validation.service';
import { ClaimsHistoryoData } from '../../../../../../modules/submission/data/claims/claims-history.data';
import { ClickTypes } from '@app/shared/constants/click-type.constants';
import { PathConstants } from '@app/shared/constants/path.constants';

@Component({
  selector: 'app-policy-claims-history',
  templateUrl: './policy-claims-history.component.html',
  styleUrls: ['./policy-claims-history.component.scss']
})
export class PolicyClaimsHistoryComponent implements OnInit {

  constructor(private navigationService: NavigationService,
              public Data: ClaimsHistoryoData,
              public claimsValidation: ClaimsHistoryValidationService) { }

  ngOnInit() {
    this.Data.initiateFormFields();
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.Data.formInfo; }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Coverages.Index);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.DestinationInfo}`);
        break;
    }
  }

}
