import { Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { UnderwritingQuestionsComponent } from './underwriting-questions.component';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { PageSectionsValidations } from '../../../../../../../shared/constants/page-sections.contants';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';

@Injectable()
export class CanDeactivateUnderwritingQuestionsComponentGuard implements CanDeactivate<UnderwritingQuestionsComponent> {
    constructor(private submissionData: SubmissionData) {}

    canDeactivate(property: UnderwritingQuestionsComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
        const subject = new Subject<boolean>();
        const displayPopup = this.submissionData.riskDetail?.id ? true : false;
        // if (nextState.url === PageSectionsValidations.SubmissionList && displayPopup) {
        //     NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmMessageForSubmission,
        //         () => {
        //             subject.next(true);
        //         }, () => {
        //             subject.next(false);
        //         });
        //     return subject.asObservable();
        // } else {
        //     property.underwritingQuestionsValidation.checkUnderwritingQuestionsPage();
        //     if (!property.formValidation.underwritingQuestionsValidStatus && displayPopup) {
        //         NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmRateMessage,
        //             () => {
        //                 subject.next(true);
        //             }, () => {
        //                 subject.next(false);
        //             });
        //         return subject.asObservable();
        //     }
        //     if ((nextState.url.includes('/quote-options') || nextState.url.includes('/bind') || nextState.url.includes('/forms-selection'))
        //         && !property.underwritingQuestionsValidation.areAnswersConfirmed) {
        //         subject.next(false);
        //         return subject.asObservable();
        //     }
        // }
        return true;
    }
}
