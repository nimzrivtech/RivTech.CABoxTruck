import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable, of } from 'rxjs';

export interface CanComponentDeactivate {
    canDeactivate: (nextState: RouterStateSnapshot) => Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree;
}

@Injectable()
export class CanDeactivateGuard implements CanDeactivate<CanComponentDeactivate> {

  canDeactivate(component: CanComponentDeactivate, 
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot,
  nextState: RouterStateSnapshot) {
    return component.canDeactivate ? component.canDeactivate(nextState) : true;
  }
}