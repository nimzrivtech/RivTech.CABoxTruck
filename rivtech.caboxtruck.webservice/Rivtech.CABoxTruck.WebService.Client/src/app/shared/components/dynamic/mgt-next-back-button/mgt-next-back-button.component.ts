import { AfterViewInit, Component, HostListener, Input, OnChanges, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { distinctUntilChanged, first, take } from 'rxjs/operators';
import { PathConstants } from '../../../../shared/constants/path.constants';
import * as _ from 'lodash';
import { MgtSubmissionNavSavingService } from '../../../../core/services/navigation/mgt-submission-nav-saving.service';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';
import { BaseClass } from '../../../../shared/base-class';
import { takeUntil } from 'rxjs/operators';
import Utils from '../../../../shared/utilities/utils';
import { forkJoin, Subject } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-mgt-next-back-button',
  templateUrl: './mgt-next-back-button.component.html',
  styleUrls: ['./mgt-next-back-button.component.scss']
})
export class MgtNextBackButtonComponent extends BaseClass implements OnInit, OnChanges, AfterViewInit {
  cardBody: any;
  card: any;
  hasScrollBar: boolean;
  currentPage: string = '';
  @Output() clickType = new EventEmitter();
  @Input() isScrollPage: boolean = true;

  @Input() showNext: boolean = true;
  @Input() showBack: boolean = true;
  @Input() showViewQuote: boolean = false;
  @Input() showEmailQuote: boolean = false;
  @Input() showCalculate: boolean = false;
  @Input() showPropertyServices: boolean = false;
  @Input() showIssue: boolean = false;
  @Input() showReset: boolean = false;
  @Input() showPolicyIssue: boolean = false;
  @Input() showSave: boolean = false;
  @Input() showCancel: boolean = false;
  @Input() showUpdate: boolean = false;
  @Input() showReviseRenewalOffer: boolean = false;

  @Input() disableBack: boolean = false;
  @Input() disableNext: boolean = false;
  @Input() disableViewQuote: boolean = false;
  @Input() disableEmailQuote: boolean = false;
  @Input() disableIssue: boolean = false;
  @Input() disableCalculate: boolean = false;
  @Input() disablePropertyServices: boolean = false;
  @Input() disableReset: boolean = false;
  @Input() disablePolicyIssue: boolean = false;
  @Input() disableSave: boolean = false;
  @Input() disableCancel: boolean = false;
  @Input() disableUpdate: boolean = false;
  @Input() disableReviseRenewalOffer: boolean = false;

  @Input() debounceTime: number = 1000;

  riskSavingComplete$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  @Input() policyIssueValidationMessage: string;

  constructor(
    private router: Router
  ) {
    super();
  }

  ngOnInit() {
    const splittedUrl = this.router.url.split('/');
    if (splittedUrl.length > 0) {
      if (splittedUrl[splittedUrl.length - 1].toLocaleLowerCase() === 'new') {
        if (splittedUrl[splittedUrl.length - 2].toLocaleLowerCase() === 'agency') { this.currentPage = 'Agency'; } // Add Agency
        if (splittedUrl[splittedUrl.length - 2].toLocaleLowerCase() === 'retailer') { this.currentPage = 'Retailer'; } // Add Retailer
      }
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

  @HostListener('window:scroll')
  @HostListener('window:resize', ['$event'])
  onScroll(): void {
    this.cardBody = document.getElementById('card-body');
    this.card = document.getElementById('card');
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 8) {
      this.cardBody.style.background = 'white';
      this.cardBody.style.bottom = '65px';
    } else {
      if (this.cardBody) {
        this.cardBody.style.bottom = '0';
      }
    }
  }

  @HostListener('window:click')
  clickEvent(): void {
    setTimeout(() => { this.onScroll(); }, 70);
  }

  onClick(event?) {
    this.clickType.emit(event);
  }

}

