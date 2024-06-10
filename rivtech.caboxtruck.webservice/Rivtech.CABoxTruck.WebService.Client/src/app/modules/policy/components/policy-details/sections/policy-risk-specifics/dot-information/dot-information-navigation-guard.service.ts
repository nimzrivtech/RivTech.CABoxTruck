import { Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { DotInformationComponent } from './dot-information.component';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { PageSectionsValidations } from '../../../../../../../shared/constants/page-sections.contants';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';

@Injectable()
export class CanDeactivateDotInformationComponentGuard implements CanDeactivate<DotInformationComponent> {
    constructor(private submissionData: SubmissionData) {}

    canDeactivate(property: DotInformationComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
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
        //     property.dotInfoValidation.checkDotInfoPage();
        //     if (!property.formValidation.dotInfoValidStatus && displayPopup) {
        //         NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmRateMessage,
        //             () => {
        //                 subject.next(true);
        //             }, () => {
        //                 subject.next(false);
        //             });
        //         return subject.asObservable();
        //     }
        // }
        return true;
    }
}
