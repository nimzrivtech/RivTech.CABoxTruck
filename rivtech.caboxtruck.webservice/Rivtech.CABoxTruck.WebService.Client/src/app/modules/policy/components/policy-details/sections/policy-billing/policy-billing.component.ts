import { Component, OnInit } from '@angular/core';
import { NavigationService } from 'app/core/services/navigation/navigation.service';
import { PathConstants } from 'app/shared/constants/path.constants';
import { AuthService } from 'app/core/services/auth.service';
import { BaseClass } from 'app/shared/base-class';
import { takeUntil } from 'rxjs/operators';
import { ClickTypes } from '../../../../../../shared/enum/click-type.enum';

@Component({
  selector: 'app-policy-billing',
  templateUrl: './policy-billing.component.html',
  styleUrls: ['./policy-billing.component.scss']
})
export class PolicyBillingComponent extends BaseClass implements OnInit {

  isInternal: boolean;

  constructor(private authService: AuthService,
    public navigationService: NavigationService) {
    super();
  }

  ngOnInit() {
    this.authService.userType.pipe(takeUntil(this.stop$)).subscribe(userType => {
      this.isInternal = this.isInternalUser(userType);
    });
  }

  public onClick(clickType?: ClickTypes): void {
    switch (clickType) {
      case ClickTypes.Back:
        // to do back
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.History);
        break;
      case ClickTypes.Next:
        // to do next
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.NameAndAddress}`);
        break;
    }
  }

  public isInternalUser(userType: string): boolean {
    return userType.toLocaleLowerCase() === 'internal';
  }
}
