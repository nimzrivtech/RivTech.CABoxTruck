import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { BusinessDetailsDTO } from '../../../shared/models/submission/applicant/businessDetailsDto';
import { EntitySubsidiaryDTO, SaveEntitySubsidiaryDTO } from '../../../shared/models/submission/entitySubsidiaryDto';
import { catchError, map } from 'rxjs/operators';
import { DatasetDTO } from '@app/shared/models/submission/datasetDTO';

@Injectable({
  providedIn: 'root'
})
export class ApplicantBusinessDetailsService {
    private baseUrl: string;

    constructor(private http: HttpClient, private commonService: CommonService) {
        this.http = http;
        this.baseUrl = environment.ApiUrl.replace('/api', '');
    }

    getBusinessDetails(id: string): Observable<BusinessDetailsDTO> {
        const url_ = this.baseUrl + `/api/ApplicantBusinessDetails/${id}`;
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined.');
        }

        return this.http.get(url_)
            .map(data => {
                return data as BusinessDetailsDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    saveBusinessDetails(body: BusinessDetailsDTO | undefined): Observable<BusinessDetailsDTO> {
        const url_ = this.baseUrl + '/api/ApplicantBusinessDetails';
        return this.http.post(url_, body).catch(this.commonService.handleObservableHttpError);
    }

    saveApplicantSubsidiary(body: SaveEntitySubsidiaryDTO | undefined): Observable<EntitySubsidiaryDTO> {
        const url_ = this.baseUrl + '/api/ApplicantBusinessDetails/SaveSubsidiary';

        return this.http.post(url_, body)
            .map(data => {
                return data as EntitySubsidiaryDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    deleteApplicantSubsidiary(id: string): Observable<any> {
        const url_ = this.baseUrl + `/api/ApplicantBusinessDetails/DeleteSubsidiary/${id}`;

        return this.http.delete(url_)
            .catch(this.commonService.handleObservableHttpError);
    }

    deleteAllApplicantSubsidiary(id: string): Observable<any> {
        const url_ = this.baseUrl + `/api/ApplicantBusinessDetails/DeleteSubsidiaryAll/${id}`;

        return this.http.delete(url_)
            .catch(this.commonService.handleObservableHttpError);
    }

    saveApplicantCompany(body: SaveEntitySubsidiaryDTO | undefined): Observable<EntitySubsidiaryDTO> {
        const url_ = this.baseUrl + '/api/ApplicantBusinessDetails/SaveCompany';

        return this.http.post(url_, body)
            .map(data => {
                return data as EntitySubsidiaryDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    deleteApplicantCompany(id: string): Observable<any> {
        const url_ = this.baseUrl + `/api/ApplicantBusinessDetails/DeleteCompany/${id}`;

        return this.http.delete(url_)
            .catch(this.commonService.handleObservableHttpError);
    }

    deleteAllApplicantCompany(id: string): Observable<any> {
        const url_ = this.baseUrl + `/api/ApplicantBusinessDetails/DeleteCompanyAll/${id}`;

        return this.http.delete(url_)
            .catch(this.commonService.handleObservableHttpError);
    }

    getApplicantMainGVW(isCurrentlySupported: boolean): Observable<DatasetDTO[]> {
        const url_ = this.baseUrl + `/api/ApplicantBusinessDetails/GetMainGVWDataset?isCurrentlySupported=${isCurrentlySupported}`;
        return this.http.get(url_)
            .pipe(
                map(data => {
                    return data as DatasetDTO[];
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getApplicantSecondaryBusGroup(isCurrentlySupported: boolean): Observable<DatasetDTO[]> {
        const url_ = this.baseUrl + `/api/ApplicantBusinessDetails/GetSecondaryBusinessGroupDataset?isCurrentlySupported=${isCurrentlySupported}`;
        return this.http.get(url_)
            .pipe(
                map(data => {
                    return data as DatasetDTO[];
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

}
