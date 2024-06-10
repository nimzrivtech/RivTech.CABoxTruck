import { AfterViewInit, Component, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { RiskSpecificsData } from '../../../../../data/risk-specifics.data';
import { Location } from '@angular/common';
import { createSubmissionDetailsMenuItems } from '../../../submission-details-navitems';
import { NavData } from '../../../../../../../_nav';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { QuoteLimitsData } from '@app/modules/submission/data/quote/quote-limits.data';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { BaseResizer, nextbackButtonResizer } from '@app/modules/submission/helpers/submission-resizer';

@Component({
  selector: 'app-next-back-button',
  templateUrl: './next-back-button.component.html',
  styleUrls: ['./next-back-button.component.scss']
})
export class NextBackButtonComponent implements OnInit, OnChanges, AfterViewInit {

  cardBody: any;
  card: any;
  hasScrollBar: boolean;
  @Output() clickType = new EventEmitter();
  @Input() showNext: boolean = true;
  @Input() showBack: boolean = true;
  @Input() isScrollPage: boolean = true;
  @Input() showViewQuote: boolean = false;
  @Input() disableBack: boolean = false;
  @Input() disableNext: boolean = false;
  @Input() disableCalculate: boolean = false;
  @Input() disableViewQuote: boolean = false;
  @Input() disableIssue: boolean = false;
  @Input() showIssue: boolean = false;
  @Input() hideQuote: boolean = false;
  @Input() showQuoteProposalOnly: boolean = false;
  @Input() showGeneratePolicyNumber: boolean = false;

  private nbResizer: BaseResizer = nextbackButtonResizer.Initialize();

  constructor(private riskSpecificsData: RiskSpecificsData,
              public submissionData: SubmissionData,
              public quoteLimitsData: QuoteLimitsData,
              public raterApiData: RaterApiData,
              ) { }

  ngOnInit() {
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

  // @HostListener('window:scroll') onScroll(): void {
  //   this.cardBody = document.getElementById('card-body');
  //   this.card = document.getElementById('card');
  //   (<nextbackButtonResizer>this.nbResizer).scrollBottom('html').then((obj) => {
  //     if (obj) {
  //       this.cardBody.style.bottom = '65px';
  //       this.cardBody.classList.remove('addShadow');
  //     } else {
  //       this.cardBody.style.bottom = '0';
  //       this.cardBody.classList.add('addShadow');
  //     }
  //   }).catch((obj) => {
  //     try {
  //       this.cardBody.style.bottom = '0';
  //       this.cardBody.classList.add('addShadow');
  //     } catch { }
  //   });
  // }

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

  // // tslint:disable-next-line: use-lifecycle-interface
  // ngAfterViewInit() {
  //   this.resizeObject(500);
  // }

  // @HostListener('window:resize', ['$event']) onResize(event) {
  //   // tslint:disable-next-line: no-unused-expression
  //   event.target.innerWidth;
  //   this.resizeObject(300);
  // }

  // @HostListener('window:click', ['$event.target'])
  // onClickWindow(targetElement: string) {
  //   //this.resizeObject(300);
  // }

  // @HostListener('document:click', ['$event']) onDocumentClick(evt: MouseEvent) {
  //   evt.target;
  //   // this.resizeObject(400);
  // }

  // //#region  FUNCTION HELPERS
  // private resizeObject(delay: number = 500) {
  //   setTimeout(() => {
  //     (<nextbackButtonResizer>this.nbResizer).scrollVisible('html').then((obj) => {
  //       // this.isScrollPage = (obj as boolean);
  //     }).catch((obj) => {
  //       //  this.isScrollPage = (obj as boolean);
  //     }).finally(() => {
  //       this.nbResizer.resizeObject().then(() => { }, () => { });
  //       this.onScroll();
  //     });
  //   }, delay);
  // }

  private _location: Location = null;

  get location(): Location {
    return this._location;
  }
  set location(value: Location) {
    this._location = value;
  }

  public arrUrl(url?: string): Array<any> {
    if (url?.length > 0) url.split("/");
    return this.location.path().split("/");
  }

  public getParentURL(): string {
    return this.location.path().split("/", 3).join('/') + "/";
  }

  public getSubmissionId(): string {
    const getId = this.location.path().split('/', 3)[2];
    // return isNaN(getId) ? 0 : getId;
    return getId;
  }

  public getParentMenuArray(): any {
    const navItems = createSubmissionDetailsMenuItems(this.getSubmissionId(), null, this.riskSpecificsData?.hiddenNavItems);
    this.removeHiddenParentItems(this.riskSpecificsData?.hiddenNavItems, navItems);
    navItems.forEach((parent) => {
      this.removeHiddenItems(parent, navItems);
    });
    return navItems;
  }

  public getCurrentModuleIndex(list: NavData[], val: NavData): number {
    return list.indexOf(val);
  }

  public getCurrentModuleObject(list: NavData[], val: string): NavData {
    // return list.find(x=> list.indexOf(x)  == idx);
    return list.find(e => e.url.toLowerCase().split("/").indexOf(val.toLowerCase()) > 0);
  }

  public getPrev(): string {
    return this.nextPrev(false);
  }

  public getNext(): string {
    return this.nextPrev(true);
  }

  nextPrev(isNext: boolean): string {
    let getCurrent: NavData;
    let getNextPrev: NavData;
    let arrNav: NavData[] = this.getParentMenuArray();
    let getIndexModule: number;

    let url = this.getParentURL();

    for (let i = 3; i < this.arrUrl().length; i++) {
      getCurrent = this.getCurrentModuleObject(arrNav, this.arrUrl()[i]);
      getIndexModule = this.getCurrentModuleIndex(arrNav, getCurrent);
      if (getCurrent.children && getCurrent.children.length) {
        arrNav = getCurrent.children;
      }
    }

    if (
      (getIndexModule == (arrNav.length - 1) && isNext) ||// only diff
      (getIndexModule == 0 && !isNext)// only diff
    ) {
      arrNav = this.getParentMenuArray();
      getCurrent = this.getCurrentModuleObject(arrNav, this.arrUrl()[3]);
      getIndexModule = this.getCurrentModuleIndex(arrNav, getCurrent);
    }

    if (isNext) { getNextPrev = arrNav[getIndexModule + 1]; } else if (!isNext) { getNextPrev = arrNav[getIndexModule - 1]; }// only diff

    if (getNextPrev) {
      while (getNextPrev.children && getNextPrev.children.length) {
        arrNav = getNextPrev.children;

        if (isNext) { getNextPrev = getNextPrev.children[0]; } else if (!isNext) { getNextPrev = getNextPrev.children[getNextPrev.children.length - 1]; }// only diff
      }
      url = getNextPrev.url;
    }
    return url;
  }

  private removeHiddenItems(obj: NavData, parent: NavData[]) {
    if (obj?.attributes && obj.attributes.hidden !== null) {
      const index = parent.findIndex(n => n.url === obj.url);
      parent.splice(index, 1);
      return;
    }
    if (obj.children && obj.children.length > 0) {
      obj.children.forEach((child) => {
        this.removeHiddenItems(child, obj.children);
      });
    }
  }

  // remove 2 consecutive screens if one is included already in hiddenItems
  private removeHiddenParentItems(hiddenItems: string[], navItems: NavData[]) {
    if (hiddenItems.length > 0) {
      if (hiddenItems?.includes('Historical Coverage') && hiddenItems?.includes('Claims History')) {
        const index = navItems.findIndex(n => n.name === 'HISTORICAL COVERAGE');
        navItems.splice(index, 2);
      }
    }
  }
  //#endregion  FUNCTION HELPERS
}


