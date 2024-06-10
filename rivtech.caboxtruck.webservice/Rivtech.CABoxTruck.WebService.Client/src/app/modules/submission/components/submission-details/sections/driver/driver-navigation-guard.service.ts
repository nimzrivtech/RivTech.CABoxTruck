import { of, Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { DriverComponent } from './driver.component';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';
import { PageSectionsValidations } from '../../../../../../shared/constants/page-sections.contants';
import NotifUtils from '../../../../../../shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { DriverData } from '@app/modules/submission/data/driver/driver.data';
import { catchError, map, take, tap } from 'rxjs/operators';

@Injectable()
export class CanDeactivateDriverComponentGuard implements CanDeactivate<DriverComponent> {
    constructor(
        private submissionData: SubmissionData,
        private driverData: DriverData,
    ) { }

    canDeactivate(property: DriverComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
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
            property.driverValidation.checkDriverPage();
            // if (!property.formValidation.driverValidStatus && displayPopup) {
            //     NotifUtils.showConfirmMessage(PageSectionsValidations.ConfirmRateMessage,
            //         () => {
            //             subject.next(true);
            //         }, () => {
            //             subject.next(false);
            //         });
            //     return subject.asObservable();
            // }

            Utils.blockUI();

            return this.driverData.updateDriverExclusionEndorsementForm$(this.submissionData.riskDetail.id).pipe(
                take(1),
                tap(() => {
                    Utils.unblockUI();
                }),
                map(() => true), // return true if success
                catchError((err) => { // show notif error if failed
                    console.error(err);
                    let errorMessage = `Error occured. `;
                    if (err?.message !== undefined) {
                        errorMessage += `\n${err?.message}`;
                    }
                    NotifUtils.showError(errorMessage);
                    return of(false);
                }),
            );
        }
    }
}
