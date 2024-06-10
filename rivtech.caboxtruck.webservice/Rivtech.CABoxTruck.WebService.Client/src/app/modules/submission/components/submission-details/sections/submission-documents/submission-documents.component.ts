import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { NextBackButtonComponent } from '../../submission-details-shared/components/next-back-button/next-back-button.component';
import { DocumentFile } from '../../../../../documents/document-file';
import { Router } from '@angular/router';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { Location } from '@angular/common';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { DocumentData } from '@app/modules/documents/document.data';

@Component({
  selector: 'app-submission-documents',
  templateUrl: './submission-documents.component.html',
  styleUrls: ['./submission-documents.component.css']
})
export class SubmissionDocumentsComponent implements OnInit {

  documents: DocumentFile[];
  hideMe: boolean = false;
  disableItems = {
    updateIcon: true,
    deleteIcon: true,
  };

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  constructor(private router: Router,
    private menuOverrideService: MenuOverrideService,
    public submissionData: SubmissionData,
    private documentsData: DocumentData,
    private location: Location) { }

  ngOnInit(): void {
    this.documentsData.sourcePage = 'BindDocuments';
  }


  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  public onClick(clickType?) {
    this.nextBack.location = this.location;
    let url = '';
    switch (clickType) {
      case ClickTypes.Back:
        url = this.nextBack.getPrev();
        this.router.navigate([url]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        url = this.nextBack.getNext();
        this.router.navigate([url]);
        break;
    }
  }

  searchDocuments(keyword: string) {
    this.documentsData.currentPage = 1;
    this.documentsData.sortField ='';
    this.documentsData.order ='DESC';
    keyword = keyword?.trim().toLowerCase();
    this.documentsData.keyword = keyword === '' ? null : keyword;
    this.loadSubmissionDocuments(this.documentsData.currentPage, this.documentsData.itemPerPage,keyword, this.documentsData.sortField, this.documentsData.order);
  }

  sortBy(sortBy: string) {
    this.documentsData.currentPage = 1
    this.documentsData.sortField =sortBy;
    this.documentsData.order= this.documentsData.order =='ASC' ? 'DESC' : 'ASC';
    this.loadSubmissionDocuments(this.documentsData.currentPage, this.documentsData.itemPerPage,this.documentsData.keyword, this.documentsData.sortField, this.documentsData.order);
  
  }

  loadSubmissionDocuments(currentPage:number,itemPerPage:number,keyword: string,sortField:string, order: string)
  {
    this.documentsData.getSubmissionDocsList(this.documentsData.getInitialPolicyListPayload(currentPage, itemPerPage, false,keyword,sortField, order)) 
  }
}
