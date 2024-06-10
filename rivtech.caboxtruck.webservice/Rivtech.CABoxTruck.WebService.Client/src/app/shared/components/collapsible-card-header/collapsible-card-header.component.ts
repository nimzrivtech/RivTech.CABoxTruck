import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-collapsible-card-header',
  templateUrl: './collapsible-card-header.component.html',
  styleUrls: ['./collapsible-card-header.component.scss']
})
export class CollapsibleCardHeaderComponent implements OnInit {

  @Input() text: string;
  @Input() isOpen: boolean;
  @Input() target: string;  

  get targetId() : string {
    return `#${this.target}`;
  }

  constructor() { }

  ngOnInit() {
  }

  toggle() {
    this.isOpen = !this.isOpen;
  }
}
