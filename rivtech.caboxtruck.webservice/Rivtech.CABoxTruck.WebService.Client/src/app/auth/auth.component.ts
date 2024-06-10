import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { environment } from "environments/environment";
import { NavigationService } from '../core/services/navigation/navigation.service';
import Utils from '../shared/utilities/utils';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService,private navService: NavigationService) { }

  ngOnInit() {
    this.authService.setAuth(null);
    this.setToken();
  }

  setToken(){
    let token;
    let qtoken: string = this.route.snapshot.queryParamMap.get('token');
    if(qtoken){
        try{
          this.authService.setAuth({ "token": qtoken, "expiration":0, "refresh_token":null });
          let auth = this.authService.getAuth();
          token = auth ? auth.token : null;
          this.navService.initializeMenus().subscribe(() => {
              Utils.unblockUI();
              this.router.navigate(["/dashboard"]);
            },err => {
              Utils.unblockUI();
          });
        }
        catch(e){
          // window.location.href = environment.ClientManagementUrl;
        }
    }else{
      // window.location.href = environment.ClientManagementUrl;
    }
  }
}
