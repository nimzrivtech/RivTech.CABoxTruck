import { Component, HostListener, Inject, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './core/services/auth.service';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'body',
  template: '<router-outlet></router-outlet>',
})
export class AppComponent implements OnInit {
  subscription: Subscription;
  constructor(private router: Router,
    private authService: AuthService,
    @Inject(DOCUMENT) private document: Document) {
      console.log('app comp constructor');
      this.checkIsLoggedInSessionStorage();
      let userLoggedInData;
      this.authService.getUserLoggedIn().subscribe(userLoggedIn => {
        userLoggedInData = userLoggedIn;
        if(userLoggedIn && this.authService.isLoggedIn()){
          this.authService.idle.watch();
        }
      });

      this.authService.getUserLoggedIn().subscribe(userLoggedIn => {
        userLoggedInData = userLoggedIn;
      });

      if(userLoggedInData === undefined){
        if(localStorage.getItem('isUserLoggedIn') === 'true' && this.authService.isLoggedIn()){
          this.authService.idle.watch();
        }
      }

      this.subscription = router.events.subscribe((event) => {
        if (event instanceof NavigationStart) {
          if(localStorage.getItem('isUserLoggedIn') === 'true'){
            this.authService.setUserLoggedIn(true);
          }
        }
    });
    
    
    window.onload = (e)=> {
      let tabId=sessionStorage.getItem("tabId");
      if (!tabId||(tabId!==sessionStorage.getItem("tabClosed"))){
          tabId=Math.random().toString(36).substr(2)+Date.now().toString(36).substr(5);
          sessionStorage.setItem("tabId",tabId);
      }

      if(localStorage.getItem("tabList")){
        let currentTabList = JSON.parse(localStorage.getItem("tabList"));
        let tabData = sessionStorage.getItem("tabId");
        let checkcurrentTabList = currentTabList.includes(tabData);
        if(!checkcurrentTabList){
          currentTabList.push(tabData);
          localStorage.removeItem('tabList');
          localStorage.setItem('tabList',JSON.stringify(currentTabList));
        }

      }else{
        let tabId=sessionStorage.getItem("tabId");
          let tabList = [];
          tabList.push(tabId)
          let newTabList = JSON.stringify(tabList);
          localStorage.setItem("tabList",newTabList);
      }
    }

    window.onbeforeunload = (e)=>{
      let tabIdDetails = sessionStorage.getItem('tabId');
      let tabListDetails = JSON.parse(localStorage.getItem('tabList'));

      tabListDetails = tabListDetails.filter(item => item != tabIdDetails);
      localStorage.removeItem('tabList');
      localStorage.setItem('tabList',JSON.stringify(tabListDetails));
    }

    let tabId=sessionStorage.getItem("tabId");
    sessionStorage.removeItem("tabClosed");
    window.addEventListener("unload",()=>{
        sessionStorage.setItem("tabClosed",tabId);
    });
  }

  @HostListener('document:click', ['$event'])
  handlerFunction(e: MouseEvent) {
    const url = this.router.url;
    const targetElement = document.querySelector('.my-modal');
    console.log('Trigger click event on page: ', url);
    if(localStorage.getItem('auth')){
      if(url === '/login'){
        this.document.location.href = '/dashboard';
      }
    } else {
        if (
          url !== '/login' &&
          !url.includes('/404') &&
          url !== '/account/forgot-password' &&
          url !== '/account/change-password-new-user' &&
          !url.includes('/account/reset-password') &&
          !url.includes('/redirectmakeapayment') &&
          !url.includes('/makeapayment') &&
          !url.includes('/payment-portal')
        ) {
          this.document.location.href = '/login';
        }
      }
  }

  ngOnInit() {
    this.authService.idleTime();
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }

  checkIsLoggedInSessionStorage(){
    console.log('isLoggedIn: ', !sessionStorage.getItem('isLoggedIn'));
    console.log('tabList', localStorage.getItem('tabList'));
    if(!sessionStorage.getItem('isLoggedIn')){
      let tabList = JSON.parse(localStorage.getItem('tabList'));
      console.log('tabList?.length', tabList?.length);
      if( tabList?.length < 1){
        localStorage.clear();
      }
    }
  }
}
