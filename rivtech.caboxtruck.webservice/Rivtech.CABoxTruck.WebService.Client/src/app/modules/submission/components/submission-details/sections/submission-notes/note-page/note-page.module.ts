import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoteSectionComponent } from './note-section/note-section.component';
import { SubmissionNotesRoutingModule } from '../submission-notes-routing.module';
import { NotePageComponent } from './note-page.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';



@NgModule({
  declarations: [
    NotePageComponent,
    NoteSectionComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    SubmissionNotesRoutingModule,
    SubmissionDetailsSharedModule,
  ],
  exports: [
    NoteSectionComponent
  ]
})
export class NotePageModule { }
