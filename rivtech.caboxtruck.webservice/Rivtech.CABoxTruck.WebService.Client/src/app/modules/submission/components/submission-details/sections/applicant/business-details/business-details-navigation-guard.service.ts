import { Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { BusinessDetailsComponent } from './business-details.component';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { PageSectionsValidations } from '../../../../../../../shared/constants/page-sections.contants';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';

@Injectable()
export class CanDeactivateBusinessDetailsComponentGuard implements CanDeactivate<BusinessDetailsComponent> {
    constructor(private submissionData: SubmissionData) {}

    canDeactivate(property: BusinessDetailsComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
        const subject = new Subject<boolean>();
        const displayPopup = this.submissionData.riskDetail?.id ? true : false;
        if (nextState.url === PageSectionsValidations.SubmissionList && displayPopup) {
            NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmMessageForSubmission,
                () => {
                    subject.next(true);
                }, () => {
                    subject.next(false);
                });
            return subject.asObservable();
        } else {
            property.applicantBusinessValidation.checkBusinessDetailsPage();
            // if (!property.formValidation.applicantBusinessValidStatus && displayPopup) {
            //     NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmRateMessage,
            //         () => {
            //             subject.next(true);
            //         }, () => {
            //             subject.next(false);
            //         });
            //     return subject.asObservable();
            // }
        }
        return true;
    }
}
