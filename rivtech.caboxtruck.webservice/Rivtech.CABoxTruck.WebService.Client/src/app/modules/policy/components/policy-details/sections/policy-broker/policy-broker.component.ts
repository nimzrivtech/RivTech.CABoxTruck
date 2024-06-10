import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { BrokerGenInfoData } from '@app/modules/submission/data/broker/general-information.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { ClickTypes } from '@app/shared/constants/click-type.constants';
import { PathConstants } from '@app/shared/constants/path.constants';
import { BrokerInfoDto } from '@app/shared/models/submission/brokerinfo/brokerInfoDto';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { createPolicyDetailsMenuItems } from '../../policy-details-navitems';
import * as _ from 'lodash';

@Component({
  selector: 'app-policy-broker',
  templateUrl: './policy-broker.component.html',
  styleUrls: ['./policy-broker.component.scss']
})
export class PolicyBrokerComponent implements OnInit {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;
  private saveCategoryEndSource = new Subject<void>();
  private saveCategoryEnded = this.saveCategoryEndSource.asObservable();

  constructor(
    public Data: BrokerGenInfoData,
    private navigationService: NavigationService,
    private router: Router,
    private layoutService: LayoutService,
    private riskSpecificsData: RiskSpecificsData,
    private submissionData: SubmissionData,
    private policySummaryData: PolicySummaryData
    ) { }

  ngOnInit() {
    this.Data.initiateFormGroup(new BrokerInfoDto());
    this.resetDropdowns();
    this.Data.dateFormatOptions();
    this.Data.initiatePolicyContacts();
    this.savingUponNavigate();
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Manuscript);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Limits.Index);
        break;
    }
  }

  resetDropdowns() {
    this.Data.dropdownList.agencyList = this.Data.dropdownList.agentList =
    this.Data.dropdownList.retailerList = this.Data.dropdownList.retailAgentList =
    this.Data.dropdownList.reasonForMove = [];
  }

  savingUponNavigate() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .pipe(takeUntil(this.reset$)).subscribe((event: NavigationEnd) => {
        console.log('Saving upon navigate....');
        if (this.isPolicyUrl) { return; }
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        if (['dashboard', 'login'].includes(this.newCategoryRoute, 0)) {
          return;
        }
        if (this.currentCategoryRoute === PathConstants.Submission.Broker.Index) {
          this.submissionData.validationList.broker = this.checkPolicyContactsSection();
          this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
        }
        this.currentCategoryRoute = this.newCategoryRoute;
      });
  }

  get isPolicyUrl () {
    const splitUrl = _.split(this.router.url, '/');
    return splitUrl[0] === 'policies';
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  private checkPolicyContactsSection(): boolean {
    return this.submissionData.riskDetail?.riskPolicyContacts?.filter(x => x.typeId == 1)?.length > 0;
  }

}
