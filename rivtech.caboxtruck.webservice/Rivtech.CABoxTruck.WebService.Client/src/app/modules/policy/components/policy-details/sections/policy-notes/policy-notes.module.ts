import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PolicyNotesComponent } from './policy-notes.component';
import { PolicyNotesRoutingModule } from './policy-notes-routing.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { NotePageModule } from '../../../../../../modules/submission/components/submission-details/sections/submission-notes/note-page/note-page.module';
import { NotesData } from '@app/modules/submission/data/note/note.data';


@NgModule({
  declarations: [PolicyNotesComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    DataTablesModule,
    PolicyNotesRoutingModule,
    NotePageModule
  ],
  providers: [
    NotesData
  ]
})
export class PolicyNotesModule { }
