import {Injectable} from '@angular/core';
import { BlacklistedDriverDTO } from '../../../shared/models/blacklisted-driver/blacklisted-driver';
import {HttpClient} from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import {catchError, map} from 'rxjs/operators';
import {VehicleDto} from '@app/shared/models/submission/Vehicle/VehicleDto';

@Injectable({
  providedIn: 'root'
})

export class BlacklistedDriverService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  post(blackListedDriver: BlacklistedDriverDTO) {
    return this.http.post(`${this.baseUrl}/api/BlacklistedDriver/AddUpdate`, blackListedDriver)
      .catch(this.commonService.handleObservableHttpError);
  }

  searchDriver(driverName: string, driverLicenseNumber: string) {
    const url = `${this.baseUrl}/api/BlacklistedDriver/SearchDriver?driverName=${driverName}&licenseNumber=${driverLicenseNumber}`;

    return this.http.get(url)
      .pipe(
        map(data => {
          return data as BlacklistedDriverDTO[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  validateDriver(driverName: string, driverLicenseNumber: string) {
    const url = `${this.baseUrl}/api/BlacklistedDriver/ValidateDriver?driverName=${driverName}&licenseNumber=${driverLicenseNumber}`;

    return this.http.get(url)
      .pipe(
        map(data => {
          return data as boolean;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }
}
