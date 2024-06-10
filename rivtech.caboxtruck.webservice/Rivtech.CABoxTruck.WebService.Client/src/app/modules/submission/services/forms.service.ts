import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { FormDTO } from '../../../shared/models/submission/forms/FormDto';
import { FormOtherInfoDTO } from '@app/shared/models/submission/forms/FormOtherInfoDto';
import { FormMcs90DTO } from '@app/shared/models/submission/forms/FormMcs90Dto';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FormsService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService, private httpbackend: HttpBackend) {
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  get(riskDetailId: string): Observable<FormDTO[]> {
    return this.http.get(new URL(`/api/forms/${riskDetailId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getByForm(riskDetailId: string, formId: string) : Observable<FormDTO> {
    return this.http.get(new URL(`/api/forms/GetByForm/${riskDetailId}?formId=${formId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(obj: FormDTO[]): Observable<FormDTO[]> {
    return this.http.post(new URL(`/api/forms`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  postDefaults(riskId: string): Observable<FormDTO[]> {
    return this.http.post(new URL(`/api/forms/defaults/${riskId}`, this.baseUrl).href, riskId)
      .catch(this.commonService.handleObservableHttpError);
  }

  postDefaultsNew(riskId: string): Observable<FormDTO[]> {
    return this.http.post(new URL(`/api/forms/defaultsNew/${riskId}`, this.baseUrl).href, riskId)
      .catch(this.commonService.handleObservableHttpError);
  }

  postUserForm(inputData?: any): Observable<any> {
    // this.http = new HttpClient(this.httpbackend);
    return this.http.post(new URL(`/api/forms/user`, this.baseUrl).href, inputData, { reportProgress: true, observe: 'events', responseType: 'json' })
    .catch(this.commonService.handleObservableHttpError);
  }

  postPreForms(obj: FormDTO[]): Observable<FormDTO[]> {
    return this.http.post(new URL(`/api/forms/selected`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  generate(riskForm: FormDTO[]) {
    return this.http.post(new URL(`/api/PolicyPacketReport/Generate`, this.baseUrl).href, riskForm)
      .catch(this.commonService.handleObservableHttpError);
  }

  generateInvoice(id: string, invoice: any) {
    return this.http.post(new URL(`/api/InvoiceReport/${id}`, this.baseUrl).href, invoice)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateForm(riskForm: FormDTO): Observable<FormDTO> {
    return this.http.post(new URL(`/api/forms/updateForm`, this.baseUrl).href, riskForm)
      .catch(this.commonService.handleObservableHttpError);
  }

  removeForm(riskFormId: string): Observable<FormDTO> {
    return this.http.delete(new URL(`/api/forms/${riskFormId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateExclusionRadius(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/exclusionRadius`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updatehiredPhysdam(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/hiredPhysdam`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updatedriverRequirement(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/driverRequirement`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateTerritoryExclusion(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/territoryExclusion`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateManuscript(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/manuscript`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateTrailerInterchange(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/trailerInterchange`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateGeneralLiability(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/generalLiability`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateMcs90(otherInfo: FormMcs90DTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/mcs90`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateDefaultStates(riskId: string) {
    return this.http.post(new URL(`/api/formsOtherInfo/defaultStates/${riskId}`, this.baseUrl).href, riskId)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateLossPayee(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/lossPayee`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateStatePAaddcombFPB(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/StatePAaddcombFPB`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateCoveredAutoDesignationSymbol(otherInfo: FormOtherInfoDTO) {
    return this.http.post(new URL(`/api/formsOtherInfo/coveredAutoDesignationSymbol`, this.baseUrl).href, otherInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  getSmybolTenResponses(riskDetailId: string): Observable<any>{
    return this.http.get(new URL(`/api/formsOtherInfo/symbolTenResponses/${riskDetailId}`, this.baseUrl).href).pipe(
      map(data => {
        return data as any;
      }),
      catchError(this.commonService.handleObservableHttpError)
    );
  }

  getStates(): Observable<any> {
    return this.http.get(new URL('/api/State', environment.GenericServiceUrl).href)
      .pipe(
        map(data => {
          return data as any;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  hasInitialDriverExclusion(riskId: string) {
    return this.http.get(new URL(`/api/forms/HasDriverExclusionForms/${riskId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateFormEffectiveDate(riskDetailId: string): Observable<any> {
    return this.http.post(new URL(`/api/forms/updateFormEffectiveDate/${riskDetailId}`, this.baseUrl).href, riskDetailId)
      .catch(this.commonService.handleObservableHttpError);
  }
}
