import { Component, OnInit, ViewChild } from '@angular/core';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';

@Component({
  selector: 'app-bind-documents',
  templateUrl: './bind-documents.component.html',
  styleUrls: ['./bind-documents.component.scss']
})
export class BindDocumentsComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  disableItems = {
    updateIcon: true,
    deleteIcon: true,
  };

  constructor(public submissionData: SubmissionData) { }

  ngOnInit() { }
}
