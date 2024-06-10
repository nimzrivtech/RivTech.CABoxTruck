import { Component, OnInit } from '@angular/core';
import { LayoutService } from '../../../../core/services/layout/layout.service';

@Component({
  selector: 'app-policy-list',
  templateUrl: './policy-list.component.html',
  styleUrls: ['./policy-list.component.css']
})
export class PolicyListComponent implements OnInit {

  constructor(private layoutService: LayoutService) { }

  ngOnInit(): void {
    this.layoutService.leftSidebarVisible = false;
  }

}
