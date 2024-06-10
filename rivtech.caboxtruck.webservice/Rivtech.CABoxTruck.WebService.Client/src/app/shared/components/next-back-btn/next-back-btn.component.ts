import { Location } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { PolicyIssuanceData } from '@app/modules/policy/data/policy-issuance.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { PremiumCalculateButtonComponent } from '@app/modules/rating/components/premium-calculate-button/premium-calculate-button.component';
import { PreloadingStatus } from '@app/modules/rating/models/preloading-status';
import { FormsData } from '@app/modules/submission/data/forms/forms.data';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { IssueEndorsementModalComponent } from '@app/modules/policy/components/policy-details/sections/policy-issuance/issue-endorsement-modal/issue-endorsement-modal.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ResetEndorsementModalComponent } from '../../../modules/policy/components/policy-details/sections/policy-issuance/reset-process-modal/reset-process-modal.component';
import NotifUtils from '../../utilities/notif-utils';
import Utils from '../../utilities/utils';
import { BindAndIssueLabelConstants } from '../../constants/bind-and-issue.labels.constants';

@Component({
  selector: 'app-next-back-btn',
  templateUrl: './next-back-btn.component.html',
  styleUrls: ['./next-back-btn.component.scss']
})
export class NextBackBtnComponent implements OnInit, OnChanges, AfterViewInit {

  cardBody: any;
  card: any;
  hasScrollBar: boolean;
  @Output() clickType = new EventEmitter();
  @Output() onCalculate = new EventEmitter();
  @Input() showNext: boolean = true;
  @Input() showBack: boolean = true;
  @Input() isScrollPage: boolean = true;
  @Input() disableBack: boolean = false;
  @Input() disableNext: boolean = false;
  @Input() isIssuePage: boolean = false;
  @Input() isEditablePage: boolean = false;
  @Input() hasCalculate: boolean = false;
  private _location: Location = null;
  private preloadStatus: PreloadingStatus = null;
  
  issueEndorsementModal: BsModalRef;
  resetEndorsementModal: BsModalRef;

  constructor(public submissionData: SubmissionData,
              public policySummaryData: PolicySummaryData,
              public policyIssuanceData: PolicyIssuanceData,
              private raterApiData: RaterApiData,
              private formsData: FormsData,
              private modalService: BsModalService) { }

  ngOnInit() {
    this.raterApiData.preloadingStatus.subscribe((status) => {
      this.preloadStatus = status;
    });
  }

  get canRate() : boolean {
    return this.preloadStatus === PreloadingStatus.Complete;  
  }

  get issuePreloadTooltip() {
    if (this.preloadStatus === PreloadingStatus.InProgress) {
      return "Preloading rater...";
    } else if (this.preloadStatus === PreloadingStatus.Failed) {
      return "Preloading rater failed.";
    }
  }



  ngAfterViewInit() {
    this.cardBody = document.getElementById('card-body');
    if (!window.scrollY) {
      this.cardBody.style.background = 'white';
      this.cardBody.style.bottom = '65px';
    }
    this.onScroll();
  }

  ngOnChanges(simpleChanges: SimpleChanges) {
    if (simpleChanges.isScrollPage) {
      this.isScrollPage = simpleChanges.isScrollPage.currentValue;
    }
  }

  get location(): Location {
    return this._location;
  }
  set location(value: Location) {
    this._location = value;
  }

  @HostListener('window:scroll')
  @HostListener('window:resize', ['$event'])
  onScroll(): void {
    this.cardBody = document.getElementById('card-body');
    this.card = document.getElementById('card');
    if (this.cardBody === null) { return; }
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 8) {
      this.cardBody.style.background = 'white';
      this.cardBody.style.bottom = '65px';
    } else {
      this.cardBody.style.bottom = '0';
    }

    if (document.body.classList.contains('sidebar-show') && window.innerWidth < 992) {
      this.cardBody.style.left = '230px';
    } else if (window.innerWidth === 992) {
      this.cardBody.style.left = '270px';
    } else {
      this.cardBody.style.left = null;
    }
  }

  @HostListener('window:click')
  clickEvent(): void {
    setTimeout(() => { this.onScroll(); }, 70);
  }

  onClick(event?) {
    this.clickType.emit(event);
  }

  /*
  calculatePremium() {
    Utils.blockUI();
    const bindOptionId = Number(this.submissionData?.riskDetail?.binding?.bindOptionId) ?? 1;
    this.raterApiData.calculateForQuote(bindOptionId)
      .then(() => {
        this.formsData.isPolicyUI = true;
        this.formsData.refreshQuoteOptionsAndSummaryHeader();
      }).catch((error) => {
        console.error(error);
      }).finally(() => {
        setTimeout(() => {
          this.raterApiData.disableCalculatePremium = false;
        }, 6000);
      });
  }
  */

  get canIssue() {
    let canRate = this.policyIssuanceData.validateRequiredToRateModules();
    return !this.policyIssuanceData.hasError 
            && this.policyIssuanceData.hasPendingEndorsement 
            && this.policyIssuanceData.issuanceForm.valid
            && canRate
            && !this.policySummaryData.getIsPolicyLocked
            && !this.policySummaryData.getIsPolicyFailed;
  }

  get canReset() {
    return this.policyIssuanceData.hasPendingEndorsement && (!this.policySummaryData.getIsPolicyLocked || this.policySummaryData.getIsPolicyFailed);
  }

  calculate() {
    this.onCalculate.emit();
  }

  showIssueEndorsementModal() {
    if (this.issueEndorsementModal == null) {
      this.issueEndorsementModal = this.modalService.show(IssueEndorsementModalComponent, {
        keyboard: false,
        backdrop: 'static',
        ignoreBackdropClick: true,
        focus: true,
        animated: true,
        class: 'issue-endorsement-modal  modal-dialog-centered',
      });
      this.issueEndorsementModal.content?.close.subscribe(() => {
        this.issueEndorsementModal?.hide();
      });
    }
  }

  showResetModal() {
    NotifUtils.showConfirmMessage(BindAndIssueLabelConstants.areYouSureYouWantToReset, () => {
      this.resetEndorsementModal = this.modalService.show(ResetEndorsementModalComponent, {
        keyboard: false,
        backdrop: 'static',
        ignoreBackdropClick: true,
        focus: true,
        animated: true,
        class: 'issue-endorsement-modal  modal-dialog-centered',
      });
      this.resetEndorsementModal.content?.close.subscribe(() => {
        this.resetEndorsementModal?.hide();
      });
    });
  }

  triggerIssueEndorsementProcess() {
    if (!this.canIssue) {
      return;
    }
    this.policyIssuanceData.triggerIssueEndorsementBackgroundProcess("IssueEndorsement");
  }

  triggerResetProcess() {
    if (!this.canReset) {
      return;
    }
    this.policyIssuanceData.triggerResetBackgroundProcess("Reset");
  }
}


  