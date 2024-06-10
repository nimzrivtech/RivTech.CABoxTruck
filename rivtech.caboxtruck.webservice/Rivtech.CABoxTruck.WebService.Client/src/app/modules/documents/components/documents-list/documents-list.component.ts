import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DatePipe } from '@angular/common';
import { DocumentFile } from '../../document-file';
import { DocumentData } from '../../document.data';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { BsModalRef, BsModalService, PageChangedEvent } from 'ngx-bootstrap';
import { DocumentsAddDialogComponent } from '../documents-add-dialog/documents-add-dialog.component';
import { SubmissionData } from '../../../../modules/submission/data/submission.data';
import { TableData } from '../../../../modules/submission/data/tables.data';
import { TableConstants } from '../../../../shared/constants/table.constants';
import { NotificationMessageConstant } from '../../../../shared/constants/notification-message.constants';
import { BindingData } from '../../../../modules/submission/data/binding/binding.data';

@Component({
  selector: 'app-documents-list',
  templateUrl: './documents-list.component.html',
  styleUrls: ['./documents-list.component.scss'],
  providers: [DatePipe]
})
export class DocumentsListComponent implements OnInit {
  public showRowsNumber: number = 10;
  public showBillingRowsNumber: number = 10;
 


  @Input()
  items: DocumentFile[];
  allChecked: boolean = false;
  modalRef: BsModalRef;
  TableConstants = TableConstants;
  @Input() disableItems = {
    updateIcon: false,
    deleteIcon: false,
  };
  @Input() policy: boolean = false;
  @Input() isBillingCategory: boolean = false;
  @Input() isPolicyCategory: boolean = true;

  constructor(
    private modalService: BsModalService,
    public documentData: DocumentData,
    private submissionData: SubmissionData,
    private tableData: TableData,
    public datePipe: DatePipe,
    private bindingData: BindingData
  ) { }

  ngOnInit(): void {
    this.loadPaginationDefaults();
    this.documentData.documentList.subscribe(data => this.items = data);
    this.documentData.policy = this.policy;
    this.documentData.loadDocuments();
    this.documentData.policydocsOnFirstLoad = true;
  }

  onEditItem(doc?: DocumentFile) {
    if (!doc) {
      doc = DocumentFile.CreateNew(this.submissionData.riskDetail.id);

      this.openDocuModal(doc);
    } else {
      this.documentData.checkDocumentIsAllowedToEditDelete(doc.id).subscribe(data => {
        if (data === true) {
          this.openDocuModal(doc);
        } else {
          NotifUtils.showInfo(NotificationMessageConstant.fileNotAllowedToEditDeleteErrorMessage);
        }
      });
    }
  }

  openDocuModal(doc: DocumentFile) {
    this.documentData.changeEditDocumentForm(doc);
    this.modalRef = this.modalService.show(DocumentsAddDialogComponent, {
      initialState: {
        category: (this.isBillingCategory) ? 'Billing' : 'Policy'
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
  }

  onDeleteItem(doc: DocumentFile) {
    this.documentData.checkDocumentIsAllowedToEditDelete(doc.id).subscribe(data => {
      if (data == true) {
        NotifUtils.showConfirmMessage('Are you sure you want to delete this document?', () => {
          this.documentData.deleteDoc(doc);
          this.bindingData.DeleteFileRequirements(doc.tableRefId);
          this.bindingData.DeleteFileQuoteCondition(doc.tableRefId);
        });
      } else {
        NotifUtils.showInfo(NotificationMessageConstant.fileNotAllowedToEditDeleteErrorMessage);
        return;
      }
    });
  }

  toggleAllCheck() {
    this.documentData.toggleAllCheck(this.allChecked);
  }

  viewDocument(doc: DocumentFile) {
    console.log('document', doc);
    this.documentData.checkDocumentIsUploaded(doc.id).subscribe(data => {
      if (data == true) {
        this.documentData.getSASURL(doc.filePath).pipe().take(1).subscribe(data => {
          const win = window.open(data, '_blank');
          win.focus();
        });
      } else {
        NotifUtils.showInfo(NotificationMessageConstant.fileNotYetReadyErrorMessage);
      }
    });
  }

  downloadDocuments() {
    this.documentData.downloadDocs();
  }

  sort(targetElement) {
    this.tableData.sort(targetElement, this.items, 1);
  }

  changeShowRowsNumber(value): void{

    if (this.documentData.itemPerPage === value) { return; }
    this.showRowsNumber = value;
    this.documentData.itemPerPage = value;
    this.documentData.currentPage = 1;

    this.loadPolicyDocuments(this.documentData.currentPage, this.documentData.itemPerPage,this.documentData.keyword, this.documentData.sortField, this.documentData.order);
   
  }
  policyPageChanged(event: PageChangedEvent): void {
      this.documentData.currentPage = event.page;
      this.loadPolicyDocuments(this.documentData.currentPage, this.documentData.itemPerPage,this.documentData.keyword, this.documentData.sortField, this.documentData.order);
  }

  loadPolicyDocuments(currentPage:number,itemPerPage:number,keyword: string,sortField:string, order: string)
  {
    this.policy ? 
    this.documentData.getPolicyDocsList(this.documentData.getInitialPolicyListPayload(currentPage, itemPerPage, false,keyword,sortField, order)) : 
    this.documentData.getSubmissionDocsList(this.documentData.getInitialPolicyListPayload(currentPage, itemPerPage, false,keyword,sortField, order));
  }

  changeBillingRowsNumber(value): void{

    if (this.documentData.itemPerPageBilling === value) { return; }
    this.showBillingRowsNumber = value;
    this.documentData.itemPerPageBilling = value;
    this.documentData.currentPageBillingDocs = 1;

    this.loadBillingDocuments(this.documentData.currentPageBillingDocs, this.documentData.itemPerPageBilling,true,this.documentData.keywordBillingDocs, this.documentData.sortFieldBillingDocs, this.documentData.orderBillingDocs);
   
  }
  billingPageChanged(event: PageChangedEvent): void {
    
    this.documentData.currentPageBillingDocs = event.page;
    this.loadBillingDocuments(this.documentData.currentPageBillingDocs, this.documentData.itemPerPageBilling,true,this.documentData.keywordBillingDocs, this.documentData.sortFieldBillingDocs, this.documentData.orderBillingDocs);
  }

  loadBillingDocuments(loadcurrent:number,loadpageitem:number, isBilling: boolean, keyword: string = '',sortField:string = '', order: string = '')
  {
    this.policy ? 
    this.documentData.getBillingDocsList(this.documentData.getInitialPolicyListPayload(loadcurrent, loadpageitem, isBilling, keyword, sortField, order)) : 
    this.documentData.getSubmissionDocsList(this.documentData.getInitialPolicyListPayload(loadcurrent, loadpageitem, isBilling, keyword, sortField, order));
  }

  ngOnDestroy() {
    this.documentData.currentPage = 1;
    this.documentData.itemPerPage = 10;
    this.documentData.totalRowCount = 9;
    this.documentData.itemPerPageBilling = 10;
    this.documentData.currentPageBillingDocs = 1;
    this.documentData.totalBillingRowCount=9
  }

  
  searchDocuments(keyword: string) {
    this.documentData.currentPage = 1;
    this.documentData.sortField ='';
    this.documentData.order ='DESC';
    keyword = keyword?.trim().toLowerCase();
    this.documentData.keyword = keyword === '' ? null : keyword;
    this.loadPolicyDocuments(this.documentData.currentPage, this.documentData.itemPerPage,keyword, this.documentData.sortField, this.documentData.order);
  }

  sortBy(sortBy: string, isBilling: Boolean = false) {
    if (isBilling){
      this.documentData.currentPageBillingDocs = 1;
      this.documentData.sortFieldBillingDocs =sortBy;
      this.documentData.orderBillingDocs= this.documentData.orderBillingDocs =='ASC' ? 'DESC' : 'ASC';

      this.loadBillingDocuments(this.documentData.currentPageBillingDocs, 
        this.documentData.itemPerPageBilling,
        true,
        this.documentData.keywordBillingDocs, 
        this.documentData.sortFieldBillingDocs, 
        this.documentData.orderBillingDocs);
    }
    else{
      this.documentData.currentPage = 1;
      this.documentData.sortField =sortBy;
      this.documentData.order= this.documentData.order =='ASC' ? 'DESC' : 'ASC';
  
        this.loadPolicyDocuments(this.documentData.currentPage, 
                                 this.documentData.itemPerPage,
                                 this.documentData.keyword, 
                                 this.documentData.sortField, 
                                 this.documentData.order);
  
    }
  }

  loadPaginationDefaults(){
    this.documentData.keyword='';
    this.documentData.sortField='CreatedDate';
    this.documentData.order='DESC';

    this.documentData.keywordBillingDocs='';
    this.documentData.sortFieldBillingDocs='CreatedDate';
    this.documentData.orderBillingDocs='DESC';
  }
}
