import { Component, Input, OnInit } from '@angular/core';
import { NotesData } from '../../../../../../../../modules/submission/data/note/note.data';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { NoteService } from '../../../../../../../../modules/submission/services/notes.service';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NoteDTO } from '../../../../../../../../shared/models/submission/note/NoteDto';
import Utils from '../../../../../../../../shared/utilities/utils';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { formatDate } from '@angular/common';
import { ErrorMessageConstant } from '../../../../../../../../shared/constants/error-message.constants';
import { UserIdentity } from '../../../../../../../../shared/constants/user-identity.constants';
import { AuthService } from '@app/core/services/auth.service';
import { UserService } from '@app/core/services/management/user.service';
import { environment } from '../../../../../../../../../environments/environment';
import { UserData } from '@app/modules/management/data/user.data';

@Component({
  selector: 'app-note-section',
  templateUrl: './note-section.component.html',
  styleUrls: ['./note-section.component.scss']
})
export class NoteSectionComponent implements OnInit {
  hideMe: boolean = false;
  addNewItem: boolean = false;
  notesForm: FormGroup;
  notesModel: any;
  editItem: any;
  isEdit: boolean = false;
  selectedRow: number = -1;
  TableConstants = TableConstants;
  UserIdentity = UserIdentity;
  currentUser: string;
  currentUserName: string;

  constructor(
    protected fb: FormBuilder,
    public notesData: NotesData,
    private noteService: NoteService,
    private userService: UserService,
    private userData: UserData,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    private authService: AuthService) { }

  ngOnInit() {
    this.getUserIdentity(this.authService.getCurrentUserId());
    this.notesData.initiateFormFields();
    this.notesData.retrieveDropdownValues();
    this.userData.getUserList(() => {
      this.notesData.updateNotes();
      this.notesData.populateFields();
    });
  }

  onCreateNewItem(): void {
    this.notesData.initiateFormFields();
    this.notesData.retrieveDropdownValues();
    this.notesData.populateFields();
    this.notesData.moveToLastPage();
    this.addNewItem = true;
  }

  openEditNoteModal(item, index) {
    if (this.addNewItem || item.sourcePage !== this.notesData.sourcePage) { return; }
    if (!(item.createdBy == this.currentUser)) { return; }
    if (this.disableActionButton) { return; }
    this.isEdit = true;
    this.editItem = item;
    this.selectedRow = index;
    this.notesData.notesForm.controls['categoryId'].patchValue(this.editItem.categoryId);
    this.notesData.notesForm.controls['description'].patchValue(this.editItem.description);
    this.notesData.notesForm.controls['dateAdded'].patchValue({ isRange: false, singleDate: { jsDate: new Date(this.editItem.createdDate) } });
  }

  deleteNote(item) {
    if (item.sourcePage !== this.notesData.sourcePage) { return; }
    if (!this.actionsEnabled(formatDate(item.createdDate, 'MM/dd/yyyy', 'en'), item.createdBy)) { return; }
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${item.noteCategoryOption.description}' note?`, () => {
      this.noteService.deleteNote(item.id).subscribe(() => {
        const index: number = this.notesData.notes.indexOf(item);
        if (index !== -1) {
          this.notesData.notes.splice(index, 1);
          this.submissionData.riskDetail.riskNotes = this.notesData.notes;
          this.notesData.setPage(this.notesData.currentPage, this.notesData.notes);
          this.addNewItem = false;
          this.isEdit = false;
        }
      });
    });
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  initForm(): void {
    const currentServerDateTime = this.submissionData.currentServerDateTime;
    this.notesForm.get('dateAdded').setValue({ isRange: false, singleDate: { jsDate: new Date(this.notesModel?.dateAdded ?? currentServerDateTime) } });
  }

  get f() { return this.notesData.notesForm.controls; }

  onCancel(value: any): void {
    NotifUtils.showConfirmMessage('You have unsaved changes that will be lost', () => {
        this.notesData.notesForm.reset();
        this.addNewItem = false;
        this.isEdit = false;
      },
      () => { }
    );
  }

  saveNote() {
    const data = new NoteDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      categoryId: this.notesData.notesForm.value.categoryId ? parseInt(this.notesData.notesForm.value.categoryId) : null,
      description: this.notesData.notesForm.value.description,
      sourcePage: this.notesData.sourcePage
    });

    if (this.isEdit) {
      data.id = this.editItem.id;
    }

    Utils.blockUI();
    // tslint:disable-next-line: deprecation
    this.noteService.saveNote(data).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      if (this.isEdit) {
        console.log('Note Edited!');
        this.editItem.description = data.description;
        this.editItem.categoryId = data.categoryId;
        this.editItem.noteCategoryOption = data.noteCategoryOption;
        this.notesData.notesForm.reset();
        this.addNewItem = false;
        this.isEdit = false;
        return;
      }

      console.log('Note Added!');
      data.createdDate = this.submissionData.currentServerDateTime;
      const user = this.userData.userListData.find(x => x.userId == data.createdBy);
      data.createdByName = `${user?.firstName} ${user?.lastName}`;
      this.notesData.notes.push(data);
      this.submissionData.riskDetail.riskNotes = this.notesData.notes;
      this.notesData.notesForm.reset();
      this.addNewItem = false;
      this.isEdit = false;
      if (this.notesData.currentPage < 1) { this.notesData.currentPage = 1; }
      this.notesData.setPage(this.notesData.currentPage, this.notesData.notes, true);
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
  }

  actionsEnabled(createdDate, createdBy) {
    const currentServerDateTime = this.submissionData.currentServerDateTime;
    if (currentServerDateTime == null || currentServerDateTime === undefined) { return; }
    const currentDate = formatDate(currentServerDateTime, 'MM/dd/yyyy', 'en');
    createdDate = formatDate(createdDate, 'MM/dd/yyyy', 'en');
    return (createdDate === currentDate && createdBy == this.currentUser);
  }

  get disableActionButton() {
    return this.submissionData?.isIssued && this.notesData.sourcePage === this.notesData.BIND_NOTES;
  }

  formatDateOnGrid(date) {
    return formatDate(new Date(date), 'MM/dd/yyyy', 'en');
  }

  getUserIdentity(userId) {
    this.currentUser = userId;
    this.userService.getUserInfoById(userId).subscribe((data) => {
      this.currentUserName = `${data?.firstName} ${data?.lastName}`;
    });
  }
}
