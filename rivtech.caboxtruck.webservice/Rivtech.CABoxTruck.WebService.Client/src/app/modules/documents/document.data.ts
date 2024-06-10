import { HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import Utils from '../../shared/utilities/utils';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { BehaviorSubject, Observable } from 'rxjs';
import { SubmissionData } from '../submission/data/submission.data';
import { DocumentApiService } from './document-api.service';
import { DocumentFile, IPolicyDocumentResultDTO, IPolicyDocumentListRequestDTO } from './document-file';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { TableData } from '../submission/data/tables.data';
import { PagerService } from '../../core/services/pager.service';
import * as moment from 'moment';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../core/services/auth.service';
@Injectable({
    providedIn: 'root'
})
export class DocumentData {
    //new implementation of paging declarion
    itemPerPage: number = 10;
    itemPerPageBilling : number =10;
    totalRowCount: number = 9;
    totalBillingRowCount:number =9;
    OnFirstLoad: boolean = true;
    docListPagedItems: DocumentFile[] = [];
    docListPager: any = {};
    policyDocList : DocumentFile[] = [];
    policydocsOnFirstLoad: boolean = true;

    loading: boolean;
    pager: any = {};
    currentPage: number = 1;
    pagedItems: any[];
    keyword : string;
    sortField : string;
    order : string;

    loadingBillingDocs: boolean;
    pagerBillingDocs: any = {};
    currentPageBillingDocs: number = 1;
    pagedItemsBillingDocs: any[];
    keywordBillingDocs : string;
    sortFieldBillingDocs : string;
    orderBillingDocs : string;

    sourcePage: string = '';

    datepickerOptions: IAngularMyDpOptions;

    categories: any[];

    policy: boolean = false;

    documentFormData: DocumentFile;
    /* Observable list of documents.  */
    private documentListData: BehaviorSubject<DocumentFile[]> = new BehaviorSubject<DocumentFile[]>([]);
    public documentsListData: any = {};
    public billingDocumentsListData: any = {};

    get documentForm(): DocumentFile {
        return this.documentFormData ?? DocumentFile.CreateNew();
    }

    get documentList(): Observable<DocumentFile[]> {
        return this.documentListData.asObservable();
    }

    get isFormDocumentNew(): boolean {
        return this.documentForm?.id == null ?? true;
    }

    get disableActionButton(): boolean {
        return this.submissionData?.isIssued && this.sourcePage === 'BindDocuments';
    }

    private baseUrl: string;
    btnShowLoadingUploader: any;
    message: string;
    dotanimation: string = '';
    progress: number;
    onUploadFinished: any;
    constructor(
        private submissionData: SubmissionData,
        private documentApiService: DocumentApiService,
        private tableData: TableData,
        public pagerService: PagerService,
        private toastr: ToastrService,
        private authService: AuthService
    ) {

        this.datepickerOptions = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy',
            disableSince: { year: 0, month: 0, day: 0 },
            disableUntil: { year: 0, month: 0, day: 0 }
        };

        this.documentApiService.getCategories().subscribe(data => {
            this.categories = data;
        });
        this.baseUrl = environment.ApiUrl.replace('/api', '');
    }

    checkDocumentIsUploaded(id: string) {
        return this.documentApiService.getDocumentIsUploaded(id);
    }

    checkDocumentIsAllowedToEditDelete(id: string) {
        return this.documentApiService.getDocumentIsAllowedToEditDelete(id);
    }


    changeEditDocumentForm(doc?: DocumentFile) {
        doc.dateAdded = this.formatDateForPicker(doc.createdDate);
        this.documentFormData = doc;
    }

    clearDocumentForm() {
        this.documentFormData = null;
    }

    get defaultPolicyState(): string { return this.submissionData.riskDetail?.nameAndAddress?.state; }

    loadDocuments() {
        if (this.policy) {
          this.getPolicyDocsList(this.getInitialPolicyListPayload(this.currentPage, this.itemPerPage, false,'','',''));
          this.getBillingDocsList(this.getInitialPolicyListPayload(this.currentPageBillingDocs, this.itemPerPageBilling, true,'','',''));
          this.loadBillingDocuments();
        } else {
            this.getSubmissionDocsList(this.getInitialPolicyListPayload(this.currentPage,  this.itemPerPage,false,'','',''));
        }
      }

    saveDocument(riskDocument: DocumentFile, fileInfo: any, modal: any) {
        Utils.blockUI();
        const formData = new FormData();
        if (fileInfo?.length > 0) {
            // const file = <File>fileInfo[0];
            // formData.append('file', file, file.name);
            Array.from(fileInfo).forEach(file => {
                if (file instanceof File) {
                  formData.append('file', (<File>file), (<File>file).name);
                }
              });
        }

        riskDocument.createdDate = riskDocument.dateAdded?.singleDate?.jsDate?.toLocaleDateString();
        formData.append('riskDocument', JSON.stringify(riskDocument));
        formData.append('states', this.defaultPolicyState);

        this.documentApiService.addDocument(formData).subscribe(data => {
            if (data.body?.message) {
                NotifUtils.showError(data.body.message);
                return;
            }

            if (data.type === HttpEventType.Response) {
                this.loadDocuments();
                Utils.unblockUI();
                modal.hide();
            }
        });
    }

    deleteDoc(doc: DocumentFile) {
        this.documentApiService.delete(doc.id).subscribe(() => {
            this.loadDocuments();
        });
    }

    toggleAllCheck(value: boolean) {
        const newData = this.documentListData.getValue();
        newData.forEach(x => x.checked = value);
        this.documentListData.next(newData);
    }

    downloadDocs() {        
        const selectedItemIds = this.documentListData.getValue().filter(x => x.checked).map(x => x.id);
        if (selectedItemIds.length === 0) { return; }
        Utils.blockUI();
        this.documentApiService.downloadDocs(selectedItemIds).subscribe((data) => {
            if (data.type === HttpEventType.Response) {
                Utils.unblockUI();
                const blob = new Blob([data.body], { type: 'application/zip' });
                const url = window.URL.createObjectURL(blob);
                const anchor = document.createElement('a');
                anchor.download = `BT Documents.zip`;
                anchor.href = url;
                anchor.click();
            }
        });
    }

    emailDocs() {
        const selectedItemIds = this.documentListData.getValue().filter(x => x.checked).map(x => x.id);
        this.documentApiService.emailDocs(selectedItemIds);
    }

    formatDateForPicker(dateValue?: any) {
        const dateSet = dateValue ? new Date(dateValue) : new Date();
        return { isRange: false, singleDate: { jsDate: dateSet } }
    }

    getSASURL(id: string) {
        return this.documentApiService.getDocumentSASURL(id);
    }

    getPolicyDocsList(payload: IPolicyDocumentListRequestDTO = {}): void {
        this.documentApiService.getPolicyDocuments(payload).subscribe(data => {
            this.totalRowCount = data.paginationDetails.totalCount;

            var resullist = data.fileUploadDocuments;
            resullist.forEach(d => {
                d.category = this.categories.find(c => c.id === d.fileCategoryId)?.description;
            });
            //resullist = resullist.sort((a, b) => a.createdDate > b.createdDate ? -1 : 1);
            this.pagedItems = resullist;
        });
        
      }
    getBillingDocsList(payload: IPolicyDocumentListRequestDTO = {}): void {
        this.documentApiService.getPolicyDocuments(payload).subscribe(data => {
            this.totalBillingRowCount = data.paginationDetails.totalCount;

            var resullist = data.fileUploadDocuments;
            resullist.forEach(d => {
                d.category = this.categories.find(c => c.id === d.fileCategoryId)?.description;
            });
            //resullist = resullist.sort((a, b) => a.createdDate > b.createdDate ? -1 : 1);
            this.pagedItemsBillingDocs = resullist;
           
        });
        
      }
      
    getSubmissionDocsList(payload: IPolicyDocumentListRequestDTO = {}): void {
        this.documentApiService.getPolicyDocuments(payload).subscribe(data => {
            this.totalRowCount = data.paginationDetails.totalCount;

            var resullist = data.fileUploadDocuments.filter(d => d.fileCategoryId);
            resullist.forEach(d => {
                d.category = this.categories.find(c => c.id === d.fileCategoryId)?.description;
            });
            //resullist = resullist.sort((a, b) => a.createdDate > b.createdDate ? -1 : 1);
           
            this.documentsListData = data;
            this.pagedItems = resullist;
        });
      }

    loadBillingDocuments() {
        this.documentApiService.getRiskDocuments(this.submissionData.riskDetail?.riskId).subscribe(data => {
            data = data.filter(d => d.fileCategoryId && d.fileCategoryId === 3);
            console.log(JSON.stringify(data.filter(d => d.fileCategoryId)));
            data.forEach(d => {
                d.category = this.categories.find(c => c.id === d.fileCategoryId)?.description;
            });
            data = data.sort((a, b) => a.createdDate > b.createdDate ? -1 : 1);
            // this.documentListData.next(data);
            this.billingDocumentsListData = data;
            
        });
    }
    
    getInitialPolicyListPayload(pageNumber: number = 1, pageSize: number = this.itemPerPage, isBilling : boolean, keyword : string, sortField : string, order : string): IPolicyDocumentListRequestDTO {
        const payload: IPolicyDocumentListRequestDTO = {
          pageNumber: pageNumber,
          pageSize: pageSize,
          riskDetailId :this.policy? this.submissionData.riskDetail?.riskId : this.submissionData.riskDetail?.id,
          isPolicy : this.policy,
          isBilling : isBilling,
          keyword: keyword,
          sortField:sortField,
          order:order
        };
        return payload;
      }

    downloadAllDocuments() {
        const policyName = this.submissionData.riskDetail?.nameAndAddress?.businessName;
        //#region  loading codde
        const dvloaderbar = document.querySelector('#dvloaderbar') as any;
    
        this.message = `Requesting download`;
        this.progress = 0;
    
        this.btnShowLoadingUploader.nativeElement.click();
        dvloaderbar.style.width = `0%`;
    
        const intervalID = this.dotAnimation();
    
        //#endregion Get data sets
        this.downloadAllDocumentsAPI(
          (event) => {
            this.progress = 0;
            this.message = 'Preparing download attachments';
            dvloaderbar.style.width = `${this.progress}%`;
          },
          (event) => {
            if (event.lengthComputable) {
              this.progress = Math.round(100 * event.loaded / event.total);
              this.message = 'Downloading';
              dvloaderbar.style.width = `${this.progress}%`;
            }
          })
          .then(blobFile => {
            setTimeout(() => {
              this.message = `Download complete`;
              dvloaderbar.style.width = `100%`;
              this.toastr.success('Download Successful!', 'Success!');
    
              const fileName = `${policyName}, Policy Documents`;
              this.downLoadFile((<Blob>blobFile), fileName);
              this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
            }, 2000);
          }).catch(blobFile => {
            const errMsg = 'Error encountered while downloading the file. Please try again.';
            NotifUtils.showError(errMsg, () => { });
            this.toastr.error('Error encountered while downloading the file.', 'Failed!');
            this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
          }).finally(() => {
            clearInterval(intervalID);
          });
      }

    downloadAllDocumentsAPI(callbackStart?: Function, callbackDownload?: Function): Promise<unknown> {
        const promise = new Promise((resolve, reject) => {
            try {
              let xhr = new XMLHttpRequest();
              xhr.open('POST', new URL(`/api/FileUpload/DownloadAll`, this.baseUrl).href, true);
              xhr.setRequestHeader('Content-type', 'application/json');
              xhr.responseType = 'blob';
            
                let token = null;
                if (this.authService.isLoggedIn()) {
                    const auth = this.authService.getAuth();
                    token = auth ? auth.token : null;
                }
                if (token) {
                xhr.setRequestHeader('Authorization', `Bearer ${token}`);
                } 

              if (callbackStart) {
                xhr.addEventListener('loadstart', callbackStart.bind(this), false);
              }
      
              if (callbackDownload) {
                xhr.addEventListener('progress', callbackDownload.bind(this), false);
              }
      
              xhr.onreadystatechange = function (response) {
                if (xhr.readyState === 4) {
                  if (xhr.status === 200) {
                    resolve(xhr.response);
                  } else {
                    reject(xhr.response);
                  }
                }
              };
              xhr.send(JSON.stringify(this.getInitialPolicyListPayload(this.currentPage, this.itemPerPage, false,'','','')));
            } catch {
              reject(null);
            }
          });
          return promise;
        
      }

      dotAnimation() {
        let counter = 1;
        const intervalID = setInterval(() => {
          if (counter > 0 && counter < 5) {
            this.dotanimation = this.dotanimation + '.';
          } else {
            counter = 0;
            this.dotanimation = '.';
          }
          counter += 1;
        }, 700);
        return intervalID;
      }

      downLoadFile(blob: Blob, fileName: string) {
        const downloadLink: HTMLAnchorElement = document.createElement('a') as HTMLAnchorElement;
        const url = window.URL.createObjectURL(blob);
        downloadLink.href = url;
        downloadLink.download = `${fileName}.zip`;
        document.body.appendChild(downloadLink);
        downloadLink.click();
        document.body.removeChild(downloadLink);
        URL.revokeObjectURL(url);
      }
}
