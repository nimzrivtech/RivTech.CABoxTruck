import { Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { NameAndAddressComponent } from './name-and-address.component';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { PageSectionsValidations } from '../../../../../../../shared/constants/page-sections.contants';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { LayoutService } from '../../../../../../../core/services/layout/layout.service';

@Injectable()
export class CanDeactivateNameAndAddressComponentGuard implements CanDeactivate<NameAndAddressComponent> {
  constructor(private submissionData: SubmissionData, private layoutService: LayoutService) { }

  canDeactivate(property: NameAndAddressComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
    const subject = new Subject<boolean>();
    const displayPopup = !this.submissionData.isNew && this.submissionData.riskDetail?.id ? true : false;

    //this should be in constant component if getting plenty of entries;
    const lstURLExcemption = ["dashboard", "login"];

    if (this.submissionData.isNew) {
      const isExcempt = lstURLExcemption.find(x => (nextState.url.indexOf(x) != -1) == true);
      if (nextState.url === PageSectionsValidations.SubmissionList
        || nextState.url === PageSectionsValidations.ManagementPage || isExcempt !== undefined) {
        return true;
      }
      NotifUtils.showError('Please click Next Button on Name and Address');
      subject.next(false);
      return subject.asObservable();
    }
    if (nextState.url === PageSectionsValidations.SubmissionList && displayPopup) {
      NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmMessageForSubmission,
        () => {
          subject.next(true);
        }, () => {
          subject.next(false);
        });
      return subject.asObservable();
    } else {
      property.applicantNameAndAddressValidation.checkNameAndAddressPage();
      if (!property.applicantData.validateRequired()) {
        NotifUtils.showError(PageSectionsValidations.NameAndAddressError);
        subject.next(false);
        return subject.asObservable();
      }
      // if (!property.formValidation.applicantNameAndAddressValidStatus && displayPopup) {
      //   NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmRateMessage,
      //     () => {
      //       subject.next(true);
      //     }, () => {
      //       subject.next(false);
      //     });
      //   return subject.asObservable();
      // }
    }
    return true;
  }
}
