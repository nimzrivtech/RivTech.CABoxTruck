import { Component, OnInit, Input } from '@angular/core';
import { StatusCounter } from './status-counter';

@Component({
  selector: 'app-status-counter',
  templateUrl: './status-counter.component.html',
  styleUrls: ['./status-counter.component.scss']
})
export class StatusCounterComponent implements OnInit {

  @Input() model: StatusCounter;

  constructor() { }

  ngOnInit() {
  }

}
