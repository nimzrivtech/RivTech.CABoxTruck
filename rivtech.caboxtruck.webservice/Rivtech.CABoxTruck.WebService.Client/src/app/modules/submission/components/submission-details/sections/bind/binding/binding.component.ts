import { Component, OnInit, ViewChild } from '@angular/core';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { BindingData } from '../../../../../../../modules/submission/data/binding/binding.data';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';



@Component({
  selector: 'app-binding',
  templateUrl: './binding.component.html',
  styleUrls: ['./binding.component.scss']
})
export class BindingComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  @ViewChild(ToggleCollapseComponent) toggleCollapseCarrierInfo: ToggleCollapseComponent;

  constructor(public Data: BindingData,
    public submissionData: SubmissionData
  ) { }

  get fg() { return this.Data.formBinding; }
  get fc() { return this.fg.controls; }

  ngOnInit() {
    this.Data.bindingDefault();
  }
}
