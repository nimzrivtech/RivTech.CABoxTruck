import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from '@app/core/services/common.service';
import { environment } from '../../../environments/environment';
import { Observable, of } from 'rxjs';
import { DocumentFile, IPolicyDocumentResultDTO,IPolicyDocumentListRequestDTO } from './document-file';
import { catchError, map } from 'rxjs/operators';
import { FileUploadDocumentDto } from '@app/shared/models/submission/binding/FileUploadDocumentDto';

@Injectable({
    providedIn: 'root'
})
export class DocumentApiService {

    private data: DocumentFile[];
    private baseUrl: string;
    constructor(private http: HttpClient, private commonService: CommonService, private httpbackend: HttpBackend) {
        this.baseUrl = environment.ApiUrl.replace('/api', '');
    }

    getDocuments(riskDetailId: string): Observable<DocumentFile[]> {
        return this.http.get(`${this.baseUrl}/api/FileUpload/GetFileUploadByRiskDetailId/${riskDetailId}`)
            .pipe(
                map(data => {
                return data as DocumentFile[];
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }
    getPolicyDocuments(payload: IPolicyDocumentListRequestDTO): Observable<IPolicyDocumentResultDTO> {
        return this.http.post(`${this.baseUrl}/api/FileUpload/Documentlist`,payload)
            .pipe(
                map(data => {
                return data as IPolicyDocumentResultDTO;
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

   
    getRiskDocuments(riskId: string): Observable<DocumentFile[]> {
        return this.http.get(`${this.baseUrl}/api/FileUpload/GetFileUploadByRiskId/${riskId}`)
            .pipe(
                map(data => {
                return data as DocumentFile[];
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getCategories(): Observable<any> {
        const url_ = this.baseUrl + `/api/KeyValue/generic/FileCategory`;
        return this.http.get(url_)
            .pipe(
                map(data => {
                return data as any;
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getBindOption(): Observable<any> {
        const url_ = this.baseUrl + `/api/KeyValue/generic/BindOption`;
        return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
    }

    addDocument(inputData?: any): Observable<any> {
        // this.http = new HttpClient(this.httpbackend);
        return this.http.post(new URL(`/api/FileUpload/User`, this.baseUrl).href, inputData, { reportProgress: true, observe: 'events', responseType: 'json' })
            .catch(this.commonService.handleObservableHttpError);
    }

    delete(id: string): Observable<boolean> {
        return this.http.delete(`${this.baseUrl}/api/FileUpload/${id}`)
        .catch(this.commonService.handleObservableHttpError);
    }

    emailDocs(selectedItemIds: string[]) {
        throw new Error('Method not implemented.');
    }

    downloadDocs(selectedItemIds: string[]) {
        // this.http = new HttpClient(this.httpbackend);
        return this.http.post(new URL(`/api/FileUpload/Download`, this.baseUrl).href, selectedItemIds, { reportProgress: true, observe: 'events', responseType: 'blob' })
            .catch(this.commonService.handleObservableHttpError);
    }

    getDocumentIsUploaded(id: string): Observable<boolean> {
        return this.http.get(`${this.baseUrl}/api/FileUpload/CheckFileUploadId/${id}`)
            .pipe(
                map(data => {
                return data as boolean;
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getDocumentIsAllowedToEditDelete(id: string): Observable<boolean> {
        return this.http.get(`${this.baseUrl}/api/FileUpload/CheckIsAllowedToEditDelete/${id}`)
            .pipe(
                map(data => {
                return data as boolean;
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    saveQuoteInfo(input: FileUploadDocumentDto) {
        // this.http = new HttpClient(this.httpbackend);
        return this.http.post(new URL(`/api/FileUpload/QuoteInfo`, this.baseUrl).href, input, { reportProgress: true, observe: 'events', responseType: 'blob' })
            .catch(this.commonService.handleObservableHttpError);
    }

    saveAccountsReceivable(inputData?: any) {
        return this.http.post(new URL(`/api/FileUpload/AccountsReceivable`, this.baseUrl).href, inputData, { reportProgress: true, observe: 'events', responseType: 'text' })
            .catch(this.commonService.handleObservableHttpError);
    }

    savePaymentPosting(inputData?: any) {
        return this.http.post(new URL(`/api/FileUpload/PaymentPosting`, this.baseUrl).href, inputData, { reportProgress: true, observe: 'events', responseType: 'text' })
            .catch(this.commonService.handleObservableHttpError);
    }

    saveBrokerCommission(inputData?: any) {
        return this.http.post(new URL(`/api/FileUpload/BrokerCommission`, this.baseUrl).href, inputData, { reportProgress: true, observe: 'events', responseType: 'text' })
            .catch(this.commonService.handleObservableHttpError);
    }

    getDocumentSASURL(url: string): Observable<string> {
        return this.http.post(new URL(`/api/SASURLGenerator?url=${url}`, this.baseUrl).href, {})
            .catch(this.commonService.handleObservableHttpError);
    }

    getDocumentSASURL2(url: string, filename: string): Observable<string> {
        return this.http.post(new URL(`/api/SASURLGenerator/GenerateSASURL2?url=${url}&newFilename=${filename}`, this.baseUrl).href, {})
            .catch(this.commonService.handleObservableHttpError);
    }
}
