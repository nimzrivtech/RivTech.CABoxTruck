import { Subject } from 'rxjs';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { VehicleComponent } from './vehicle.component';
import { PageSectionsValidations } from '../../../../../../shared/constants/page-sections.contants';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';
import NotifUtils from '../../../../../../shared/utilities/notif-utils';

@Injectable()
export class CanDeactivateVehicleComponentGuard implements CanDeactivate<VehicleComponent> {
    constructor(private submissionData: SubmissionData) {}

    canDeactivate(property: VehicleComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot, nextState: RouterStateSnapshot) {
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
            property.vehicleValidation.checkVehiclePage();
            // if (!property.formValidation.vehicleValidStatus && displayPopup) {
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
