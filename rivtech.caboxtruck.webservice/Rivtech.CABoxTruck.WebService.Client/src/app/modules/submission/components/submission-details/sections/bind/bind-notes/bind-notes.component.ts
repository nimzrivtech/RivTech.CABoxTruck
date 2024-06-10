import { Component, OnInit, ViewChild } from '@angular/core';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';

@Component({
  selector: 'app-bind-notes',
  templateUrl: './bind-notes.component.html',
  styleUrls: ['./bind-notes.component.scss']
})
export class BindNotesComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;

  constructor() { }

  ngOnInit() {
  }

}
