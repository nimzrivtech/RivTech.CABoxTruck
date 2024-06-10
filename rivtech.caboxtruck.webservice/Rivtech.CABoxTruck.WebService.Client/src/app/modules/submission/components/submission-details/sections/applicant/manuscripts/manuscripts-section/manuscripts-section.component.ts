import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { BsModalRef, BsModalService, PageChangedEvent } from 'ngx-bootstrap';
import { ManuscriptDialogComponent } from './manuscript-dialog.component';
import { SubmissionData } from '../../../../../../data/submission.data';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { ManuscriptsData } from '../../../../../../../../modules/submission/data/manuscripts/manuscripts.data';
import { ManuscriptDTO } from '../../../../../../../../shared/models/submission/manuscript/ManuscriptDto';

@Component({
  selector: 'app-manuscripts-section',
  templateUrl: './manuscripts-section.component.html',
  styleUrls: ['./manuscripts-section.component.scss']
})
export class ManuscriptsSectionComponent implements OnInit {
  modalRef: BsModalRef | null;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  hideMe: boolean = false;
  TableConstants = TableConstants;
  itemPerPage: number = 10;
  isNumRowsChanged : boolean = false;
  rowsAfterChange : number;
  localEvent: PageChangedEvent = { itemsPerPage:10, page:1};
  public showRowsNumber: number = 10;

  constructor(private modalService: BsModalService,
    public submissionData: SubmissionData,
    public data: ManuscriptsData) {
  }

  ngOnInit() {
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };
    this.data.sortField = 'CreatedDate';
    this.data.order  = 'ASC';
    this.data.currentPage = 1;
    this.data.itemPerPage = 10;
    this.data.getAllManuscriptList(this.data.getMSListPayload(this.data.currentPage, this.data.itemPerPage, false)) ;
  }

  get form() { return this.data.manuscriptForm; }

  edit(manuscript: ManuscriptDTO) {
    this.showDialog(manuscript, 'Edit Manuscript', 'Save');
  }

  add() {
    const broker = this.submissionData.riskDetail?.brokerInfo;
    const manuscript = new ManuscriptDTO({
      effectiveDate: broker?.effectiveDate,
      expirationDate: broker?.expirationDate,
      isProrate: false
    });
    this.showDialog(manuscript, 'Add Manuscript', 'Add Manuscript');
  }

  delete(manuscript: ManuscriptDTO) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${manuscript.title}'?`, () => {
      this.data.delete(manuscript.id);
    });
  }

  alPdChanged(manuscript: ManuscriptDTO, value: string) {
    manuscript.premiumType = value;
    this.data.save(manuscript);
  }

  prorateChanged(manuscript: ManuscriptDTO, value: boolean, field: string) {
    manuscript.isProrate = value;
    this.data.save(manuscript);
  }

  private showDialog(manuscript: ManuscriptDTO, title: string, saveText: string) {
    this.form.reset();
    this.form.patchValue(manuscript);
    this.modalRef = this.modalService.show(ManuscriptDialogComponent, {
      initialState: {
        isEdit: true,
        modalTitle: title,
        modalButton: saveText,
      },
      backdrop: true,
      ignoreBackdropClick: true,
    });
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  get manuscriptEffectiveDate() {
    return this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.data.currentServerDateTime);
  }

  get manuscriptExpirationDate() {
    return this.submissionData.riskDetail?.brokerInfo?.expirationDate ?? new Date(this.data.currentServerDateTime);
  }

  changeShowRowsNumber(value): void{
    this.showRowsNumber = value;
    this.data.itemPerPage = value;
    this.localEvent.itemsPerPage = value;
    this.localEvent.page =this.localEvent.page>1 ? 1: this.localEvent.page;
    this.isNumRowsChanged = true;
    this.msPageChanged(this.localEvent);
  }

  msPageChanged(event: PageChangedEvent): void {
    this.localEvent = event;
    event.itemsPerPage = this.isNumRowsChanged? this.showRowsNumber  : event.itemsPerPage;
    this.data.getAllManuscriptList(this.data.getMSListPayload(event.page, event.itemsPerPage, false)) ;
  }
  
  sort(targetElement) {
    const property = targetElement.getAttribute('data-name');
    this.data.sortField =property;
    this.data.order= this.data.order =='ASC' ? 'DESC' : 'ASC';
    this.data.getSortManuscript(this.data.getMSListPayload(this.data.currentPage, this.data.itemPerPage, false)) ;
  }
  ngOnDestroy() {
    this.data.currentPage = 1;
    this.data.itemPerPage = 10;
  }

}
