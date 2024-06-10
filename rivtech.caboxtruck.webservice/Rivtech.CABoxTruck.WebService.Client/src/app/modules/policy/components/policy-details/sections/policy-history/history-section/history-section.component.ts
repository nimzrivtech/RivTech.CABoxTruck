import { Component, OnInit } from '@angular/core';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PolicyHistoryData } from '@app/modules/policy/data/policy-history.data';
import { ClickTypes } from '@app/shared/constants/click-type.constants';
import { PathConstants } from '@app/shared/constants/path.constants';
import { PolicyHistoryDTO } from '@app/shared/models/policy/PolicyHistoryDto';

@Component({
  selector: 'app-history-section',
  templateUrl: './history-section.component.html',
  styleUrls: ['./history-section.component.scss']
})
export class HistorySectionComponent implements OnInit {

  hideMe: boolean = false;
  historyTableHeaders: string[] = [
    '',
    'Date',
    'Effective Date',
    'Transaction Type',
    'Annual Premium for Current Endorsement',
    'Premium Before Endorsement',
    'Premium After Endorsement',
    'Premium Change'
  ];

  constructor(private navigationService: NavigationService, public policyHistoryData: PolicyHistoryData) { }

  ngOnInit() {
    this.policyHistoryData.populatePolicyHistory();
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Summary);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Billing);
        break;
    }
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  getTransactionType(item: PolicyHistoryDTO) {
    if (item.endorsementNumber == '0') {
      return 'NEW';
    } else if (item.policyStatus == 'Canceled') {
      return 'CANCELED';
    } else if (item.policyStatus == 'Reinstated') {
      return 'REINSTATED';
    } else if (item.policyStatus == 'Rewrite') {
      return 'CANCELED / REWRITE';
    } else {
      return `ENDORSEMENT ${item.endorsementNumber}`;
    }
  }

  parseHistoryDetail(detail: string) {
    return JSON.parse(detail);
  }

}
