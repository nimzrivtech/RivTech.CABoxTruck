import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable, of } from 'rxjs';
import { VehicleDto } from '../../../shared/models/submission/Vehicle/VehicleDto';
import { VehicleHeaderDto } from '../../../shared/models/submission/Vehicle/VehicleHeaderDto';
import { VehicleInformationDropDowns } from '../data/vehicle/vehicle.data';
import { catchError, map } from 'rxjs/operators';
import { RiskCoverageDTO } from '@app/shared/models/submission/limits/riskCoverageDto';
import { DatasetDTO } from '@app/shared/models/submission/datasetDTO';
import { SelectItem } from '@app/shared/models/dynamic/select-item';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
 
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService, private httpbackend: HttpBackend) {
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  uploadExcelFile(inputData?: any): Observable<VehicleDto[]> {
    //this.http = new HttpClient(this.httpbackend);
    return this.http.post(new URL(`/api/Vehicle/AddFromExcel`, this.baseUrl).href, inputData, { reportProgress: true, observe: 'events', responseType: 'json' })
      .catch(this.commonService.handleObservableHttpError);
  }

  downloadTemplate(): Observable<any> {
    //this.http = new HttpClient(this.httpbackend);
    return this.http.get(new URL(`/api/Vehicle/Template`, this.baseUrl).href, { reportProgress: true, observe: 'events', responseType: 'blob' })
      .catch(this.commonService.handleObservableHttpError);
  }

  downloadTemplateV7(): Observable<any> {
    //this.http = new HttpClient(this.httpbackend);
    return this.http.get(new URL(`/api/Vehicle/TemplateV7`, this.baseUrl).href, { reportProgress: true, observe: 'events', responseType: 'blob' })
      .catch(this.commonService.handleObservableHttpError);
  }

  getstyleList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/styleList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getvehicleDescList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/vehicleDescList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getaverageMileageList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/averageMileageList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getautoBiLimitList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/autoBiLimitList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getautoPdLimitList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/autoPdLimitList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getliabilityDeductibleList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/liabilityDeductibleList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getmedPayList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/medPayList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getpipList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/pipList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getuMBIList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/uMBIList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getuMPDList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/uMPDList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getuIMBIList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/uIMBIList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getuMStackingList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/uMStackingList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getnonTruckingLiabilityList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/nonTruckingLiabilityList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getoverrideColDedList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/overrideColDedList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getoverrideCompDedList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/overrideCompDedList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getcARGOCOMPLimitsList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/cARGOCOMPLimitsList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getcARGOCOLLLimitsList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/cARGOCOLLLimitsList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getaccidentalDeathNJPAList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/accidentalDeathNJPAList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getessentialServicesNJList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/essentialServicesNJList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getfuneralBenefitsNJPAList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/funeralBenefitsNJPAList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getincomeLossNJPAVAList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/incomeLossNJPAVAList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getuseClassList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/useClassList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getStateList(): Observable<any> {
    return this.http.get(`${this.genericUrl}/api/State`)
      .pipe(
        map(data => {
          return data as any;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }
  getgaragingZipCodeList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/garagingZipCodeList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getuseClassCodeList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/useClassCodeList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getclassCodeMeaningList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/classCodeMeaningList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getVehicleLevelBusinessClassList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/VehicleLevelBusinessClassList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getliabRequestedList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/liabRequestedList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getcollRequestedList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/collRequestedList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  getCompFireTheftRequestedList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/CompFireTheftRequestedList/`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRiskVehicles(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/GetByRiskInclude/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  get(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/GetInclude/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getByRiskDetailId(riskDetailId: string, fromPolicy?: boolean): Observable<VehicleDto[]> {
    fromPolicy = fromPolicy || false;
    const url_ = this.baseUrl + `/api/Vehicle/GetByRiskInclude/${riskDetailId}?fromPolicy=${fromPolicy}`;

    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as VehicleDto[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }
  getByRiskDetailIdPolicy(riskDetailId: string, fromPolicy?: boolean): Observable<VehicleDto[]> {
    fromPolicy = fromPolicy || false;
    const url_ = this.baseUrl + `/api/Vehicle/GetByRiskIncludePolicy/${riskDetailId}?fromPolicy=${fromPolicy}`;

    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as VehicleDto[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }
  
  getPreviousVehicles(vehicle: VehicleDto): Observable<any> {
    return this.http.get(new URL(`/api/Vehicle/GetPreviousVehicles/${vehicle.id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  PutListOptions(obj: VehicleDto[]): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Vehicle/PutListOptions`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(vehicle: VehicleDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/Vehicle`, vehicle)
      .catch(this.commonService.handleObservableHttpError);
  }

  postVehicles(vehicles: VehicleDto[]): Observable<VehicleDto[]> {
    return this.http.post(`${this.baseUrl}/api/Vehicle/ExcelVehicles`, vehicles)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(vehicle: VehicleDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Vehicle/${vehicle.id}`, vehicle)
      .catch(this.commonService.handleObservableHttpError);
  }

  putAddresses(obj: VehicleDto[]): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Vehicle/Addresses`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  putOtherCoverages(data: { vehicles: VehicleDto[], oldLimitsData: RiskCoverageDTO }): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Vehicle/OtherCoverages`, data)
      .catch(this.commonService.handleObservableHttpError);
  }

  delete(id: string, fromEndorsement?: boolean): Observable<any> {
    return this.http.delete(`${this.baseUrl}/api/Vehicle/${id}?fromEndorsement=${(fromEndorsement ?? "")}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  reinstate(id: string) {
    return this.http.put(`${this.baseUrl}/api/Vehicle/reinstate/${id}`, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  checkIfHasPreviousVersion$(vehicleId: string) : Observable<boolean> {
    return this.http.get(`${this.baseUrl}/api/Vehicle/HasPreviousVehicleVersions/${vehicleId}`)
      .pipe(
        map(data => data as boolean)
      ).catch(this.commonService.handleObservableHttpError);
  }

  deleteall(id: string) {
    return this.http.put(`${this.baseUrl}/api/Vehicle/deleteAll/${id}`, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  // saveVehicleHeader(data: VehicleHeaderDto): Observable<any> {
  //   return this.http.post(`${this.baseUrl}/api/Vehicle/SaveVehicleHeader`, data)
  //     .catch(this.commonService.handleObservableHttpError);
  // }


  saveVehicleHeader(obj: VehicleHeaderDto): Observable<any> {
    return this.http.post(new URL(`/api/Vehicle/SaveVehicleHeader`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }
  
  getVehicleGVWList(isCurrentlySupported: boolean): Observable<DatasetDTO[]> {
    const url_ = this.baseUrl + `/api/Vehicle/GetVehicleGVWDataset?isCurrentlySupported=${isCurrentlySupported}`;
    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as DatasetDTO[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getVehicleDescriptionList(isCurrentlySupported: boolean): Observable<DatasetDTO[]> {
    const url_ = this.baseUrl + `/api/Vehicle/GetVehicleDescriptionDataset?isCurrentlySupported=${isCurrentlySupported}`;
    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as DatasetDTO[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

}
