import { Component, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { NotesData } from '../../../../../../../modules/submission/data/note/note.data';

@Component({
  selector: 'app-note-page',
  templateUrl: './note-page.component.html',
  styleUrls: ['./note-page.component.scss']
})
export class NotePageComponent implements OnInit {

  hideMe: boolean = false;

  constructor(
    public location: Location,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    public notesData: NotesData
  ) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  ngOnInit() {
    this.notesData.sourcePage = this.notesData.BIND_NOTES;
  }

  public onClick(clickType?) {
    this.nextBack.location = this.location;
    const prevUrl = this.nextBack.getPrev();
    const nextUrl = this.nextBack.getNext();
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([prevUrl]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([nextUrl]);
        break;
    }
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
