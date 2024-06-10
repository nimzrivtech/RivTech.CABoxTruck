import { Component, OnInit } from '@angular/core';
import {formatDate} from '@angular/common';

@Component({
  selector: 'app-footer-admin',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
  
  currentAppVersion = formatDate(new Date(), 'MMddyyyy', 'en') + "-1";

  constructor() { }

  ngOnInit() {
  }

}
