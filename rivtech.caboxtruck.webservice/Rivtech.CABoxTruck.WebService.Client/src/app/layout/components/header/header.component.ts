import {Component, OnInit, ViewChild, AfterViewInit, AfterContentInit, Input, ViewEncapsulation, OnDestroy} from '@angular/core';
import {BsModalRef, BsModalService} from 'ngx-bootstrap';
import {NavigationService} from '../../../core/services/navigation/navigation.service';
import {LayoutService} from '../../../core/services/layout/layout.service';
import {Router} from '@angular/router';
import {AuthService} from '../../../core/services/auth.service';
import {BlacklistedDriverModalComponent} from '@app/modules/blacklisted-driver/components/blacklisted-driver-modal/blacklisted-driver-modal.component';
import {AddDriverVerificationModalComponent} from '@app/modules/blacklisted-driver/components/blacklisted-driver-modal/add-blacklisted-driver-modal/add-driver-verification-modal.component';
import { AccountsReceivableComponent } from '@app/modules/dashboard/components/reports/accounts-receivable/accounts-receivable.component';
import { DMVModalComponent } from '@app/modules/dashboard/components/reports/dmv/dmv-modal.component';
import { PaymentPostingModalComponent } from '@app/modules/dashboard/components/reports/payment-posting/payment-posting-modal.component';
import { BrokerCommissionModalComponent } from '@app/modules/dashboard/components/reports/broker-commission/broker-commission-modal.component';
import { PreBindPaymentModalComponent } from '@app/modules/dashboard/components/reports/prebind-payment/prebind-payment-modal.component';
import Utils from '@app/shared/utilities/utils';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import { catchError, finalize, map, switchMap, take, takeLast, takeUntil, tap } from 'rxjs/operators';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { of, timer, Subject, Subscription, Observable, throwError } from 'rxjs';
import { BordereauxModalComponent } from '@app/modules/dashboard/components/reports/bordereaux/bordereaux-modal.component';
import { PremiumEarnedTaxModalComponent } from '@app/modules/dashboard/components/reports/premium-earned-tax/premium-earned-tax-modal.component';
import { SubmissionQuoteModalComponent } from '@app/modules/dashboard/components/reports/submission-quote/submission-quote-modal.component';
import { InvoiceDetailModalComponent } from '@app/modules/dashboard/components/reports/invoice-detail/invoice-detail-modal.component';
import { VehicleData } from '@app/modules/submission/data/vehicle/vehicle.data';
import { HttpEventType } from '@angular/common/http';
import { DriverData } from '@app/modules/submission/data/driver/driver.data';
import { BackgroundService } from '@app/core/services/BackgroundService';
import { NotificationService } from '@app/core/services/NotificationService';
import { NotificationFilterDTO } from '@app/shared/models/notification/notification-filter.dto';
import { NotificationDTO } from '@app/shared/models/notification/notification.dto';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { DatePipe } from '@angular/common';
import { AgedReceivablesComponent } from '@app/modules/dashboard/components/reports/aged-receivables/aged-receivables.component';
//import { PolicyIssuanceData } from '@app/modules/policy/data/policy-issuance.data';

@Component({
  selector: 'app-header-admin',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None // rolan use to take effect on css and override the unmodified css.
})
export class HeaderComponent implements OnInit, OnDestroy {

  datePipe: DatePipe;
  @Input() leftSidebarAvailable: boolean = true;

  private readonly destroy$ = new Subject();
	notificationTimerSubsription: Subscription;

  uname: string;
  currentUserType: string = 'exernal';
  notifications: NotificationDTO[];

  modalRef: BsModalRef | null;

  constructor(public layoutService: LayoutService, 
    public navService: NavigationService, 
    private authService: AuthService, 
    private router: Router,
    private modalService: BsModalService,
    private submissionData: SubmissionData,
    private reportsService: ReportsService,
    private vehicleData: VehicleData,
    public driverData: DriverData,
    public backgroundService: BackgroundService,
    public notificationService: NotificationService,
    public policySummaryData: PolicySummaryData,
    //private policyIssuanceData: PolicyIssuanceData,
  ) {
  }

  get hasPolicy() : boolean {
    var riskDetail = this.submissionData.riskDetail;
    return riskDetail?.isPolicy === true ?? false;
  }

  public colorStatus(value:string) : string {
    if(value == "InProgress")
    {
      return "blue";
    }
    else if(value == "Success")
    {
      return "green";
    }
    else
    {
      return "red";
    }
  }

  ngOnInit() {
    this.uname = localStorage.getItem('uname');
    
    this.notificationTimerSubsription = timer(0, 2000).pipe(
      switchMap(() => this.retrieveNotifications()), 
      takeUntil(this.destroy$),
    ).subscribe();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    this.notificationTimerSubsription?.unsubscribe();
    this.notifications = [];
  }

  private retrieveNotifications(): Observable<any> {
		const notificationDetails = {
    };

    const notificationFilterDTO: NotificationFilterDTO = {
    };

		return this.notificationService.getNotificationsByUserId(notificationFilterDTO).pipe(
      takeUntil(this.destroy$),
      tap(data => this.updateNotificationList(data),
      catchError((error) => {
        console.error(error);
        NotifUtils.showError("There was a problem while fetching notifications. Please contact administrator.");
        return throwError(error);
      }))
    );
	}

  public readNotifications() {
		const notificationDetails = {
    };

    const notificationFilterDTO: NotificationFilterDTO = {
    };

		this.notificationService.UpdateNotificationsIsReadFlag(notificationFilterDTO).subscribe();
	}

  private updateNotificationList(data: NotificationDTO[]) {
    this.notifications = data.slice(0);

    var issueEndorsementCompleted = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'Success' && n.category === 'IssueEndorsement');
    var issueEndorsementInProgress = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'InProgress' && n.category === 'IssueEndorsement');
    
    var issuanceCompleted = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'Success' && n.category === 'Issuance');
    var issuanceInProgress = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'InProgress' && n.category === 'Issuance');
    
    var issuanceDefaultCompleted = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'Success' && n.category === 'IssuanceDefault');
    var issuanceDefaultInProgress = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'InProgress' && n.category === 'IssuanceDefault');
    
    var resetCompleted = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'Success' && n.category === 'Reset');
    var resetInProgress = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'InProgress' && n.category === 'Reset');

    var notificationFailed = this.notifications.find(n => n.riskId === this.submissionData?.riskDetail?.riskId && n.notificationStatus === 'Failed');
    
    let url = '';
    let policyUrl = '';
    let policyUrlOld = '';
    let policyUrlNew = '';
    let policyUrlCurrent = '';
    let notification:NotificationDTO = {};
    
    this.policySummaryData.isPolicyLocked = false;
    this.policySummaryData.canEditPolicy = this.policySummaryData.isPreviouslyEdited;
    this.policySummaryData.canEditActions = true;
    this.policySummaryData.isEndorsementIssuancePending = false;
    this.policySummaryData.isPolicyFailed = false;

    if(typeof notificationFailed != 'undefined' && notificationFailed != null){
      url = this.router.url;
      policyUrl = '/policies/' + notificationFailed.riskId;
      policyUrlOld = '/policies/' + notificationFailed.riskId + '/' + notificationFailed.oldRiskDetailId;
      
      if(url.includes(policyUrl))
      {
        this.policySummaryData.isPolicyFailed = true;
      }
    }
    else if (typeof issueEndorsementInProgress != 'undefined' && issueEndorsementInProgress != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + issueEndorsementInProgress.riskId;
      policyUrlOld = '/policies/' + issueEndorsementInProgress.riskId + '/' + issueEndorsementInProgress.oldRiskDetailId;
      
      if(url.includes(policyUrl))
      {
        this.policySummaryData.isPolicyLocked = true;
        this.policySummaryData.canEditPolicy = false;
        this.policySummaryData.canEditActions = false;
        this.policySummaryData.isEndorsementIssuancePending = true;
      }
    }
    else if (typeof issueEndorsementCompleted != 'undefined' && issueEndorsementCompleted != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + issueEndorsementCompleted.riskId;
      policyUrlNew = '/policies/' + issueEndorsementCompleted.riskId + '/' + issueEndorsementCompleted.newRiskDetailId;
      
      if(url.includes(policyUrl) && !url.includes(policyUrlNew))
      {
        if(issueEndorsementCompleted.isRendered == false){
          
          notification = issueEndorsementCompleted;
          notification.isRendered = true;
          this.notificationService.UpdateNotificationIsRendered(notification).subscribe();
          
          window.location.href = `/policies/${issueEndorsementCompleted.riskId}/${issueEndorsementCompleted.newRiskDetailId}/summary`;
        }
      }
    }
    else if (typeof issuanceInProgress != 'undefined' && issuanceInProgress != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + issuanceInProgress.riskId;
      policyUrlOld = '/policies/' + issuanceInProgress.riskId + '/' + issuanceInProgress.oldRiskDetailId;
      
      if(url.includes(policyUrl) && url.includes(policyUrlOld))
      {
        this.policySummaryData.isPolicyLocked = true;
        this.policySummaryData.canEditPolicy = false;
        this.policySummaryData.canEditActions = false;
      }
    }
    else if (typeof issuanceCompleted != 'undefined' && issuanceCompleted != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + issuanceCompleted.riskId;
      policyUrlOld = '/policies/' + issuanceCompleted.riskId + '/' + issuanceCompleted.oldRiskDetailId;
      policyUrlCurrent = '/policies/' + issuanceCompleted.riskId + '/' + issuanceCompleted.oldRiskDetailId + '/issuance';
      
      if(url.includes(policyUrl) && url.includes(policyUrlOld))
      {
        if(issuanceCompleted.isRendered == false){
          notification = issuanceCompleted;
          notification.isRendered = true;
          this.notificationService.UpdateNotificationIsRendered(notification).subscribe();

          if(url.includes(policyUrlCurrent)){
            window.location.reload();
          }
        }
      }
    }
    else if (typeof issuanceDefaultInProgress != 'undefined' && issuanceDefaultInProgress != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + issuanceDefaultInProgress.riskId;
      policyUrlOld = '/policies/' + issuanceDefaultInProgress.riskId + '/' + issuanceDefaultInProgress.oldRiskDetailId;
      
      if(url.includes(policyUrl) && url.includes(policyUrlOld))
      {
        this.policySummaryData.isPolicyLocked = true;
        this.policySummaryData.canEditPolicy = false;
        this.policySummaryData.canEditActions = false;
      }
    }
    else if (typeof issuanceDefaultCompleted != 'undefined' && issuanceDefaultCompleted != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + issuanceDefaultCompleted.riskId;
      policyUrlOld = '/policies/' + issuanceDefaultCompleted.riskId + '/' + issuanceDefaultCompleted.oldRiskDetailId;
      policyUrlCurrent = '/policies/' + issuanceDefaultCompleted.riskId + '/' + issuanceDefaultCompleted.oldRiskDetailId + '/issuance';
      
      if(url.includes(policyUrl) && url.includes(policyUrlOld))
      {
        if(issuanceDefaultCompleted.isRendered == false){
          
          notification = issuanceDefaultCompleted;
          notification.isRendered = true;
          this.notificationService.UpdateNotificationIsRendered(notification).subscribe();
          
          if(url.includes(policyUrlCurrent)){
            window.location.reload();
          }
        }
      }
    }
    else if (typeof resetInProgress != 'undefined' && resetInProgress != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + resetInProgress.riskId;
      policyUrlOld = '/policies/' + resetInProgress.riskId + '/' + resetInProgress.oldRiskDetailId;
      
      if(url.includes(policyUrl) && url.includes(policyUrlOld))
      {
        this.policySummaryData.isPolicyLocked = true;
        this.policySummaryData.canEditPolicy = false;
        this.policySummaryData.canEditActions = false;
        //this.policyIssuanceData.isIssuanceLoadSuccessful = false;
      }
    }
    else if (typeof resetCompleted != 'undefined' && resetCompleted != null)
    {
      url = this.router.url;
      policyUrl = '/policies/' + resetCompleted.riskId;
      policyUrlOld = '/policies/' + resetCompleted.riskId + '/' + resetCompleted.oldRiskDetailId;
      policyUrlCurrent = '/policies/' + resetCompleted.riskId + '/' + resetCompleted.oldRiskDetailId + '/issuance';
      
      if(url.includes(policyUrl) && url.includes(policyUrlOld))
      {
        if(resetCompleted.isRendered == false){
          
          notification = resetCompleted;
          notification.isRendered = true;
          this.notificationService.UpdateNotificationIsRendered(notification).subscribe();
          
          if(url.includes(policyUrlCurrent)){
            window.location.reload();
          }
        }
      }
    }
  }

  get notificationCount() {
    const count = this.notifications?.filter(x => x.isRead === false).length;
    return count > 0 ? count : '';
  }

  transform(value: any): any {
    if (value) {
        const seconds = Math.floor((+new Date() - +new Date(value)) / 1000);
        if (seconds < 29) // less than 30 seconds ago will show as 'Just now'
            return 'Just now';
        const intervals = {
            'year': 31536000,
            'month': 2592000,
            'week': 604800,
            'day': 86400,
            'hour': 3600,
            'minute': 60,
            'second': 1
        };
        let counter;
        for (const i in intervals) {
            counter = Math.floor(seconds / intervals[i]);
            if (counter > 0)
                if (counter === 1) {
                    return counter + ' ' + i + ' ago'; // singular (1 day ago)
                } else {
                    return counter + ' ' + i + 's ago'; // plural (2 days ago)
                }
        }
    }
    return value;
  }

  notificationLink(value: any): any {
    if (value) {
        
      if(value.notificationStatus == 'Success' && value.category == 'IssueEndorsement')
      {
        return `/policies/${value.riskId}/${value.newRiskDetailId}/summary`;
      }
      else if (value.notificationStatus == 'Success' && (value.category == 'Issuance' || value.category == 'IssuanceDefault' || value.category == 'Reset'))
      {
        return `/policies/${value.riskId}/${value.oldRiskDetailId}/summary`;
      }
      else if (value.notificationStatus == 'InProgress' || value.notificationStatus == 'Failed')
      {
        return `/policies/${value.riskId}/${value.oldRiskDetailId}/summary`;
      }
    }
    return 'javascript:void(0)';
  }

  logout() {
    this.authService.logUserLogout(this.uname).subscribe(() => {
      return;
    });
    this.authService.logout();
    this.router.navigate(['login']);
  }

  onToolClick(obj) {
    obj.preventDefault();

    switch (obj.target.innerText) {
      case 'Driver Verification':
        this.onDriverVerificationClick();
        break;
      case 'Vehicle Template':
          this.onVehicleTemplateClick();
          break;
      case 'Driver Template':
          this.onDriverTemplateClick();
          break;
      case 'Accounts Receivable':
        this.showAccountsReceivableDialog();
        break;
      case 'Bordereaux':
        this.showBordereauxDialog();
        break;
      case 'Broker Commission':
        this.showBrokerCommissionDialog();
        break;
      case 'Invoice Details':
        this.showInvoiceDetailDialog();
        break;
      case 'Payment Posting':
        this.showPaymentPostingDialog();
        break;
      case 'Pre-Bind Payments':
        this.showPreBindPaymentsDialog();
        break;
      case 'Premium Earned Tax':
        this.showPremiumEarnedTaxDialog();
        break;
      case 'Generate DMV Report':
        this.showDMVDialog();
        break;
      case 'Policy Statement':
        this.generatePolicyStatement();
        break;
      case 'Submissions and Quotes':
        this.showSubmissionQuoteDialog();
        break;
      case 'Aged Receivables':
        this.showAgedReceivablesDialog();
        break;
    }
  }


  onDriverVerificationClick(): void {
    const initialState = {
      title: 'Search Blacklisted Drivers',
    };
    this.modalRef = this.modalService.show(BlacklistedDriverModalComponent, {
      initialState,
      class: 'modal-xl modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true,
    });

    this.modalRef.content.showAnotherModal.subscribe((data) => {
      this.showAddDriverModal(data);
    });
  }

  onVehicleTemplateClick(): void {
    this.vehicleData.downloadTemplate().subscribe(data => {
      if (data.type === HttpEventType.Response) {
        const blob = new Blob([data.body], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        // anchor.download = 'Vehicles-Upload-Template.xlsx';
        anchor.download = `BT Vehicle Upload Template.xlsx`;

        anchor.href = url;
        anchor.click();
      }
    });
  }
  

  onDriverTemplateClick(): void {
    this.driverData.downloadTemplate().subscribe(data => {
      if (data.type === HttpEventType.Response) {
        const blob = new Blob([data.body], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        anchor.download = `BT Driver Upload Template.xlsx`;
        anchor.href = url;
        anchor.click();
      }
    });
  }
  showBordereauxDialog(): void {
    const initialState = {
      title: 'Bordereaux Report'
    };

    this.modalRef = this.modalService.show(BordereauxModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showSubmissionQuoteDialog(): void {
    const initialState = {
      title: 'Submission And Report Report'
    };

    this.modalRef = this.modalService.show(SubmissionQuoteModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showPremiumEarnedTaxDialog(): void {
    const initialState = {
      title: 'Premium Earned Tax Report'
    };

    this.modalRef = this.modalService.show(PremiumEarnedTaxModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showAgedReceivablesDialog(): void {
    const initialState = {
      title: 'Aged Receivables Report'
    }

    this.modalRef = this.modalService.show(AgedReceivablesComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showAccountsReceivableDialog(): void {
    const initialState = {
      title: 'Accounts Receivable Report'
    };

    this.modalRef = this.modalService.show(AccountsReceivableComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showPaymentPostingDialog(): void {
    const initialState = {
      title: 'Payment Posting'
    };

    this.modalRef = this.modalService.show(PaymentPostingModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showBrokerCommissionDialog(): void {
    const initialState = {
      title: 'Broker Commission'
    };

    this.modalRef = this.modalService.show(BrokerCommissionModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showPreBindPaymentsDialog(): void {
    const initialState = {
      title: 'Pre-Bind Payments'
    };

    this.modalRef = this.modalService.show(PreBindPaymentModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showDMVDialog(): void {
    const initialState = {
      title: 'DMV Report'
    };

    this.modalRef = this.modalService.show(DMVModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showInvoiceDetailDialog(): void {
    const initialState = {
      title: 'Invoice Details Report'
    };

    this.modalRef = this.modalService.show(InvoiceDetailModalComponent, {
      initialState,
      class: 'modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });
  }

  showAddDriverModal(initialState: any) {
    this.modalRef = this.modalService.show(AddDriverVerificationModalComponent, {
      initialState,
      class: 'modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true
    });

    this.modalRef.content.onClose.subscribe((data) => {
      this.onDriverVerificationClick();
    });
  }

  generatePolicyStatement() {

    Utils.blockUIWithMessage("Generating policy statement...");

    const policyData =  this.submissionData.riskDetail;
    const riskId = policyData.riskId;
    this.reportsService.generatePolicyStatement(riskId).pipe(
      catchError((err) => {
        console.error(err?.message);
        NotifUtils.showError("Error generating policy statement!");
        return of(err);
      }),
    ).subscribe((data) => {
      try {
        let url = window.URL.createObjectURL(data.body);
        let a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = `Policy Statement ${policyData.policyNumber}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        Utils.unblockUI();
      } catch(e) {
        console.error(e);
      } finally {
      }
    });
  }

}
