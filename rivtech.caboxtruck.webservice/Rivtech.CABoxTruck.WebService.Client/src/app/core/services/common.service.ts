import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { of, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { ProgramDTO } from '../../shared/models/management/program-dto';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  public programIdList: Array<number> = [];
  public programList: ProgramDTO[] = [];

  constructor() { }

  handleObservableHttpError(error: any): Observable<any> {
    console.error('server error:', error);  // debug
    if (error instanceof HttpErrorResponse) {
      return throwError(error);
    }
    return throwError(JSON.stringify(error) || 'backend server error');
  }

  handleNotFoundHttpError(error: any): Observable<any> {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 404) {
        return of({ status: error.status });
      } else {
        return throwError(error);
      }
    }
    return throwError(JSON.stringify(error) || 'backend server error');
  }
}
