import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { PathConstants } from '../../shared/constants/path.constants';
import { Menu } from '../../shared/models/menu';
import { NavigationService } from '../services/navigation/navigation.service';
import JsUtils from '../../shared/utilities/js.utils';

@Injectable()
export class RoleGuard implements CanActivate {
    constructor(private _router: Router, private navService: NavigationService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      return true;
        let authorizedMenus: Menu[] = this.navService.menuList;

        let selectedControllerName: string = route.data.controllerName;

        let menu = authorizedMenus.find(m => m.controllerName == selectedControllerName);
        if (menu && authorizedMenus.map(res => res.menuId).indexOf(menu.parentId) > -1) {
            return true;
        }
        else {
            let firstPath: string = '/' + route.data.controllerName.split('/')[1];

            //If dashboard is not accessible, go to the first accessible menu
            if (firstPath == `/${PathConstants.Dashboard.Index}`) {

                //get parent menu
                let parentMenus = authorizedMenus.filter(menu => menu.parentId == menu.menuId || menu.menuId == null);

                if (parentMenus.length > 0) {
                    let firstAccessibleMenu = parentMenus[0];

                    this._router.navigateByUrl(firstAccessibleMenu.controllerName);
                    return true;
                }
            }
            else {
                //Redirect to first accessible child menu
                let parentMenu: Menu = authorizedMenus.filter(p => p.controllerName == firstPath)[0];

                if (parentMenu) {
                    let childMenus: Menu[] = JsUtils.getNestedChildren(
                        authorizedMenus.map(m => ({...m, id: m.menuId})),
                        parentMenu.menuId);

                    if (childMenus.length > 0) {
                        this._router.navigateByUrl(childMenus[0].controllerName);

                        return true;
                    }
                }
            }
        }

        this._router.navigateByUrl('/auth');

        return false;
    }
}
