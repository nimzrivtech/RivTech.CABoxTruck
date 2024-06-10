import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { PolicyIssuanceData } from '@app/modules/policy/data/policy-issuance.data';
import Utils from '@app/shared/utilities/utils';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-premium-changes',
  templateUrl: './premium-changes.component.html',
  styleUrls: ['./premium-changes.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class PremiumChangesComponent implements OnInit {

  hidePremiumChanges: boolean = false;
  premiumChangesHeaders: string[] = [
    'Coverages',
    'Annual Premium for Current Endorsement',
    'Premium Before Endorsement',
    'Premium After Endorsement',
    'Premium Difference'
  ];
  premiumChangesCoverages: string[] = [
    'Vehicle',
    'Policy',
    'Total Premium',
    'Fees (Fully Earned)',
  ];

  constructor(public policyIssuanceData: PolicyIssuanceData) { }

  ngOnInit() {

  }

  TogglePremiumChanges() {
    this.hidePremiumChanges = !this.hidePremiumChanges;
  }

  highlightTotalRows(obj) {
    if (this.premiumChangesCoverages.some(x => x === obj.description)) {
      return true;
    }
    return false;
  }
}
