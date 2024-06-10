import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SubmissionNotesRoutingModule } from './submission-notes-routing.module';
import { FormsModule } from '@angular/forms';
import { NotePageModule } from './note-page/note-page.module';
import { SubmissionNotesComponent } from './submission-notes.component';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [SubmissionNotesComponent],
  imports: [
    CommonModule,
    FormsModule,
    SubmissionNotesRoutingModule,
    NotePageModule,
    SubmissionDetailsSharedModule,
  ]
})
export class SubmissionNotesModule { }
