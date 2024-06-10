import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation-dialogue',
  templateUrl: './confirmation-dialogue.component.html'
})
export class ConfirmationDialogueComponent implements OnInit {
  constructor(public confirmationDialogRef: MatDialogRef<ConfirmationDialogueComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

  onYesClick(): void {
    this.confirmationDialogRef.close(true);
  }

  onNoClick(): void {
    this.confirmationDialogRef.close(false);
  }
}
