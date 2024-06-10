import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRouteSnapshot, NavigationStart, Router, RouterStateSnapshot } from '@angular/router';
import { Location } from '@angular/common';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { LayoutService } from '../../../../../../../core/services/layout/layout.service';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { ApplicantNameaddressValidationService } from '../../../../../../../core/services/validations/applicant-nameaddress-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';
import { Guid } from 'guid-typescript';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { PageSectionsValidations } from '../../../../../../../shared/constants/page-sections.contants';
import { Observable, of, Subject, throwError } from 'rxjs';
import Utils from '@app/shared/utilities/utils';
import { catchError, finalize, map, takeUntil, tap } from 'rxjs/operators';
import { PathConstants } from '@app/shared/constants/path.constants';

@Component({
  selector: 'app-name-and-address',
  templateUrl: './name-and-address.component.html',
  styleUrls: ['./name-and-address.component.scss']
})
export class NameAndAddressComponent implements OnInit {
  tempriskHeaderId: any;
  private reset$ = new Subject();

  constructor(private router: Router,
    private menuOverrideService: MenuOverrideService,
    public location: Location,
    public submissionData: SubmissionData,
    public applicantNameAndAddressValidation: ApplicantNameaddressValidationService,
    public formValidation: SubmissionNavValidateService,
    private navigationService: NavigationService,
    public applicantData: ApplicantData) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  ngOnInit() {
  }

  canDeactivate(nextState): Observable<boolean> | boolean {
    Utils.blockUI();
    return this.savingUponNavigate(nextState)
      .pipe(
        map(val => {
          return true;
        }),
        catchError(error => {
          console.log('Error saving upon navigate.');
          return of(false); //return from(['A','B','C'])
        }),
        finalize(() => Utils.unblockUI())
      );
  }

  routerNavigation(): Observable<any> {
    return this.router.events.filter(event => event instanceof NavigationStart)
      .pipe(takeUntil(this.reset$));
  }

  savingUponNavigate(nextState): Observable<any> {
      console.log('Saving upon navigate....', nextState.url);
      const subject = new Subject<boolean>();
      const displayPopup = !this.submissionData.isNew && this.submissionData.riskDetail?.id ? true : false;

      //this should be in constant component if getting plenty of entries;
      const lstURLExcemption = ["dashboard", "login"];

      if (this.submissionData.isNew) {
        const isExcempt = lstURLExcemption.find(x => (nextState.url.indexOf(x) != -1) == true);
        if (nextState.url === PageSectionsValidations.SubmissionList
          || nextState.url === PageSectionsValidations.ManagementPage || isExcempt !== undefined) {
            subject.next(true);
            return of({});
        }
        // const pageId = nextState.url.split('/').pop();
        // if (pageId === PathConstants.Submission.Applicant.BusinessDetails) {
        //   this.applicantData.applicantForms.nameAndAddressForm.markAllAsTouched();
        //   // stop here if form is invalid
        //   if (!this.applicantData.validateRequired()) {
        //     NotifUtils.showError(PageSectionsValidations.NameAndAddressError);
        //     subject.next(false);
        //     return subject.asObservable();
        //   }
        //   return this.applicantData.saveNameAndAddress$(null).pipe(
        //     tap(() => subject.next(true)),
        //     catchError((err) => throwError(err)),
        //     finalize(() => {
        //       console.log('Saving upon navigate from Name and Address finished!');
        //       return of({});
        //     })
        //   )
        // }

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
        this.applicantNameAndAddressValidation.checkNameAndAddressPage();
        if (!this.applicantData.validateRequired()) {
          NotifUtils.showError(PageSectionsValidations.NameAndAddressError);
          subject.next(false);
          return subject.asObservable();
        }
      }
      
      if (!this.submissionData.isNew) {
        return this.applicantData.saveNameAndAddress$(null).pipe(
          tap(() => subject.next(true)),
          catchError((err) => throwError(err)),
          finalize(() => {
            console.log('Update upon navigate from Name and Address finished!');
            return subject.asObservable();
          })
        )
      }
      return of({});
  }

  public onClick(clickType?) {
    this.nextBack.location = this.location;
    const nextUrl = this.nextBack.getNext();
    switch (clickType) {
      case ClickTypes.Next:
        this.applicantData.applicantForms.nameAndAddressForm.markAllAsTouched();
        // stop here if form is invalid
        if (!this.applicantData.validateRequired()) {
          NotifUtils.showError(PageSectionsValidations.NameAndAddressError);
          return;
        }

        this.menuOverrideService.overrideSideBarBehavior();
        if (this.submissionData.isNew) {
          this.applicantData.saveNameAndAddress(this.navigationService.navigateRoute(nextUrl, this.submissionData.riskDetail.id));
        } else {
          this.router.navigate([nextUrl]);
        }
        break;
    }
  }
}
