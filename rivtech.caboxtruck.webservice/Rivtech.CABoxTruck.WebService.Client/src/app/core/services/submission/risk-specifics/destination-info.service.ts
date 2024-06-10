import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment';
import { IDestinationDto } from '../../../../shared/models/submission/riskspecifics/destinationDto';
import { CommonService } from '../../common.service';

@Injectable({
  providedIn: 'root'
})

export class DestinationInfoService {
  private baseUrl: string;

  private selectedDestinationSource = new BehaviorSubject<IDestinationDto>(null);
  selectedDestination$ = this.selectedDestinationSource.asObservable();

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl;
  }

  getAllDestinations(riskDetailId: string): Observable<IDestinationDto[]> {
    return this.http.get(`${this.baseUrl}/riskspecificdestination/list/${riskDetailId}`)
                      .pipe(
                        map((data) => {
                          return data as IDestinationDto[];
                        }),
                        catchError(this.commonService.handleObservableHttpError)
                      );
  }

  getDestinationById(id: string): Observable<IDestinationDto> {
    return this.http.get(`${this.baseUrl}/riskspecificdestination/${id}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateDestination(destination: IDestinationDto): Observable<string> {
    return this.http.put(`${this.baseUrl}/riskspecificdestination/${destination.id}`, destination, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  addNewDestination(destination: IDestinationDto): Observable<string> {
    return this.http.post(`${this.baseUrl}/riskspecificdestination`, destination, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  deleteDestination(id: string): Observable<string> {
    return this.http.delete(`${this.baseUrl}/riskspecificdestination/${id}`, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  setSelectedDestination(destination: IDestinationDto) {
    this.selectedDestinationSource.next(destination);
  }
}
