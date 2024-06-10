import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '../../../../../../core/services/navigation/navigation.service';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
  selector: 'app-policy-summary-page',
  templateUrl: './policy-summary-page.component.html',
  styleUrls: ['./policy-summary-page.component.css']
})
export class PolicySummaryPageComponent implements OnInit {

  constructor(private navigationService: NavigationService, private summaryData: PolicySummaryData
    ) { }

  ngOnInit(): void {
    this.summaryData.coverageSummary();
    this.summaryData.reloadActionButtonsValidation();
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.History);
        break;
    }
  }
}

