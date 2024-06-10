import { Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { PageSectionsValidations } from '../../../../../../../shared/constants/page-sections.contants';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { FilingsInformationComponent } from './filings-information.component';

@Injectable()
export class CanDeactivateFilingsInformationComponentGuard implements CanDeactivate<FilingsInformationComponent> {
    constructor(private submissionData: SubmissionData) {}

    canDeactivate(property: FilingsInformationComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
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
            property.filingsValidation.checkFilingsInformationPage();
            // if (!property.formValidation.applicantFilingValidStatus && displayPopup) {
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
