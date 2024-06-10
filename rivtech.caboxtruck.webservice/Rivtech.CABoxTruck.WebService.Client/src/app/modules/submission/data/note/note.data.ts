import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PagerService } from '../../../../core/services/pager.service';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { take } from 'rxjs/operators';
import { NoteService } from '../../services/notes.service';
import { SubmissionData } from '../submission.data';
import { TableData } from '../tables.data';
import { NoteCategoryOptionDTO } from '../../../../shared/models/submission/note/NoteDto';
import { UserData } from '@app/modules/management/data/user.data';

@Injectable()
export class NotesData {
  notes: any[];
  categoryList: NoteCategoryOptionDTO[];
  notesForm: FormGroup;
  dateOptions: IAngularMyDpOptions;
  pager: any = {};
  currentPage: number = 0;
  pagedItems: any[];
  loading: boolean;
  BIND_NOTES = 'BindNotes';
  POLICY_NOTES = 'PolicyNotes';
  sourcePage: string = this.BIND_NOTES;

  constructor(
    private fb: FormBuilder,
    private noteService: NoteService,
    private userData: UserData,
    private submissionData: SubmissionData,
    private tableData: TableData,
    private pagerService: PagerService
  ) { }

  initiateFormFields() {
    this.notes = [];
    this.categoryList = [];
    this.notesForm = this.fb.group({
      categoryId: [null, Validators.required],
      description: [null, Validators.required],
      dateAdded: [null]
    });

    this.initDateOptions();
    this.notesForm.get('dateAdded').setValue({ isRange: false, singleDate: { jsDate: this.submissionData.currentServerDateTime } });
  }

  retrieveDropdownValues() {
    // tslint:disable-next-line: deprecation
    this.noteService.getNoteCategoryOptions().pipe(take(1)).subscribe(data => {
      this.categoryList = data;
    });
  }

  populateFields() {
    if (this.submissionData.riskDetail.riskNotes) {
      this.notes = this.submissionData.riskDetail.riskNotes;
      this.setPage(1, this.notes);
    }
  }

  updateNotes() {
    if (this.submissionData.riskDetail.riskNotes) {
      this.submissionData.riskDetail.riskNotes.forEach((data) => {
        const user = this.userData.userListData.find(x => x.userId == data.createdBy);
        data.createdByName = `${user?.firstName} ${user?.lastName}`;
      });
    }
  }

  initDateOptions() {
    this.dateOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
    };
  }

  sort(targetElement) {
    let sortData = this.tableData.sortPagedData(targetElement, this.notes, this.currentPage);
    this.notes = sortData.sortedData;
    this.pagedItems = sortData.pagedItems;
  }

  setPage(page: number, data: any[], isNew: boolean = false) {
    if (page < 1) {
      return;
    }

    this.pager = this.pagerService.getPager(this.notes.length, page);

    if (isNew) {
      this.currentPage = this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }

      this.currentPage = page;
    }

    this.pagedItems = data.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  moveToLastPage() {
    this.pager = this.pagerService.getPager(this.notes.length);
    this.setPage(this.pager.endPage, this.notes);
  }
}
