import { Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { BrokerComponent } from './broker.component';
import { PageSectionsValidations } from '../../../../../../shared/constants/page-sections.contants';
import NotifUtils from '../../../../../../shared/utilities/notif-utils';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';

@Injectable()
export class CanDeactivateBrokerComponentGuard implements CanDeactivate<BrokerComponent> {
    constructor(private submissionData: SubmissionData) {}

    canDeactivate(property: BrokerComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
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
            property.brokerValidation.checkBrokerPage();
            // if (!property.formValidation.brokerValidStatus && displayPopup) {
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
