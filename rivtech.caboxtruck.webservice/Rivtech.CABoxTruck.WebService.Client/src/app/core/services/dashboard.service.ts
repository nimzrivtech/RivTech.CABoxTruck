import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBanner } from 'app/shared/models/dashboard.banner.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashBoardService {
  baseUrl: string;

  constructor(private http: HttpClient) {
    this.baseUrl = environment.ApiUrl;
  }

  getBanners(): Observable<IBanner[]> {
    const url = `${this.baseUrl}/Banner`;
    return this.http.get<IBanner[]>(url);
  }
}
