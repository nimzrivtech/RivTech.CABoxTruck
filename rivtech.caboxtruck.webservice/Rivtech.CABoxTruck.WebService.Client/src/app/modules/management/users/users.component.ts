import { Component, OnInit } from '@angular/core';
import { UserData } from '../data/user.data';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  constructor(private userData: UserData) { }

  ngOnInit() {
    this.userData.initializeForms();
    this.userData.resetList();
  }

}
