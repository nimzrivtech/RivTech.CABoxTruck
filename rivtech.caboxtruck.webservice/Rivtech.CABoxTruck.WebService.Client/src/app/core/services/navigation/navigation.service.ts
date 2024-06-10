import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, combineLatest, of } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { map, catchError, takeUntil } from 'rxjs/operators';
import { NavData } from '../../../_nav';
import { AuthService } from '../auth.service';
import { StorageService } from '../storage.service';
import { Menu } from '../../../shared/models/menu';
import { UserAccessRight } from '../../../shared/models/userAccessRight';
import NotifUtils from '../../../shared/utilities/notif-utils';
import JsUtils from '../../../shared/utilities/js.utils';
import { BaseComponent } from '../../../shared/base-component';
import {createSubmissionMenuItems} from '../../../modules/submission/pages/submission/submission-navitems';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NavigationService extends BaseComponent implements OnInit {

  selectedChildMenuList: NavData[];

  constructor(private http: HttpClient,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private storageService: StorageService) {

    super();
  }

  ngOnInit(): void { }

  initializeMenus(): Observable<any> {
    let values = combineLatest(
      this.getMenuList(),
      this.getAuthorizedMenuList()
    ).pipe(
      takeUntil(this.stop$),
      map(([menus, authMenus]) => {

        this.menuList = [];
        this.mainMenuList = [];
        this.allMenuList = [];

        menus = menus.map(m => {
          m.name = m.menuLabel;
          m.url = m.controllerName;
          return m;
        });

        this.allMenuList = menus;

        //Select only menus that are authorized
        menus = menus.filter(p => authMenus.map(a => a.menuId).indexOf(p.menuId) != -1);

        if (authMenus.length > 0) {
          let authorizedMenus: Menu[] = JsUtils.mergeArrayObjects(
            menus.map(m => ({ ...m, id: m.menuId })),
            authMenus.map(m => ({ ...m, id: m.menuId })), this.menuComparator);

          this.menuList = authorizedMenus;
          this.mainMenuList = authorizedMenus.filter(p => p.parentId == p.menuId);
        }
      })
      , catchError(() =>
        of(
          NotifUtils.showError("Something went wrong in initializing menus.")
        )
      )
    )

    return values;
  }

  navigateRoute(page: string, riskDetailId?: string) {
    const nextPageData = (page).split('/');
    const url = this.route.snapshot['_routerState'].url;
    const urlData = url.split('/');
    if (urlData && urlData[2].toLowerCase()  !== 'new') {
      nextPageData[2] = urlData[2];
    }
    if (riskDetailId && urlData[2].toLowerCase() === 'new') {
      nextPageData[2] = riskDetailId;
    }
    const nextUrl = nextPageData.join('/');
    return nextUrl;
  }


  private getMenuList(): Observable<Menu[]> {
    const applicationId = environment.ApplicationId;
    const url = `/api/Programs/${applicationId}/menus`;

    return this.http.get<any>(`${environment.IdentityServiceUrl}${url}`).pipe(
      takeUntil(this.stop$),
      map((res) => {
        return res;
      })
      , catchError(() =>
        of(
          NotifUtils.showError("Something went wrong in initializing menus.")
        )
      )
    )
  }


  private getAuthorizedMenuList(): Observable<UserAccessRight[]> {
    return this.authService.getUserAccessRights().pipe(
      takeUntil(this.stop$),
      map(res => { return res.filter(p => p.isView); }));
  }

  public getChildMenusByRoute(route: string) {

    // sheh temporary assign the menus
    this.selectedChildMenuList = createSubmissionMenuItems(1);
    return;

    let firstPath = '/' + route.split('/')[1];
    let parentMenu: Menu = this.menuList.filter(p => p.controllerName == firstPath)[0];
    // let regx = new RegExp('^' + firstPath, 'i');
    // let parentMenu: Menu = this.menuList.filter(p => regx.test(p.controllerName))[0];

    // this.getChildMenusByParentId(parentMenu.menuId);
    this.selectedChildMenuList = JsUtils.getNestedChildren(this.menuList.map(m => ({...m, id: m.menuId})), parentMenu.menuId);
  }

  public getChildMenusByParentId(menuId: number) {

    if (this.menuList) {
      return this.selectedChildMenuList = this.menuList.filter(q => q.parentId == menuId)
        .map((res) => { return { name: res.menuLabel, url: res.controllerName.replace(':id', '0') } });
      //TODO: id is temporarility set to zero. Fix this later
    }
  }

  public getMenuByControllerName(controllerName: string): Menu {

    let menu = this.menuList.filter(m => m.controllerName == controllerName)[0];

    return menu;
  }

  public get menuList(): Menu[] {
    return JSON.parse(this.storageService.getItem('menu'));
  }

  public set menuList(lstMenu: Menu[]) {
    this.storageService.setItem('menu', JSON.stringify(lstMenu));
  }

  public get mainMenuList(): Menu[] {
    return JSON.parse(this.storageService.getItem('mainMenu'));
  }

  public set mainMenuList(lstMenu: Menu[]) {
    this.storageService.setItem('mainMenu', JSON.stringify(lstMenu));
  }

  public get allMenuList(): Menu[] {
    return JSON.parse(this.storageService.getItem('allMenu'));
  }

  public set allMenuList(lstMenu: Menu[]) {
    this.storageService.setItem('allMenu', JSON.stringify(lstMenu));
  }

  private menuComparator(a, b) {
    return a.menuId - b.menuId;
  }

  navigatePolicyRoute(page: string, isApplicantPage: boolean = false): void {
    const url = this.route.snapshot['_routerState'].url;
    // console.log(page);
    const urlData = url.split('/');
    urlData[4] = page;
    isApplicantPage = urlData[5] ? true : isApplicantPage;
    if (isApplicantPage) {
      urlData.splice(5, 1);
    }
    const nextUrl = urlData.join('/');
    this.router.navigate([nextUrl]);
  }
}




