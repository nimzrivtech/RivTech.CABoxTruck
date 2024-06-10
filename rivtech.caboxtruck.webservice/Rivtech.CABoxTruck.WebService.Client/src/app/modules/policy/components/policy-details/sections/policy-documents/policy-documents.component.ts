import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '../../../../../../core/services/navigation/navigation.service';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { ToggleCollapseComponent } from '../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { DocumentData } from '../../../../../../modules/documents/document.data';

@Component({
  selector: 'app-policy-documents',
  templateUrl: './policy-documents.component.html',
  styleUrls: ['./policy-documents.component.css']
})
export class PolicyDocumentsComponent implements OnInit {
  @ViewChild('btnShowLoadingUploader') btnShowLoadingUploader;
  @Output() public onUploadFinished = new EventEmitter();
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  hidePolicyDocuments: boolean = false;
  hideBillingDocuments: boolean = false;
  disableItems = {
    updateIcon: true,
    deleteIcon: true,
  };

  constructor(private navigationService: NavigationService,
    public documentsData: DocumentData
    ) { }

  ngOnInit(): void {
    this.documentsData.sourcePage = 'PolicyDocuments';
  }

  public ToggleHidePolicyDocuments(): void {
    this.hidePolicyDocuments = !this.hidePolicyDocuments;
  }

  public ToggleHideBillingDocuments(): void {
    this.hideBillingDocuments = !this.hideBillingDocuments;
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Issuance);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Notes);
        break;
    }
  }

  searchDocuments(keyword: string) {
    this.documentsData.currentPage = 1;
    this.documentsData.sortField ='';
    this.documentsData.order ='DESC';
    keyword = keyword?.trim().toLowerCase();
    this.documentsData.keyword = keyword === '' ? null : keyword;
    this.loadPolicyDocuments(this.documentsData.currentPage, this.documentsData.itemPerPage,keyword, this.documentsData.sortField, this.documentsData.order);
  }

  sortBy(sortBy: string) {
    this.documentsData.currentPage = 1
    this.documentsData.sortField =sortBy;
    this.documentsData.order= this.documentsData.order =='ASC' ? 'DESC' : 'ASC';
    this.loadPolicyDocuments(this.documentsData.currentPage, this.documentsData.itemPerPage,this.documentsData.keyword, this.documentsData.sortField, this.documentsData.order);
  
  }

  loadPolicyDocuments(currentPage:number,itemPerPage:number,keyword: string,sortField:string, order: string)
  {
    this.documentsData.getPolicyDocsList(this.documentsData.getInitialPolicyListPayload(currentPage, itemPerPage, false,keyword,sortField, order)) 
  }
  
  clickDownloadAttachments() {
    this.documentsData.btnShowLoadingUploader = this.btnShowLoadingUploader;
    this.documentsData.onUploadFinished = this.onUploadFinished;
    this.documentsData.downloadAllDocuments();
  }
}

