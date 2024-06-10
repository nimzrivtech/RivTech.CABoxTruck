import { Component, OnInit } from '@angular/core';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { NavigationService } from '../../../../../../core/services/navigation/navigation.service';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { NotesData } from '@app/modules/submission/data/note/note.data';

@Component({
  selector: 'app-policy-notes',
  templateUrl: './policy-notes.component.html',
  styleUrls: ['./policy-notes.component.css']
})
export class PolicyNotesComponent implements OnInit {
  hideMe: boolean = false;

  constructor(private navigationService: NavigationService,
    private notesData: NotesData
  ) { }

  ngOnInit() {
    this.notesData.sourcePage = this.notesData.POLICY_NOTES;
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Documents);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Tasks);
        break;
    }
  }

}
